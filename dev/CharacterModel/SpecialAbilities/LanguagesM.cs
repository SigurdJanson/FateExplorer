﻿using System;

namespace FateExplorer.CharacterModel.SpecialAbilities
{
    public enum LanguageId
    {
        Alaani = 1,
        Angram = 2,
        Asdharia = 3,
        Atak = 4,
        Aureliani = 5,
        Bosparano = 6,
        Fjarningsch = 7,
        Garethi = 8,
        Goblinisch = 9,
        Isdira = 10,
        Mohisch = 11,
        Nujuka = 12,
        Ogrisch = 13,
        Oloarkh = 14,
        Ologhaijan = 15,
        Rabensprache = 16,
        Rogolan = 17,
        Rssahh = 18,
        Ruuz = 19,
        SagaThorwalsch = 20,
        Thorwalsch = 21,
        Trollisch = 22,
        Tulamidya = 23,
        UrTulamidya = 24,
        Zelemja = 25,
        Zhayad = 26,
        Zyklopäisch = 27,
        Dschuku = 49,
        Pardiral = 72
    }


    public class LanguageM : ISpecialAbilityM
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
    }
}
