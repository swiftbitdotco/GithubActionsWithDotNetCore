using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GithubActions.Shared.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<T> GetContentAsAsync<T>(this HttpResponseMessage httpResponseMessage) where T : class
        {
            var contentType = httpResponseMessage.Content.Headers.ContentType.MediaType;
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

        public static async Task<string> TryGetPrettyPrintJson(this HttpContent httpContent)
        {
            var rawJson = await httpContent.ReadAsStringAsync();

            return rawJson.TryGetPrettyPrintJson();
        }

        private static string TryGetPrettyPrintJson(this string rawJson)
        {
            try
            {
                return JToken.Parse(rawJson).ToString(Formatting.Indented);
            }
            catch
            {
                return rawJson;
            }
        }

        public static async Task<string> TryGetPrettyPrintXml(this HttpContent httpContent)
        {
            var rawXml = await httpContent.ReadAsStringAsync();

            return rawXml.TryGetPrettyPrintXml();
        }

        private static string TryGetPrettyPrintXml(this string rawXml)
        {
            try
            {
                return XDocument.Parse(rawXml).ToString();
            }
            catch
            {
                return rawXml;
            }
        }
    }
}