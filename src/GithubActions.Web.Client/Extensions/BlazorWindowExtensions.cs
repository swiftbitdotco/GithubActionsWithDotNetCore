using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GithubActions.Web.Client.Extensions
{
    public static class BlazorWindowExtensions
    {
        public static async Task FocusElementByIdAsync(this IJSRuntime jsRuntime, string elementId)
        {
            await jsRuntime.InvokeVoidAsync("blazorExtensions.FocusElementById", elementId);
        }

        public static ValueTask FocusElementAsync(this IJSRuntime jsRuntime, ElementReference elementReference)
        {
            return jsRuntime.InvokeVoidAsync("blazorExtensions.FocusElement", elementReference);
        }
    }
}