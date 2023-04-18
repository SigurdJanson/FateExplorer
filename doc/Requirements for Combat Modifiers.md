# Requirements for Combat Modifiers

## Effects

Combat modifiers affect effective values (`EV`) in several ways:
- ± x (self-explanatory)
- `min(x, 1)` this check can only be successful with a lucky punch/shot/roll of a 1.
- &infin; makes the check impossible. You can't do it under any circumstances, like a parry action against an ogre with a tooth pick.

In any case after modification the final operation shall set `EV = max(EV, 0)`. All checks against 0 fail automatically. All checks against 1 can only succeed if critical.


## Target Size

| | Size  | Example |
|-|---|---|
| 0 | Tiny   | Ratte, Kröte, Spatz    |
| 1 | Small  | Rehkitz, Schaf, Ziege  |
| 2 | Medium | Mensch, Zwerg, Esel    |
| 3 | Large  | Oger, Troll, Rind      |
| 4 | Huge   | Drache, Riese, Elefant |


* Close combat: melee and unarmed
  * Parry actions against >= *large* are not possible
  * Attack -4 against *tiny*
* Close combat: shield
  * Parry actions against *huge*
  * Attack -4 against *tiny*
* Ranged
  * Attack from s1 to 5: -8, -4, 0, 4, 8
  * Parry. 
    * Some ranged weapon can be used as improvised parry weapon.
    * Parry actions against >= *large* are not possible
* Dodge is always possible



## Under Water

> Note! Many of these levels are not explicitly part of the rules. Documented is the -2 for waist-deep water. So is the -6 for submerged in close combat and the &infin; in ranged combat.

|   |   |
|---|---|
| 0 | Dry       |
| 1 | KneeDeep  |
| 2 | WaistDeep |
| 3 | ChestDeep |
| 4 | NeckDeep  |
| 5 | Submerged |

* Attack, parry from s1 to s5: 0, -1, -2, -4, -5, -6
* Ranged: 
  * Attack from s1 to 5: 0, 0, 2, &infin;, &infin;, &infin;
  * Parry: no modifiers
* Dodge from s1 to s5: 0, -1, -2, -4, -5, -6

## Visibility

|   | Visibility | Descr. | Example |
|---|---|---|---|
| 0 | Clear       | Klare Sicht                   |  |
| 1 | Impaired    | Leichte Störung der Sicht     | Leichtes Blattwerk, Morgendunst |
| 2 | ShapesOnly  | Ziel als Silhouette erkennbar | Nebel, Mondlicht |
| 3 | Barely      | Ziel schemenhaft erkennbar    | Starker Nebel, Sternenlicht |
| 4 | NoVision    | Ziel unsichtbar               | Dichter Rauch, völlige Dunkelheit |



### Skills

Visibility also affects skills


### Close Combat

Melee, Unarmed, Shield; incl. dodge actions

|   | Visibility | Mod AT | Mod PA, DO |
|---|---|---|---|---|
| 0 | Clear       |  0  |  0  |
| 1 | Impaired    | -1  | -1  |
| 2 | ShapesOnly  | -2  | -2  |
| 3 | Barely      | -3  | -3  |
| 4 | NoVision    | x/2 |  `min(1, x)` |


### Ranged Combat

|   | Visibility | Mod AT |
|---|---|---|---|
| 0 | Clear       |  0 |
| 1 | Impaired    | -2 |
| 2 | ShapesOnly  | -4 |
| 3 | Barely      | -6 |
| 4 | NoVision    | `min(1, x)` |






# Close Combat, Only

## Weapon's Reach


| H / E | Hero | Enemy |
|---|---|---|
| Short  / Short  |  0 |  0 |
| Short  / Medium | -2 |  0 |
| Short  / Long   | -4 |  0 |
| Medium / Short  |  0 | -2 |
| Medium / Medium |  0 |  0 |
| Medium / Long   | -2 |  0 |
| Long   / Short  |  0 | -4 |
| Long   / Medium |  0 | -2 |
| Long   / Long   |  0 |  0 |



## Cramped Space

| Art | Mod AT | Mod PA |
|----|---|---|---|
| Kurze Waffen     | +/–0 | +/–0 |
| Mittlere Waffen  | –4   |  –4 |
| Lange Waffen     | –8   |  –8 |
| Kleine Schilde   | –2   |  –2 |
| Mittlere Schilde | –4   |  –3 |
| Große Schilde    | –6   |  –4 |



# Ranged Combat, Only

## Target Distance

|   | Distance | Mod |
|---|---|---|
| 0 | Close    | +2 |
| 1 | Medium   |  0 |
| 2 | Far      | -2 |



## Hero Movement

* Human speed categories: `enum {Stationary = 0, Slow = 1, Fast = 2}`
* Horses speed categories: `enum {Stationary = 0, Walk = 1, Trot = 2, Gallop = 3}`

|   | Movement | |
|---|---|---|---|
| 0 | Stationary | 0|
| 1 | Slow       | < 4 yds / action |
| 2 | Fast       | > 4 yds / action |


### On Foot
|   | Movement | Mod AT |
|---|---|---|
| 0 | Stationary    |  0 |
| 1 | Slow          | -2 |
| 2 | Fast          | -4 |


### Mounted
|   | Movement | Mod AT |
|---|---|---|
| 0 | Stationary    |  0 |
| 1 | Walk          | -4 |
| 2 | Trot          | min(x, 1) |
| 3 | Gallop        | -8 |


## Opponent Movement

|   | Movement | Mod AT |
|---|---|---|
| 0 | Stationary    |  2 |
| 1 | Slow          | 0  |
| 2 | Fast          | -2 |


## Evasive Movements of Opponent

|   | Movement | Mod AT |
|---|---|---|
| 0 | Straight    |  0 |
| 1 | Zigzag      | -4 |

Zigzag also halves the speed


## Taking Aim

Each action you spend aiming gives a bonus of 2 (maximum of 4) to your next shot. Aiming counts as a long action until you actually make the attack (see Long Actions, page 228).


## Shooting into Melee

If a ranged combatant fires at an enemy engaged in close combat (in other words, if another combatant is within attack distance of the target), the shot suffers a penalty of 2. If the check fails, no one is hit.

