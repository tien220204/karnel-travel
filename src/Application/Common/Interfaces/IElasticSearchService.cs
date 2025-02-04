namespace KarnelTravel.Application.Common.Interfaces;
public interface IElasticSearchService<T>
{
	//create index
	Task CreateIndexIfNotExisted(string indexName);

	//add or update data
	Task<bool> AddOrUpdate(T dataObject);

	//update bulk data
	Task<bool>  AddOrUpdateBulk(IEnumerable<T> dataObjects, string indexName);

	//get data
	Task<T> Get(string key);

	//get all object data
	Task<List<T>> GetAll();

	//remove key
	Task<bool> Remove(string key);

	//remove all
	Task<long?> RemoveAll(string key);
}
