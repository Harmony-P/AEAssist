using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;
using ff14bot.Managers;

namespace AEAssist.AI.Samurai.GCD
{
    public class SamuraiGCD_Iaijutsu : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            // 1 -- dot
            // 2 -- aoe
            // 3 -- single target
            if (SamuraiSpellHelper.SenCounts() == 0)
            {
                return -10;
            }
            if (MovementManager.IsMoving)
            {
                return -5;
            }
            //AOE
            if (SamuraiSpellHelper.SenCounts() == 2)
            {
                if (!SpellsDefine.Yukikaze.IsUnlock())
                {
                    return 1;
                }
                if (TargetHelper.CheckNeedUseAOEByMe(8, 8, 3))
                {
                    return 2;
                }
            }
            //Single Target
            //dot
            if (SamuraiSpellHelper.SenCounts() == 1)
            {
                if (SamuraiSpellHelper.NeedsDot())
                {
                    return 1;
                }
            }

            if (SamuraiSpellHelper.SenCounts() == 3)
            {
                return 3;
            }
            
            return -1;
        }

        private static SpellEntity GetSpell()
        {
            if (SamuraiSpellHelper.SenCounts() == 3)
            {
                return SpellsDefine.MidareSetsugekka.GetSpellEntity();
            }

            if (SamuraiSpellHelper.SenCounts() == 2)
            {
                return SpellsDefine.TenkaGoken.GetSpellEntity();
            }

            return SpellsDefine.Higanbana.GetSpellEntity();
        }
        

        
        public async Task<SpellEntity> Run()
        {
            var spell = GetSpell();
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret)
                return spell;
            return null;
        }
    }
}