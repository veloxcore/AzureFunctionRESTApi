using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Rest.Api
{
    /// <summary>
    /// Binding attribute to place on user code for WebJobs. 
    /// </summary>
    [Binding]
    public class SampleAttribute : Attribute
    {
        // Name of file to read. 
        [AutoResolve]
        [RegularExpression("^[A-Za-z][A-Za-z0-9]{2,128}$")]
        public string FileName { get; set; }

        // path where 
        [AppSetting(Default = "SamplePath")]
        public string Root { get; set; }
    }

    public class SampleItem
    {
        public string Name { get; set; }
        public string Contents { get; set; }
    }

    public class SampleExtensions : IExtensionConfigProvider
    {
        // Root path where files are written. 
        // Used when attribute.Root is blank 
        // This is an example of extension-global configuration. 
        // Generally, attributes should be able to override these settings. 
        // Make sure these settings are Json serialization friendly. 
        [JsonProperty("Root")]
        public string Root { get; set; }

        /// <summary>
        /// This callback is invoked by the WebJobs framework before the host starts execution. 
        /// It should add the binding rules and converters for our new <see cref="SampleAttribute"/> 
        /// </summary>
        /// <param name="context"></param>
        public void Initialize(ExtensionConfigContext context)
        {
            // Register converters. These help convert between the user's parameter type
            //  and the type specified by the binding rules. 

            // This allows a user to bind to IAsyncCollector<string>, and the sdk
            // will convert that to IAsyncCollector<SampleItem>
            context.AddConverter<string, SampleItem>(ConvertToItem);

            // This is useful on input. 
            context.AddConverter<SampleItem, string>(ConvertToString);

            // Create 2 binding rules for the Sample attribute.
            var rule = context.AddBindingRule<SampleAttribute>();

            rule.BindToInput<SampleItem>(BuildItemFromAttr);
            rule.BindToCollector<SampleItem>(BuildCollector);
        }

        private string GetRoot(SampleAttribute attribute)
        {
            var root = attribute.Root ?? this.Root ?? Path.GetTempPath();
            return root;
        }

        private string ConvertToString(SampleItem item)
        {
            return item.Contents;
        }

        private SampleItem ConvertToItem(string arg)
        {
            var parts = arg.Split(':');
            return new SampleItem
            {
                Name = parts[0],
                Contents = parts[1]
            };
        }

        private IAsyncCollector<SampleItem> BuildCollector(SampleAttribute attribute)
        {
            var root = GetRoot(attribute);
            return new SampleAsyncCollector(root);
        }

        // All {} and %% in the Attribute have been resolved by now. 
        private SampleItem BuildItemFromAttr(SampleAttribute attribute)
        {
            var root = GetRoot(attribute);
            var path = Path.Combine(root, attribute.FileName);
            if (!File.Exists(path))
            {
                return null;
            }
            var contents = File.ReadAllText(path);
            return new SampleItem
            {
                Name = attribute.FileName,
                Contents = contents
            };
        }
    }

    internal class SampleAsyncCollector : IAsyncCollector<SampleItem>
    {
        // Root path for where to write. This cna be a combination of the extension configuration 
        // and the attribute. 
        private readonly string _root;

        public SampleAsyncCollector(string root)
        {
            _root = root;
        }

        public Task AddAsync(SampleItem item, CancellationToken cancellationToken = default(CancellationToken))
        {
            var path = Path.Combine(_root, item.Name);
            Directory.CreateDirectory(Path.GetFullPath(_root));
            File.WriteAllText(path, item.Contents);
            return Task.CompletedTask;
        }

        public Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }
    }
}
