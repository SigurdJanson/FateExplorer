using Microsoft.JSInterop;
using System;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FateExplorer.Shared.ClientSideStorage;



/// <summary>
/// Provides a client side storage using browser cookies.
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
    /// Set the default lifetime of stored data by the number of days it shall survive.
    /// </summary>
    public int ExpireDays
    {
        set => expires = DateToUTC(value);
    }


    /// <param name="days">Set the number of days the cookie should survive. 
    /// If <c>null</c> use the default <see cref="DefaultExpirationDays"/>.</param>
    /// <exception cref="ArgumentException"/>
    /// <remarks>Each value is a cookie. Note that the number of cookies per site is restricted.</remarks>
    /// <inheritdoc/>
    public async Task Store(string key, string value, int? days = null)
    {
        if (HasNonASCIIChars(key)) 
            throw new ArgumentException("Only ascii characters are allowed in key string", nameof(key));

        string EncValue = Uri.EscapeDataString(value);
        var curExp = (days != null) ? (days > 0 ? DateToUTC(days.Value) : "") : expires;
        await SetCookie($"{key}={EncValue}; expires={curExp}; path=/");
    }


    /// <inheritdoc/>
    public async Task Store<T>(string key, T value, int? days = null)
    {
        string jsonString = JsonSerializer.Serialize(value);
        string ValueB64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonString));

        await Store(key, ValueB64, days);
    }



    /// <inheritdoc/>
    public async Task<string> Retrieve(string key, string? defaultVal = "")
    {
        var cValue = await GetCookie();
        if (string.IsNullOrEmpty(cValue)) return defaultVal ?? string.Empty;

        var vals = cValue.Split(';');
        foreach (var val in vals)
            if (!string.IsNullOrEmpty(val) && val.IndexOf('=') > 0)
                if (val[..val.IndexOf('=')].Trim().Equals(key, StringComparison.OrdinalIgnoreCase))
                    return Uri.UnescapeDataString(val[(val.IndexOf('=') + 1)..]);

        return defaultVal ?? string.Empty;
    }



    /// <exception cref="JsonException" />
    /// <inheritdoc/>
    public async Task<T?> Retrieve<T>(string key, T? defaultVal = default)
    {
        string resultStr = await Retrieve(key, string.Empty);
        if (string.IsNullOrEmpty(resultStr)) return defaultVal;

        string jsonString = Encoding.UTF8.GetString(Convert.FromBase64String(resultStr));
        T? result = JsonSerializer.Deserialize<T>(jsonString);
        if (result is null) throw new JsonException("Stored data cannot be deserialised");
        return result;
    }



     //   string ascii = "{\"a\": 1.2, \"string\": \"abc&Ω\"}";
     //   string b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(ascii));
     //   Console.WriteLine(b64);
     //   Console.WriteLine(Encoding.UTF8.GetString(Convert.FromBase64String(b64)));


    /// <inheritdoc/>
    public async Task Delete(string key)
    {
        // Delete cookie by setting max-age to a sec ago
        // "document.cookie = "user=John; max-age=-1"";
        await SetCookie($"{key}=; max-age=-1");
    }


    /// <inheritdoc/>
    public async Task<bool> Exists(string key)
    {
        return await Retrieve(key, null) is not null;
    }


    // HELPERS

    private async Task SetCookie(string value)
        => await JSRuntime.InvokeVoidAsync("eval", $"document.cookie = \"{value}\"");

    private async Task<string> GetCookie()
        => await JSRuntime.InvokeAsync<string>("eval", $"document.cookie");


    private static string DateToUTC(int days) => DateTime.Now.AddDays(days).ToUniversalTime().ToString("R");

    private static bool HasNonASCIIChars(string str)
    {
        return System.Text.Encoding.UTF8.GetByteCount(str) != str.Length;
    }
}

