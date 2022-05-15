
# Currencies

## Middenrealm Ducat

Coins, weight, and value:

| Coin | Weight | Value | Symbol | Ref |
|---|---|---|---|---|
| Ducat        |   25 g |  0.1 S | D | A127 |
| Silverthaler |    5 g |    1 S | S |   WA |
| Haler        |  2.5 g |  10 S | H |   WA |
| Kreutzer     | 1.25 g | 100 S | K |   WA |



# Requirements

* Each currencies is split into various coins
* One of the coins must be the reference coin 
  * for conversions between currencies
  * Coins add up to a total amount
  * Total amount should be expressed in reference coin unless explicitly requested.
* Each coin has a weight
* `ToString()` feature with formatting options:
  * Choose the coin(s)
  * Optimal coin: split the amount so that the smallest number of coins remains
* I18N
  * Names should be localisable
    * UI language
    * (Aventurian) Native language
  * Abbreviations should be localisable
    * UI language
    * (Aventurian) Native language
  * An object should include the currency
    * Currency conversion creates new object with equivalent money in new currency
* Constructors
  * One with total amount
  * One with an amount for each coin



# Sources

A127 - ["Drei Millionen Dukaten"](https://de.wiki-aventurica.de/wiki/Drei_Millionen_Dukaten)
WA - [Currencies in the Wiki Aventurica](https://de.wiki-aventurica.de/wiki/W%C3%A4hrung)