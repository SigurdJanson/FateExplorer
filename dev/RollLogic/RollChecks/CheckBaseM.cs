using FateExplorer.Shared;
using System.Collections.Generic;

namespace FateExplorer.RollLogic
{
    public abstract class CheckBaseM
    {
        #region Identification

        public const string checkTypeId = "DSA5/0";

        /// <summary>
        /// The type of check (string in path format, e.g. "DSA5/0/skill/magic").
        /// </summary>
        public virtual string CheckTypeId { get => checkTypeId.TrimEnd('/'); }

        /// <summary>
        /// The (character) attribute to be rolled against.
        /// </summary>
        public string AttributeId { get; protected set; } = "";

        /// <summary>
        /// A unique id to identify each particluar check
        /// </summary>
        public string UniqueId { get; protected set; }

        /// <summary>
        /// A combined id that contains all the information of the check
        /// incl. the <see cref="CheckTypeId">id path</see>, the <see cref="AttributeId">attribute</see> and 
        /// the <see cref="UniqueId">unique check id</see>.
        /// </summary>
        public string Id 
        { 
            get => CheckTypeId + "/" + AttributeId.TrimEnd('/') + "/" + UniqueId;
        }

        #endregion

        /// <summary>
        /// A string describing the 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A category of a roll
        /// </summary>
        public enum RollType { Primary = 0, Confirm = 1, Damage = 2, Botch = 3, BotchDamage = 4 }


        /// <summary>
        /// The (pending) success level of the whole check.
        /// </summary>
        public abstract RollSuccessLevel Success { get; }

        /// <summary>
        /// Get the success level of a given roll
        /// </summary>
        /// <param name="Roll"></param>
        /// <returns></returns>
        public abstract RollSuccessLevel RollSuccess(int Roll);

        /// <summary>
        /// This checks modifier
        /// </summary>
        public ICheckModifierM CheckModifier { get; set; }

        /// <summary>
        /// The series of rolls as they were needed during the check.
        /// It needs to be built roll after roll by classes implementing checks.
        /// </summary>
        public List<IRollM> RollSeries { get; protected set; }

        /// <summary>
        /// The target attribute to roll against.
        /// </summary>
        public int[] Attribute { get; protected set; }



        /// <summary>
        /// Implement this method to return an instance of the next roll.
        /// Create the instance and add it to the RollSeries.
        /// </summary>
        /// <returns>A roll object; null if the check has allows no further rolls.</returns>
        public abstract IRollM RollNextStep();



        /// <summary>
        /// Does the primary roll require a confirmation roll?
        /// By default confirmation is required when the current 
        /// <see cref="Success">success level</see> is "pending".
        /// </summary>
        public virtual bool NeedsConfirmation
        { get => Success == RollSuccessLevel.PendingBotch || Success == RollSuccessLevel.PendingCritical; }



        /// <summary>
        /// Needs a roll to determine the effect of a botch roll. By default 
        /// a botch roll is required when the current 
        /// <see cref="Success">success level</see> is "botch".
        /// </summary>
        public virtual bool NeedsBotchEffect
        { get => Success == RollSuccessLevel.Botch; }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This method does not need to be overwritten unless addtional conditions 
        /// than <c>NextStep() == 0</c> must be met.
        /// </remarks>
        public virtual bool HasNextStep()
        {
            return RollNextStep() is not null;
        }

    }
}
