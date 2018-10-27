using System.Threading.Tasks;
using RestSharp;
using RestSharp.Serializers.Newtonsoft.Json;
using RestRequest = RestSharp.RestRequest;

namespace PonyChallenge.Api
{
    public class PonyApi
    {
        private const string ApiRoot = @"https://ponychallenge.trustpilot.com/pony-challenge/maze";

        private readonly RestClient _client;

        public PonyApi()
        {
            _client = new RestClient(ApiRoot);
            _client.AddHandler("application/json", new NewtonsoftJsonSerializer());
        }

        public async Task<string> CreateNewMaze(CreateNewMazeCommand command)
        {
            var request = new RestRequest(Method.POST)
            {
                JsonSerializer = new NewtonsoftJsonSerializer()
            }.AddJsonBody(command);

            dynamic response = await _client.PostAsync<object>(request);
            return response["maze_id"];
        }

        public async Task<MazeDto> GetMaze(string mazeId)
        {
            var request = new RestRequest("{mazeId}", Method.GET)
                {
                    JsonSerializer = new NewtonsoftJsonSerializer(),
                }
                .AddParameter("mazeId", mazeId, ParameterType.UrlSegment);

            return await _client.GetAsync<MazeDto>(request);
        }

        public async Task<string> Move(string mazeId, Direction direction)
        {
            var request = new RestRequest("{mazeId}", Method.POST)
                {
                    JsonSerializer = new NewtonsoftJsonSerializer(),
                }
                .AddParameter("mazeId", mazeId, ParameterType.UrlSegment)
                .AddJsonBody(direction);

            dynamic response = await _client.PostAsync<object>(request);
            return response["state"];
        }

        public async Task<string> Preview(string mazeId)
        {
            var request = new RestRequest("{mazeId}/print", Method.GET)
                {
                    JsonSerializer = new NewtonsoftJsonSerializer(),
                }
                .AddParameter("mazeId", mazeId, ParameterType.UrlSegment);

            var response = await _client.ExecuteGetTaskAsync(request);
            return response.Content;
        }
    }
}
