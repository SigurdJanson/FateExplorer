namespace Aventuria;

public class DereCultureData : IComparable<DereCultureData>
{
    public string Code { get; init; }
    public string Name { get; init; }
    public string EnglishName { get; init; }
    public string NativeName { get; init; }
    public string ParentCode { get; init; }
    public DereCultureData Parent { get; set; }
    public bool IsHuman { get; }
    public bool IsPlayable { get; }

    private DereCultureData() { }

    public static DereCultureData Create(string code)
    {
        return new DereCultureData()
        {
            Code = code,
            Name = "code",
            EnglishName = "code",
            NativeName = "",
            ParentCode = "code",
        };
    }


    // Optional: lazy children loading if you ever need it
    //public Func<Task<IEnumerable<DereCultureData>>>? LoadChildrenAsync { get; set; }

    public override string ToString() => $"{Name} ({Code})";

    // implements IComparable
    public int CompareTo(DereCultureData? other)
    {
        return Name.CompareTo(other?.Name);
    }
}