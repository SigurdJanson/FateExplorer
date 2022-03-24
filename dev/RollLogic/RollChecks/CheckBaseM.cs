using FateExplorer.GameData;
using FateExplorer.Shared;
using System;

namespace FateExplorer.RollLogic
{
    public abstract class CheckBaseM : IDisposable
    {
        protected IGameDataService GameData { get; set; }  // injected


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gameData">Access to the data base with basic game data(injection)</param>
        protected CheckBaseM(IGameDataService gameData)
        {
            GameData = gameData;
            Success = new();
        }




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
        /// This checks general modifier
        /// </summary>
        public ICheckModifierM CheckModifier { get; set; }



        /// <summary>
        /// The (pending) success level of the whole check.
        /// </summary>
        public abstract RollSuccess Success { get; protected set; }

        /// <summary>
        /// Get the success level of a given roll
        /// </summary>
        /// <param name="Roll"></param>
        /// <returns></returns>
        public abstract RollSuccess.Level SuccessOfRoll(RollType Which);



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
        /// An extended description of the classification
        /// </summary>
        public abstract string ClassificationDescr { get; }



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
        public virtual bool NeedsConfirmation => Success.IsPending;


        /// <summary>
        /// Needs a roll to determine the effect of a botch. By default 
        /// a botch roll is required when the current 
        /// <see cref="Success">success level</see> is "botch".
        /// </summary>
        public virtual bool NeedsBotchEffect
        { get => RollList[RollType.Confirm] is not null &&
                RollList[RollType.Botch] is null &&
                Success.CurrentLevel == RollSuccess.Level.Botch;
        }

        /// <summary>
        /// Needs a roll to determine the damage caused by a combat roll. By default 
        /// this is false.
        /// </summary>
        public virtual bool NeedsDamage
        {
            get => false;
        }


        #region Update when modifier has changed

        /// <summary>
        /// Update the check assessment after a modifier update
        /// </summary>
        public abstract void UpdateAfterModifierChange();



        #endregion

        //
        #region IDisposable implementation

        /// <summary>
        /// Flag that tells if <see cref="Dispose()"/> has been called successfully
        /// </summary>
        protected bool IsDisposed = false;

        //public void Free()
        //{
        //    if (IsDisposed)
        //        throw new ObjectDisposedException(GetType().ToString());
        //}

        /// <summary>
        /// Call <c>Dispose()</c> to free resources explicitly.
        /// </summary>
        public void Dispose()
        {
            // Pass true in dispose method to clean managed resources too and say GC to skip finalize in next line.
            Dispose(true);
            // If dispose is called already then say GC to skip finalize on this instance.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Makes sure the unmanaged resources will be disposed eventually.
        /// </summary>
        ~CheckBaseM()
        {
            // Pass false as param because no need to free managed resources when
            // you call finalize it will be done by GC itself as its work of
            // finalize to manage managed resources.
            Dispose(false);
        }

        /// <summary>
        /// Derived classes shall implement <c>Dispose(bool)</c> to free 
        /// unmanaged resources.
        /// <example>
        /// Do it like this:
        /// <code>
        /// if (!IsDisposed)
        /// {
        ///     IsDisposed = true;
        ///     // Released unmanaged Resources
        ///     if (disposedStatus)
        ///     {
        ///         // Released managed Resources
        ///     }
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="disposedStatus">If true the method will managed resources, too 
        /// (which it does when called from <see cref="Dispose()"/>). If false only unmanaged 
        /// resources will be removed.</param>
        protected virtual void Dispose(bool disposedStatus)
        {
            //if (!IsDisposed)
            //{
            //    IsDisposed = true;
            //    // Released unmanaged Resources
            //    if (disposedStatus)
            //    {
            //        // Released managed Resources
            //    }
            //}
        }

        #endregion
    }
}
