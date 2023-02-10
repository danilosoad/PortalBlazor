using Microsoft.JSInterop;

namespace PortalBlazor.Client.Utils
{
    public static class IJSRuntimeExtensions
    {
        public static ValueTask<object> SetItemInLocalStorage(this IJSRuntime js, string key, string content) => js.InvokeAsync<object>("localStorage.setItem", key, content);

        public static ValueTask<string> GetItemFromLocalStorage(this IJSRuntime js, string key) => js.InvokeAsync<string>("localStorage.getItem", key);

        public static ValueTask<object> RemoveItemInLocalStorage(this IJSRuntime js, string key) => js.InvokeAsync<object>("localStorage.removeItem", key);
    }
}