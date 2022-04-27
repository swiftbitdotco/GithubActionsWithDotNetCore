using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GithubActions.Shared.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<T> GetContentAsAsync<T>(this HttpResponseMessage httpResponseMessage) where T : class
        {
            var contentType = httpResponseMessage.Content.Headers.ContentType?.MediaType;
            if (string.IsNullOrWhiteSpace(contentType))
            {
                throw new HttpResponseMessageExtensionsException("Content Type header should exist in response");
            }

            return await httpResponseMessage.DeserializeToObjectAsync<T>();
        }

        private static async Task<T> DeserializeToObjectAsync<T>(this HttpResponseMessage httpResponseMessage) where T : class
        {
            var contentString = await httpResponseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(contentString);
        }
    }
}