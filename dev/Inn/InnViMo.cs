using FateExplorer.Shared;
using Aventuria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FateExplorer.Inn;

public enum DishUnit
{
    Jug = 1,    // Krug
    Serving,    // Portion
    TubeCup,    // Schlauchbecher
    Mug,        // Becher
    SmallCup,   // Becherchen
    Tankard,    // Humpen

    Piece,      // Stück
    Portion,
    SmallPortion,
    Stone35
}




public record struct InnMenuItemDTO
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    public DishUnit Unit { get; set; }

    public Food Section { get; set; }
}



public class InnViMo
{
    protected HttpClient DataSource; // inject

    private InnDataM innData;
    private readonly Random RNG = new();

    public InnViMo(HttpClient dataSource)
    {
        DataSource = dataSource;
    }




    /// <summary>
    /// Generate a name for an inn and return it.
    /// </summary>
    /// <param name="region">A region where the inn might be located. Could also be 
    /// used to indicate an "enclave" inn (e.g. the tavern "Old Gear" in Vinsalt).</param>
    /// <param name="Quality">The quality level of the inn between 1-6.</param>
    /// <returns>A string containing an inn name</returns>
    public string GetName(Region region, QualityLevel Quality)
    {
        // Determines the ratio between generated inn names and those taken from the list.
        // 4 indicates a chance of 1:4 for pre-generated:generated names.
        int Ratio = 3;
        int Which = RNG.Next(Ratio);

        if (Which == 0) // Pick a name from the list
        {
            WeightedList<InnNameM> ToPickFrom = new(innData.FullName.Where(i => i.CanBeFound(region)));
            // Set weights
            for(int i = 0; i < ToPickFrom.Count; i++)
                ToPickFrom.SetWeightAt(i, ToPickFrom[i].GetProbability(Quality));
            return ToPickFrom.Random().Name;
        }
        else // Generate a name
        {
            int Pick = RNG.Next(innData.NameBase.Length);
            var Base = innData.NameBase[Pick];
            Pick = RNG.Next(innData.Qualifier.Length);
            var Quali = innData.Qualifier[Pick];

            string Qualifier;
            if (RNG.Next(2) == 0) // no adjective with a 33% chance
                Qualifier = "";
            else
                Qualifier = $"{Quali.Name} ";

            string Prefix, Name;
            if (Quali.NeedPrefix)
                Prefix = $"{Base.Prefix} ";
            else 
                Prefix = "";

            if (Quali.NeedPlural)
                Name = Base.Plural;
            else
                Name = Base.Singular; 

            return $"{Prefix}{Qualifier}{Name}";
        }
    }


    private const int BeverageCount = 3;
    private const int ColdDishes = 5;
    private const int HotDishes = 2;
    private const int Desserts = 1;


    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Member als statisch markieren", Justification = "Not important")]
    public int GetMenuSectionLen(Food food, QualityLevel Quality) => food switch
    {
        Food.Salad => 0, // currently not enough salads in data base
        Food.Alcoholic => BeverageCount + (int)Quality / 2,
        Food.NonAlcoholic => BeverageCount + (int)Quality / 2,
        Food.Dessert => Desserts + (int)Quality / 2,
        Food.MainDish => HotDishes + (int)Quality / 2,  // more QL, more hot dishes
        Food.BreadDish => ColdDishes - (int)Quality / 2, // more QL, fewer cold dishes
        _ => 0
    };

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Member als statisch markieren", Justification = "Not important")]
    private decimal PriceFactor(QualityLevel Quality, PriceLevel Price)
    {
        if (Price == 0) Price = (PriceLevel)Quality;
        return Price switch
        {
            0 => 1.0m, 
            PriceLevel.VeryCheap => 0.5m, PriceLevel.Cheap => 0.75m,
            PriceLevel.Normal => 1, PriceLevel.Expensive => 1.5m,
            PriceLevel.VeryExpensive => 2.0m, PriceLevel.Horrendous => 4.0m,
            _ => throw new ArgumentOutOfRangeException(nameof(Price))
        };

    }


    /// <summary>
    /// Compiles a menu for an inn in the given region and quality level.
    /// </summary>
    /// <param name="region">A region where the inn might be located. Could also be 
    /// used to indicate an "enclave" inn (e.g. the tavern "Old Gear" in Vinsalt).</param>
    /// <param name="Quality">The quality level of the inn between 1-6.</param>
    /// <returns>A list of dishes</returns>
    public List<InnMenuItemDTO> GetMenu(Region region, QualityLevel Quality, PriceLevel Price)
    {
        int Total =  0;
        foreach (Food f in Enum.GetValues<Food>().Cast<Food>()) 
            Total += GetMenuSectionLen(f, Quality);

        List<InnMenuItemDTO> Result = new(Total);
        // Select dishes available in selected region
        WeightedList<InnDishM> ToPickFrom = new(innData.Dish.Where(i => i.CanBeFound(region)));

        // Pick beverages and dishes: cycle through all kinds to get a balanced menu.
        foreach (Food f in Enum.GetValues<Food>().Cast<Food>())
        {
            // ... set weights, first, ...
            for (int i = 0; i < ToPickFrom.Count; i++)
            {
                float Probability;
                if (ToPickFrom[i].Is(f))
                    Probability = ToPickFrom[i].GetProbability(Quality);
                else
                    Probability = 0;
                ToPickFrom.SetWeightAt(i, Probability);
            }
            // ... pick and add to Result.
            for (int dish = 0; dish < GetMenuSectionLen(f, Quality); dish++)
            {
                if (ToPickFrom.TrueCount == 0) break;
                var r = ToPickFrom.Random();
                Result.Add(new()
                {
                    Name = r.Name,
                    Price = r.Price * PriceFactor(Quality, Price),
                    Unit = r.Unit,
                    Section = f
                });
            }
        }

        return Result;
    }


    public InnMenuItemDTO? GetMenuEntry(Region region, QualityLevel Quality, PriceLevel Price, Food food)
    {
        InnMenuItemDTO? Result = null;

        // Select dishes available in selected region
        WeightedList<InnDishM> ToPickFrom = new(innData.Dish.Where(i => i.CanBeFound(region)));

        // ... set weights, first, ...
        for (int i = 0; i < ToPickFrom.Count; i++)
        {
            float Probability;
            if (ToPickFrom[i].Is(food))
                Probability = ToPickFrom[i].GetProbability(Quality);
            else
                Probability = 0;
            ToPickFrom.SetWeightAt(i, Probability);
        }
        // ... pick and add to Result.
        if (ToPickFrom.TrueCount == 0) return null;
        var r = ToPickFrom.Random();
        Result = new()
        {
            Name = r.Name,
            Price = r.Price * PriceFactor(Quality, Price),
            Unit = r.Unit,
            Section = food
        };

        return Result;
    }



    /// <summary>
    /// Load the data
    /// </summary>
    public async Task InitializeGameDataAsync()
    {
        string Language = System.Globalization.CultureInfo.CurrentUICulture.Name;
        if (Language.StartsWith("de"))
            Language = "de";
        else
            Language = "en";


        string fileName = $"data/inns_{Language}.json";
        innData = await DataSource.GetFromJsonAsync<InnDataM>(fileName);
    }
}
