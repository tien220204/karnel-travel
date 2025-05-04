using Elastic.Clients.Elasticsearch;
using Infrastructure.ElasticSearch.Settings;
using KarnelTravel.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace Infrastructure.ElasticSearch.Service;
public class ElasticSearchService : IElasticSearchService
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

	public async  Task<bool> AddOrUpdate<T>(T dataObject)
	{
		var response = await _elasticsearchClient.IndexAsync(dataObject, idx => idx.Index(_elasticSettings.DefaultIndex).OpType(OpType.Index));

		return response.IsValidResponse;
	}

	public async  Task<bool> AddOrUpdateBulk<T>(IEnumerable<T> dataObjects, string indexName)
	{
		var response = await _elasticsearchClient.BulkAsync(b => b.Index(_elasticSettings.DefaultIndex)
		.UpdateMany(dataObjects, (ud, u) => ud.Doc(u).DocAsUpsert(true))
		);

		return response.IsValidResponse;
	}

	public async Task<T> Get<T>(string key)
	{
		var response = await _elasticsearchClient.GetAsync<T>(key, g => g.Index(_elasticSettings.DefaultIndex));

		return response.Source;
	}

	public async Task<List<T>?> GetAll<T>()
	{
        var response = await _elasticsearchClient.SearchAsync<T>( g => g.Index(_elasticSettings.DefaultIndex));

        return response.IsValidResponse ? response.Documents.ToList() : default; 
    }

	public async Task<bool> Remove<T>(string key)
	{
        var response = await _elasticsearchClient.DeleteAsync<T>(key, g => g.Index(_elasticSettings.DefaultIndex));

        return response.IsValidResponse;
    }

	public async Task<long?> RemoveAll<T>(string key)
	{
		var response = await _elasticsearchClient.DeleteByQueryAsync<T>(d => d.Indices(_elasticSettings.DefaultIndex));

		return response.IsValidResponse ? response.Deleted : default;
	}
}
