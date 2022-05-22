namespace FateExplorer.Shared.ClientSideStorage;

public interface IClientSideStorage
{
    /// <summary>
    /// Write a value into storage.
    /// </summary>
    /// <param name="key">A key to identify (and later retrieve) the stored data.</param>
    /// <param name="value">The value to store.</param>
    /// <param name="days">Set an expiration date as number of days the cookie should survive. 
    /// If <c>null</c>, provide a default.</param>
    /// <returns>-</returns>
    public Task Store(string key, string value, int? days = null);



    /// <summary>
    /// Put an object into storage.
    /// </summary>
    /// <typeparam name="T">Any serialisable type; preferably a mere data transfer object.</typeparam>
    /// <inheritdoc cref = "Store" />
    public Task Store<T>(string key, T value, int? days = null);



    /// <summary>
    /// Retrieve a single value from storage.
    /// </summary>
    /// <param name="key">A key to identify and retrieve the stored data.</param>
    /// <param name="defaultVal">The default value to be returned if <see cref="key"/> is 
    /// not found. Default of the default is an empty string.</param>
    /// <returns>The stored value as string</returns>
    public Task<string> Retrieve(string key, string? defaultVal = "");



    /// <summary>
    /// Retrieve an object from storage.
    /// </summary>
    /// <typeparam name="T">Any serialisable type; preferably a mere data transfer object.</typeparam>
    /// <returns>The stored object</returns>
    /// <inheritdoc cref = "Retrieve" />
    public Task<T?> Retrieve<T>(string key, T? defaultVal);



    /// <summary>
    /// Remove value from storage
    /// </summary>
    /// <param name="key">The key to identify the value</param>
    /// <returns></returns>
    public Task Delete(string key);



    /// <summary>
    /// Check if value exists
    /// </summary>
    /// <param name="key">A key to identify the stored data.</param>
    /// <returns><c>true</c> if it exists; otherwise <c>false</c>.</returns>
    public Task<bool> Exists(string key);
}
