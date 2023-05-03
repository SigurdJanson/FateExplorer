# Fate Explorer Code Manual


## Basic Terms and Types

* A **proficiency value** is anything you can use for a roll. May it be a check against a limit (e.g. ability checks) or a combative check (like initiative).
  * Ability values
  * Combat values attack and parry
  * Dodge
  * Initiative
  * Skill values
  * Movement
* An **energy** may be life, astral or karma energy.
  * Roles may add/reduce energy.
* A **resilience** is either spirit or toughness.


A `Check` is a sequence of `Roll`s. It needs at least one primary roll. The required rolls in the sequence may depend on context. Roll types (enum `RollType`) are:

* **Primary**. Some rolls end here. E.g. an initiative roll.
* **Confirm**. A confirmation roll depending on the result of the primary roll. Dark Eye example: An ability roll of 1 requires a second confirmation roll to determine if the 1 is a critical.
* **Botch**. A botch roll determines the consequences of a botched check.
* **BotchDamage**. If the character suffers damage as the result of a botch, this roll determines that.
* **Damage**. Attack rolls only.


Roll modifiers are applied on several levels:

1. Character 1: modifiers received through dis-/advantages.
2. Character 2: special abilities are handled differently because they apply only depending on the situation.
3. Asset: weapons may imply modifiers.
4. Context: the free additive modifier on each character sheet is a context modifier. It is applies last.


# Known limitations

## Relevant for role play

Fate explorer is not aware of anything that happens around the character you are playing.

* Date conversion does not consider that time went twice as fast in the past.
* Special abilities and Dis-/Advantages
  * Only a limited collection of special abilities are implemented. Requests are possible on Github.
  * Context dependent special abilities cannot be considered and has to be handled by players. Change the free modifiers to handle it accordingly.
*  A halved check after an opponent's critical attack has to be set manually (due to unawareness of surroundings).



## In the code

* Modifier cannot sum up two (or more) halve modifiers.
* Skill checks (incl. routine checks) work only with additive modifiers.
