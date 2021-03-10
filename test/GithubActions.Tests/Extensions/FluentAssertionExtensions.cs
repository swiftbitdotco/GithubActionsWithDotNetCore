using System;
using System.Linq;

namespace GithubActions.Tests.Extensions
{
    public static class FluentAssertionExtensions
    {
        public static bool CheckObjectPropertiesAreNotNull<T>(this T objectToCheck, params Func<T, object>[] properties)
        {
            if (properties.Any(p => p(objectToCheck) == null))
            {
                return false;
            }
            return true;
        }
    }
}