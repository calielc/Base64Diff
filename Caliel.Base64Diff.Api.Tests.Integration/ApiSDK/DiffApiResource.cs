using System;
using System.Text;

namespace Caliel.Base64Diff.Api.Tests.Integration.ApiSDK {
    internal class DiffApiResource : ApiResource {
        public DiffApiResource(string baseUri) : base(baseUri) { }

        public Response<string> PostLeft(string id, PostModel model) => ExecutePost($"{id}/left", model);

        public Response<string> PostRight(string id, PostModel model) => ExecutePost($"{id}/right", model);

        public Response<T> Get<T>(string id) => ExecuteGet<T>(id);

        public struct PostModel {
            public PostModel(string data) {
                Data = data;
            }

            public string Data { get; }

            public static implicit operator PostModel(string text) {
                var bytes = Encoding.UTF8.GetBytes(text);
                var base64String = Convert.ToBase64String(bytes);

                return new PostModel(base64String);
            }
        }

        public struct GetModel {
            public string Situation { get; set; }
            public OffsetLengthModel[] Diffs { get; set; }
        }

        public struct OffsetLengthModel {
            public int Offset { get; set; }
            public int Length { get; set; }
        }
    }
}