using System;
using System.Security.Permissions;

namespace FateExplorer.Shared
{
    [Serializable]
    public class ChrImportException : Exception
    {
        /// <summary>
        /// The piece of information that is the cause for a failed import
        /// </summary>
        public enum Property 
        { 
            /// <summary>
            /// The JSON format does not fit expectations
            /// </summary>
            Format, 
            /// <summary>
            /// The basic character specification cannot be interpreted (e.g. species, gender, or name)
            /// </summary>
            Specification, 
            /// <summary>
            /// An attribute (ability, derived characteristic, ...)
            /// </summary>
            Attribute,
            /// <summary>
            /// Energy, life, arcane energy, karma energy
            /// </summary>
            Energy,
            /// <summary>
            /// A special ability
            /// </summary>
            SpecialAbility,
            /// <summary>
            /// (Dis-) Advantage
            /// </summary>
            DisAdvantage,
            /// <summary>
            /// A skill, spell, cantrip, liturgy, ...
            /// </summary>
            Skills,
            /// <summary>
            /// Combat technique
            /// </summary>
            CombatTechnique,
            /// <summary>
            /// A purchasable item, a weapon or something else
            /// </summary>
            Belonging, 
            /// <summary>
            /// A pet or other specified companion
            /// </summary>
            Companion,
            /// <summary>
            /// Relationships like pacts, ...
            /// </summary>
            Ties,
            /// <summary>
            /// The cause a failed import is unclear
            /// </summary>
            Undefined
        }

        /// <summary>
        /// The cause of the failed import
        /// </summary>
        protected Property Cause { get; set; }

        public string GetCause() => Cause.ToString();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChrImportException(Property cause = Property.Undefined) : base() 
        {
            Cause = cause;
        }

        public ChrImportException(string message, Property cause = Property.Undefined)
            : base(message)
        {
            Cause = cause;
        }

        public ChrImportException(string message, Exception inner, Property cause = Property.Undefined)
            : base(message, inner)
        {
            Cause = cause;
        }


        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        [Obsolete]
        protected ChrImportException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }


        // GetObjectData performs a custom serialization. 
        [Obsolete]
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        {
            // ...
            info.AddValue("Cause", Cause);

            base.GetObjectData(info, context);
        }
    }
}
