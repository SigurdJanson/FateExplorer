# About Combat 


## Roll Check Models


```plantuml
@startuml Combat

class CheckBaseM {
  + ICheckContextM Context
}

class AttackCheckM extends CheckBaseM {
  + 
}
class ParryCheckM extends CheckBaseM {
  + 
}
class HruruzatAttackM extends AttackCheckM {
  + 
}
class DodgeCheckM extends CheckBaseM {
  + 
}

@enduml
```




## Overview

```plantuml
@startuml Combat

title Character Combat Relationships

'left to right direction
'top to bottom direction


namespace RollLogic {
  class AttackCheckM {}
  class ParryCheckM {}
}


namespace CharacterModel {
    class CharacterM {
      + List<WeaponM>
    }

    class WeaponM {
      + int GetAt(isMainHand, CombatBranch otherHand)
      + int GetPa(isMainHand, CombatBranch otherHand)
      ----
      + int AtSkill
      + int PaSkill
    }
}


namespace ViewModel {
    'class RollHandlerViMo {
    '  +OpenCombatRollCheck(
    '    CombatTech, Action, Weapon)
    '}

    class RollCheckResultViMo {

    }

    class WeaponViMo {
      # WeaponM WeaponM
      --
      + int AtSkill
      + int PaSkill
    }

    class HandsViMo {
      + WeaponViMo MainWeapon
      + WeaponViMo OffWeapon
    }

    class TheHeroViMo {
      + HandsViMo Hands
    }
}

namespace View {
  class CharacterFight {

  }
}


'ViewModel.RollHandlerViMo 

ViewModel.RollCheckResultViMo *-- RollLogic.AttackCheckM
ViewModel.RollCheckResultViMo *-- RollLogic.ParryCheckM
View.CharacterFight "1" *-- "4" ViewModel.RollCheckResultViMo: displays

View.CharacterFight "1" *-- "1" ViewModel.TheHeroViMo: uses
' View.CharacterFight "1" *-right- "1" ViewModel.HandsViMo: uses
View.CharacterFight "1" o-right- "n" ViewModel.WeaponViMo: uses


ViewModel.WeaponViMo *-right- CharacterModel.WeaponM
ViewModel.TheHeroViMo *--right-- CharacterModel.CharacterM

ViewModel.TheHeroViMo "1" *-up- "1" ViewModel.HandsViMo: has a pair of
ViewModel.HandsViMo "1" *-up- "2" ViewModel.WeaponViMo: carry each a

CharacterModel.CharacterM "0" o-up-- "n" CharacterModel.WeaponM: owns


@enduml
```

