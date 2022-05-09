﻿using System.Threading.Tasks;

namespace FateExplorer.Shared.ClientSideStorage;

public interface IClientSideStorage
{
    /// <summary>
    /// Write a value into storage. Overwrites existing values.
    /// </summary>
    /// <param name="key">A key to identify (and later retrieve) the stored data.</param>
    /// <param name="value">The value to store.</param>
    /// <param name="days">Set an expiration date as number of days the cookie should survive. 
    /// If <c>null</c>, provide a default.</param>
    /// <returns>-</returns>
    public Task SetValue(string key, string value, int? days = null);


    /// <summary>
    /// Retrieve a single value from storage.
    /// </summary>
    /// <param name="key">A key to identify and retrieve the stored data.</param>
    /// <param name="defaultVal">The default value to be returned if <see cref="key"/> is 
    /// not found. Default of the default is an empty string.</param>
    /// <returns>The stored value as string</returns>
    public Task<string> GetValue(string key, string defaultVal = "");
}