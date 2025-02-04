using Elastic.Clients.Elasticsearch;
using Infrastructure.ElasticSearch.Settings;
using KarnelTravel.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace Infrastructure.ElasticSearch.Service;
public class ElasticSearchService<T> : IElasticSearchService<T>
{
	protected readonly ElasticsearchClient _elasticsearchClient;
	protected readonly ElasticSettings _elasticSettings;

    public ElasticSearchService(IOptions<ElasticSettings> optionsMonitor)
    {
        _elasticSettings = optionsMonitor.Value;

		var settings = new ElasticsearchClientSettings(new Uri(_elasticSettings.Url))
			//.Authentication()
			.DefaultIndex(_elasticSettings.DefaultIndex);

		_elasticsearchClient = new ElasticsearchClient(settings);
	}

	public async Task CreateIndexIfNotExisted(string indexName)
	{
		var response = await _elasticsearchClient.Indices.ExistsAsync(indexName);
		if (!response.Exists)
			await _elasticsearchClient.Indices.CreateAsync(indexName);
	}

	public  Task<bool> AddOrUpdate(T dataObject)
	{
		throw new NotImplementedException();
	}

	public Task<bool> AddOrUpdateBulk(IEnumerable<T> dataObjects, string indexName)
	{
		throw new NotImplementedException();
	}

	public Task CreateIndexIfNotExisted(string indexName)
	{
		throw new NotImplementedException();
	}

	public Task<T> Get(string key)
	{
		throw new NotImplementedException();
	}

	public Task<List<T>> GetAll()
	{
		throw new NotImplementedException();
	}

	public Task<bool> Remove(string key)
	{
		throw new NotImplementedException();
	}

	public Task<long?> RemoveAll(string key)
	{
		throw new NotImplementedException();
	}
}
