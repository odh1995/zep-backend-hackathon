using Busan_Hackathon_2022.Services.Interfaces;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

namespace Busan_Hackathon_2022.Services.Implementations
{
    public class CropsService : ICropsService
    {
        private Container _container;
        private static readonly JsonSerializer Serializer = new JsonSerializer();
        public CropsService(CosmosClient dbClient, string databaseName, string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }
        public async Task<IEnumerable<Crops>> GetCrops(string queryString)
        {
            var query = this._container.GetItemQueryIterator<Crops>(new QueryDefinition(queryString));
            List<Crops> results = new();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task<Crops> GetCropsByCoordinate(int x, int y)
        {
            string query = $"SELECT * FROM c WHERE c.x_coordinate = {x} AND c.y_coordinate = {y}";;
            var streamResultSet = this._container.GetItemQueryIterator<Crops>(query);
            List<Crops> results = new();

            while (streamResultSet.HasMoreResults)
            {
                var response = await streamResultSet.ReadNextAsync();

                results.AddRange(response.ToList());
            }
            return results.FirstOrDefault();
        }

        public async Task<bool> GetCropsBoolById(string id)
        {
            string query = $"SELECT * FROM c WHERE c.Id = @cropsId";
            QueryDefinition queryDefinition = new QueryDefinition(query).WithParameter("cropsId", id);
            List<Crops> cropsResults = new();
            FeedIterator streamResultSet = this._container.GetItemQueryStreamIterator(
                queryDefinition,
                requestOptions: new QueryRequestOptions()
                {
                    PartitionKey = new PartitionKey(id),
                    MaxItemCount = 10,
                    MaxConcurrency = 1
                });

            while (streamResultSet.HasMoreResults)
            {
                using (ResponseMessage responseMessage = await streamResultSet.ReadNextAsync())
                {

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        dynamic streamResponse = FromStream<dynamic>(responseMessage.Content);
                        List<Crops> familyResult = streamResponse.Documents.ToObject<List<Crops>>();
                        cropsResults.AddRange(familyResult);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            if (cropsResults != null && cropsResults.Count > 0)
            {
                return true;
            }
            return false;
        }

        private static T FromStream<T>(Stream stream)
        {
            using (stream)
            {
                if (typeof(Stream).IsAssignableFrom(typeof(T)))
                {
                    return (T)(object)stream;
                }

                using (StreamReader sr = new StreamReader(stream))
                {
                    using (JsonTextReader jsonTextReader = new JsonTextReader(sr))
                    {
                        return Serializer.Deserialize<T>(jsonTextReader);
                    }
                }
            }
        }

        public async Task<Crops> GetCropsById(string id)
        {
            string query = $"SELECT * FROM c WHERE c.id = @cropsId";
            QueryDefinition queryDefinition = new QueryDefinition(query).WithParameter("@cropsId", id);
            var streamResultSet = this._container.GetItemQueryIterator<Crops>(queryDefinition);
            List<Crops> results = new();

            while (streamResultSet.HasMoreResults)
            {
                var response = await streamResultSet.ReadNextAsync();

                results.AddRange(response.ToList());
            }
            return results.FirstOrDefault();
        }

        public async Task CreateCrops(Crops crops)
        {
            await this._container.CreateItemAsync<Crops>(crops, new PartitionKey(crops.Id));
        }

        public async Task UpdateCrops(Crops crops)
        {
            await this._container.ReplaceItemAsync<Crops>(crops, crops.Id, new PartitionKey(crops.Id));
        }

        public async Task DeleteCrop(string id)
        {
            await this._container.DeleteItemAsync<Crops>(id, new PartitionKey($"{id}"));
        }
    }
}
