# DSA FateExplorer - PWA

An interface to roll the dice based on the ["dark eye" rule system](https://ulisses-regelwiki.de/index.php/home.html) (version 5).

The rule system is sophisticated. This app makes it easier to handle the rules and helps you to focus on your gameplay, your character and your story.

Moved the Fate Explorer from R Shiny to [.NET Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor?msclkid=36ec3b93b1da11ec8ab5eea725ae4f42) as progressive web app.


## Features

* Use your character from [Optolith character sheets](https://optolith.app/en/)
* *NEW* in version 3: Manage health, arcane energy, and karma and get direct feedback.
* Ability rolls
* Skill rolls incl. routine checks for mundane, magical or blessed skills
* Combat rolls
  * Attack & parry
  * Dodge
  * Initiative rolls
  * *NEW* in version 3: combat rolls support left/right hand rules
* Bilingual (English and German)
* *NEW* in version 3: Supports advantages *ambidextrous*, *wizard*, 
* *NEW* in version 3: Supports special ability *two-handed combat*


## Outlook for Upcoming Release

The next release is going by the name "Phexens Sneeze Stimulus". It will include several new groundbreaking features:

* **Calendar**: a calendar to enhance ambience of the game play shows you the current date (or any date that fits your current game).
  * Including the day of the week so you never miss a Praiosday or a Marketday.
  * Including many (fixed) holidays.
  * Including Moon phases.
  * The app remembers your last date (stored locally in a cookie).
* Character:
  * Your **character's age** will be computed based on the date when your game takes place.
  * The **travel purse** helps you track the money you are carrying.
  * Several (dis-) advantages are recognized to compute the energy points (HP, AE, KP) correctly.
* **Routine skill checks**
* Combat
  * Ranged weapons show the **range brackets**.
  * Support of "Combat Style Special Abilities" **Hruruzat**.
  * Improvised weapons now correctly count the 19 as botch.
  * ...and a few minor layout improvements.
* **Persistence**: FE stores data locally so that you can pick up where you left off once you have loaded a character.
  * Effective character values are stored (locally in a cookie).
  * App settings will be stored (locally in a cookie).
* Bazaar: many more bazaar items have **structure points**, now. For gemstone the hardness was deduced from the Vickers hardness scale.

... and many bug fixes.

## Roadmap

* Allow the user to set effective values for basic abilities COU, SAG, ...
* Feature to subtract falling damage from HP (VR1, p. 340).
* A master sheet to handle group fights.
* Keep track of the character's states and conditions.
* Modifier computer for combat situations (visibility, reach, close quarters, under water, ...)
* FateExplorer can compute the appropriate modifiers in combat situation like weapons range, movement, or visibility.
* Details for movement and jumping (VR1 349 and 350)
* Bazaar: explain the value of money.
* Extra page to keep track of character's belongings.
  * Healing and magic potions.
  * Add magical artefacts that allow anyone to cast magic.
* Show the probabilities of roll checks.

## Limitations at Current State of Development

* Users set the modifiers manually in the UI of the app and roll the dice. The character sheet import support only a small number of special abilities and advantages, no states or conditions, etc. 
* FE is still ignorant of some very pecial rules for weapons; e.g. rolling a 19 with an improvised weapon is actually a botch but FE is not aware of that.
* FE does not know rules that are specific for a weapon.
