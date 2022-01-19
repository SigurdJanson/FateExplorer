using FateExplorer.RollLogic;
using FateExplorer.Shared;
using System.Threading.Tasks;

namespace FateExplorer.ViewModel
{

    /// <summary>
    /// Once registered at the <see cref="WebAssemblyHostBuilder"/> you have 
    /// to call <see cref="ReadRollMappingsAsync"/> and <see cref="RegisterChecks"/>
    /// </summary>
    public interface IRollHandlerViMo
    {
        /// <summary>
        /// Reads the mappings from a file that connect the attribute id for which the user 
        /// requested a check to the id of the class that does the check (e.g. "TAL" for 
        /// basic skill checks to "DSA/skill/mundane"). <br/>Call this after registering the service.
        /// </summary>
        /// <returns></returns>
        Task ReadRollMappingsAsync();

        /// <summary>
        /// 
        /// </summary>
        void RegisterChecks();


        /// <summary>
        /// Creates and returns a new (unmodified) roll check with the first roll
        /// already done.
        /// </summary>
        /// <param name="AttrId">The attribute id</param>
        /// <returns>A roll check</returns>
        RollCheckResultViMo OpenRollCheck(string AttrId, ICharacterAttributDTO AttrData, ICharacterAttributDTO[] RollAttr = null);


        /// <summary>
        /// Creates and returns a new roll check with the first roll already done. The modifier 
        /// will be applied.
        /// </summary>
        /// <param name="AttrId">The attribute id</param>
        /// <param name="AttrData"></param>
        /// <param name="Modifier"></param>
        /// <param name="Args">Additional arguments sent to the constructor of the instantiated check</param>
        /// <returns>A roll check</returns>
        RollCheckResultViMo OpenRollCheck(string AttrId, ICharacterAttributDTO AttrData, ICheckModifierM Modifier, ICharacterAttributDTO[] RollAttr = null);
    }
}