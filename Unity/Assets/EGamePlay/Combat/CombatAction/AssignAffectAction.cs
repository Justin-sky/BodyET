using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace EGamePlay.Combat
{
    /// <summary>
    /// ����Ч����Ϊ
    /// </summary>
    public class AssignAffectAction : CombatAction
    {
        //Ч������
        public AffectType AffectType { get; set; }
        //Ч����ֵ
        public string AffectValue { get; set; }


        private void BeforeAssign()
        {

        }

        public void ApplyAssignAction()
        {
            BeforeAssign();

            AfterAssign();
        }

        private void AfterAssign()
        {

        }
    }

    public enum AffectType
    {
        DamageAffect = 1,
        NumericModify = 2,
        StatusAttach = 3,
        BuffAttach = 4,
    }
}