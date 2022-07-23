using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AEAssist.AI.Monk.GCD
{
    public class MonkGCD_MasterfulBlitz : IAIHandler
    {
        //Elixir Field 苍气炮 Action Id:3545 阴必杀
        //Flint Strike 爆裂脚 Action Id:25882 阳必杀
        //Celestial Revolution 翻天脚 Action Id:25765 兔必杀
        //Tornado Kick 斗魂旋风脚 Action Id:3543 真必杀
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.ElixirField.IsUnlock())
            {
                return -10;
            }
            if (ActionResourceManager.Monk.BlitzTimer != TimeSpan.Zero ||
                !ActionResourceManager.Monk.MastersGauge.Contains(ActionResourceManager.Monk.Chakra.None))
            {
                if (AIRoot.GetBattleData<MonkBattleData>().CurrentMonkNadiCombo != MonkNadiCombo.None)
                {
                    AIRoot.GetBattleData<MonkBattleData>().CurrentMonkNadiCombo = MonkNadiCombo.None;
                }
                if (ActionResourceManager.Monk.MastersGauge.Distinct().ToArray().Length == 1 && ActionResourceManager.Monk.BlitzTimer != TimeSpan.FromSeconds(3))
                {
                    if (!TargetHelper.CheckNeedUseAOEByMe(5, 5, 1))
                    {
                        return -1;
                    }
                }
                return 0;
            }
            return -4;
        }

        private SpellEntity GetMasterfulBlitz()
        {
            //真必杀
            if (ActionResourceManager.Monk.ActiveNadi == ActionResourceManager.Monk.Nadi.Both)
            {
                //Phantom Rush 梦幻斗舞 Action Id:25769
                if (SpellsDefine.PhantomRush.IsUnlock())
                {
                    return SpellsDefine.PhantomRush.GetSpellEntity();
                }
                return SpellsDefine.TornadoKick.GetSpellEntity();
            }
            //阴必杀

            if (ActionResourceManager.Monk.MastersGauge.Distinct().ToArray().Length == 1)
            {
                return SpellsDefine.ElixirField.GetSpellEntity();
            }
            //阳必杀
            if (ActionResourceManager.Monk.MastersGauge.Distinct().ToArray().Length == 3)
            {
                //Rising Phoenix 凤凰舞 Action Id:25768
                if (SpellsDefine.RisingPhoenix.IsUnlock())
                {
                    return SpellsDefine.RisingPhoenix.GetSpellEntity();
                }
                return SpellsDefine.FlintStrike.GetSpellEntity();
            }

            return SpellsDefine.CelestialRevolution.GetSpellEntity();
        }

        public async Task<SpellEntity> Run()
        {
            var spell = GetMasterfulBlitz();
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret)
                return spell;
            return null;
        }
    }
}