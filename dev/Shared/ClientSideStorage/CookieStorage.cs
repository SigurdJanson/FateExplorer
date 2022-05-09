using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace FateExplorer.Shared.ClientSideStorage;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// Adapted from <see href="https://stackoverflow.com/a/69873060/13241545">create a client-side cookie</see>
/// from <see href="https://stackoverflow.com/users/1358148/mohsenb">MohsenB</see> 
/// </remarks>
public class CookieStorage : IClientSideStorage
{
    private readonly IJSRuntime JSRuntime;
    private const int DefaultExpirationDays = 300;
    string expires = "";


    public CookieStorage(IJSRuntime jsRuntime)
    {
        JSRuntime = jsRuntime;
        ExpireDays = DefaultExpirationDays;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="days">Set the number of days the cookie should survive. 
    /// If <c>null</c> use the default <see cref="DefaultExpirationDays"/>.</param>
    /// <returns></returns>
    public async Task SetValue(string key, string value, int? days = null)
    {
        var curExp = (days != null) ? (days > 0 ? DateToUTC(days.Value) : "") : expires;
        await SetCookie($"{key}={value}; expires={curExp}; path=/");
    }

    public async Task<string> GetValue(string key, string def = "")
    {
        var cValue = await GetCookie();
        if (string.IsNullOrEmpty(cValue)) return def;

        var vals = cValue.Split(';');
        foreach (var val in vals)
            if (!string.IsNullOrEmpty(val) && val.IndexOf('=') > 0)
                if (val.Substring(0, val.IndexOf('=')).Trim().Equals(key, StringComparison.OrdinalIgnoreCase))
                    return val.Substring(val.IndexOf('=') + 1);
        return def;
    }

    private async Task SetCookie(string value)
    {
        await JSRuntime.InvokeVoidAsync("eval", $"document.cookie = \"{value}\"");
    }

    private async Task<string> GetCookie()
    {
        return await JSRuntime.InvokeAsync<string>("eval", $"document.cookie");
    }

    /// <summary>
    /// Set the lifetime of the cookie by the number of days it shall survive.
    /// </summary>
    public int ExpireDays
    {
        set => expires = DateToUTC(value);
    }

    private static string DateToUTC(int days) => DateTime.Now.AddDays(days).ToUniversalTime().ToString("R");
}

