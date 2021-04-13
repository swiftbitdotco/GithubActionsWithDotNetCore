using System;

namespace GithubActions.Shared.Extensions
{
    [Serializable]
    public class HttpResponseMessageExtensionsException : Exception
    {
        public HttpResponseMessageExtensionsException(string message) : base(message)
        {
        }
    }
}