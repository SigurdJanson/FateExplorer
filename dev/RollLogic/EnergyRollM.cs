using System;

namespace FateExplorer.RollLogic
{
    public class EnergyRollM : DieRollM
    {
        public const int _Sides = 6;

        public ICheckModifierM SiteModifier { get; protected set; }
        public ICheckModifierM DisturbModifier { get; protected set; }
        public ICheckModifierM SicknessModifier { get; protected set; }
        public ICheckModifierM OtherModifier { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="site">Classifcation of the accomodation for the resting period</param>
        /// <param name="disturb">Classifcation of the amount of disturbance</param>
        /// <param name="poisonedSick">Is the character poisoned or sick?</param>
        /// <param name="modifier">An additional free modifier (usually to factor unrecognised (dis-)advantages in</param>
        /// <exception cref="NotImplementedException"></exception>
        public EnergyRollM(
            RegenerationSite site, RegenerationDisturbance disturb, 
            bool sickPoisoned, int modifier)
            : base(_Sides)
        {
            SiteModifier = site switch
            {
                RegenerationSite.Default => new SimpleCheckModifierM(0),
                RegenerationSite.Good => new SimpleCheckModifierM(1),
                RegenerationSite.Poor => new SimpleCheckModifierM(-1),
                RegenerationSite.Bad => new HalfModifier(),
                RegenerationSite.Terrible => new ForcefulModifier(0),
                _ => throw new NotImplementedException()
            };
            DisturbModifier = disturb switch
            {
                RegenerationDisturbance.None => new SimpleCheckModifierM(0),
                RegenerationDisturbance.Brief => new SimpleCheckModifierM(-1),
                RegenerationDisturbance.Prolonged => new SimpleCheckModifierM(-2),
                _ => throw new NotImplementedException()
            };
            if (sickPoisoned)
                SicknessModifier = new ForcefulModifier(0);
            else
                SicknessModifier = new SimpleCheckModifierM(0);
            OtherModifier = new SimpleCheckModifierM(modifier);
        }


        /// <inheritdoc/>
        public override int[] Roll()
        {
            base.Roll(); // executes the roll

            int BeforeMod = OpenRoll[0];
            OpenRoll[0] = OtherModifier.Apply(OpenRoll[0]);
            OpenRoll[0] = DisturbModifier.Apply(OpenRoll[0]);
            OpenRoll[0] = SiteModifier.Apply(OpenRoll[0]);
            OpenRoll[0] = SicknessModifier.Apply(OpenRoll[0]);

            // energy regeneration cannot be below 0
            if (OpenRoll[0] < 0) OpenRoll[0] = 0;

            ModifiedBy = new int[1] { OpenRoll[0] - BeforeMod };

            return OpenRoll;
        }


    }
}
