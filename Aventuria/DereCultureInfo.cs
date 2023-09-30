using System.Globalization;

namespace Aventuria;

public class DereCultureInfo : CultureInfo
{
    private const string DefaultDereCountry = "MI";
    public readonly string[] DereCountryCodes = new[] { "MI", "HO" };
    private readonly Dictionary<string, string> CountryNames = new()
    {
        { "MI", "Middenrealm" },
        { "HO", "Horasian Empire" }
    };

    /// <inheritdoc />
    /// <remarks>Not available for Dere cultures.</remarks>
    public DereCultureInfo(int culture) : base(culture) =>
        throw new NotImplementedException("Dere cultures cannot be initialised by LCID.");

    /// <inheritdoc />
    public DereCultureInfo(string name, string DereCountry = DefaultDereCountry) : base(name)
    {
        foreach (string s in DereCountryCodes)
        {
            if (s == DereCountry)
                Country = s;
        }

        if (string.IsNullOrWhiteSpace(Country))
            Country = DefaultDereCountry;
        if (string.IsNullOrWhiteSpace(Country))
            throw new ArgumentException("Insufficient culture information", nameof(name));
        Calendar = CurrentUICulture.Calendar;
        NumberFormat = CurrentUICulture.NumberFormat;
        TextInfo = CurrentUICulture.TextInfo;

        englishName = CountryNames[Country];
        this.name = englishName;
        displayName = Properties.Resources.ResourceManager.GetString(nameof(Country) + Country) ?? "";
        nativeName = displayName;
    }

    /// <inheritdoc />
    /// <remarks>Not available for Dere cultures.</remarks>
    public DereCultureInfo(int culture, bool useUserOverride) : base(culture, useUserOverride) =>
        throw new NotImplementedException("Dere cultures cannot be initialised by LCID.");

    /// <inheritdoc />
    public DereCultureInfo(string name, bool useUserOverride) : base(name, useUserOverride)
    {
        if (string.IsNullOrWhiteSpace(Country))
            Country = DefaultDereCountry;
        Calendar = CurrentUICulture.Calendar;
        NumberFormat = CurrentUICulture.NumberFormat;
        TextInfo = CurrentUICulture.TextInfo;

        englishName = CountryNames[Country];
        this.name = englishName;
        displayName = Properties.Resources.ResourceManager.GetString(nameof(Country) + Country) ?? "";
        nativeName = displayName;
    }


    private readonly string Country; // the country on Dere to overwrite "Erde" cultures


    private readonly string name = "";
    public override string Name => string.IsNullOrEmpty(name) ? base.Name : name;

    private readonly string displayName = "";
    public override string DisplayName => string.IsNullOrEmpty(displayName) ? base.DisplayName : displayName;

    private readonly string englishName = "";
    public override string EnglishName => string.IsNullOrEmpty(englishName) ? base.EnglishName : englishName;

    private readonly string nativeName = "";
    public override string NativeName => string.IsNullOrEmpty(nativeName) ? base.NativeName : nativeName;

    //public override string TwoLetterISOLanguageName => base.TwoLetterISOLanguageName;

    //public override string ThreeLetterISOLanguageName => base.ThreeLetterISOLanguageName;

    //public override string ThreeLetterWindowsLanguageName => base.ThreeLetterWindowsLanguageName;

    public override DateTimeFormatInfo DateTimeFormat => base.DateTimeFormat;
    /*
    Calendar

    AbbreviatedDayNames
    AbbreviatedMonthGenitiveNames
    AbbreviatedMonthNames
    AMDesignator
    CalendarWeekRule
    CurrentInfo
    DateSeparator
    DayNames
    FirstDayOfWeek
    FullDateTimePattern
    InvariantInfo
    IsReadOnly
    LongDatePattern
    LongTimePattern
    MonthDayPattern
    MonthGenitiveNames
    MonthNames
    NativeCalendarName
    PMDesignator
    RFC1123Pattern
    ShortDatePattern
    ShortestDayNames
    ShortTimePattern
    SortableDateTimePattern
    TimeSeparator
    UniversalSortableDateTimePattern
    YearMonthPattern
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
    --Clone
    --ReadOnly
    */

    public override Calendar Calendar { get; }
    public override NumberFormatInfo NumberFormat { get; set; }
    //CurrencySymbol

    public override TextInfo TextInfo { get; }
}
