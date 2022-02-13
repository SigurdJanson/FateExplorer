using System;


namespace FateExplorer.CharacterModel
{


    public class AbilityM
    {
        public const string COU = "ATTR_1";
        public const string SGC = "ATTR_2";
        public const string INT = "ATTR_3";
        public const string CHA = "ATTR_4";
        public const string DEX = "ATTR_5";
        public const string AGI = "ATTR_6";
        public const string CON = "ATTR_7";
        public const string STR = "ATTR_8";


        public string Id { get; protected set; }

        public string Name { get; protected set; }

        public string ShortName { get; protected set; }

        public int Value { get; protected set; }

        public AbilityM(string id, string name, string shortName, int value)
        {
            Id = id;
            Name = name;
            ShortName = shortName;
            Value = value;
        }

    }


}
