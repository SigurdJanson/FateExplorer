using FateExplorer.Shared.ClientSideStorage;
using System;
using System.Threading.Tasks;

namespace FateExplorer.Calendar;

public class DateOfPlayM : IDateOfPlay
{
    protected IClientSideStorage Storage; // injected storage

    /// <inheritdoc/>
    public event Action OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();  // this method hides the OnChange to simplify it


    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="storage">A storage to keep the date across sessions</param>
    public DateOfPlayM(IClientSideStorage storage) : this(DateTime.Now, storage) 
    {}


    /// <summary>
    /// Date constructor
    /// </summary>
    /// <param name="date">A date to be initialised with</param>
    /// <param name="storage">A storage to keep the date across sessions</param>
    public DateOfPlayM(DateTime date, IClientSideStorage storage)
    {
        Storage = storage; // injection
        this.date = date;  // use field to avoid overwriting the saved date
    }


    /// <inheritdoc/>
    public async Task RestoreSavedState()
    {
        string StoredItem = await Storage.Retrieve(GetType().ToString());
        if (!DateTime.TryParse(StoredItem, out date))
            date = DateTime.Now;
        NotifyStateChanged();
    }


    private DateTime date;

    /// <inheritdoc/>
    public DateTime Date 
    { 
        get => date; 
        set
        {
            if (date == value) return;
            date = value;
            Storage.Store(GetType().ToString(), date.ToString());
            NotifyStateChanged();
        }
    }
}
