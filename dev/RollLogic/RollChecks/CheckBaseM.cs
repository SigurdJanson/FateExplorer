using System.Collections.Generic;

namespace FateExplorer.RollLogic
{
    public abstract class CheckBaseM
    {
        public const string checkTypeId = "DSA5/0";

        /// <summary>
        /// The type of check (string in path format, e.g. "DSA5/0/skill/magic").
        /// </summary>
        public virtual string CheckTypeId { get => checkTypeId.TrimEnd('/'); }

        /// <summary>
        /// The (character) attribute to be rolled against.
        /// </summary>
        public string AttributeId { get; set; }

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

        /// <summary>
        /// This checks modifier
        /// </summary>
        public ICheckModifierM CheckModifier { get; set; }

        /// <summary>
        /// The series of rolls as they were needed during the check.
        /// It needs to be built roll after roll by classes implementing checks.
        /// </summary>
        protected List<IRollM> RollSeries { get; set; }


        /// <summary>
        /// Implement this method to return an instance of the next roll.
        /// Create the instance and add it to the RollSeries.
        /// </summary>
        /// <returns></returns>
        public abstract IRollM NextStep();

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
            return NextStep() is not null;
        }

        public abstract RollResultViMo GetRollResult(int Step = -1);

        public abstract RollResultViMo GetCheckResult();

    }
}
