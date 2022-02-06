using FateExplorer.GameData;
using FateExplorer.GameLogic;
using System;

namespace FateExplorer.ViewModel
{
    /// <summary>
    /// Two hands of a character.
    /// </summary>
    public class HandsViMo
    {
        public enum Hand { Main = 0, Off = 1 };

        /// <summary>
        /// Store the "bare hands weapons" for main and off hand.
        /// </summary>
        protected WeaponViMo UnarmedMainHand, UnarmedOffHand;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hero">The hero (model) these hands belong to</param>
        /// <param name="gameData">Access to the game data base</param>
        public HandsViMo(ICharacterM hero, IGameDataService gameData)
        {
            UnarmedMainHand = GetBareHandsAsWeapon(hero, gameData);
            UnarmedOffHand = GetBareHandsAsWeapon(hero, gameData);

            Weapon = new WeaponViMo[2];
            Weapon[(int)Hand.Main] = UnarmedMainHand;
            Weapon[(int)Hand.Off] = UnarmedOffHand;

            HoldsWeapon = new bool[2];
            HoldsWeapon[(int)Hand.Main] = false;
            HoldsWeapon[(int)Hand.Off] = false;
        }

        /// <summary>
        /// The weapon carried by each hand; can be a "bare hands weapon" (i.e. <see cref="WeaponUmarmedM"/>)
        /// </summary>
        protected WeaponViMo[] Weapon { get; set; }


        /// <summary>
        /// The main hand weapon
        /// </summary>
        public WeaponViMo MainWeapon
        {
            get => Weapon[(int)Hand.Main];
            set
            {
                if (value is null)
                {
                    Weapon[(int)Hand.Main] = UnarmedMainHand;
                    HoldsWeapon[(int)Hand.Main] = false;
                }
                else
                {
                    Weapon[(int)Hand.Main] = value;
                    HoldsWeapon[(int)Hand.Main] = true;
                    if (value.IsTwohanded)
                    {
                        Weapon[(int)Hand.Off] = null; // do not use `OffWeapon` here
                    }
                }
            }
        }

        /// <summary>
        /// The off hand weapon.
        /// Setting a two-handed weapon as will set the main hand, too.
        /// Can be null when the selected weapon is two-handed..
        /// </summary>
        public WeaponViMo OffWeapon
        {
            get => Weapon[(int)Hand.Off];
            set
            {
                if (value is null)
                {
                    Weapon[(int)Hand.Off] = UnarmedOffHand;
                    HoldsWeapon[(int)Hand.Off] = false;
                }
                else
                {
                    if (value.IsTwohanded) throw new ArgumentException("Two-Handed weapon cannot be put in off-hand");
                    Weapon[(int)Hand.Off] = value;
                    HoldsWeapon[(int)Hand.Off] = true;
                }
            }
        }

        /// <summary>
        /// Does the hand hold a weapon or is it bare?
        /// </summary>
        /// <remarks>Technically, the hands hold an equipped weapon or a "bare-hands" weapon.</remarks>
        protected bool[] HoldsWeapon { get; set; }

        /// <summary>
        /// Is it possible to use the weapon in each hand?
        /// A weapon may be disabled, e.g. when a character carries a two-handed
        /// weapon and you put another item in the off-hand weapon.
        /// </summary>
        public bool IsDisabled(Hand Which)
        {
            if (Which == Hand.Off)
                return false;
            else
                return HoldsWeapon[(int)Hand.Off] && MainWeapon.IsTwohanded;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hero"></param>
        /// <param name="gameData"></param>
        /// <returns></returns>
        /// <remarks>Only used during initialisation of the class.</remarks>
        protected static WeaponViMo GetBareHandsAsWeapon(ICharacterM hero, IGameDataService gameData)
        {
            WeaponUnarmedM weapon = new(hero);
            weapon.Initialise(gameData);
            return new WeaponViMo(weapon);
        }


        /// <summary>
        /// Empties the characters hand by removing the current weapon and "replacing" it with 
        /// bare hands. So teechnically, the hands always carry a weapon.
        /// </summary>
        /// <param name="Which">Remove the weapon from which hand? 
        /// true is the dominant hand; false the non-domoinant.</param>
        public void RemoveWeapon(Hand Which)
        {
            Weapon[(int)Which] = (Which == Hand.Main) ? UnarmedMainHand : UnarmedOffHand;
            HoldsWeapon[(int)Which] = false;
        }

        /// <summary>
        /// Checks if the slected hand carries a weapon or is bare.
        /// </summary>
        /// <param name="Which">Which hand to look up</param>
        /// <returns>true if the hand is bare</returns>
        public bool IsBare(Hand Which) => !HoldsWeapon[(int)Which];

    }
}
