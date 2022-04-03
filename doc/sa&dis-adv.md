# Special Abilities & Advantages

## (Dis-) Advantage

| (Dis-) Advantage | Id | Tier |
| -- | -- | -- |
| Aptitude           | `ADV_4`  | &nbsp; |
| Ambidextrous       | `ADV_5`  | &nbsp; |
| Fox Sense          | `ADV_10` | &nbsp; |
| Blessed            | `ADV_12` | &nbsp; |
| Increased Spirit   | `ADV_26` | &nbsp; |
| Socially Adaptable | `ADV_40` | &nbsp; |
| Beautiful Voice    | `ADV_48` | &nbsp; |
| Spellcaster        | `ADV_50` | &nbsp; |



| Disadvantage | Id | Tiers | |
| -- | -- | -- | -- |
| Obligations I-III  | `DISADV_50` | 1-3 | `"sid": "1200 D"` |
| Susceptible to Poison I-II |  | 1-2 |
| Poor I - III | ?DISADV_2? | 1-3 |
| Susceptible to Poison I-II | | 1-2 |




An activated (dis-) advantage is at least an array with one empty object in json:

| State | Json |
| -- | -- |
Activated |  `[{ ... }]` |
Deactivated |  `[]` |


## Special Abilities

The first two types should cover over 90% of the special abilities. For a few special abilities using an enum the according enum is specified. If not, these special abilites are treated like switches.

1. **Switches**: Most special abilities have a simple on/off effect. You either have the ability or you don't. There are no modifications to that. Examples: "Heraldry" or "Hunter".
3. `TieredActivatableM`: Further special abilities are tiered i.e. they have a level like "Iron Will" which has two levels. This class handles the switches, too. For switches, `tier = 0`.
2. `SpecialAbilityEnumM<T>` are special abilities that are basically switches, however there is an additional id (an enum) to qualify the exact meaning. "Literacy" is an example. These special abilities can be activated once for each enum value.
4. `FreetextSpecialAbilityM` are abilities (especially the user-defined one but also "Area Knowledge") which simply require any free text (as `sid`).
5. A `SkillSpecialAbilityM` uses a string `sid` that repesents an identifier to reference a skill (including combat techniques, cantrips, ...).
6. Language is special. It can be considered mandatory because there can't be any character without any form of communication. `LanguageM`.



### General


| Special Ability | Id | Tiers | Info |
| --       | -- | -- | -- |
| Language I-III | `SA_29` | tier 1-3; native language = 4 | `sid`: language enum, Garethian  (e.g. `sid 8`) |
| Literacy       | `SA_27` | - | `sid`: Literacy enum |



### General Talents

| Special Ability | Id | Tiers | Info | 
| -- | -- | -- | -- |
| Geländekunde [Terrain Knowledge] | `SA_12`  | - | sid is `enum Terrain` |
| Iron Will I-II    | `SA_5`       | 1-2 | |
| Ortskenntnis [Area Knowledge]    | `SA_22`  | - | sid: string |
| Fertigkeitsspezialisierung (Talente) | `SA_9` | - | sid: talent id; sid2: `enum` |
| Belastungsgewöhnung I-II | `SA_41` | - | |
| Aufmerksamkeit    | `SA_40` | | |
| Schmerzen unterdrücken | `SA_25` | | Schicksal |
| Prunkkleidung herstellen | SA_130 | | |
| Diener            | `SA_105` | | |
| Abrollen          | `SA_96` | | |
| Meister der Improvisation |`SA_21` | | |
| Füchsisch         | `SA_11` | | |



Combat 

| Special Ability | Id | Tiers | Info | 
| -- | -- | -- | -- |
| Two-Handed Combat  | `SA_42`  | 1-2 `[{"tier": 1}]` | |
| Wuchtschlag I-III  | `SA_67`  | 1-3 | - |
| Attacke verbessern | `SA_34`  | - | - |
| Drohgebärden       | `SA_106` | - | - |
| Finte I-III        | `SA_48`  | 1-3 | - |
| Schnellladen       | `SA_60`  | | in optholit an sid enum; in fact it's a combat technique id |
| Hruruzat           | `SA_186` | - | |

| Hose Herunterziehen/Hemd Hochziehen | `SA_424` | | |
| Handkantenschlag | `SA_203` | | |


Magic

| Special Ability | Id | Tiers | Info |
| -- | -- | -- | -- |
| Scholar des Demirion Ophenos | `SA_281` | - |
| Bindung des Stabes       | `SA_76`  | - |
| Ewige Flamme             | `SA_78`  | - |
| Kraftfokus               | `SA_80`  | - |
| Vollkommener Beherrscher | `SA_315` | - |
| Astrale Meditation       | `SA_233` | - |
| Materielle Verbindung    | `SA_785` | - |
| Verbessertes Ausweichen I-III | `SA_64` | 1-3 | - |
| Bindung des Bannschwerts | `SA_344` | - | sid: combat technique id `CT_xx`. |


Karma

| Special Ability | Id | Tiers | Info |
| -- | -- | -- | -- |
| Lieblingsliturgie | SA_569 | | |

