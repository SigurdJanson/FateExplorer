using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FateExplorer.CharacterData
{
    public class CharacterImportOptM : ICharacterImporter
    {
        #region Optolith
        [JsonPropertyName("clientVersion")]
        public string ClientVersion { get; set; }

        [JsonPropertyName("dateCreated")]
        public DateTime DateCreated { get; set; }

        [JsonPropertyName("dateModified")]
        public DateTime DateModified { get; set; }

        [JsonPropertyName("id")]
        public string CharacterId { get; set; }

        [JsonPropertyName("phase")]
        public int Phase { get; set; }

        [JsonPropertyName("rules")]
        public RulesOptM Rules { get; set; }

        #endregion



        [JsonPropertyName("locale")]
        public string Locale { get; set; }



        #region Character Data =============
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Experience or adventure points
        /// </summary>
        [JsonPropertyName("ap")]
        public ExperiencePointsOpt Ap { get; set; }

        [JsonPropertyName("el")]
        public string El { get; set; }

        /// <summary>
        /// Race, e.g. human, elf
        /// </summary>
        [JsonPropertyName("r")]
        public string R { get; set; }

        /// <summary>
        /// Race variant, e.g. Thorwalian for a human or wood elf for an elf.
        /// </summary>
        [JsonPropertyName("rv")]
        public string Rv { get; set; }

        /// <summary>
        /// Culture
        /// </summary>
        [JsonPropertyName("c")]
        public string C { get; set; }

        /// <summary>
        /// Profession
        /// </summary>
        [JsonPropertyName("p")]
        public string P { get; set; }

        /// <summary>
        /// Profession variation, e.g. a golgarithian for a boronian priest
        /// </summary>
        [JsonPropertyName("pv")]
        public string PV { get; set; }

        /// <summary>
        ///  Gender
        /// </summary>
        [JsonPropertyName("sex")]
        public string Sex { get; set; }

        [JsonPropertyName("pers")]
        public PersonOpt Pers { get; set; }

        /// <summary>
        /// Container for several character attributes
        /// </summary>
        [JsonPropertyName("attr")]
        public Attributes Attr { get; set; }

        [JsonIgnore, JsonPropertyName("activatable")] // TODO: is currently ignored
        public Activatable Activatable { get; set; }

        [JsonPropertyName("talents"), JsonConverter(typeof(JsonOptSkillsConverter))]
        public Dictionary<string, int> Talents { get; set; }

        [JsonPropertyName("ct"), JsonConverter(typeof(JsonOptSkillsConverter))]
        public Dictionary<string, int> CombatTechniques { get; set; }

        [JsonPropertyName("spells"), JsonConverter(typeof(JsonOptSkillsConverter))]
        public Dictionary<string, int> Spells { get; set; }

        [JsonPropertyName("cantrips")]
        public List<string> Cantrips { get; set; }

        [JsonPropertyName("liturgies"), JsonConverter(typeof(JsonOptSkillsConverter))]
        public Dictionary<string, int> Liturgies { get; set; }

        [JsonPropertyName("blessings")]
        public List<string> Blessings { get; set; }

        [JsonPropertyName("belongings")]
        public BelongingsOptM Belongings { get; set; }

        [JsonPropertyName("pets")]
        public PetsOptM Pets { get; set; }

        [JsonPropertyName("pact")]
        public PactOptM Pact { get; set; }

        #endregion


        #region IMPLEMENTs ICharacterImporter

        public string GetName() => Name;

        public string GetPlaceOfBirth() => Pers.Placeofbirth;

        public string GetDateOfBirth() => Pers.Dateofbirth;


        public int CountAbilities() => Attr.Abilities.Count;

        public IEnumerable<KeyValuePair<string, int>> GetAbilities()
        {
            foreach (var a in Attr.Abilities)
            {
                yield return new KeyValuePair<string, int>(a.Id, a.Value);
            }
        }

        public int CountTalentSkills() => Talents.Count;

        public IEnumerable<KeyValuePair<string, int>> GetTalentSkills()
        {
            foreach (var s in Talents)
            {
                yield return s;
            }
        }

        public int GetTalentSkill(string Id)
        {
            if (Talents.TryGetValue(Id, out var skill))
                return skill;
            else
                return 0;
        }


        public int CountArcaneSkills() => Spells.Count;

        public IEnumerable<KeyValuePair<string, int>> GetArcaneSkills()
        {
            foreach (var s in Spells)
            {
                yield return s;
            }
        }

        public int CountKarmaSkills() => Liturgies.Count;

        public IEnumerable<KeyValuePair<string, int>> GetKarmaSkills()
        {
            foreach (var s in Liturgies)
            {
                yield return s;
            }
        }


        public int GetAddedEnergy(CharacterEnergyClass energyClass) =>
            energyClass switch
            {
                CharacterEnergyClass.LP =>
                    Attr.Lp + Attr.PermanentLP.Lost + Attr.PermanentLP.Redeemed,
                CharacterEnergyClass.AE =>
                    Attr.Ae + Attr.PermanentAE.Lost + Attr.PermanentAE.Redeemed,
                CharacterEnergyClass.KP =>
                    Attr.Kp + Attr.PermanentKP.Lost + Attr.PermanentKP.Redeemed,
                _ => 0
            };


        /// <inheritdoc/>
        public bool IsSpellcaster() // TODO: use the advantage here, not the crutch
        {
            return CountArcaneSkills() > 0;
        }

        /// <inheritdoc/>
        public bool IsBlessed() // TODO: use the advantage here, not the crutch
        {
            return CountKarmaSkills() > 0;
        }

        public string GetSpeciesId()
        {
            return R;
        }


        #endregion
    }






    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class ExperiencePointsOpt
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }


    public class PersonOpt
    {
        [JsonPropertyName("placeofbirth")]
        public string Placeofbirth { get; set; }

        [JsonPropertyName("dateofbirth")]
        public string Dateofbirth { get; set; }

        [JsonPropertyName("age")]
        public string Age { get; set; }

        [JsonPropertyName("haircolor")]
        public int Haircolor { get; set; }

        [JsonPropertyName("eyecolor")]
        public int Eyecolor { get; set; }

        [JsonPropertyName("size")]
        public string Size { get; set; }

        [JsonPropertyName("weight")]
        public string Weight { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("socialstatus")]
        public int SocialStatus { get; set; }

        [JsonPropertyName("cultureAreaKnowledge")]
        public string CultureAreaKnowledge { get; set; }
    }



    public class AttrValue
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("value")]
        public int Value { get; set; }
    }



    public class PermanentEnergy
    {
        [JsonPropertyName("lost")]
        public int Lost { get; set; }

        [JsonPropertyName("redeemed")]
        public int Redeemed { get; set; }
    }



    public class Attributes
    {
        [JsonPropertyName("values")]
        public List<AttrValue> Abilities { get; set; }

        [JsonPropertyName("attributeAdjustmentSelected")]
        public string AttributeAdjustmentSelected { get; set; }

        [JsonPropertyName("ae")]
        public int Ae { get; set; }

        [JsonPropertyName("kp")]
        public int Kp { get; set; }

        [JsonPropertyName("lp")]
        public int Lp { get; set; }

        [JsonPropertyName("permanentAE")]
        public PermanentEnergy PermanentAE { get; set; }

        [JsonPropertyName("permanentKP")]
        public PermanentEnergy PermanentKP { get; set; }

        [JsonPropertyName("permanentLP")]
        public PermanentEnergy PermanentLP { get; set; }
    }



    //public class ADV50
    //{
    //}

    //public class ADV10
    //{
    //}

    //public class ADV26
    //{
    //}

    //public class ADV40
    //{
    //}

    //public class ADV48
    //{
    //}

    //public class DISADV2
    //{
    //    [JsonPropertyName("tier")]
    //    public int Tier { get; set; }
    //}

    //public class DISADV37
    //{
    //    [JsonPropertyName("sid")]
    //    public int Sid { get; set; }
    //}

    //public class DISADV50
    //{
    //    [JsonPropertyName("sid")]
    //    public string Sid { get; set; }

    //    [JsonPropertyName("tier")]
    //    public int Tier { get; set; }
    //}

    //public class DISADV71
    //{
    //}

    //public class SA78
    //{
    //}

    //public class SA76
    //{
    //}

    //public class SA1
    //{
    //}

    //public class SA3
    //{
    //    [JsonPropertyName("sid")]
    //    public int Sid { get; set; }
    //}

    //public class SA27
    //{
    //    [JsonPropertyName("sid")]
    //    public int Sid { get; set; }
    //}

    //public class SA29
    //{
    //    [JsonPropertyName("sid")]
    //    public int Sid { get; set; }

    //    [JsonPropertyName("tier")]
    //    public int Tier { get; set; }
    //}

    //public class SA39
    //{
    //}

    //public class SA70
    //{
    //}

    //public class SA80
    //{
    //}

    //public class SA233
    //{
    //}

    //public class SA267
    //{
    //}

    //public class SA315
    //{
    //}


    // TODO
    public class Activatable
    {
        //[JsonPropertyName("ADV_50")]
        //public List<ADV50> ADV50 { get; set; }

        //[JsonPropertyName("ADV_36")]
        //public List<object> ADV36 { get; set; }

        //[JsonPropertyName("ADV_10")]
        //public List<ADV10> ADV10 { get; set; }

        //[JsonPropertyName("ADV_26")]
        //public List<ADV26> ADV26 { get; set; }

        //[JsonPropertyName("ADV_40")]
        //public List<ADV40> ADV40 { get; set; }

        //[JsonPropertyName("ADV_48")]
        //public List<ADV48> ADV48 { get; set; }

        //[JsonPropertyName("DISADV_5")]
        //public List<object> DISADV5 { get; set; }

        //[JsonPropertyName("DISADV_2")]
        //public List<DISADV2> DISADV2 { get; set; }

        //[JsonPropertyName("DISADV_37")]
        //public List<DISADV37> DISADV37 { get; set; }

        //[JsonPropertyName("DISADV_50")]
        //public List<DISADV50> DISADV50 { get; set; }

        //[JsonPropertyName("DISADV_71")]
        //public List<DISADV71> DISADV71 { get; set; }

        //[JsonPropertyName("DISADV_29")]
        //public List<object> DISADV29 { get; set; }

        //[JsonPropertyName("DISADV_48")]
        //public List<object> DISADV48 { get; set; }

        //[JsonPropertyName("DISADV_49")]
        //public List<object> DISADV49 { get; set; }

        //[JsonPropertyName("DISADV_46")]
        //public List<object> DISADV46 { get; set; }

        //[JsonPropertyName("SA_78")]
        //public List<SA78> SA78 { get; set; }

        //[JsonPropertyName("SA_76")]
        //public List<SA76> SA76 { get; set; }

        //[JsonPropertyName("SA_1")]
        //public List<SA1> SA1 { get; set; }

        //[JsonPropertyName("SA_3")]
        //public List<SA3> SA3 { get; set; }

        //[JsonPropertyName("SA_27")]
        //public List<SA27> SA27 { get; set; }

        //[JsonPropertyName("SA_29")]
        //public List<SA29> SA29 { get; set; }

        //[JsonPropertyName("SA_39")]
        //public List<SA39> SA39 { get; set; }

        //[JsonPropertyName("SA_681")]
        //public List<object> SA681 { get; set; }

        //[JsonPropertyName("SA_70")]
        //public List<SA70> SA70 { get; set; }

        //[JsonPropertyName("SA_80")]
        //public List<SA80> SA80 { get; set; }

        //[JsonPropertyName("SA_233")]
        //public List<SA233> SA233 { get; set; }

        //[JsonPropertyName("SA_267")]
        //public List<SA267> SA267 { get; set; }

        //[JsonPropertyName("SA_315")]
        //public List<SA315> SA315 { get; set; }

        //[JsonPropertyName("SA_421")]
        //public List<object> SA421 { get; set; }
    }






    public class BelongingItem //TODO Next
    {
        /// <summary>
        /// The item id in the characters' file (format "ITEM_1").
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        /// <summary>
        /// Item weight in stone per piece. 1 stone are 2 pounds.
        /// </summary>
        [JsonPropertyName("weight")]
        public double Weight { get; set; }

        /// <summary>
        /// Typical price of the item in silverthalers.
        /// </summary>
        [JsonPropertyName("price")]
        public double Price { get; set; }

        /// <summary>
        /// The location where the character wears/carries this item.
        /// </summary>
        [JsonPropertyName("where")]
        public string Where { get; set; }


        #region Unclear
        /// <summary>
        /// ?
        /// </summary>
        [JsonPropertyName("stp")]
        public int? Stp { get; set; }
        #endregion



        #region Optolith-specific properties

        /// <summary>
        /// String referring to the Optolith template id.
        /// </summary>
        [JsonPropertyName("template")]
        public string Template { get; set; }

        /// <summary>
        /// An item cannot be changed by the user when the template can be 
        /// changed by the user.
        /// </summary>
        [JsonPropertyName("isTemplateLocked")]
        public bool IsTemplateLocked { get; set; }

        /// <summary>
        /// An item group like close combat weapon, clothing, ...
        /// </summary>
        [JsonPropertyName("gr")]
        public int Group { get; set; }

        #endregion



        #region Weapon
        /// <summary>
        /// AT-Mod
        /// </summary>
        [JsonPropertyName("at")]
        public int AttackMod { get; set; }

        /// <summary>
        /// PA-Mod
        /// </summary>
        [JsonPropertyName("pa")]
        public int ParryMod { get; set; }

        /// <summary>
        /// Number of dice to get the damage, i.e. the N in "Nd6 + 3".
        /// </summary>
        [JsonPropertyName("damageDiceNumber")]
        public int DamageDiceNumber { get; set; }

        /// <summary>
        /// Damage bonus for a hit with a weapon, i.e. the N in "1d6 + N".
        /// </summary>
        [JsonPropertyName("damageFlat")]
        public int DamageFlat { get; set; }

        /// <summary>
        /// Length of the item in half fingers (Halbfinger = 1 cm)
        /// </summary>
        [JsonPropertyName("length")]
        public int Length { get; set; }

        /// <summary>
        /// The combat technique used to 
        /// </summary>
        [JsonPropertyName("combatTechnique")]
        public string CombatTechnique { get; set; }

        /// <summary>
        /// Sides of the hit point dice
        /// </summary>
        [JsonPropertyName("damageDiceSides")]
        public int DamageDiceSides { get; set; }

        /// <summary>
        /// Weapon's reach. Close combat only.
        /// </summary>
        [JsonPropertyName("reach")]
        public int Reach { get; set; }

        /// <summary>
        /// Distances that define the weapons range short / middle / far. 
        /// Ranged weapons only.
        /// </summary>
        [JsonPropertyName("range")]
        public List<int> Range { get; set; }

        /// <summary>
        /// Primary thresholds "P+T" for a weapon (Leiteigenschaft, L+S).
        /// </summary>
        [JsonPropertyName("primaryThreshold")]
        public PrimaryThreshold PrimaryThreshold { get; set; }

        #endregion



        #region Armour
        /// <summary>
        /// Encumbrance (Behinderung, BE) caused by the armour.
        /// </summary>
        [JsonPropertyName("enc")]
        public int? Encumbrance { get; set; }

        /// <summary>
        /// Protection against damage (Rüstungsschutz, RS).
        /// </summary>
        [JsonPropertyName("pro")]
        public int? Protection { get; set; }

        // ?
        [JsonPropertyName("armorType")]
        public int? ArmorType { get; set; } //TODO
        #endregion
    }

    public class PrimaryThreshold
    {
        [JsonPropertyName("threshold")]
        public int Threshold { get; set; }
    }


    /// <summary>
    /// A set of armour. Each area may have a different armour.
    /// 
    /// </summary>
    public class Armor
    {
        /// <summary>
        /// Optolith id in the format "ARMORZONES_1".
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// User-defined string to identify the armour
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// The armour to protect the head.
        /// The Optolith item id of the armour (format: "ITEMTPL_691").
        /// </summary>
        [JsonPropertyName("head")]
        public string Head { get; set; }

        /// <summary>
        /// The wear through use or age.
        /// </summary>
        [JsonPropertyName("headLoss")]
        public long HeadLoss { get; set; }

        /// <summary>
        /// The armour to protect the left arm.
        /// The Optolith item id of the armour (format: "ITEMTPL_691").
        /// </summary>
        [JsonPropertyName("leftArm")]
        public string LeftArm { get; set; }

        [JsonPropertyName("leftArmLoss")]
        public long LeftArmLoss { get; set; }

        /// <summary>
        /// The armour to protect the right arm.
        /// The Optolith item id of the armour (format: "ITEMTPL_691").
        /// </summary>
        [JsonPropertyName("rightArm")]
        public string RightArm { get; set; }

        [JsonPropertyName("rightArmLoss")]
        public long RightArmLoss { get; set; }

        /// <summary>
        /// The armour to protect the torso.
        /// The Optolith item id of the armour (format: "ITEMTPL_691").
        /// </summary>
        [JsonPropertyName("torso")]
        public string Torso { get; set; }

        [JsonPropertyName("torsoLoss")]
        public long TorsoLoss { get; set; }

        /// <summary>
        /// The armour to protect the left leg.
        /// The Optolith item id of the armour (format: "ITEMTPL_691").
        /// </summary>
        [JsonPropertyName("leftLeg")]
        public string LeftLeg { get; set; }

        [JsonPropertyName("leftLegLoss")]
        public long LeftLegLoss { get; set; }

        /// <summary>
        /// The armour to protect the right leg.
        /// The Optolith item id of the armour (format: "ITEMTPL_691").
        /// </summary>
        [JsonPropertyName("rightLeg")]
        public string RightLeg { get; set; }

        [JsonPropertyName("rightLegLoss")]
        public long RightLegLoss { get; set; }
    }


    public class PurseOptM
    {
        /// <summary>
        /// (Gold) Ducat (D) in the purse
        /// </summary>
        [JsonPropertyName("d")]
        public string D { get; set; }

        /// <summary>
        /// (Silver) Thalers in the purse
        /// </summary>
        [JsonPropertyName("s")]
        public string S { get; set; }

        /// <summary>
        /// (Copper) Farthings in the purse
        /// </summary>
        [JsonPropertyName("h")]
        public string H { get; set; }

        /// <summary>
        /// (Iron) Kreutzer (K)	in the purse
        /// </summary>
        [JsonPropertyName("k")]
        public string K { get; set; }
    }


    /// <summary>
    /// All the personal belongings.
    /// </summary>
    public class BelongingsOptM
    {
        [JsonPropertyName("items"), JsonConverter(typeof(JsonFakeListConverter<BelongingItem>))]
        public List<BelongingItem> Items { get; set; }

        [JsonPropertyName("armorZones"), JsonConverter(typeof(JsonFakeListConverter<Armor>))]
        public List<Armor> ArmorZones { get; set; }

        [JsonPropertyName("purse")]
        public PurseOptM Purse { get; set; }
    }


    public class RulesOptM
    {
        [JsonPropertyName("higherParadeValues")]
        public int HigherParadeValues { get; set; }

        [JsonPropertyName("attributeValueLimit")]
        public bool AttributeValueLimit { get; set; }

        [JsonPropertyName("enableAllRuleBooks")]
        public bool EnableAllRuleBooks { get; set; }

        [JsonPropertyName("enabledRuleBooks")]
        public List<object> EnabledRuleBooks { get; set; }

        [JsonPropertyName("enableLanguageSpecializations")]
        public bool EnableLanguageSpecializations { get; set; }
    }


    public class PetsOptM
    {
        /// <summary>
        /// So far the Optolith seems to support only a single pet (20220102).
        /// </summary>
        [JsonPropertyName("PET_1")]
        public Pet Pet { get; set; }
    }

    /// <summary>
    /// Pets and other compansions.
    /// </summary>
    public class Pet
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("size")]
        public string Size { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("attack")]
        //[JsonConverter(typeof(ParseStringConverter))] // Convert to int
        public string Attack { get; set; }

        [JsonPropertyName("dp")]
        public string Dp { get; set; }

        [JsonPropertyName("reach")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string Reach { get; set; }

        [JsonPropertyName("actions")]
        public string Actions { get; set; }

        [JsonPropertyName("talents")]
        public string Talents { get; set; }

        [JsonPropertyName("skills")]
        public string Skills { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; }

        [JsonPropertyName("spentAp")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string SpentAp { get; set; }

        [JsonPropertyName("totalAp")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string TotalAp { get; set; }

        [JsonPropertyName("cou")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string Cou { get; set; }

        [JsonPropertyName("sgc")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string Sgc { get; set; }

        [JsonPropertyName("int")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string Int { get; set; }

        [JsonPropertyName("cha")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string Cha { get; set; }

        [JsonPropertyName("dex")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string Dex { get; set; }

        [JsonPropertyName("agi")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string Agi { get; set; }

        [JsonPropertyName("con")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string Con { get; set; }

        [JsonPropertyName("str")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string Str { get; set; }

        [JsonPropertyName("lp")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string Lp { get; set; }

        [JsonPropertyName("ae")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string Ae { get; set; }

        [JsonPropertyName("spi")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string Spi { get; set; }

        [JsonPropertyName("tou")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string Tou { get; set; }

        [JsonPropertyName("pro")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string Pro { get; set; }

        [JsonPropertyName("ini")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string Ini { get; set; }

        [JsonPropertyName("mov")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string Mov { get; set; }

        [JsonPropertyName("at")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string At { get; set; }

        [JsonPropertyName("pa")]
        ////[JsonConverter(typeof(ParseStringConverter))]
        public string Pa { get; set; }
    }


    /// <summary>
    /// Characters can make a pact with a non-human creature. 
    /// Only one pact per character is possible.
    /// </summary>
    public class PactOptM
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Fairy or demon
        /// </summary>
        [JsonPropertyName("category")]
        public int Category { get; set; }

        [JsonPropertyName("domain")]
        public int Domain { get; set; }

        [JsonPropertyName("type")]
        public int Type { get; set; }

        /// <summary>
        /// "Paktstufe" (bei Feen), "Kreis der Verdammnis" (bei Dämonen).
        /// </summary>
        [JsonPropertyName("level")]
        public int Level { get; set; }
    }

}
