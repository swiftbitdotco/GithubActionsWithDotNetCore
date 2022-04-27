using System;
using System.Linq;
using FluentAssertions;

namespace GithubActions.Tests.Extensions
{
    public static class AssertionExtensions
    {
        public static void ShouldBeDefined<T>(this T theEnum) where T : Enum
        {
            var theType = typeof(T);
            Enum.IsDefined(theType, theEnum).Should().BeTrue($"{theEnum} is not a defined {theType} enum value");
        }

        public static bool ShouldNotHaveAnyNullProperties<T>(this T objectToCheck, params Func<T, object>[] properties)
        {
            if (properties.Any(p => p(objectToCheck) == null))
            {
                return false;
            }
            return true;
        }
    }
}