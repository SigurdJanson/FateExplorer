namespace FateExplorer.Shared
{
    /// <summary>
    /// Supplies the application with id strings to identify the character's
    /// attributes. Mostly taken from the core rules VR1
    /// </summary>
    /// <remarks>
    /// For internal use. Though these strings often match the published abbreviations,
    /// these shall not be used in the UI.
    /// </remarks>
    public static class ChrAttrId
    {
        /// <summary>
        /// Character's initiative value
        /// </summary>
        public const string INI = "INI";
        /// <summary>
        /// Character's movement value
        /// </summary>
        public const string MOV = "MOV";
        /// <summary>
        /// Character's encumbrance value
        /// </summary>
        public const string ENC = "ENC";

        /// <summary>
        /// Character's dodge value or a dodge action
        /// </summary>
        public const string DO = "DO";
        /// <summary>
        /// Identifies an attack
        /// </summary>
        public const string AT = "AT";
        /// <summary>
        /// Identifies a parry
        /// </summary>
        public const string PA = "PA";


        /// <summary>
        /// Character's toughness value (Zähigkeit)
        /// </summary>
        public const string TOU = "TOU";
        /// <summary>
        /// Character's spirit value (Seelenkraft)
        /// </summary>
        public const string SPI = "SPI";



        public const string Regenerate = "REGENERATE";

        /// <summary>
        /// Character's life points (energy): Id
        /// </summary>
        public const string LP = "LP";
        /// <summary>
        /// Character's arcane energy points (energy): Id
        /// </summary>
        public const string AE = "AE";
        /// <summary>
        /// Character's karma points (energy): Id
        /// </summary>
        public const string KP = "KP";

        /// <summary>
        /// Abilities are numbered
        /// </summary>
        public const string AbilityBaseId = "ATTR";

        public const string CombatTecBaseId = "CT";

        public const string Routine = "RC";

        public const string Skill = "TAL";
        public const string Spell = "SPELL";
        public const string Liturgy = "LITURGY";
    }
}
