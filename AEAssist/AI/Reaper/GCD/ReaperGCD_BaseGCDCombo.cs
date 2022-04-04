﻿using System.Threading.Tasks;
using AEAssist.DataBinding;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;

namespace AEAssist.AI.Reaper.GCD
{
    public class ReaperGCD_BaseGCDCombo : IAIHandler
    {
        public bool Check(SpellData lastSpell)
        {
            return true;
        }

        public async Task<SpellData> Run()
        {
            if (BaseSettings.Instance.DoubleEnshroudPrefer 
                && Core.Me.ContainMyAura(AurasDefine.Enshrouded))
            {
                if (TargetHelper.CheckNeedUseAOE(Core.Me.CurrentTarget, 5, 5))
                {
                    if (AIRoot.Instance.ReaperBattleData.CurrCombo != ReaperComboStages.NightmareScythe)
                    {
                        if (await SpellHelper.CastGCD(SpellsDefine.WhorlOfDeath, Core.Me.CurrentTarget))
                        {
                            return SpellsDefine.WhorlOfDeath;
                        }
                    }
                }
                else
                {
                    if (await SpellHelper.CastGCD(SpellsDefine.ShadowOfDeath, Core.Me.CurrentTarget))
                    {
                        return SpellsDefine.ShadowOfDeath;
                    }
                }

            }

            return await ReaperSpellHelper.BaseGCDCombo(Core.Me.CurrentTarget);
        }
    }
}