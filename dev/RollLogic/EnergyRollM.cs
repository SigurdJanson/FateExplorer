using System;

namespace FateExplorer.RollLogic
{
    public class EnergyRollM : DieRollM
    {
        public const int _Sides = 6;

        public ICheckModifierM SiteModifier { get; protected set; }
        public ICheckModifierM DisturbModifier { get; protected set; }

        public EnergyRollM(RegenerationSite site, RegenerationDisturbance disturb) : base(_Sides)
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

        }

        /// <inheritdoc/>
        public override int[] Roll()
        {
            base.Roll(); // executes the roll

            int BeforeMod = OpenRoll[0];
            OpenRoll[0] = DisturbModifier.Apply(OpenRoll[0]);
            OpenRoll[0] = SiteModifier.Apply(OpenRoll[0]);
            // energy regeneration cannot be below 0
            if (OpenRoll[0] < 0) OpenRoll[0] = 0;

            ModifiedBy = new int[1] { OpenRoll[0] - BeforeMod };

            return OpenRoll;
        }


    }
}
