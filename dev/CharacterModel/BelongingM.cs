
using FateExplorer.Shared;

namespace FateExplorer.CharacterModel;

public class BelongingM
{
    /// <summary>
    /// The item id in the characters' file specific to the belongings of the character..
    /// </summary>
    /// <remarks>Not a universal id (like a data base or shop id).</remarks>
    public string Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Amount
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// Item weight in stone per piece. 1 stone are 2 pounds.
    /// </summary>
    public decimal Weight { get; set; }

    /// <summary>
    /// Typical price of the item in Silverthalers. 
    /// Price is per item. Total price must be mulitplied with <see cref="Amount"/>
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// The location where the character wears/carries this item.
    /// </summary>
    public string Where { get; set; }

    /// <summary>
    /// An indicator what type of object the belonging is.
    /// </summary>
    public GroupId Group { get; set; }
}