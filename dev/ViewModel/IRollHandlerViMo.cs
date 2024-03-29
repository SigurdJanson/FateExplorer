﻿using FateExplorer.RollLogic;
using FateExplorer.Shared;
using System.Threading.Tasks;

namespace FateExplorer.ViewModel
{

    /// <summary>
    /// Once registered at the <see cref="WebAssemblyHostBuilder"/> you have 
    /// to call <see cref="ReadRollMappingsAsync"/> and <see cref="RegisterChecks"/>
    /// </summary>
    public interface IRollHandlerViMo
    {
        /// <summary>
        /// Reads the mappings from a file that connect the attribute id for which the user 
        /// requested a check to the id of the class that does the check (e.g. "TAL" for 
        /// basic skill checks to "DSA/skill/mundane"). <br/>Call this after registering the service.
        /// </summary>
        /// <returns></returns>
        Task ReadRollMappingsAsync();

        /// <summary>
        /// 
        /// </summary>
        void RegisterChecks();


        /// <summary>
        /// Does a skill support a routine check?
        /// </summary>
        /// <param name="Skill">The skill to be used</param>
        /// <param name="Abilities">Abilities needed to check that skill</param>
        /// <param name="Modifier">The check's modifier</param>
        /// <returns>true if the routine check can be performed; otherwise false</returns>
        bool CanRoutineSkillCheck(SkillsDTO Skill, AbilityDTO[] Abilities, Modifier Mod);


        /// <summary>
        /// Does a skill support a routine check?
        /// </summary>
        /// <param name="Skill">The skill to be used</param>
        /// <param name="Abilities">Abilities needed to check that skill</param>
        /// <param name="Modifier">The check's modifier</param>
        /// <returns>true if the routine check can be performed; otherwise false</returns>
        RollCheckResultViMo OpenRoutineSkillCheck(Check AttrId, SkillsDTO Skill, AbilityDTO[] Abilities, CheckContextViMo Context);



        /// <summary>
        /// Creates and returns a new (unmodified) roll check with the first roll
        /// already done.
        /// </summary>
        /// <param name="AttrId">The attribute id</param>
        /// <param name="AttrData">The skill or ability to be used</param>
        /// <param name="RollAttr">Additional ability or skill values needed to 
        /// perform the roll (a skill roll e.g. needs 3 abilities)</param>
        /// <returns>The result object of the roll check</returns>
        RollCheckResultViMo OpenRollCheck(Check AttrId, ICharacterAttributDTO TargetAttr, ICheckContextViMo Context, ICharacterAttributDTO[] RollAttr = null);


        /// <summary>
        /// Creates and returns a new (unmodified) combat roll check with the first roll
        /// already done.
        /// </summary>
        /// <param name="actionId">String distinguising a type of combat action, i.e. attack or parry</param>
        /// <param name="weapon">The weapon to be used for the action</param>
        /// <param name="Hands">The Hands object of the character.</param>
        /// <param name="Context">The battle ground (or other situation spec) where the action takes place</param>
        /// <returns>The result object of the roll check</returns>
        RollCheckResultViMo OpenCombatRollCheck(Check actionId, HandsViMo heroHands, bool UseMainHand, BattlegroundViMo Context);


        /// <summary>
        /// Creates and returns a new dodge roll check with the first roll
        /// already done.
        /// </summary>
        /// <param name="AttrId">The attribute id</param>
        /// <param name="TargetAttr">The skill or ability to be used</param>
        /// <param name="Context">The battle ground (or other situation spec) where the action takes place</param>
        /// <param name="CarriesWeapon">Does the character carry a weapon?.</param>
        /// <returns>The result object of the roll check</returns>
        RollCheckResultViMo OpenDodgeRollCheck(Check AttrId, CharacterAttrDTO TargetAttr, BattlegroundViMo Context, bool CarriesWeapon);
    }
}