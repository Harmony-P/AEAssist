using System;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.AI.Samurai.Ability
{
    public class SamuraiAbility_Meikyo : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.MeikyoShisui.IsUnlock())
            {
                return -20;
            }

            if (Core.Me.ClassLevel < 88)
            {
                if (SpellsDefine.MeikyoShisui.GetSpellEntity().SpellData.Cooldown.TotalMilliseconds > 0)
                {
                    return -10;
                }
            }
            else
            {
                if (!SpellsDefine.MeikyoShisui.IsReady())
                {
                    return -10;
                }
            }

            if (Core.Me.HasAura(AurasDefine.MeikyoShisui) || SpellsDefine.MeikyoShisui.RecentlyUsed())
            {
                return -20;
            }


            if (ActionManager.LastSpellId != SpellsDefine.Hakaze &&
                ActionManager.LastSpellId != SpellsDefine.Jinpu &&
                ActionManager.LastSpellId != SpellsDefine.Shifu &&
                ActionManager.LastSpellId != SpellsDefine.Fuga && ActionManager.LastSpellId != SpellsDefine.Fuko
               )
            {
                //AOE
                if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 3))
                {
                    return 0;
                }
                else
                {
                    //dot
                    if (SamuraiSpellHelper.TargetNeedsDot())
                    {
                        if (SamuraiSpellHelper.SenCounts() == 0 ||
                            SamuraiSpellHelper.SenCounts() == 3)
                        {
                            return 1;
                        }
                    }

                    if (ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Setsu) &&
                        SamuraiSpellHelper.SenCounts() < 3)
                    {
                        return 0;
                    }
                }
            }

            return -5;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.MeikyoShisui.GetSpellEntity();
            if (spell == null) return null;
            if (await spell.DoAbility())
                return spell;
            return null;
        }
    }
}