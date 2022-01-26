
# Attributes

## Wording

Any character value is called an **attribute**.

Courage, strength, ... are called abilites.

Skills are basic, mundane skills (like perception, ...). But arcane and karma skills are called skills as much as the combat techniques. Only that the program logic handles combat techniques differently.

The combat technique describes the basic skill with a certain weapon's branch. Each particular weapon may modify the attack and parry skill, as well as the 


## ...

* Imported value: the value as it was understood by importing a character sheet.
* True value: the correct character's attribute after applying special abilities, advantages and disadvantages.
* Effective value. An attribute value modified either...
  * by a modification at the GMs discretion.
  * by temporary states and conditions.
  * for dependent attributes when the dependencies change.


# Rolls & Checks

## Types of Checks (Rule Book)

### Simple Checks

The simple check is a regular skill check. The outcome of this check depends on whether the hero succeeds at the check. If the hero
succeeds the result will be classified.

### Competitive Checks

The competitive check allows you to compare two contestants, and the one with the higher QL wins the check.

### Cumulative Check

Sometimes it takes a certain amount of time and more than one skill check to accomplish a task. In such cases, the GM calls for a cumulative check. This consists of multiple skill checks of the same kind, wherein the hero must accumulate a total of 10 QL in order to accomplish the task at hand.

### Group Checks

A group of heroes must work together to achieve certain goals. When several heroes use skills to work together, the procedure is called a group check. Group checks can be competitive or cumulative checks, but never simple checks. In group checks, add up the combined results of all participating characters.

## Add Combat Checks

All attack checks are handled by the same class with only the exception of shields. The class distinguishes botch rolls that are different for unarmed combat, shields or ranged combat. It also distinguises 

To add a combat check you need a new entry in "rollresolver.json". Example:

``` json
"CT_3/AT":  {"id": "CT_3/AT",  "roll": "DSA5/0/combat/attack", "name": "Attack", "type": "compare"}
```

* The key of the entry starts the combat technique. The "AT" or "PA" identifier dinstinguishes attack and parry actions. 
* The `roll` attribute denotes the `checkTypeId` of the roll check class (derived from `CheckBaseM`).
* `name`is an arbitrary string to describe the action in a human readable form (unsused so far).
* `type` (unused)



## FateExplorer

The FE distinguishes rolls and checks. A roll is whatever you can do with rolling one dice cup. A check requires several rolls and may involve additional criteria, tables and comparisons.




## Checks: Overview

| Type | Roll | Criterion | Result |
| --- | --- | --- | --- |
| Botch (effects) roll  | 2d6 | botch table | Enum |
| Skill roll  | 3d20 | (abilities - mods) + skill => Quality level | `RollSuccessLevel` |
| Routine skill "roll" |  |  | `RollSuccessLevel` + Quality level |
| Attack Roll | 1d20 | attack skill - mod |`RollSuccessLevel`|
| Parry Roll | 1d20 | attack skill - mod (mod halves skill for critical) | `RollSuccessLevel` |
| Critical confirmation roll | 1d20 | attack skill - mod | `RollSuccessLevel` |
| Ability roll | 1d20 | ability + mod | `RollSuccessLevel` |
| Damage roll  | NdM + x | - (m is usually 6) | points |
| Initiative roll | INI + 1d6 + mod | - | points |
| Regeneration roll | 1d6 + mod | - | points |
| Meditation roll |  |  |  |



## Roll Sequences

1. Single roll
2. 1d20 --> &empty; | Damage | Confirmation --> Damage | Botch roll --> [Damage] | Effect

| Type | Roll | Criterion |
| --- | --- | --- |
|  |  |  |
|  |  |  |
|  |  |  |




# Character Attributes

One an roll checks against these:

**Abilities**

| Name | Abbr. | Original |
| --- | --- | --- |
| Courage   | COU | Mut |
| Sagacity  | SAG | Klugheit |
| Intuition |  | Intuition |
| Charisma  |  | Charisma |
|  |  | Fingerfertigkeit |
|  |  | Gewandheit |
| Constitution |  | Konstitution |
| Strength     |  | Körperkraft |


**Resources**.

* Health/Life Points
* Arcane Energy
* Karma Points


**Initiative**. (INI).

**Dodge**. (DO; Ausweichen, AW)

The `resilience` attributes act as modifiers

* Spirit (SPI; Seelenkraft, SK)
* Toughness (TOU; Zähigkeit, ZK)

These are required for additional rules:

**Movement**. (MOV; Geschwindigkeit, GW)

**Fate Points**.


