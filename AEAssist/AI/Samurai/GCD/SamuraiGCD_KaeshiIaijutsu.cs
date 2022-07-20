using System;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;
using ff14bot.Managers;

namespace AEAssist.AI.Samurai.GCD
{
    public class SamuraiGCD_KaeshiIaijutsu : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.TsubameGaeshi.IsUnlock())
            {
                return -10;
            }
            
            if (SpellsDefine.TsubameGaeshi.GetSpellEntity().SpellData.Cooldown > TimeSpan.FromSeconds(60))
            {
                return -20;
            }

            var bdls = AIRoot.GetBattleData<BattleData>().lastGCDSpell;


            if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 3))
            {
                if (ActionManager.CanCastOrQueue(SpellsDefine.KaeshiGoken.GetSpellEntity().SpellData, Core.Me) ||
                    bdls == SpellsDefine.KaeshiGoken.GetSpellEntity())
                {
                    return 2;
                }
            }
            // public const uint KaeshiSetsugekka = 16486;
            // if (ActionManager.CanCastOrQueue(DataManager.GetSpellData(16486),
            //         Core.Me.CurrentTarget))
            // {
            //     return 3;
            // }

            if (ActionManager.CanCastOrQueue(SpellsDefine.KaeshiSetsugekka.GetSpellEntity().SpellData,
                    Core.Me.CurrentTarget) ||
                bdls == SpellsDefine.MidareSetsugekka.GetSpellEntity())
            {
                return 3;
            }


            return -1;
        }

        private static SpellEntity GetSpell()
        {
            if (ActionManager.CanCastOrQueue(SpellsDefine.KaeshiSetsugekka.GetSpellEntity().SpellData,
                    Core.Me.CurrentTarget))
            {
                return SpellsDefine.KaeshiSetsugekka.GetSpellEntity();
            }

            return SpellsDefine.KaeshiGoken.GetSpellEntity();
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