SA_663 ??? Liturgieerweiterung?

        "SA_9": [ {"sid2": 5, "sid": "TAL_34"} ],
        "SA_27": [ {"sid": 14} ],
        "SA_29": [
            { "sid": 23, "tier": 4 },
            { "sid":  8, "tier": 2 },
            { "sid": 12, "tier": 1 },
            { "sid": 21, "tier": 1 }
        ],
        "SA_41": [ {"tier": 1} ],
        "SA_569": [ {"sid": "LITURGY_26"} ],
        "SA_663": [ {"sid": 532} ],



### Magic Traditions

| Magic tradition | Id | Primary ability | Ability Id | 
| -- | -- | -- | -- |
| Runenschöpfer |  | - | &nbsp; |
| Animisten     | na | Intuition / 2 | `ATTR_3` |
| Brobim-Geode  | na | Charisma | `ATTR_4` |
| Druide        | `SA_346` | Klugheit | `ATTR_2` |
| Dämonen       | - | Mut | `ATTR_1` |
| Einhorn       | - | Intuition | `ATTR_3` |
| Elfen         |   | Intuition | `ATTR_3` |
| Feen          | - | Charisma | `ATTR_4` |
| Geoden        | na | Charisma | `ATTR_4` |
| Gildenmagier  | `SA_70` | Klugheit | `ATTR_2` |
| Goblinzauberinnen  | - | Intuition | `ATTR_3` |
| Hexe          | `SA_255` | Charisma | `ATTR_4` |
| Meistertalentierte |  | - | &nbsp; |
| Nachtalben    | - | Mut | `ATTR_1` |
| Necker        | - | Charisma | `ATTR_4` |
| Qabalyamagier | na | Klugheit | `ATTR_2` |
| Scharlatane   | `SA_676` | Charisma | `ATTR_4` |
| Schelme       | `SA_726` | Intuition | `ATTR_3` |
| Sphinx        | - | Intuition | `ATTR_3` |
| Tsatuara-Anhängerin | na | Charisma | `ATTR_4` |
| Zauberalchimisten   | `SA_750` | Klugheit / 2 | `ATTR_2` |
| Zauberbarden  | na | Charisma / 2 | `ATTR_4` |
| Zaubertänzer  | na | Charisma / 2 | `ATTR_4` |
| Zibiljas      | na | Intuition | `ATTR_3` |
| Intuitive Zauberer | `SA_679` | - | &nbsp; |


### Religious Traditions

| Karma tradition | Id | Primary ability | Id | 
| -- | -- | -- | -- |
| Tradition (Angroschkirche) | na | Intuition | `ATTR_3` |
| Tradition (Aveskirche)     | `SA_694` | Intuition | `ATTR_3` |
| Tradition (Boronkirche)    | `SA_683` | Mut | `ATTR_1` |
| Tradition (Efferdkirche)   | `SA_687` | Charisma | `ATTR_4` |
| Tradition (Ferkinaschamanen) | na     | Intuition | `ATTR_3` |
| Tradition (Firunkirche)    | `SA_689` | Mut | `ATTR_1` |
| Tradition (Fjarningerschamanen) | na  | Intuition | `ATTR_3` |
| Tradition (Gjalskerschamanen) | na    | Intuition | `ATTR_3` |
| Tradition (Hesindekirche)  | `SA_684` | Klugheit | `ATTR_2` |
| Tradition (Ifirnkirche)    | `SA_695` | Charisma | `ATTR_4` |
| Tradition (Ingerimmkirche) | `SA_691` | Intuition | `ATTR_3` |
| Tradition (Korkirche)      | `SA_696` | Mut | `ATTR_1` |
| Tradition (Levthankult)    | na | Charisma | `ATTR_4` |
| Tradition (Marbokult)      | na | Charisma | `ATTR_4` |
| Tradition (Namenloser Kult) | `SA_693` | Mut | `ATTR_1` |
| Tradition (Nanduskirche)   | na | Klugheit | `ATTR_2` |
| Tradition (Nivesenschamen) | na | Intuition | `ATTR_3` |
| Tradition (Numinorukult)   | `SA_1049` | Klugheit | `ATTR_2` |
| Tradition (Perainekirche)  | `SA_686` | Intuition | `ATTR_3` |
| Tradition (Phexkirche)     | `SA_685` | Intuition | `ATTR_3` |
| Tradition (Praioskirche)   | `SA_86` | Klugheit | `ATTR_2` |
| Tradition (Rahjakirche)    | `SA_692` | Charisma | `ATTR_4` |
| Tradition (Rondrakirche)   | `SA_682` | Mut | `ATTR_1` |
| Tradition (Swafnirkirche)  | na | Mut | `ATTR_1` |
| Tradition (Tahayaschamanen)| na | Intuition | `ATTR_3` |
| Tradition (Tairachkult)    | na | - | &nbsp; |
| Tradition (Traviakirche)   | `SA_688` | Klugheit | `ATTR_2` |
| Tradition (Trollzackerschamanen) | na | Intuition | `ATTR_3` |
| Tradition (Tsakirche)      | `SA_690` | Charisma | `ATTR_4` |

