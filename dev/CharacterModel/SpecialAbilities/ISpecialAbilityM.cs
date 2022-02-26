namespace FateExplorer.CharacterModel
{
    public interface ISpecialAbilityM
    {
        /// <summary>
        /// A string id
        /// </summary>
        string Id { get; }

        /// <summary>
        /// A tier (i.e. a level) of the special ability. 0 if it is ignored i.e. the
        /// special ability has no levels.
        /// </summary>
        int Tier { get; }


        /// <summary>
        /// Determines whether the special ability is known by fate explorer
        /// and included in computations.
        /// </summary>
        bool IsRecognized { get; }


        //string Sid { get; }
        //bool HasSid();
        //Type GetSid1Type();
        //string Sid2 { get; }
        //bool HasSid2();
        //Type GetSid2Type();
    }
}
