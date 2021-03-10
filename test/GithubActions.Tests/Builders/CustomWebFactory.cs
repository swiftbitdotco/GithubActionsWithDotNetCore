using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace GithubActions.Tests.Builders
{
    public class CustomWebFactory<TStartUp> : WebApplicationFactory<TStartUp>
        where TStartUp : class
    {
        private readonly List<Func<IServiceCollection, bool>> _replacementFunctions = new List<Func<IServiceCollection, bool>>();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                foreach (var replacementFunction in _replacementFunctions)
                {
                    replacementFunction(services);
                }
            });
        }

        public void WithServiceReplacement(Func<IServiceCollection, bool> replacementFunction)
        {
            _replacementFunctions.Add(replacementFunction);
        }
    }
}