namespace FateExplorer.RollLogic
{


    public class SkillCheckM : CheckBaseM
    {
        public new const string checkTypeId = "DSA5/0/skill";

        public override string CheckTypeId { get => checkTypeId + Domain; }

        public SkillDomain Domain { get; protected set; }

        public SkillCheckM(string attributeId)
        {
            AttributeId = attributeId;
            // Set the skill domain

        }



        public override RollResultViMo GetCheckResult()
        {
            return null;
        }

        public override bool HasNextStep()
        {
            throw new System.NotImplementedException();
        }

        public override RollResultViMo GetRollResult(int Step = -1)
        {
            throw new System.NotImplementedException();
        }

        public override IRollM NextStep()
        {
            throw new System.NotImplementedException();
        }
    }
}
