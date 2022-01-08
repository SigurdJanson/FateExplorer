using FateExplorer.RollLogic;

namespace FateExplorer.RollLogic
{
    public interface ICheckModifierM
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Before">An</param>
        /// <returns></returns>
        void Apply(IRollM Before);
    }
}
