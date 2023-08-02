# Aventurian Dates

## Sources
http://liebliches-feld.net/wiki/index.php/Jahreskalender_(Fortsetzung)

## Current Date

Fate Explorer uses the **method of "equal turn of years"**. Many official publications use this method.

* Default calendar uses Bosparans Fall
* Year: (Aventurian year BF) = (earth year) - 977
* New Years on earth is the 1. Praios
* Each year has ...
  * 365 days without exceptions.
  * 12 months and 5 extra days.
* Months
  * Order of months: Praios, Rondra, Efferd, Travia, Boron, Hesinde, Firun, Tsa, Phex, Peraine, Ingerimm, Rahja.
  * Each month has exactly 30 days.
* The 5 extra days at the end of the year are the days of the Nameless One. They follow the month of Rahja.



### Week Days

German:

|Tag|Mittelreich|Abkürzung|Abweichungen|Irdische<br/>Entsprechung|Anmerkungen|
|---|-----------|---------|------------|--------------------------|-----------|
|1. |Windstag |Wi | Thorwal 'Trondesdag' (vor 1027 BF  'Orozarsdag') |Donnerstag |
|2. |Erd(s)tag |Er | Thorwal 'Ifirnsdag'|Freitag |alle vier Wochen Vollmond oder Neumond |
|3. |Markttag |Ma | Horasreich 'Horastag'<br/>Thorwal 'Firunsdag' |Samstag |
|4. |Praiostag |Pr/Bt/St |Neetha & Herzogtum Weiden & bei den Amazonen 'Rondratag'<br />Thorwal 'Swafnirsdag'<br />im tiefen Süden (Al'Anfa, Káhet Ni Kemi, Mengbilla) 'Borontag'|Sonntag |allgemeiner Ruhetag |
|5. |Rohalstag |Ro |Bornland 'Schneetag'<br/> in Thorwal 'Traviasdag' |Montag |
|6. |Feuertag |Fe |Thorwal 'Jurgasdag' |Dienstag |
|7. |Wassertag |Wa |Bornland 'Zinstag'<br/>Thorwal 'Hjaldisdag'|Mittwoch |


## Requirements

* Convert earth dates to BF

Days in the BF schema
* Each calendar can have it's own day names
* Map leap days to the previous day (i.e. a 29th of Feb will repeat the day before)
* Map week days according to each calendar/region

Weeks (in the BF schema)
\-

Months (in the BF schema)
* Name months according to BF calendar
* Each calendar can have it's own month names

Years (in the BF schema)
* Calendar may have deviating New Year days
* Convert years of each calendar to BF and vice versa
* Handle missing year 0

Open schema
* Other calendars may slice time into very different units
* Support years that do not have 365 days

| # | Aventurian | Seasonal Equivalent | Solar Equivalent |
|-- | ---------- | ------------------- | ---------------- |
| 1| Praios     | July     | January |
| 2| Rondra     | August   | February |
| 3| Efferd     | September | March |
| 4| Travia     | October  | April  |
| 5| Boron      | November | May    |
| 6| Hesinde    | December | June   |
| 7| Firun      | January  | July   |
| 8| Tsa        | February | August |
| 9| Phex       | March | September |
|10| Peraine    | April | October   |
|11| Ingerimm   | May   | November  |
|12| Rahja      | Juni  | December  |
|13| Namenloser | Juli  | December  |

## Calendars

Only calendars with a * will be implemented at first.

Overview over year conversions:
<table>
<thead>
    <tr>
        <th>#</th>
        <th>Name</th>
        <th title="Notation for years >= 0">Jahr+</th>
        <th title="Notation for years < 0">Jahr-</th>
        <th title="Umrechnung zur Zeitrechung nach Bosparans Fall">Jahr 0 BF</th>
        <th>Jahr 0</th>
    </tr>
</thead>
<tbody>
    <tr><td>0</td><td>Horas</td><td>Horas</td><td>v.Hor</td><td>1492</td><td>nein</td></tr>
    <tr><td>1</td><td>der Unabhängigkeit (Nostria/Andergast)</td><td>d.U.</td><td>v.d.U.</td><td>855</td><td>ja</td></tr>
    <tr><td>2</td><td>Engasal</td><td>nach der Engalsal-Akte</td><td>vor der Engasal-Akte</td><td>346</td><td>nein</td></tr>
    <tr><td>3</td><td>Bosparans Fall</td><td>BF</td><td>v.BF</td><td>0</td><td>ja</td></tr>
    <tr><td>4</td><td>Priesterkaiser, Jahre des Lichts</td><td>Jahr des Lichts</td><td>vor den Jahren des Lichts</td><td>-335</td><td>nein</td></tr>
    <tr><td>5</td><td>nach Kurkum</td><td>nach Kurkum</td><td>vor Kurkum</td><td>-416</td><td>nein</td></tr>
    <tr><td>6</td><td>Golgaris Erscheinen</td><td>GE</td><td>v.GE</td><td>-685</td><td>nein</td></tr>
    <tr><td>7</td><td>Perval</td><td>Perval</td><td>v.P.</td><td>-933</td><td>ja</td></tr>
    <tr><td>8</td><td>Bardo Cella</td><td>BC</td><td>v.BC</td><td>-948</td><td>ja</td></tr>
    <tr><td>9</td><td>Reto</td><td>Reto</td><td>v.Reto</td><td>-975</td><td>ja</td></tr>
    <tr><td>10</td><td>Hal</td><td>Hal</td><td>v.Hal</td><td>-993</td><td>ja</td></tr>
    <tr><td>11</td><td>Aranien</td><td>Aran.</td><td>v.d.U. (Aran.)</td><td>-995</td><td>nein</td></tr>
    <tr><td>12</td><td>Trahelien</td><td>Trah.</td><td>v.d.U. (Trah.)</td><td>-997</td><td>nein</td></tr>
</tbody>
</table>


## International: Bosparan's Fall<sup>*</sup> (neutral culture)

### Years 
* Identifier: "# BF" (for positive numbers) and "# v. BF" (negative numbers); English: "# b. BF"

### Weekdays
```json
"Windstag", "Erdtag", "Markttag", "Praiostag", "Rohalstag", "Feuertag", "Wassertag"
"Wi", "Er", "Ma", "Pr", "Ro", "Fe", "Wa"
"W", "E", "M", "P", "R", "F", "W"
```

### Months
```json
"Praios", "Rondra", "Efferd", "Travia", "Boron", "Hesinde", "Firun", "Tsa", "Phex", "Peraine", "Ingerimm", "Rahja", "Namenlose Tage"
"PRA", "RON", "EFF", "TRA", "BOR", "HES", "FIR", "TSA", "PHE", "PER", "ING", "RAH", "NL"
```

## Mittelreich: Hal<sup>*</sup>

### Years

Hal's calendar counts years differently:

* Conversion: (Hal) + 993 = (BF)
* Identifier: Hal

### Weekdays
see Bosparan's Fall

### Months
see Bosparan's Fall


## Andergast & Nostria

The warring kingdoms count the years with their independence. They share their identifier with other regions honouring their independence using their calendar.

* Identifier: #### d.U. (der Unabhängigkeit); in Englisch "#### o.I." (of independence)
* \# d.U. = \# BF + 854

### Weekdays
see Bosparan's Fall

### Months
see Bosparan's Fall



## Arania

* Identifier: #### d.U. (der Unabhängigkeit); in Englisch "#### o.I." (of independence)
* \# d.U. = \# BF - 994

### Weekdays
see Bosparan's Fall

### Months
see Bosparan's Fall



## Kahet ni Kemi

* Identifier: #### d.U. (der Unabhängigkeit); in Englisch "#### o.I." (of independence)
* \# d.U. = \# BF - 996


## Horasian Empire

The Horasian Empire uses both calendars, BF and the reckoning since Horas' Arrival (1491 b. BF ).

* Identifier: "#### Horas" or "#### v. Horas"; in Englisch "#### b. Horas"
* There is no year 0.
* Conversion: (Horas) = (BF) + 1492

### Weekdays 
```json
"Windstag", "Erdtag", "Horastag", "Praiostag", "Rohalstag", "Feuertag", "Wassertag"
"Wi", "Er", "Ho", "Pr", "Ro", "Fe", "Wa"
"W", "E", "H", "P", "R", "F", "W"
```

### Months
see Bosparan's Fall




## Al'Anfa<sup>*</sup>

Golgaris calendar counts years since the Arrival of Golgari (686 BF). It is used only in the Empire of Al'Anfa

* Identifier: "#### GE" (Golgaris Erscheinen) and "#### v. GE"; in Englisch "#### b. GE"
* There is no year 0.
* Conversion: (GE) = (BF) - 685

### Wochentage
```json
"Windstag", "Erdtag", "Markttag", "Borontag", "Rohalstag", "Feuertag", "Wassertag"
"Wi", "Er", "Ma", "Bo", "Ro", "Fe", "Wa"
"W", "E", "M", "B", "R", "F", "W"
```

### Monate
see Bosparan's Fall


## Thorwal

\- 

### Weekdays
```json
"Trondesdag", "Ifirnsdag", "Firunsdag", "Swafnirsdag", "Traviasdag", "Jurgasdag", "Hjaldisdag"
"Tr", "If", "Fi", "Sw", "Tr", "Ju", "Hj"
"T", "I", "F", "S", "T", "J", "H"
```

### Months
```json
"Midsonnmond", "Kornmond", "Heimamond", "Schlachtmond", "Sturmmond", "Frostmond", "Grimfrostmond", "Goimond", "Friskenmond", "Eimond", "Faramond", "Vinmond", "Hranngartage"
```


## Maraskan<sup>*</sup>

The essential date for Maraskan is when the god Rur threw the "Disque that is the World" to his brother Gror. That was the 19. Rondra 3822 v. BF when everything began.

* Identifier: "FdW" (Flug des Weltendiskus); English: "FoW".
* Conversion: (FdW) = (BF) + 3822
* There are no negative numbers. The world did not exist before that year.
* Special date is the 19. Rondra 274 BF. On that day the Beni Rurech arrived at Maraskan. It is supposed to be the day when the Disque that is the World has crossed half of the distance between the two god brothers.
* That means there can be no year beyond 4096 BF.

Open questions: what about months and days???



## Dwarves

The dwarves do not have a systematic way to count years. Among each other they use important events to identify dates, e.g. 3x3 years after the ................ of Grandfather Amaxoschs.

* Conversion: not possible

### Weekdays
see Bosparan's Fall

### Months

Dwarves use use the months from Bosparan time but they use different names for the months:

```json
"Sommermond", "Hitzemond", "Regenmond", "Weinmond", "Nebelmond", "Dunkelmond", "Frostmond", "Neugeburt", "Marktmond", "Saatmond", "Feuermond", "Brautmond", "Drachentage"
```

* Instead of "Feuermond" some use "Feiermond"
* English translation: 
  * Summer Moon, Heat Moon, Rain Moon, Wine Moon, Fog Moon, Dark Moon, Frozen Moon, Rebirth, Market Moon, Sewing Moon, Fire (also Feast) Moon, and Bride Moon.
  * The Nameless days are called Dragon days (Drachentage)


# L10N["Bornland"]

## Weekdays
```json
"Windstag", "Erdtag", "Markttag", "Borontag", "Schneetag", "Feuertag", "Zinstag"
"Wi", "Er", "Ma", "Bo", "Sc", "Fe", "Zi"
"W", "E", "M", "B", "S", "F", "Z"
```

## Monate

* TODO


## Novadic<sup>*</sup>

* Week days: ########## Zu den novadischen Tagesnamen siehe Wochentag.
* The year starts with the 23. Boron
  * On that day Rastullah arrived in Keft.
* The days are identifed by one of the God's names and one of 9 days. The remaining 5 days are a holy festive season called "Rastullahellah".
* Format: 
  * vor der Offenbarung (v. d. O.) / before the Revelation (b.t.R.)
  * nach der Offenbarung (n. d. O.) / after the Revelation (a.t.R.)
* Conversion: (n. d. O.) = (BF) - 759
* There is no year 0.



## Black Lands (Schwarze Lande)

### Months

```json
"Blakharaz", "Belhalhar", "Charyptoroth", "Lolgramoth", "Thargunitoth", "Amazeroth", "Belshirash", "Asfaloth", "Tasfarelel", "Belzhorash", "Agrimoth", "Belkelel", "Namenlose Tage"
```


## Amazons

In the Kingdoms of the AMazons they celebrate the completion of Castle Kurkum.

* Format: vor/nach Kurkum; English: "before/after Kurkum".
* Conversion: (nach Kurkum) = (BF) - 415



## Jilaskan
\-




## E. File

\-



## L10N[NORBARDEN]

\-

## Orcs
\-

## L10N[GJALSKER]
\-

## L10N[SAURIAN/LIZARDS]

* A week ("wss") has 5 days (day = "ggg"): Sz'G, Drs'G, Gzht'G, Lhn'G, Rsz'G (in this order).
* A month ("ffn") has 33 days.
* 553 months make a (unnamed) unit of 50 solar years.
  * According to an [unauthorized player aid](http://asboran.de/wp-content/uploads/2017/05/Echsisch-f%C3%BCr-Menschen.pdf) this is a *krstl*.
  * Though the unit itself is unnamed each of them has a name: Monitor, Gorger, Snake, Salamander, Dragon, Winged Lizard, Turtle, Chameleon, Sea Serpent, and Crocodile.
* 10 of those unnamed units make an *Ehhn*. An Ehhn are 500 solar years. 
* 33 Ehhn make a *Tsiina* (16500 solar years).

Example: 

The date  1. Praios 1000 BF is the 18. Tag of 219. month in the section of the Dragon, in 4. Ehhn of the 4. Tsiina. It is a Gzht'G.

Dating this back to the first day of the Saurian calendar should be: 14. Rahja 50217 v. BF.



<!-- ****************************************************************** */ -->
------------------------------------------------------------------------------
<!-- ****************************************************************** */ -->
# Holidays

Sources: VG1, VG2, VG3, VG4, http://pdiefenbach.de/dsatool, Wiki Aventurica


## Holidays not in Data Base

* Several trade fairs of the Almanach.
| Markt und Spiele, Lowangen (Market and Games) | TRA and PHE; ????last week of Phex | |
Many holidays found in the "Regionalspielhilfen"


**Template**
            {"month":  2, "day": 22, "duration": 1,  "name": "", "descr": ""},


**??**

* Nameless Days, "Storm Time Warring Kingdoms", "Prayers to Elida, a saint of Efferd, especially in coastal regions and Salta"
* Anfang Phex Greifenpassrennen zur Öffnung des Passes; der erste Händler, der Gratenfels  erreicht, ist für das Jahr von Markt- und Wegzöllen befreit.
* Mitte Phex - Albenhuser Fassfest
* 

**? Translation of Twergenhus**

PHE 22.-24. "Twergenhausener Metallwarenmesse"

            {"month":  9, "day": 22, "duration": 3,  "name": "Twergenhausener Metallwarenmesse", "descr": ""},
            {"month":  9, "day": 22, "duration": 3,  "name": "Twergenhausener Hardware SHow", "descr": ""},



## Floating Holidays

| Holiday  | When? | Descr |
|---|---|---|
| Madatag (Madaday) | PRA, 1st Earthday | Von vielen Zauberkundigen gefeiert. Zauberer gedenken Mada. Häufig wird der Tanz der Mada getanzt, Häufig kommt es zu Auseinandersetzungen mit den gleichzeitig stattfindenden Praiosfeierlichkeiten. |
| Koschtaler Bierfest | TRA, 1st Marketday | Koschtaler Bierfest; beliebtes Volksfest mit Märkten, Auktionen und vor allem Bier |
| Festumer Warenschau | TRA und ING, starts with 1. Marketday, lasts 1 week | |
| Purgatoria | TRA, last Praiosday |  |
| Warenschau und große Sklavenauktion | BOR ab dem 2. Boronstag vier Tage lang |  |
| Avestag | PHE, last Windsday | Avestag, guter Tag für die erste große Schiffsreise nach dem Winter, Begrüßung der zurückkehrenden Zugvögel |
|	Aves-Rennen, Gareth | PER 2. Windsday | |
| Big Cattle Market, Andergast | PER, 1st Week | |
| Elenviner Ross- und Wagenmarkt, Elenvina | PHE, 1. Windsday | |
| Tolles Treiben, Angbar | PHE, 1st Marketday | Blumengeschmücktes Vieh wird durch die Gassen getrieben |
| Elenviner Handelshallenausstellung, Elenvina | PHE, 2. Marketday |  |
| Wahl der Fischkönigin, Nostria (Election of the Fisher Queen) | EFF, first new moon | by the Stone of Nosteria |
| Bukenbrinn | ING, 1. full moon | Verehrung und Feier Sumus. Man hält Wacht gegen böse Geister und sucht die Druiden auf. |
| Horse Market in Teshkal | ING, 1. full moon |  |
| Day of Andra's Sacrifice, Andergast | TSA, 1st full moon | Memorial Day in honor of Andrafall |


## Floating Holidays (not in Data Base)

| Herzogenturnier, Elenvina | TRA 10.-11., alle sechs Jahre | |

* Oxen Market of Baliho (18th-22nd)
* | Immanmeisterschaften | TRA Ab 2. Rohalstag zwei Wochen lang | |
* | Regatta der Sieben Winde auf den Zyklopeninseln | PER letzter Windstag | |
* PER Mitte	Viehmarkt in Andergast (UdW S. 145)		Andergast
* PER 1. Woche - Gratenfelser Schützenfest, alle drei Jahre
* Markttag im Phex, der auf den 1. Tag eines Monatsfünftels fällt: Horas’ Entrückung. Feiertag im Horasreich.


## Possible Local Additions

* TRA 30. "Nacht der Ahnen (AGF Seite 84), Havena", "Nacht auf den 1. Boron" - ***Das Totenfest ist bereits berücksichtigt, hat aber eine besondere Bedeutung in Havena***.
* https://de.wiki-aventurica.de/wiki/Andrafaller_Holzf%C3%A4llerspiele
* http://wiki.nordmarken.de/bin/view/Nordmarken/NordmaerkerFeste


## Requirements for Floating Holidays

* n-th weekday W in month M (e.g. Madaday)
* Last weekday W in month M (e.g. Purgatoria)
* \1. Moon phase in month
* (Last-n)th week in month M
* n-th week in month M
* (Last-n)th day in month, e.g. Aves day

## Special Holidays

* Fixed day but only once every N years; reference year required (e.g. Gratenfelser Schützenfest)




<!-- ****************************************************************** */ -->
------------------------------------------------------------------------------
<!-- ****************************************************************** */ -->
## Questions

* Can local customs for country-wide be handled?
* How to handle dates that are likely to change often?
  * e.g. Tsatag des Fürsten, Kosch", "Trinken auf das Wohl des Fürsten"
  * RAH 26. Tsatag der Kaiserin (Kosch); man trinkt auf Rohajas Wohl
  * TRA 7. "Befreiungstag, Elburische Halbinsel", "Feiert das Ende der oronischen Herrschaft"

