using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FateExplorer.CharacterData
{
    public class CharacterImportOptM
    {
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

        [JsonPropertyName("locale")]
        public string Locale { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("ap")]
        public ExperiencePointsOpt Ap { get; set; }

        [JsonPropertyName("el")]
        public string El { get; set; }

        [JsonPropertyName("r")]
        public string R { get; set; }

        [JsonPropertyName("rv")]
        public string Rv { get; set; }

        [JsonPropertyName("c")]
        public string C { get; set; }

        [JsonPropertyName("p")]
        public string P { get; set; }

        [JsonPropertyName("sex")]
        public string Sex { get; set; }

        [JsonPropertyName("pers")]
        public PersonOpt Pers { get; set; }

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
        public Belongings Belongings { get; set; }

        [JsonPropertyName("rules")]
        public Rules Rules { get; set; }

        [JsonPropertyName("pets")]
        public Pets Pets { get; set; }

        [JsonPropertyName("pact")]
        public Pact Pact { get; set; }
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



    public class PermanentAE
    {
        [JsonPropertyName("lost")]
        public int Lost { get; set; }

        [JsonPropertyName("redeemed")]
        public int Redeemed { get; set; }
    }



    public class PermanentKP
    {
        [JsonPropertyName("lost")]
        public int Lost { get; set; }

        [JsonPropertyName("redeemed")]
        public int Redeemed { get; set; }
    }



    public class PermanentLP
    {
        [JsonPropertyName("lost")]
        public int Lost { get; set; }
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
        public PermanentAE PermanentAE { get; set; }

        [JsonPropertyName("permanentKP")]
        public PermanentKP PermanentKP { get; set; }

        [JsonPropertyName("permanentLP")]
        public PermanentLP PermanentLP { get; set; }
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
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("gr")]
        public int Gr { get; set; }

        [JsonPropertyName("isTemplateLocked")]
        public bool IsTemplateLocked { get; set; }

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("weight")]
        public double Weight { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("at")]
        public int Attack { get; set; }

        [JsonPropertyName("damageDiceNumber")]
        public int DamageDiceNumber { get; set; }

        [JsonPropertyName("damageFlat")]
        public int DamageFlat { get; set; }

        [JsonPropertyName("length")]
        public int Length { get; set; }

        [JsonPropertyName("pa")]
        public int Parry { get; set; }

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
        /// close combat only
        /// </summary>
        [JsonPropertyName("reach")]
        public int Reach { get; set; }

        /// <summary>
        /// Ranged weapons only
        /// </summary>
        [JsonPropertyName("range")]
        public List<int> Range { get; set; }

        /// <summary>
        /// String referring to the Optolith template id.
        /// </summary>
        [JsonPropertyName("template")]
        public string Template { get; set; }

        [JsonPropertyName("enc")]
        public int? Enc { get; set; }

        [JsonPropertyName("pro")]
        public int? Pro { get; set; }

        /// <summary>
        /// ?
        /// </summary>
        [JsonPropertyName("armorType")]
        public int? ArmorType { get; set; } //TODO

        [JsonPropertyName("where")]
        public string Where { get; set; }

        [JsonPropertyName("primaryThreshold")]
        public PrimaryThreshold PrimaryThreshold { get; set; }

        /// <summary>
        /// ?
        /// </summary>
        [JsonPropertyName("stp")]
        public int? Stp { get; set; }
    }

    public class PrimaryThreshold
    {
        [JsonPropertyName("threshold")]
        public int Threshold { get; set; }
    }


    public class Armor
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("head")]
        public string Head { get; set; }

        [JsonPropertyName("leftArm")]
        public string LeftArm { get; set; }

        [JsonPropertyName("rightArm")]
        public string RightArm { get; set; }

        [JsonPropertyName("torso")]
        public string Torso { get; set; }

        [JsonPropertyName("leftLeg")]
        public string LeftLeg { get; set; }

        [JsonPropertyName("rightLeg")]
        public string RightLeg { get; set; }
    }

    public class ArmorZones
    {
        [JsonPropertyName("ARMORZONES_1")] // TODO
        public List<Armor> Armors { get; set; }
    }

    public class Purse
    {
        [JsonPropertyName("d")]
        public string D { get; set; }

        [JsonPropertyName("s")]
        public string S { get; set; }

        [JsonPropertyName("h")]
        public string H { get; set; }

        [JsonPropertyName("k")]
        public string K { get; set; }
    }

    public class Belongings
    {
        [JsonPropertyName("items"), JsonConverter(typeof(JsonOptBelongingsConverter))]
        public List<BelongingItem> Items { get; set; }

        [JsonPropertyName("armorZones")]
        public ArmorZones ArmorZones { get; set; }

        [JsonPropertyName("purse")]
        public Purse Purse { get; set; }
    }


    public class Rules
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

    public class Pets
    {
        // TODO
    }

    public class Pact
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("category")]
        public int Category { get; set; }

        [JsonPropertyName("domain")]
        public int Domain { get; set; }

        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("level")]
        public int Level { get; set; }
    }

}
