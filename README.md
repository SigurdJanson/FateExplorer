# FateExplorer PWA

An interface to roll the dice based on the ["dark eye" rule system](https://ulisses-regelwiki.de/index.php/home.html) (version 5).

The rule system is sophisticated. This app makes it easier to handle the rules and helps you to focus on your gameplay, your character and your story.

Moved the Fate Explorer from R Shiny to .NET Blazor


## Features

* Use your character from [Optolith character sheets](https://optolith.app/en/)
* Manage health, arcane energy, and karma and get direct feedback.
* Ability rolls
* Skill rolls incl. routine checks for mundane, magical or blessed skills
* Combat rolls
  * *NEW* in version 2: initiative rolls
* Display of probabilities for different results
* Bilingual (English and German - can be changed in the code of the script)


## Roadmap

* FateExplorer can consider the combat situation like weapons range, movement, or visibility.


## Limitations at Current State of Development

* Users set the modifiers manually in the UI of the app and roll the dice. The character sheet import does not support special abilities, states, conditions, etc. 
* Language can only be changed in the source code of the script.
* FE is still ignorant of some very pecial rules for weapons; e.g. rolling a 19 with an improvised weapon is actually a botch but FE is not aware of that.
* FE does not know rules that are specific for a weapon.
