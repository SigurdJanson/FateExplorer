# Conditions

1. Conditions accumulate
   * Effects of different conditions add up
   * Levels of one condition add up when they are caused by different sources
2. The total modifier of all conditions cannot exceed -5 `Total <- max(sum(conditions), -5L)`
3. A hero with a total of 8 levels in all conditions combined is "incapacitated" regardless of the levels of each condition.




## Confusion (Verwirrung)
| Level | Description | Modifier on          | Effect |
|:-----:|:------------|:-----------------------|:------------|
| I     | Mildly confused | All checks | -1 |
| II    | Confused | All checks | -2 |
| III   | Very Confused | All checks | -3 |
|       | | Complex actions (like spell casting, liturgical chants, and the use of lore skills) are impossible | Denied |
| IV    | Incapacitated | All checks | Denied |

Requirements:

* Reference to **all** checks (incl. combat and ability)
* Reference to a list of skills (i.e. skills that involve complex actions)
* Diverging effects on level III depending on the type of skill
* Ignore MOV


## Fear (Furcht)
| Level | Description | Modifier on          | Effect |
|:-----:|:------------|:-----------------------|:------------|
| I     | Uneasy      | All checks | -1 |
| II    | Scared      | All checks | -2 |
| III   | Panicked    | All checks | -3 |
| IV    | Catatonic and therefore incapacitated | All actions | Denied |


Requirements:

* Reference to **all** checks (incl. combat and ability)


## Pain (Schmerz)
| Level | Description | Modifier on          | Effect |
|:-----:|:------------|:-----------------------|:------------|
| I     | Slight pain     | All checks, MOV | -1 |
| II    | Disturbing pain | All checks, MOV | -2 |
| III   | Severe pain     | All checks, MOV | -3 |
| IV    | Severe pain     | All checks, MOV | -3 |

Requirements:

* Reference to **all** checks (incl. combat and ability)
* Ignore MOV


## Stupor (Betäubung)
| Level | Description | Modifier on          | Effect |
|:-----:|:------------|:-----------------------|:------------|
| I     | Slightly drowsy | All checks | -1 |
| II    | Lethargic | All checks | -2 |
| III   | Very sluggish | All checks | -3 |
| IV    | Incapacitated | All checks | Denied |

Requirements:

* Reference to **all** checks (incl. combat and ability)


## Encumbrance (Belastung)
| Level | Description | Modifier on          | Effect |
|:-----:|:------------|:-----------------------|:------------|
| I     | Mildly encumbered | Skill checks | -1 |
|       | | AT, Defense, INI, and MOV      | -1 |
| II    | Encumbered  | Skill checks       | -2 |
|       | | AT, Defense, INI, and MOV      | -2 |
| III   | Very encumbered | Skill checks   | -3 |
|       | | AT, Defense, INI, and MOV      | -3 |
| IV    | Incapacitated | All actions  | Denied |

Requirements:

* Reference to all skill checks
* Reference to all combat skills
* Reference to initiative (INI)
* Ignore MOV



## Intoxicated (Berauscht)
| Level | Description | Modifier on          | Effect |
|:-----:|:------------|:-----------------------|:------------|
| I     | Tipsy | Checks on carousing | -1 |
| II    |       | Checks on carousing | -2 |
| III   | Drunk | Checks on carousing | -3 |
| IV    |       | All actions         | Denied |
|       |       | Condition "Stupor"  | +1 |
|       |       | Intoxication (further levels above 4 remain) | -4 |


Requirements:

* Reference to specific skill (Carousing)
* Carry over level 4 to stupor
* Carry over -4 levels to self


## Paralysis (Paralyse)
| Level | Description | Modifier on          | Effect |
|:-----:|:------------|:-----------------------|:------------|
| I     | Slightly paralyzed  | All checks involving movement or speech | -1 |
|       | | MOV      | -25% |
| II    | Stiff  | All checks involving movement or speech | -2 |
|       | | MOV      | -50% |
| III   | Hardly able to move | All checks involving movement or speech   | -3 |
|       | | MOV      | -75% |
| IV    | Unable to move | All actions | Denied |

Requirements:

* Reference to a list of skills (for some skills, however, it may depend on the situation whether this condition applies or not)



