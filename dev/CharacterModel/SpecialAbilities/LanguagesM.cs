using FateExplorer.Shared;
using System;

namespace FateExplorer.CharacterModel
{

    public class LanguageM : IActivatableM
    {
        const string ExpectedId = "SA_29";

        public LanguageM(string id, int tier, LanguageId language)
        {
            if (id is null) throw new ArgumentNullException(nameof(id));
            if (id != ExpectedId) throw new ArgumentException("Special ability is not a language", nameof(id));
            if (tier < 0 || tier > 4) throw new ArgumentException("Language allows skill only between 0-4", nameof(tier));

            Id = id;
            Tier = tier;
            Language = language;
        }


        /// <inheritdoc/>
        public string Id { get; protected set; }


        /// <inheritdoc/>
        /// <remarks>The skill; 4 is a native language.</remarks>
        public int Tier { get; protected set; }


        /// <summary>
        /// The language code identifying the particular language
        /// </summary>
        public LanguageId Language { get; protected set; }


        /// <inheritdoc/>
        /// <remarks>Returns true</remarks>
        public bool IsRecognized => true;

        /// <inheritdoc />
        public string[] Reference => throw new NotImplementedException("References are not implemented for languages");
    }
}
