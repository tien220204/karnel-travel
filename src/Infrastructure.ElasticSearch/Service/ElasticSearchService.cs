using Elastic.Clients.Elasticsearch;
using Infrastructure.ElasticSearch.Settings;
using KarnelTravel.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
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
			//.Authentication().
			.DefaultIndex(_elasticSettings.DefaultIndex)
			//enable detailed req/resp
			.DisableDirectStreaming();

		_elasticsearchClient = new ElasticsearchClient(settings);
	}

	public async Task CreateIndexIfNotExisted(string indexName)
	{
		try
		{
			var existsResponse = await _elasticsearchClient.Indices.ExistsAsync(indexName.ToLower());
			if (!existsResponse.Exists)
			{
				var createResponse = await _elasticsearchClient.Indices.CreateAsync(indexName.ToLower());

				//temporary exception display
				if (!createResponse.IsValidResponse)
				{
					var debugInfo = createResponse.ElasticsearchServerError?.ToString() ??
									createResponse.ElasticsearchServerError?.ToString() ??
									createResponse.DebugInformation;

					throw new Exception("Failed to create index: " + debugInfo);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception("Failed to create index: " + ex.Message, ex);
		}

	}

	//public async Task CreateIndexIfNotExisted(string indexName)
	//{
	//	var existsResponse = await _elasticsearchClient.Indices.ExistsAsync(indexName);

	//	if (!existsResponse.Exists)
	//	{
	//		var createResponse = await _elasticsearchClient.Indices.CreateAsync(indexName, c => c
	//			.Mappings(m => m
	//				.Properties(p => p
	//					.Text(t => t
	//						.Name("name")
	//					)
	//					.Number(n => n
	//						.Name("age")
	//						.Type(NumberType.Integer)
	//					)
	//					.Date(d => d
	//						.Name("created_at")
	//						.Format("yyyy-MM-dd'T'HH:mm:ss")
	//					)
	//				)
	//			)
	//		);

	//		if (!createResponse.IsValidResponse)
	//		{
	//			throw new Exception("Failed to create index: " + createResponse.ElasticsearchServerError?.ToString());
	//		}
	//	}
	//}


	public async  Task<bool> AddOrUpdate<T>(T dataObject, string indexName)
	{
		var response = await _elasticsearchClient.IndexAsync(dataObject, idx => idx.Index(indexName.ToLower()).OpType(OpType.Index));

		if (!response.IsValidResponse)
		{
			throw new Exception("Failed to index document: "+ response.ElasticsearchServerError?.ToString());
		}

		return response.IsValidResponse;
	}

	public async  Task<bool> AddOrUpdateBulk<T>(IEnumerable<T> dataObjects, string indexName)
	{
		var response = await _elasticsearchClient.BulkAsync(b => b.Index(indexName.ToLower())
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

	public async Task<bool> Remove<T>(string key, string indexName)
	{
        var response = await _elasticsearchClient.DeleteAsync<T>(key,d => d.Index(indexName.ToLower()));

        return response.IsValidResponse;
		
    }

	public async Task<long?> RemoveAll<T>(string key, string indexName)
	{
		var response = await _elasticsearchClient.DeleteByQueryAsync<T>(indexName.ToLower());

		return response.IsValidResponse ? response.Deleted : default;
	}

	//temporary
	public async Task<SearchResponse<T>> SearchMultiFieldsByKeyword<T>(List<string> fields, string keyword, string indexName)
	{

		var fieldObjects = Fields.FromStrings(fields.ToArray());

		var searchResponse = await _elasticsearchClient.SearchAsync<T>(s => s
			.Index(indexName.ToLower())
			.Query(q => q
				.MultiMatch(m => m
					.Fields(fieldObjects)
					.Query(keyword)
				)
			)
		);
		if (searchResponse.IsValidResponse)
		{
			return searchResponse;
		}
		throw new Exception("Failed to search: " + searchResponse.ElasticsearchServerError?.ToString());
	}
}