## "Overexertion" (Überanstrengung)
| Level | Description | Modifier on          | Effect |
|:-----:|:------------|:-----------------------|:------------|
| I     |  | All checks on knowledge skills | -1 |
| II    |  | All checks on knowledge skills | -2 |
| III   |  | All checks on knowledge skills | -3 |
| IV    |  | Condition "Stupor"             | +1 |
|       |  | Condition "Overexertion"       | -1 |

Requirements:

* Reference to a list of skills or to the class of knowledge skills.
* Carry over level 4 to stupor
* Carry over -1 levels to self




## Hypothermia (Unterkühlung) 

Hypothermia is a pure "carryover condition". That means, it has 
no effect on the actions of the characters itself but it sets others conditions which do.

The effects in the table are NOT additive (unlike the tables in the rule book).

| Level | Description | Modifier on          | Effect |
|:-----:|:------------|:---------------------|:------------|
| I     | | Paralysis, Confusion | +1 |
| II    | | Paralysis | +1 |
|       | | Confusion | +2 |
| III   | | Paralysis, Confusion | +3 |
| IV    | | Unconsciousness, apparent death, loss of 1D6 LP per minute | - |

Source: [VR1](https://de.wiki-aventurica.de/wiki/Regelwerk), pp. 346


## Hyperthermia (Überhitzung) 

Hyperthermia is a pure "carryover condition". That means, it has no effect on the actions of the characters itself but it sets others conditions which do.

The effects in the table are NOT additive (unlike the tables in the rule book).

| Level | Description | Modifier on          | Effect |
|:-----:|:------------|:---------------------|:------------|
| I     | | Stupor, Confusion | +1 |
| II    | | Stupor    | +2 |
|       | | Confusion | +1 |
| III   | | Stupor    | +3 |
|       | | Confusion | +2 |
| IV    | | Stupor    | +4 |
|       | | loss of 1D6 LP per minute | - |

Source: [VR1](https://de.wiki-aventurica.de/wiki/Regelwerk), pp. 347





## Further Conditions

At the moment these conditions are not represented in Fate Explorer.

### Rapture (Entrückung)

| Level | Description | Modifier on          | Effect |
|:-----:|:------------|:-----------------------|:------------|
| I     | Slight rapturous  | All skill and spell checks not agreeable to the Blessed One’s god | -1 |
| II    | Rapturous  | All skill and spell checks agreeable to the Blessed One’s god | +1 |
|       | | All other checks      | -2 |
| III   | Divinely inspired | All skill and spell checks agreeable to the Blessed One’s god   | +2 |
|       | | All other checks      | -3 |
| IV    | Implement of the god | All skill and spell checks agreeable to the Blessed One’s god   | +3 |
|       | | All other checks      | -4 |

Requirements:

* Deity connected to character
* List of skills related to that deity
* Two different kinds of impact


### Trance (Trance)

Trance is a mild form of rapture. It can be caused by the use of ceremonial objects or special abilities.

| Level | Description | Modifier on          | Effect |
|:-----:|:------------|:---------------------|:-------|
| I     | Characters feel close to their deity | No AsP regeneration in the next period. | -99 |
| II    | | All checks, except chants and skill checks agreeable to the Blessed One's god | -2 |
|       | | No AsP regeneration in the next period. | -99 |
| III   | | All checks | -3 |
|       | | No AsP regeneration in the next period. | -99 |
| IV    | Incapacitated | All checks | -99 |
|       | | | No AsP regeneration in the next period. | -99 |

Requirements:

* List of affected checks changes between levels
* Effects on Asp regeneration
* Reset on AsP regeneration as soon as regeneration has been denied once

Source: [VR4](https://de.wiki-aventurica.de/wiki/Aventurisches_G%C3%B6tterwirken), p.11



### Desire (Begehren)

| Level | Description | Modifier on          | Effect |
|:-----:|:------------|:---------------------|:------------|
| I     | Slight desire | Willpower (Resist Seduction) against the desired person | -1 |
| II    | | Willpower (resist Seduction) against the desired person | -2 |
| III   | | Willpower (resist Seduction) against the desired person | -3 |
| IV    | | Willpower (resist Seduction) against the desired person | -4 |

Requirements:

* Reference to specific skill which - however - cannot always be applied. So ho to do it in the app?


### Arousal (Erregung)

| Level | Description | Modifier on          | Effect |
|:-----:|:------------|:---------------------|:------------|
| I     | Slightly aroused | All checks on SAG incl. skill checks | -1 |
| II    | Aroused | All checks on SAG incl. skill checks | -2 |
| III   | Very aroused | All checks on SAG incl. skill checks | -3 |
| IV    | Orgasm | Arousal returns to zero | - |

Requirements:

* Reference to a list of skills
* Reference to the ability of the skill check
* Modifier for skill checks that is limited to a specific ability




### Demonic Consumption

| Level | Description | Modifier on          | Effect |
|:-----:|:------------|:-----------------------|:------------|
| I     | I  | Regeneration LeP & AsP -1 | -1 |
|       |    | All checks | -1 |
|       |    | Pact check with the chosen archdemon | +1 |
| II    | II | Regeneration LeP & AsP  | -50% |
|       |    | All other checks        | -2 |
|       |    | Pact check with the chosen archdemon | +2 |
| III   | III | Regeneration LeP & AsP | Denied |
|       |     | All other checks       | -3 |
|       |    | Pact check with the chosen archdemon | +3 |
| IV    | IV | Regeneration LeP & AsP | Denied |
|       |    | All other checks | Denied |
|       |    | Pact check with the chosen archdemon | +4 |



### Honourable Mention

Very specific conditions that apply for only a very small group of characters or game setups.

* Animosity (Animosität)
* Brazirakus heilige Wut (Brazirakus' holy rage)
* Eiskalte Einflüsterung
* Loss of Sikaryan (Sikaryan-Verlust)
* Theriak-Vorrat


## Approach

Most conditions can be handled with two mechanisms:

1. Modifier grid: 
   * List of the modifiers for all skills, attributes, combat actions.
   * Placeholders are allowed.
   * These will be applied when a check is required.
1. Grid of consequences: 
   * List of modifiers for conditions (incl. "this" condition) 
   * Will be applied when the level of the condition is set.

This works for all conditions except demonic consumption, rapture, desire and arousal; further conditions are also excluded.

Also not counting effects on regeneration.





<!-- ---------------- STATES ------------------- -->

# States

## Blind (blind)

* Skill rolls at **discretion of game master**
* Close combat: 
  * AT/2
  * Parry: criticals only (roll of 1 on 1D20)
  * Dodge: criticals only (roll of 1 on 1D20)
* Ranged combat:
  * only criticals hit (roll of 1 on 1D20)
  * Parry: criticals only (roll of 1 on 1D20)
  * Dodge: denied
* Magic: Blind characters must touch opponents
* Chants:  Blind characters must touch opponents


## Bloodlust (Blutrausch)

* Close combat
  * +4 bonus to attacks
  * +2 bonus to damage
  * PA denied
  * Dodge denied
* Ranged combat: denied
* Special abilities: combat
  * Only Forceful Blow.
* Skills
  * Feat of Strength +2
  * Non-physical skills: denied except Intimidation
  *
* Conditions
  * Ignore the effects of the condition Pain
  * When bloodlust ends, the hero gains 2 levels of Stupor
  

## Bound (Fixiert)

* MOV: 0
* Dodge -4


## Burning (Brennend)

(No penalties, damage only and skill checks are required to stop it)


## Cramped (Eingeengt)

Effects on close combat (Source: [VR1](https://de.wiki-aventurica.de/wiki/Regelwerk), pp. 36)


## Deaf (Taub)



## Disease (Krank)

No regeneration; characters lose 1d3 LP if they don't regenerate


## Immobilized (Fixiert)

MOV 0, Dogde -4


## Incapacitated (Handlungsunfähig)

MOV 0, most skill checks aren't possible.


## Invisible (Unsichtbar)

The state Invisible has consequences on others, not on the invisible hero.

Combat situations
* An opponent must be aware of the presence and approximate location of the invisible person: competitive skill check on Perception (Search) vs. Stealth (Sneak).
* If the opponent is aware of a characters location they can fight as if they had the status Blind.


## Mute (Stumm)

* Spells: denied, unless the hero is an elf
* Chants: denied


## Poisoned

No regeneration


## Prone (Liegend)

* AT -4
* PA -2
* Dodge -2
* MOV 1


## Surprised (Überrascht)

One attack actions aganist the character cannot be defended.


## Unconscious (Bewusstlos)


