using Aventuria.Calendar;
using System.Globalization;

namespace Aventuria;

public class DereCultureInfo : CultureInfo
{
    private const string DefaultDereCountry = "MI";
    public readonly string[] DereCountryCodes = ["MI", "HO"];
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
    public DereCultureInfo(string name, string DereCountry = DefaultDereCountry) : base("de-DE")
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

    public override DateTimeFormatInfo DateTimeFormat => new()
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
        ShortDatePattern = "yyyy-MM-dd",
        LongDatePattern = "dddd, MMMM dd, yyyy"
    };
    /*
    AMDesignator
    CurrentInfo
    DateSeparator
    FullDateTimePattern
    InvariantInfo
    IsReadOnly
    LongDatePattern
    LongTimePattern
    MonthDayPattern
    NativeCalendarName
    PMDesignator
    RFC1123Pattern
    ShortDatePattern
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

    public override System.Globalization.Calendar Calendar { get; }
    public override NumberFormatInfo NumberFormat { get; set; }
    //CurrencySymbol

    public override TextInfo TextInfo { get; }
}
