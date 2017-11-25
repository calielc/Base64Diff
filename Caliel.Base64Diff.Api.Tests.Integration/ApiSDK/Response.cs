using System.Net;

namespace Caliel.Base64Diff.Api.Tests.Integration.ApiSDK {
    public class Response<T> {
        public Response(HttpStatusCode statusCode, T content) {
            StatusCode = statusCode;
            Content = content;
        }


        public HttpStatusCode StatusCode { get; }

        public T Content { get; }
    }
}
