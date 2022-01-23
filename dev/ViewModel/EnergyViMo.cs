using FateExplorer.GameLogic;
using FateExplorer.RollLogic;


namespace FateExplorer.ViewModel
{
    public class EnergyViMo
    {
        /// <summary>
        /// Access to the character's view model that contains this energy
        /// </summary>
        protected ITheHeroViMo Hero { get; set; }

        /// <summary>
        /// Access to the model
        /// </summary>
        protected CharacterEnergyM Energy { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="energyM">According model object</param>
        /// <param name="id">Unique identifier</param>
        public EnergyViMo(CharacterEnergyM energyM, string id, ITheHeroViMo hero)
        {
            Energy = energyM;
            Id = id;
            Hero = hero;
        }


        /// <summary>
        /// Unique identifier fot the character attribute
        /// </summary>
        public string Id { get; protected set; }

        /// <summary>
        /// Name of the attribute
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// An abbreviation of the name (e.g. COU for courage as common in roleplay systems).
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// The character's attribute value
        /// </summary>
        public int Max
        { 
            get => Energy.Max;
            set
            {
                if (Energy.Max != value)
                {
                    Energy.Max = value;
                    Hero.OnEnergyChanged(this);
                }
            }
        }

        protected int effMax;
        /// <summary>
        /// Allows users to set their character's energy themselves
        /// </summary>
        public int EffMax
        {
            get => effMax;
            set
            {
                if (effMax != value)
                {
                    effMax = value;
                    Energy.CalcThresholds(effMax);
                    Hero.OnEnergyChanged(this);
                }
            }
        }


        /// <summary>
        /// The permitted minimum value.
        /// </summary>
        public int Min { get => Energy.Min; }


        protected int effectiveValue;
        /// <summary>
        /// The character's attribute value after adding/removing temporary changes.
        /// Reductions happen during gameplay and are defined by the game master.
        /// Calculated attributes may change due to dependencies.
        /// </summary>
        public int EffectiveValue 
        {
            get => effectiveValue;
            set
            {
                if (effectiveValue != value)
                {
                    effectiveValue = Energy.ResolveValue(value, EffMax, Min);
                    Hero.OnEnergyChanged(this);
                }
            }
        }

        /// <summary>
        /// Some energies have a number of consequences when reduced 
        /// by certain amounts i.e. crosing thresholds.
        /// </summary>
        public int CrossedThresholds => Energy.CountCrossedThresholds(EffectiveValue);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <param name="disturb"></param>
        /// <returns>The complete roll result; null if the errective value is already at max.</returns>
        public RollResultViMo Regenerate(RegenerationSite site, RegenerationDisturbance disturb)
        {
            if (effectiveValue == Max) return null;

            EnergyRollM energyRoll = new(site, disturb);
            energyRoll.Roll();
            RollResultViMo Result = new(energyRoll);

            effectiveValue = Energy.ResolveValue(effectiveValue + Result.RollResult[0], EffMax, Min); ;

            return Result;
        }

    }
}