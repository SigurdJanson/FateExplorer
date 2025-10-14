using Aventuria.Calendar;
using System.Globalization;


namespace Aventuria;

/// <summary>
/// <list type="bullet">
/// <item>Provides formats for currencies, dates, etc.</item>
/// <item>Is aware of and recognizes the UI language of the <c>System.Globalization.CultureInfo</c>.</item>
/// <item>Provides terms in the Aventurian languages localized according to the UI language.</item>
/// </list>
/// </summary>
public class DereCultureInfo : IFormatProvider, ICloneable
{
    public required CultureInfo BaseCulture { get; init; }


    #region Get Culture

    // Get the current user default culture. This one is almost always used, so we create it by default.
    private static volatile DereCultureInfo? s_userDefaultCulture;
    // These are defaults that we use if a thread has not opted into having an explicit culture
    private static volatile DereCultureInfo? s_DefaultThreadCurrentCulture;

    [ThreadStatic]
    private static DereCultureInfo? s_currentThreadCulture;

    private static AsyncLocal<DereCultureInfo>? s_asyncLocalCurrentCulture;

    public static DereCultureInfo CurrentCulture
    {
        get
        {
            return s_currentThreadCulture ??
                s_DefaultThreadCurrentCulture ??
                s_userDefaultCulture ??
                InitializeUserDefaultCulture();
        }
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            if (s_asyncLocalCurrentCulture is null)
            {
                Interlocked.CompareExchange(ref s_asyncLocalCurrentCulture, new AsyncLocal<DereCultureInfo>(AsyncLocalSetCurrentCulture), null);
            }
            s_asyncLocalCurrentCulture!.Value = value;
        }
    }


    private static void AsyncLocalSetCurrentCulture(AsyncLocalValueChangedArgs<DereCultureInfo> args)
    {
        s_currentThreadCulture = args.CurrentValue;
    }


    private static DereCultureInfo InitializeUserDefaultCulture()
    {
        Interlocked.CompareExchange(ref s_userDefaultCulture, GetUserDefaultCulture(), null);
        return s_userDefaultCulture!;
    }

    internal static DereCultureInfo GetUserDefaultCulture()
    {
        return GetCultureByName(DefaultDereCultureName);
    }

    /// <summary>
    /// We do this to try to return the system UI language and the default user languages
    /// This method will fallback if this fails (like Invariant)
    /// </summary>
    private static DereCultureInfo GetCultureByName(string name)
    {
        try
        {
            return new DereCultureInfo(name);
        }
        catch (ArgumentException)
        {
            return new DereCultureInfo(DefaultDereCultureName);
        }
    }

    #endregion



    private const string DefaultDereCultureName = "MidRealm";
    //public readonly string[] DereCountryCodes = [DefaultDereCultureName, "HorRealm"];
    private readonly Dictionary<string, string> CultureNames = new()
    {
        { "Andergast", "Andergastan" },
        { "Arania", "Aranian" },
        { "Bornland", "Bornlander" },
        { "Cyclopes", "Cyclopeans" },
        { "ForestFolk", "Forest People" },
        { "Fjarning", "Fjarninger" },
        { "HorRealm", "Horasian Empire" },
        { "Maraskan", "Maraskan" },
        { "Mhanadi", "Mhanadistani" },
        { DefaultDereCultureName, "Middenrealmer" },
        { "Nivese", "Nivese" },
        { "Norbard", "Norbards" },
        { "North", "Northern Aventurian" },
        { "Nostria", "Nostrian" },
        { "Novadi", "Novadi" },
        { "South", "Southern Aventurian" },
        { "Svellt", "Svellter" },
        { "Thorwal", "Thorwaler" },
        { "Utulu", "Utulu" }
    };



    /// <summary>
    /// 
    /// In case <paramref name="name"/> is null 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="dereCountry"></param>
    /// <param name="baseCulture">A string to identify the current <see cref="CultureInfo"/> the application is running in.</param>
    /// <exception cref="CultureNotFoundException"><paramref name="name"/> is not a valid culture name and not null or white space.</exception>
    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public DereCultureInfo(string name, string baseCulture)
    {
        try
        {
            BaseCulture = new CultureInfo(baseCulture);
        }
        catch (Exception)
        {
            baseCulture = string.Empty;
        }
        if (string.IsNullOrWhiteSpace(baseCulture))
            BaseCulture = CultureInfo.CurrentCulture;


        //if (string.IsNullOrWhiteSpace(dereCountry))
        //    Country = DefaultDereCultureName;
        //else
        //{
        //    foreach (string s in DereCountryCodes)
        //    {
        //        if (s == dereCountry)
        //            Country = s;
        //    }
        //}
        //if (string.IsNullOrWhiteSpace(Country))
        //    throw new CultureNotFoundException(nameof(name), "Insufficient culture information");

        // Game-specific information
        Calendar = BaseCulture!.Calendar;
        MoneyFormat = new MoneyFormatter();
        WeightFormat = new WeightFormatter(BaseCulture);
        //NumberFormat = BaseCulture.NumberFormat;  // not necessary
        //TextInfo = BaseCulture.TextInfo;  // not necessary

        // Names
        englishName = CultureNames[Country];
        this.name = englishName;
        displayName = Properties.Resources.ResourceManager.GetString(nameof(Country) + Country) ?? "";
        nativeName = displayName;
    }

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public DereCultureInfo(string name) : this(name, string.Empty) { }



    ///// <inheritdoc />
    //public DereCultureInfo(string name, bool useUserOverride) : base(name, useUserOverride)
    //{
    //    if (string.IsNullOrWhiteSpace(Country))
    //        Country = DefaultDereCultureName;
    //    Calendar = CurrentUICulture.Calendar;
    //    NumberFormat = CurrentUICulture.NumberFormat;
    //    TextInfo = CurrentUICulture.TextInfo;

    //    englishName = CultureNames[Country];
    //    this.name = englishName;
    //    displayName = Properties.Resources.ResourceManager.GetString(nameof(Country) + Country) ?? "";
    //    nativeName = displayName;
    //}



    // public virtual System.Globalization.CultureInfo Parent { get; } // not implemented CultureInfo property


    private readonly string Country; // the country on Dere to overwrite "Erde" cultures


    private readonly string name = "";
    public virtual string Name => name;

    private readonly string displayName = "";
    public string DisplayName => displayName;

    private readonly string englishName = "";
    public string EnglishName => englishName;

    private readonly string nativeName = "";
    public string NativeName => nativeName;

    //public override string TwoLetterISOLanguageName => base.TwoLetterISOLanguageName;

    //public override string ThreeLetterISOLanguageName => base.ThreeLetterISOLanguageName;

    //public override string ThreeLetterWindowsLanguageName => base.ThreeLetterWindowsLanguageName;

    public DateTimeFormatInfo DateTimeFormat => new()
    {
        Calendar = new BosparanCalendar(),
        CalendarWeekRule = CalendarWeekRule.FirstDay,
        FirstDayOfWeek = DayOfWeek.Monday,
        MonthNames =
        [Properties.Resources.Month12GodsName1,
            Properties.Resources.Month12GodsName2,
            Properties.Resources.Month12GodsName3,
            Properties.Resources.Month12GodsName4,
            Properties.Resources.Month12GodsName5,
            Properties.Resources.Month12GodsName6,
            Properties.Resources.Month12GodsName7,
            Properties.Resources.Month12GodsName8,
            Properties.Resources.Month12GodsName9,
            Properties.Resources.Month12GodsName10,
            Properties.Resources.Month12GodsName11,
            Properties.Resources.Month12GodsName12,
            Properties.Resources.Month12GodsName13],
        MonthGenitiveNames = [Properties.Resources.Month12GodsName1,
            Properties.Resources.Month12GodsName2,
            Properties.Resources.Month12GodsName3,
            Properties.Resources.Month12GodsName4,
            Properties.Resources.Month12GodsName5,
            Properties.Resources.Month12GodsName6,
            Properties.Resources.Month12GodsName7,
            Properties.Resources.Month12GodsName8,
            Properties.Resources.Month12GodsName9,
            Properties.Resources.Month12GodsName10,
            Properties.Resources.Month12GodsName11,
            Properties.Resources.Month12GodsName12,
            Properties.Resources.Month12GodsName13],
        AbbreviatedMonthNames =
        [Properties.Resources.Month12GodsAbbr1,
            Properties.Resources.Month12GodsAbbr2,
            Properties.Resources.Month12GodsAbbr3,
            Properties.Resources.Month12GodsAbbr4,
            Properties.Resources.Month12GodsAbbr5,
            Properties.Resources.Month12GodsAbbr6,
            Properties.Resources.Month12GodsAbbr7,
            Properties.Resources.Month12GodsAbbr8,
            Properties.Resources.Month12GodsAbbr9,
            Properties.Resources.Month12GodsAbbr10,
            Properties.Resources.Month12GodsAbbr11,
            Properties.Resources.Month12GodsAbbr12,
            Properties.Resources.Month12GodsAbbr13],
        AbbreviatedMonthGenitiveNames = [],
        DayNames = [Properties.Resources.Day12GodsName1,
            Properties.Resources.Day12GodsName2,
            Properties.Resources.Day12GodsName3,
            Properties.Resources.Day12GodsName4,
            Properties.Resources.Day12GodsName5,
            Properties.Resources.Day12GodsName6,
            Properties.Resources.Day12GodsName7],
        AbbreviatedDayNames = [Properties.Resources.Day12GodsAbbr1,
            Properties.Resources.Day12GodsAbbr2,
            Properties.Resources.Day12GodsAbbr3,
            Properties.Resources.Day12GodsAbbr4,
            Properties.Resources.Day12GodsAbbr5,
            Properties.Resources.Day12GodsAbbr6,
            Properties.Resources.Day12GodsAbbr7],
        ShortestDayNames = [Properties.Resources.Day12GodsAbbr1,
            Properties.Resources.Day12GodsAbbr2,
            Properties.Resources.Day12GodsAbbr3,
            Properties.Resources.Day12GodsAbbr4,
            Properties.Resources.Day12GodsAbbr5,
            Properties.Resources.Day12GodsAbbr6,
            Properties.Resources.Day12GodsAbbr7],
        ShortDatePattern = BaseCulture.DateTimeFormat.ShortDatePattern,
        LongDatePattern = BaseCulture.DateTimeFormat.LongDatePattern,
        ShortTimePattern = BaseCulture.DateTimeFormat.ShortTimePattern,
        AMDesignator = BaseCulture.DateTimeFormat.AMDesignator,
        DateSeparator = BaseCulture.DateTimeFormat.DateSeparator,
        FullDateTimePattern = BaseCulture.DateTimeFormat.FullDateTimePattern,
        LongTimePattern = BaseCulture.DateTimeFormat.LongTimePattern,
        MonthDayPattern = BaseCulture.DateTimeFormat.MonthDayPattern,
        PMDesignator = BaseCulture.DateTimeFormat.PMDesignator,
        TimeSeparator = BaseCulture.DateTimeFormat.TimeSeparator,
        YearMonthPattern = BaseCulture.DateTimeFormat.YearMonthPattern
    };

    /// <summary>
    /// Return/set the default calendar used by this culture.
    /// This value can be overridden by regional option if this is a current culture.
    /// </summary>
    public virtual System.Globalization.Calendar Calendar { get; init; }

    public virtual NumberFormatInfo NumberFormat => BaseCulture.NumberFormat;

    public virtual MoneyFormatter MoneyFormat {  get; init; }

    public virtual WeightFormatter WeightFormat { get; init; }


    public virtual TextInfo TextInfo => BaseCulture.TextInfo;

    public virtual CompareInfo CompareInfo => BaseCulture.CompareInfo;

    /// <summary>
    /// Gets an object that defines how to format the specified type.
    /// </summary>
    /// <param name="formatType">The <see cref="Type"/> for which to get a formatting object. 
    /// This method only supports the types <see cref="NumberFormatInfo"/>, <see cref="DateTimeFormatInfo"/>, 
    /// <see cref="MoneyFormatter"/>, and <see cref="WeightFormatter"/>.</param>
    /// <returns>
    /// The value of the according property to access the format info. If <paramref name="formatType"/> 
    /// is not available a <see cref="DefaultFormatter">default formatter</see> is returned.
    /// </returns>
    public virtual object GetFormat(Type? formatType)
    {
        if (formatType == typeof(NumberFormatInfo))
        {
            return NumberFormat;
        }
        if (formatType == typeof(DateTimeFormatInfo))
        {
            return DateTimeFormat;
        }
        if ((formatType == typeof(MoneyFormatter)))
        {
            return MoneyFormat;
        }
        if ((formatType == typeof(WeightFormatter)))
        {
            return WeightFormat;
        }

        return BaseCulture.GetFormat(formatType) ?? new DefaultFormatter();
    }



    public static DereCultureInfo CreateSpecificCulture(string name)
    {
        return new DereCultureInfo(name, CultureInfo.CurrentCulture.Name);
    }

    public object Clone()
    {
        throw new NotImplementedException();
    }
    /*
CurrencySymbol

CurrentInfo
InvariantInfo

SortableDateTimePattern
UniversalSortableDateTimePattern

GetAbbreviatedDayName
GetAbbreviatedEraName
GetAbbreviatedMonthName
GetAllDateTimePatterns
GetDayName
GetEra
GetEraName
GetFormat
GetInstance
GetMonthName
GetShortestDayName
SetAllDateTimePatterns
DateTimeStyles
DigitShapes
GregorianCalendarTypes
NumberStyles
TimeSpanStyles
UnicodeCategory

Clone
ReadOnly
*/

}
