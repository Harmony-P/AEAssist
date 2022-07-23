using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;
using ff14bot.Managers;

namespace AEAssist.AI.Samurai.GCD
{
    public class SamuraiGCD_BaseGCD : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            return 0;
        }
        //Ka -- hana
        //Setsu -- yuki
        //Getsu -- lunar 

        private static SpellEntity BaseRotation()
        {
            if (AEAssist.DataBinding.Instance.DynamicRotation)
            {
                if (ActionManager.LastSpellId == SpellsDefine.Hakaze)
                {
                    if (!Core.Me.HasMyAura(AurasDefine.Fuka))
                    {
                        if (!ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Ka))
                        {
                            if (SamuraiSpellHelper.KaCombo() != null) return SamuraiSpellHelper.KaCombo();
                        }
                    }

                    if (!Core.Me.HasMyAura(AurasDefine.Fugetsu))
                    {
                        if (!ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Getsu))
                        {
                            if (SamuraiSpellHelper.GetsuCombo() != null) return SamuraiSpellHelper.GetsuCombo();
                        }
                    }
                    
                    if (!ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Setsu))
                    {
                        if (SamuraiSpellHelper.SetsuCombo() != null) return SamuraiSpellHelper.SetsuCombo();
                    }
                    
                    if (Core.Me.CurrentTarget.IsBehind)
                    {
                        if (!ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Getsu))
                        {
                            if (SamuraiSpellHelper.GetsuCombo() != null) return SamuraiSpellHelper.GetsuCombo();
                        }

                        if (SamuraiSpellHelper.SenCounts() == 3)
                        {
                            if (SamuraiSpellHelper.GetsuCombo() != null) return SamuraiSpellHelper.GetsuCombo();
                        }

                        if (SamuraiSpellHelper.SenCounts() == 2 &&
                            !ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Ka))
                        {
                            if (SamuraiSpellHelper.KaCombo() != null) return SamuraiSpellHelper.KaCombo();
                        }
                    }

                    if (Core.Me.CurrentTarget.IsFlanking)
                    {
                        if (!ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Ka))
                        {
                            if (SamuraiSpellHelper.KaCombo() != null) return SamuraiSpellHelper.KaCombo();
                        }

                        if (SamuraiSpellHelper.SenCounts() == 3)
                        {
                            if (SamuraiSpellHelper.KaCombo() != null) return SamuraiSpellHelper.KaCombo();
                        }

                        if (SamuraiSpellHelper.SenCounts() == 2 &&
                            !ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Getsu))
                        {
                            if (SamuraiSpellHelper.GetsuCombo() != null) return SamuraiSpellHelper.GetsuCombo();
                        }
                    }


                }

                if (SamuraiSpellHelper.GetsuCombo() != null)
                {
                    if (
                        !(
                            (SamuraiSpellHelper.GetsuCombo() == SpellsDefine.Mangetsu.GetSpellEntity() ||
                             SamuraiSpellHelper.GetsuCombo() == SpellsDefine.Jinpu.GetSpellEntity()) &&
                            ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Getsu) &&
                            !ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Ka)
                        )
                    )
                    {
                        return SamuraiSpellHelper.GetsuCombo();
                    }
                }

                if (SamuraiSpellHelper.KaCombo() != null)
                {
                    if (
                        !(
                            (SamuraiSpellHelper.KaCombo() == SpellsDefine.Oka.GetSpellEntity() ||
                             SamuraiSpellHelper.KaCombo() == SpellsDefine.Shifu.GetSpellEntity()) &&
                            ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Ka) &&
                            !ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Getsu)
                        )
                    )
                    {
                        return SamuraiSpellHelper.KaCombo();
                    }
                }

                if (SamuraiSpellHelper.SetsuCombo() != null)
                {
                    return SamuraiSpellHelper.SetsuCombo();
                }
                
                if (TargetHelper.CheckNeedUseAOETest(8, 8, 3) && SpellsDefine.Fuga.IsUnlock())
                {
                    return SamuraiSpellHelper.GetFuko();
                }

                return SpellsDefine.Hakaze.GetSpellEntity();
            }
            else
            {
                if (ActionManager.LastSpellId == SpellsDefine.Hakaze)
                {
                    if (!ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Getsu))
                    {
                        if (SamuraiSpellHelper.GetsuCombo() != null) return SamuraiSpellHelper.GetsuCombo();
                    }

                    if (!ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Ka))
                    {
                        if (SamuraiSpellHelper.KaCombo() != null) return SamuraiSpellHelper.KaCombo();
                    }

                    if (!ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Setsu))
                    {
                        if (SamuraiSpellHelper.SetsuCombo() != null) return SamuraiSpellHelper.SetsuCombo();
                    }
                }

                if (SamuraiSpellHelper.GetsuCombo() != null)
                {
                    if (
                        !(
                            SamuraiSpellHelper.GetsuCombo() == SpellsDefine.Mangetsu.GetSpellEntity() &&
                            ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Getsu) &&
                            !ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Ka)
                        )
                    )
                    {
                        return SamuraiSpellHelper.GetsuCombo();
                    }
                }

                if (SamuraiSpellHelper.KaCombo() != null)
                {
                    if (
                        !(
                            SamuraiSpellHelper.KaCombo() == SpellsDefine.Oka.GetSpellEntity() &&
                            ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Ka) &&
                            !ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Getsu)
                        )
                    )
                    {
                        return SamuraiSpellHelper.KaCombo();
                    }
                }

                if (SamuraiSpellHelper.SetsuCombo() != null)
                {
                    return SamuraiSpellHelper.SetsuCombo();
                }
                

                return SpellsDefine.Hakaze.GetSpellEntity();
            }
        }


        public async Task<SpellEntity> Run()
        {
            var spell = BaseRotation();
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret)
                return spell;
            return null;
        }
    }
}