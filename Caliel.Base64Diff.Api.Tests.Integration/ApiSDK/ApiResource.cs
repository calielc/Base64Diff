using RestSharp;

namespace Caliel.Base64Diff.Api.Tests.Integration.ApiSDK {
    internal abstract class ApiResource {
        private readonly RestClient _client;

        protected ApiResource(string baseUri) {
            _client = new RestClient(baseUri);
        }

        protected Response<string> ExecutePost<T>(string url, T model) {
            var request = new RestRequest(url, Method.POST) {
                RequestFormat = DataFormat.Json
            };
            request.AddJsonBody(model);

            var response = _client.Post(request);

            var content = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(response.Content);
            return new Response<string>(response.StatusCode, content);
        }

        protected Response<T> ExecuteGet<T>(string url) {
            var request = new RestRequest(url, Method.GET) {
                RequestFormat = DataFormat.Json
            };

            var response = _client.Get(request);

            var content = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response.Content);
            return new Response<T>(response.StatusCode, content);
        }
    }
}