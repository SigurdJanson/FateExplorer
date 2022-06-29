using FateExplorer.Shared.ClientSideStorage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FateExplorer.Shared;



public sealed class AppSettings
{
    // injected values
    readonly IConfiguration Config;
    readonly IClientSideStorage Storage;



    #region Storage

    public sealed class AppSettingsDTO
    {
        [JsonPropertyName("showImprovisedWeapons")]
        public bool? showImprovisedWeapons { get; set; }
        [JsonPropertyName("mostUsedSkills")]
        public List<string> mostUsedSkills { get; set; }
        [JsonPropertyName("defaultCurrency")]
        public string defaultCurrency { get; set; }
    }

    /// <summary>
    /// Try to save the settings on a storage.
    /// </summary>
    /// <returns><c>true</c> if the method finishes successfully; otherwise <c>false</c></returns>
    private bool TryStoreSettings()
    {
        AppSettingsDTO Box = new ();
        Box.showImprovisedWeapons = showImprovisedWeapons; // use field, not property
        Box.defaultCurrency = defaultCurrency;
        Box.mostUsedSkills = mostUsedSkills;

        try
        {
            Storage.Store(nameof(AppSettings), Box);
        }
        catch (Exception) { return false; }
        return true;
    }



    /// <summary>
    /// Tries to access the settings storage and read previously stored settings.
    /// </summary>
    /// <returns></returns>
    public async Task RestoreSavedState()
    {
        AppSettingsDTO Box;

        try
        {
            Box = await Storage.Retrieve<AppSettingsDTO>(nameof(AppSettings), null) ?? null;
        }
        catch (Exception) { return; }

        showImprovisedWeapons = Box?.showImprovisedWeapons; // use field, not property
        defaultCurrency = Box?.defaultCurrency;
        mostUsedSkills = Box?.mostUsedSkills;
    }

    #endregion



    private bool? showImprovisedWeapons;
    public bool ShowImprovisedWeapons
    {
        get 
        {   
            if (!showImprovisedWeapons.HasValue) // get default
                showImprovisedWeapons = Config.GetValue<bool>("FE:Weapons:ShowImprovisedWeapons");
            return showImprovisedWeapons.Value;
        }
        set 
        {
            if (showImprovisedWeapons == value) return;
            showImprovisedWeapons = value;
            TryStoreSettings();
        }
    }


    private List<string> mostUsedSkills;
    /// <summary>
    /// A list of skill id's. These skills are shown in the characters'
    /// skill sheets as "Most used".
    /// </summary>
    public List<string> MostUsedSkills
    {
        get 
        { 
            if (mostUsedSkills is null) // get default
                mostUsedSkills = Config.GetSection("FE:Skills:MostUsedSkills").Get<List<string>>();
            return mostUsedSkills; 
        }
        set 
        {
            if (mostUsedSkills == value) return;
            mostUsedSkills = value;
            TryStoreSettings();
        }
    }


    private string defaultCurrency;
    /// <summary>
    /// Get the id of the default currency
    /// </summary>
    public string DefaultCurrency
    {
        get
        {
            if (defaultCurrency is null) // get default
                defaultCurrency = Config.GetValue<string>("FE:DefaultCurrency");
            return defaultCurrency;
        }
        set 
        {
            if (defaultCurrency == value) return;
            defaultCurrency = value;
            TryStoreSettings();
        }
    }




    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="config">Configuration service to access appsettings.json; injected</param>
    /// <param name="storage">Storage service; injected</param>
    public AppSettings(IConfiguration config, IClientSideStorage storage)
    {
        Config = config;
        Storage = storage;
    }
}
