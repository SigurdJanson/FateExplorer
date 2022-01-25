﻿using FateExplorer.Shared;
using RollLogicTests.Shared;

namespace FateExplorer.RollLogic
{
    public abstract class CheckBaseM
    {
        #region Identification

        /// <summary>
        /// Must be an EXACT match for the roll property in the 'rollresolver*.json' file.
        /// </summary>
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
        public string UniqueId { get; protected set; } //TODO: is this even being used?

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
        /// A string describing the nature of the check (human-readable, no id)
        /// </summary>
        public string Name { get; set; }




        /// <summary>
        /// The value(s) of the target attribute to roll against.
        /// </summary>
        public abstract int[] RollAttr { get; protected set; }

        /// <summary>
        /// (Human-readable) string to describe the values that is being rolled against.
        /// A single string here may be identical to <see cref="Name"/>, e.g. for
        /// ability checks.
        /// </summary>
        public abstract string[] RollAttrName { get; protected set; }


        /// <summary>
        /// The value is to compensate for rolls exceeding the attibute value 
        /// they have been rolled against.
        /// </summary>
        public abstract int? TargetAttr { get; protected set; }
        /// <summary>
        /// The target value is to compensate for rolls exceeding the attibute value 
        /// they have been rolled against. This is it's name.
        /// </summary>
        public abstract string TargetAttrName { get; protected set; }


        /// <summary>
        /// This checks modifier
        /// </summary>
        public ICheckModifierM CheckModifier { get; set; }



        /// <summary>
        /// The (pending) success level of the whole check.
        /// </summary>
        public abstract RollSuccessLevel Success { get; }

        /// <summary>
        /// Get the success level of a given roll
        /// </summary>
        /// <param name="Roll"></param>
        /// <returns></returns>
        public abstract RollSuccessLevel RollSuccess(RollType Which);



        /// <summary>
        /// The remaining eyes after evaluating the check
        /// </summary>
        public abstract int Remainder { get; }

        /// <summary>
        /// Classifies the result, e.g. the quality level of a skill check.
        /// </summary>
        public abstract string ClassificationLabel { get; }

        /// <summary>
        /// Classifies the result, e.g. the quality level of a skill check.
        /// </summary>
        public abstract string Classification { get; }


        /// <summary>
        /// The series of rolls as they were needed during the check.
        /// It needs to be built roll after roll by classes implementing checks.
        /// </summary>
        protected ArrayByEnum<IRollM, RollType> RollList { get; set; }


        /// <summary>
        /// Get remaining eyes for a particular roll after it's evaluation.
        /// </summary>
        /// <param name="Which">The desired roll type</param>
        /// <returns>A list of remainders one for each die of the roll</returns>
        public abstract int[] RollRemainder(RollType Which);


        /// <summary>
        /// Implement this method to return an instance of the next roll.
        /// Create the instance and add it to the RollSeries.
        /// </summary>
        /// <param name="Which">The desired roll type</param>
        /// <param name="AutoRoll">If the roll has not been made, yet, do it. Default = false.</param>
        /// <returns>A roll object; null if the check does not allow this roll or has ot not made, yet.</returns>
        public abstract IRollM GetRoll(RollType Which, bool AutoRoll = false);



        /// <summary>
        /// Does the primary roll require a confirmation roll?
        /// By default confirmation is required when the current 
        /// <see cref="Success">success level</see> is "pending".
        /// </summary>
        public virtual bool NeedsConfirmation
        { get => Success == RollSuccessLevel.PendingBotch || Success == RollSuccessLevel.PendingCritical; }


        /// <summary>
        /// Needs a roll to determine the effect of a botch. By default 
        /// a botch roll is required when the current 
        /// <see cref="Success">success level</see> is "botch".
        /// </summary>
        public virtual bool NeedsBotchEffect
        { get => RollList[RollType.Confirm] is not null &&
                RollList[RollType.Botch] is null &&
                Success == RollSuccessLevel.Botch;
        }

        /// <summary>
        /// Needs a roll to determine the damage caused by a combat roll. By default 
        /// this is false.
        /// </summary>
        public virtual bool NeedsDamage
        {
            get => false;
        }
    }
}
