using Rest.Data.Repository;
using Rest.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rest.Service
{
    public interface IMetaDataService
    {
        Task<IEnumerable<MetaData>> GetMetaDataAsync();
        Task InsertMetaDataAsync(MetaData metadata);
    }

    public class MetaDataService : IMetaDataService
    {
        private readonly IMetaDataRepository _metaDataRepository;
        public MetaDataService(IMetaDataRepository metaDataRepository)
        {
            _metaDataRepository = metaDataRepository;
        }
        public Task<IEnumerable<MetaData>> GetMetaDataAsync()
        {
            return _metaDataRepository.GetAllAsync();
        }

        public Task InsertMetaDataAsync(MetaData metaData)
        {
            return _metaDataRepository.InsertAsync(metaData);
        }
    }
}
