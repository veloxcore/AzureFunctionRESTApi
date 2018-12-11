using AutoMapper;
using Rest.Model.DTO;
using Rest.Model.Entity;
using Rest.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rest.Data.Repository
{
    /// <summary>
    /// Metadata Repository
    /// </summary>
    public interface IMetaDataRepository 
    {
        /// <summary>
        /// Get all metadata 
        /// </summary>
        /// <returns>List of metadata</returns>
        Task<IEnumerable<MetaData>> GetAllAsync();

        /// <summary>
        /// Insert Metadata
        /// </summary>
        /// <param name="metaData">Metadata</param>
        /// <returns></returns>
        Task InsertAsync(MetaData metaData);
    }
    
    /// <summary>
    /// Metadata Repository
    /// </summary>
    public class MetaDataRepository : Repository<MetaData>, IMetaDataRepository
    {
        public MetaDataRepository(IDbQueryProcessor dbQueryProcessor) : base(dbQueryProcessor)
        {
        }

        /// <summary>
        /// Get All Metadata
        /// </summary>
        /// <returns>List of metadata</returns>
        public async Task<IEnumerable<MetaData>> GetAllAsync()
        {
            //var records = (await GetAllAsync()).OrderByDescending(o => o.DateOfPublication).ToList();      // Also here we can use ProjectToListAsync, but Split in the ForMember is causing issue.
            IDictionary<string, object> parameter = new Dictionary<string, object>();
            return await GetAsync<MetaData>("GetAllMetaData", parameter);
        }

        /// <summary>
        /// Insert Metadata
        /// </summary>
        /// <param name="metaData">Metadata</param>
        /// <returns></returns>
        public async Task InsertAsync(MetaData metaData)
        {
            IDictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("DeviceId", metaData.DeviceId);
            parameter.Add("TimeStamp", metaData.TimeStamp);
            parameter.Add("Payload", metaData.Payload ?? string.Empty);

            await ProcessCommandAsync("InsertMetaData", parameter);
        }

        protected override object[] GetKey(MetaData metaData)
        {
            return new object[] { metaData.Id };
        }

    }
}
