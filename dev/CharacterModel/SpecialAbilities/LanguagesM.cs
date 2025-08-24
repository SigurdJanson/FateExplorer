using FateExplorer.Shared;
using System;

namespace FateExplorer.CharacterModel
{

    public class LanguageM : TieredActivatableM
    {
        public LanguageM(string id, int tier, LanguageId language) : base(id, tier, Array.Empty<string>())
        {
            if (id != SA.Language) throw new ArgumentException("Unexpected id for language", id);
            if (tier < 0 || tier > 4) throw new ArgumentException("Language allows skill only between 0-4", nameof(tier));

            Language = language;
        }


        /// <summary>
        /// The language code identifying the particular language
        /// </summary>
        public LanguageId Language { get; protected set; }


        /// <inheritdoc/>
        /// <remarks>Returns true</remarks>
        public override bool IsRecognized => true;


        /// <inheritdoc cref="TieredActivatableM.Apply(CharacterM)"/>
        public override void Apply(CharacterM character)
        {}
    }
}
