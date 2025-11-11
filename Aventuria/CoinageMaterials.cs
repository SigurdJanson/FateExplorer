using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aventuria;

internal class CoinageMaterials : Enumeration
{
    [Flags]
    public enum Materials
    {
        Gold = 1,
        Silver = 2,
        Copper = 4,
        Iron = 8,
        Bronze = 16,
        Brass = 32,
        Electrum = 64,
        Platinum = 128,
        Lead = 256
    }

    public Money Worth { get; init; }

    public CoinageMaterials(string name, Materials @enum) : base(name, (int)@enum)
    {}


    public static readonly CoinageMaterials Gold = new(nameof(Gold), Materials.Gold)
    {
        Worth = new Money(50m / 1000m, Currency.MiddenrealmDucat)
    };
    public static readonly CoinageMaterials Silver = new(nameof(Silver), Materials.Silver)
    {
        Worth = new Money(25m / 1000m, Currency.MiddenrealmDucat)
    };
    public static readonly CoinageMaterials Copper = new(nameof(Copper), Materials.Copper)
    {
        Worth = new Money(3m / 1000m, Currency.MiddenrealmDucat)
    };
    public static readonly CoinageMaterials Iron = new(nameof(Iron), Materials.Iron) 
    { 
        Worth = new Money(0.8m / 1000m, Currency.MiddenrealmDucat) 
    };
    public static readonly CoinageMaterials Bronze = new(nameof(Bronze), Materials.Bronze)
    {
        Worth = new Money(4m / 1000m, Currency.MiddenrealmDucat)
    };
    public static readonly CoinageMaterials Brass = new(nameof(Brass), Materials.Brass)
    {
        Worth = new Money(4m / 1000m, Currency.MiddenrealmDucat)
    };
    public static readonly CoinageMaterials Electrum = new(nameof(Electrum), Materials.Electrum)
    {
        Worth = new Money(30m / 1000m, Currency.MiddenrealmDucat)
    };
    public static readonly CoinageMaterials Platinum = new(nameof(Platinum), Materials.Platinum)
    {
        Worth = new Money(75m / 1000m, Currency.MiddenrealmDucat)
    };
    public static readonly CoinageMaterials Lead = new(nameof(Lead), Materials.Lead)
    {
        Worth = new Money(1m / 1000m, Currency.MiddenrealmDucat)
    };

    public static CoinageMaterials Alloy(string name, string[] materials)
    {
        int @enum = 0;
        var all = Enum.GetValues<Materials>().ToString();
        foreach (var m in materials)
            if (all?.Contains(m) ?? false)
                @enum |= (int)Enum.Parse<Materials>(m);
        return new CoinageMaterials(name, (Materials)@enum);
    }

    public static CoinageMaterials Alloy(string name, Materials[] materials)
    {
        Materials @enum = 0;
        var all = Enum.GetValues<Materials>();
        foreach (var m in materials)
            if (all?.Contains(m) ?? false)
                @enum |= m;
        return new CoinageMaterials(name, (Materials)@enum);
    }
}
