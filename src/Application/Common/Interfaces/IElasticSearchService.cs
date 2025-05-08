using System.Runtime.CompilerServices;

namespace KarnelTravel.Application.Common.Interfaces;
public interface IElasticSearchService
{
	//create index
	Task CreateIndexIfNotExisted(string indexName);

	//add or update data
	Task<bool> AddOrUpdate<T>(T dataObject, string indexName);

	//update bulk data
	Task<bool>  AddOrUpdateBulk<T>(IEnumerable<T> dataObjects, string indexName);

	//get data
	Task<T> Get<T>(string key);

	//get all object data
	Task<List<T>> GetAll<T>();

	//remove key
	Task<bool> Remove<T>(string key, string indexName);

	//remove all
	Task<long?> RemoveAll<T>(string key, string indexName);
}
 