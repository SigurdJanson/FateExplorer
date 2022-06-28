# DSA FateExplorer - PWA

An interface to roll the dice based on the ["dark eye" rule system](https://ulisses-regelwiki.de/index.php/home.html) (version 5). That rule system is vast and sophisticated. This app makes it easier to handle the rules and helps you to focus on your gameplay, your character and your story.

* **Focus on your character**. Fate Explorer is your smart character sheet with the intention to relieve you of many tasks. You can create characters with the tool Optolith and play with them with Fate Explorer.
* **Smoother game play**. Less googling or browsing through books to find what you need. Fate Explorer helps game masters and players makes information easily accessible.
* **Enrich your game play**. Find relevant knowledge about the vast DSA world that supports your game play. With in-app information and references to the Ulisses Wiki you can get lots of details.

This is the fourth release with the code name **Phexens Sneeze Stimulus**.


## Features

* Use your character from [Optolith character sheets](https://optolith.app/en/)
* Manage health, arcane energy, and karma and get direct feedback of pain levels crossed.
* Character:
  * *New in v4*: Your **character's age** will be computed based on the date when your game takes place (see below).
  * *New in v4*: The **travel purse** helps you track the money you are carrying.
* Ability rolls
* Skill rolls for mundane, magical or blessed skills
  * *New in v4*: Routine skill checks
* Combat rolls
  * Attack & parry
  * Dodge, initiative
  * Left, right, and two-combat
  * *New in v4*: Support of "Combat Style Special Abilities" **Hruruzat**.
  * *New in v4*: Improvised weapons.
* *New in v4*: A calendar enhances ambience of the game play shows you the current date, day of week, moon phase, and holidays.
* A bazaar allows you to choose among over 600 items. FateExplorer does the haggling for you and gives you the total price.
* *New in v4*: FE stores data locally so that you can pick up where you left off once you have loaded a character.
* Bilingual (English and German)



## Outlook for Upcoming Release

To be decided ...



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
* FE is still ignorant of some very special rules for weapons. There are so many special combat abilities; many of them cannot be reproduced in an app.
* FE does not (and probably will never) know rules that are specific for a weapon.



## History

Moved the Fate Explorer from R Shiny to [.NET Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor?msclkid=36ec3b93b1da11ec8ab5eea725ae4f42) as progressive web app.


