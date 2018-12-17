using Rest.Data.Repository;
using Rest.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rest.Service
{
    /// <summary>
    /// Metadata service to process with metadata repository for data operations
    /// </summary>
    public interface IMetaDataService
    {
        /// <summary>
        /// Get all metadata 
        /// </summary>
        /// <returns>List of metadata</returns>
        Task<IEnumerable<MetaData>> GetMetaDataAsync();

        /// <summary>
        /// Insert metadata
        /// </summary>
        /// <param name="metadata">metadata Object</param>
        /// <returns></returns>
        Task InsertMetaDataAsync(MetaData metadata);
    }

    public class MetaDataService : IMetaDataService
    {
        private readonly IMetaDataRepository _metaDataRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="metaDataRepository">metadata repository object to use in other methods</param>
        public MetaDataService(IMetaDataRepository metaDataRepository)
        {
            _metaDataRepository = metaDataRepository;
        }

        /// <summary>
        /// Get all metadata 
        /// </summary>
        /// <returns>List of metadata</returns>
        public Task<IEnumerable<MetaData>> GetMetaDataAsync()
        {
            return _metaDataRepository.GetAllAsync();
        }

        /// <summary>
        /// Insert metadata
        /// </summary>
        /// <param name="metadata">metadata Object</param>
        /// <returns></returns>
        public Task InsertMetaDataAsync(MetaData metaData)
        {
            return _metaDataRepository.InsertAsync(metaData);
        }
    }
}
