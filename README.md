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


## Roadmap

* FateExplorer can compute the appropriate modifiers in combat situation like weapons range, movement, or visibility.


## Limitations at Current State of Development

* Users set the modifiers manually in the UI of the app and roll the dice. The character sheet import support only a small number of special abilities and advantages, no states or conditions, etc. 
* FE is still ignorant of some very pecial rules for weapons; e.g. rolling a 19 with an improvised weapon is actually a botch but FE is not aware of that.
* FE does not know rules that are specific for a weapon.
