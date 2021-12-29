using FateExplorer.WPA.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FateExplorer.WPA.GameData
{
    interface IGameDataService
    {
        public AbilitiesDB Abilities { get; }

        public CombatTechDB CombatTechs { get; }
        
        public Task InitializeGameDataAsync();
    }
}
