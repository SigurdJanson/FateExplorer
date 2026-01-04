namespace FateExplorer.CharacterModel;



public class AbilityM : RootValue
{
    public const string COU = Shared.ChrAttrId.AbilityBaseId + "_1";
    public const string SGC = Shared.ChrAttrId.AbilityBaseId + "_2";
    public const string INT = Shared.ChrAttrId.AbilityBaseId + "_3";
    public const string CHA = Shared.ChrAttrId.AbilityBaseId + "_4";
    public const string DEX = Shared.ChrAttrId.AbilityBaseId + "_5";
    public const string AGI = Shared.ChrAttrId.AbilityBaseId + "_6";
    public const string CON = Shared.ChrAttrId.AbilityBaseId + "_7";
    public const string STR = Shared.ChrAttrId.AbilityBaseId + "_8";


    public string ShortName { get; protected set; }

    public AbilityM(string id, string name, string shortName, int value) : base(value)
    {
        Id = id;
        Name = name;
        ShortName = shortName;
        //Value = value; // done in base class
    }

}
