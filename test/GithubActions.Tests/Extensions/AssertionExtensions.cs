using System;
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
    }
}