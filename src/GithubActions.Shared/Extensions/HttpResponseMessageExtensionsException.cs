using System;

namespace GithubActions.Shared.Extensions
{
    public class HttpResponseMessageExtensionsException : Exception
    {
        public HttpResponseMessageExtensionsException(string message) : base(message)
        {
        }
    }
}