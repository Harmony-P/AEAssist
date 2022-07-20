using AEAssist.Define;
using System;
using System.Threading.Tasks;
using AEAssist.Helper;
using AEAssist.Rotations.Core;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.AI.Samurai
{
    public class SamuraiSpellHelper
    {
        public static SpellEntity CoolDownPhaseGCD(GameObject target)
        {
            // https://www.thebalanceffxiv.com/jobs/melee/samurai/basic-guide/
            // Hakaze -> Yukikaze -> Hakaze -> Jinpu -> Gekko -> Hakaze -> Shifu -> Kasha -> Midare Setsugekka -> repeat
            // refer to the balance level 90 samurai
            var lastGcd = ActionManager.LastSpellId;


            // if (AIRoot.GetBattleData<SamuraiBattleData>().CurrCombo == SamuraiComboStages.MidareSetsuGekka)
            // {
            //     if (SpellsDefine.KaeshiSetsugekka.IsReady())
            //     {
            //         AIRoot.GetBattleData<SamuraiBattleData>().CurrCombo = SamuraiComboStages.KaeshiSetsugekka;
            //         SpellsDefine.KaeshiSetsugekka.GetSpellEntity().DoAbility();
            //     } 
            // }

            if (lastGcd == SpellsDefine.Jinpu)
            {
                AIRoot.GetBattleData<SamuraiBattleData>().CurrCombo = SamuraiComboStages.Gekko;
                return SpellsDefine.Gekko.GetSpellEntity();
            }

            if (lastGcd == SpellsDefine.Shifu)
            {
                AIRoot.GetBattleData<SamuraiBattleData>().CurrCombo = SamuraiComboStages.Kasha;
                return SpellsDefine.Kasha.GetSpellEntity();
            }

            if (lastGcd == SpellsDefine.Hakaze)
            {
                if (!ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Setsu))
                {
                    AIRoot.GetBattleData<SamuraiBattleData>().CurrCombo = SamuraiComboStages.Yukikaze;
                    return SpellsDefine.Yukikaze.GetSpellEntity();
                }

                if (!ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Getsu))
                {
                    AIRoot.GetBattleData<SamuraiBattleData>().CurrCombo = SamuraiComboStages.Jinpu;
                    return SpellsDefine.Jinpu.GetSpellEntity();
                }

                if (!ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Ka))
                {
                    AIRoot.GetBattleData<SamuraiBattleData>().CurrCombo = SamuraiComboStages.Shifu;
                    return SpellsDefine.Shifu.GetSpellEntity();
                }
            }

            AIRoot.GetBattleData<SamuraiBattleData>().CurrCombo = SamuraiComboStages.Hakaze;
            return SpellsDefine.Hakaze.GetSpellEntity();
        }

        public static SpellEntity OddMinutesBurst()
        {
            // https://www.thebalanceffxiv.com/jobs/melee/samurai/basic-guide/

            // do odd minute bursts
            if (SpellsDefine.MeikyoShisui.RecentlyUsed() || Core.Me.HasMyAura(AurasDefine.MeikyoShisui))
            {
                if (SenCounts() < 1)
                {
                    AIRoot.GetBattleData<SamuraiBattleData>().CurrCombo = SamuraiComboStages.Gekko;
                    return SpellsDefine.Gekko.GetSpellEntity();
                }

                AIRoot.GetBattleData<SamuraiBattleData>().CurrCombo = SamuraiComboStages.Kasha;
                return SpellsDefine.Kasha.GetSpellEntity();
            }

            if (ActionManager.LastSpellId == SpellsDefine.Hakaze)
            {
                AIRoot.GetBattleData<SamuraiBattleData>().CurrCombo = SamuraiComboStages.Yukikaze;
                return SpellsDefine.Yukikaze.GetSpellEntity();
            }

            AIRoot.GetBattleData<SamuraiBattleData>().CurrCombo = SamuraiComboStages.Hakaze;
            return SpellsDefine.Hakaze.GetSpellEntity();
        }

        public static SpellEntity EvenMinutesBurst()
        {
            // https://www.thebalanceffxiv.com/jobs/melee/samurai/basic-guide/
            if (SpellsDefine.MeikyoShisui.RecentlyUsed() || Core.Me.HasMyAura(AurasDefine.MeikyoShisui))
            {
                if (SenCounts() < 1)
                {
                    AIRoot.GetBattleData<SamuraiBattleData>().CurrCombo = SamuraiComboStages.Gekko;
                    return SpellsDefine.Gekko.GetSpellEntity();
                }

                AIRoot.GetBattleData<SamuraiBattleData>().CurrCombo = SamuraiComboStages.Kasha;
                return SpellsDefine.Kasha.GetSpellEntity();
            }

            if (ActionManager.LastSpellId == SpellsDefine.Hakaze)
            {
                AIRoot.GetBattleData<SamuraiBattleData>().CurrCombo = SamuraiComboStages.Yukikaze;
                return SpellsDefine.Yukikaze.GetSpellEntity();
            }

            AIRoot.GetBattleData<SamuraiBattleData>().CurrCombo = SamuraiComboStages.Hakaze;
            return SpellsDefine.Hakaze.GetSpellEntity();
        }

        public static bool TargetNeedsDot()
        {
            var tar = Core.Me.CurrentTarget as Character;
            if (!AEAssist.DataBinding.Instance.UseDotSAM)
                return false;
            if (TTKHelper.IsTargetTTK(tar))
                return false;

            if (DotBlacklistHelper.IsBlackList(Core.Me.CurrentTarget as Character))
                return false;
            if (!tar.HasMyAuraWithTimeleft(AurasDefine.Higanbana, 2000))
            {
                return true;
            }

            return false;
        }

        public static async Task<SpellEntity> FillerRotations()
        {
            var baseGcdTime = RotationManager.Instance.GetBaseGCDSpell().AdjustedCooldown.TotalMilliseconds;

            if (Math.Abs(baseGcdTime - 2140) == 0) // 2.14second gcd 2 filler gcd needed
            {
                if (ActionManager.LastSpellId == SpellsDefine.Hakaze)
                {
                    if (SpellsDefine.Yukikaze.IsUnlock())
                    {
                        if (SpellsDefine.Yukikaze.IsReady())
                        {
                            await SpellsDefine.Yukikaze.DoGCD();
                            return SpellsDefine.Yukikaze.GetSpellEntity();
                        }
                    }
                }

                if (ActionManager.LastSpellId == SpellsDefine.Yukikaze)
                {
                    if (SpellsDefine.Hagakure.IsUnlock())
                    {
                        if (SpellsDefine.Hagakure.IsReady())
                        {
                            await SpellsDefine.Hagakure.DoAbility();
                            return SpellsDefine.Hagakure.GetSpellEntity();
                        }
                    }
                }

                if (SpellsDefine.Hakaze.IsUnlock())
                {
                    if (SpellsDefine.Hakaze.IsReady())
                    {
                        await SpellsDefine.Hakaze.DoGCD();
                        return SpellsDefine.Hakaze.GetSpellEntity();
                    }
                }
            }
            else if (Math.Abs(baseGcdTime - 2070) == 0) // 2.07sec gcd 3 filler gcd needed
            {
                if (ActionManager.LastSpellId == SpellsDefine.Hakaze)
                {
                    if (SpellsDefine.Jinpu.IsUnlock())
                    {
                        if (SpellsDefine.Jinpu.IsReady())
                        {
                            await SpellsDefine.Jinpu.DoGCD();
                            return SpellsDefine.Jinpu.GetSpellEntity();
                        }
                    }
                }

                if (ActionManager.LastSpellId == SpellsDefine.Jinpu)
                {
                    if (SpellsDefine.Gekko.IsUnlock())
                    {
                        if (SpellsDefine.Gekko.IsReady())
                        {
                            await SpellsDefine.Gekko.DoGCD();
                            return SpellsDefine.Gekko.GetSpellEntity();
                        }
                    }
                }

                if (ActionManager.LastSpellId == SpellsDefine.Gekko)
                {
                    if (SpellsDefine.Hagakure.IsUnlock())
                    {
                        if (SpellsDefine.Hagakure.IsReady())
                        {
                            await SpellsDefine.Hagakure.DoAbility();
                            return SpellsDefine.Hagakure.GetSpellEntity();
                        }
                    }
                }

                if (SpellsDefine.Hakaze.IsUnlock())
                {
                    if (SpellsDefine.Hakaze.IsReady())
                    {
                        await SpellsDefine.Hakaze.DoGCD();
                        return SpellsDefine.Hakaze.GetSpellEntity();
                    }
                }
            }
            else if (Math.Abs(baseGcdTime - 2000) == 0) // 2.00 seconds gcd 4 filler gcd needed (get better gear lol)
            {
                for (int i = 0; i <= 2; i++)
                {
                    if (ActionManager.LastSpellId == SpellsDefine.Hakaze)
                    {
                        if (SpellsDefine.Yukikaze.IsUnlock())
                        {
                            if (SpellsDefine.Yukikaze.IsReady())
                            {
                                await SpellsDefine.Yukikaze.DoGCD();
                                return SpellsDefine.Yukikaze.GetSpellEntity();
                            }
                        }
                    }

                    if (ActionManager.LastSpellId == SpellsDefine.Yukikaze)
                    {
                        if (SpellsDefine.Hagakure.IsUnlock())
                        {
                            if (SpellsDefine.Hagakure.IsReady())
                            {
                                await SpellsDefine.Hagakure.DoGCD();
                                return SpellsDefine.Hagakure.GetSpellEntity();
                            }
                        }
                    }

                    if (SpellsDefine.Hakaze.IsUnlock())
                    {
                        if (SpellsDefine.Hakaze.IsReady())
                        {
                            await SpellsDefine.Hakaze.DoGCD();
                            return SpellsDefine.Hakaze.GetSpellEntity();
                        }
                    }
                }
            }

            return null;
        }


        public static SpellEntity GetMidareSetsuGekka()
        {
            if (SenCounts() == 3 && !MovementManager.IsMoving)
            {
                AIRoot.GetBattleData<SamuraiBattleData>().CurrCombo = SamuraiComboStages.MidareSetsuGekka;
                return SpellsDefine.MidareSetsugekka.GetSpellEntity();
            }

            return null;
        }

        public static SpellEntity AoEGCD()
        {
            var lastGcd = ActionManager.LastSpellId;

            if (SenCounts() == 2)
            {
                return SpellsDefine.TenkaGoken.GetSpellEntity();
            }

            if (Core.Me.HasAura(AurasDefine.MeikyoShisui))
            {
                if (!ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Ka))
                {
                    return SpellsDefine.Oka.GetSpellEntity();
                }

                if (!ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Getsu))
                {
                    return SpellsDefine.Mangetsu.GetSpellEntity();
                }
            }

            if (lastGcd == SpellsDefine.Fuga)
            {
                if (!ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Ka))
                {
                    return SpellsDefine.Oka.GetSpellEntity();
                }

                if (!ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Getsu))
                {
                    return SpellsDefine.Mangetsu.GetSpellEntity();
                }
            }

            return SpellsDefine.Fuga.GetSpellEntity();
        }

        public static SpellEntity GetHissatsuShinten()
        {
            if (SpellsDefine.HissatsuShinten.IsUnlock())
            {
                if (SpellsDefine.HissatsuShinten.IsReady())
                {
                    if (ActionResourceManager.Samurai.Kenki >= 25)
                    {
                        return SpellsDefine.HissatsuShinten.GetSpellEntity();
                    }
                }
            }

            return null;
        }

        public static SpellEntity IaijutsuCanSpell()
        {
            if (!Core.Me.HasAura(AurasDefine.Kaiten))
                return null;
            if (!Core.Me.HasAura(AurasDefine.Kaiten))
                return null;
            return null;
        }

        public static uint SenCounts()
        {
            uint counts = 0;
            if (ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Getsu))
                counts++;

            if (ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Setsu))
                counts++;

            if (ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Ka))
                counts++;
            return counts;
        }

        public static bool GekkoPOSCheck()
        {
            // Gekko
            if (ActionManager.LastSpellId == SpellsDefine.Jinpu && Core.Me.CurrentTarget.IsBehind)
            {
                return false;
            }

            // Gekko
            if (ActionManager.LastSpellId == SpellsDefine.Jinpu)
            {
                return true;
            }

            return false;
        }

        public static bool KashaPOSCheck()
        {
            // Kasha
            if (ActionManager.LastSpellId == SpellsDefine.Shifu && Core.Me.CurrentTarget.IsFlanking)
            {
                return false;
            }

            // Kasha
            if (ActionManager.LastSpellId == SpellsDefine.Shifu)
            {
                return true;
            }

            return false;
        }

        public static SpellEntity GetsuCombo()
        {
            var lastGcd = ActionManager.LastSpellId;
            //AOE
            if (SpellsDefine.Mangetsu.IsUnlock())
            {
                if (lastGcd == SpellsDefine.Fuga || lastGcd == SpellsDefine.Fuko)
                {
                    return SpellsDefine.Mangetsu.GetSpellEntity();
                }
            }

            if (TargetHelper.CheckNeedUseAOETest(8, 8, 3) && SpellsDefine.Fuga.IsUnlock())
            {
                return GetFuko();
            }

            //Single Target
            if (lastGcd == SpellsDefine.Hakaze)
            {
                if (SpellsDefine.Jinpu.IsUnlock())
                {
                    return SpellsDefine.Jinpu.GetSpellEntity();
                }
            }

            if (lastGcd == SpellsDefine.Jinpu)
            {
                if (SpellsDefine.Gekko.IsUnlock())
                {
                    return SpellsDefine.Gekko.GetSpellEntity();
                }
            }

            return null;
        }

        public static SpellEntity KaCombo()
        {
            var lastGcd = ActionManager.LastSpellId;
            //AOE
            if (SpellsDefine.Oka.IsUnlock())
            {
                if (lastGcd == SpellsDefine.Fuga || lastGcd == SpellsDefine.Fuko)
                {
                    return SpellsDefine.Oka.GetSpellEntity();
                }
            }
            if (TargetHelper.CheckNeedUseAOETest(8, 8, 3) && SpellsDefine.Fuga.IsUnlock())
            {
                return GetFuko();
            }

            //Single Target
            if (lastGcd == SpellsDefine.Hakaze)
            {
                if (SpellsDefine.Shifu.IsUnlock())
                {
                    return SpellsDefine.Shifu.GetSpellEntity();
                }
            }

            if (lastGcd == SpellsDefine.Shifu)
            {
                if (SpellsDefine.Kasha.IsUnlock())
                {
                    return SpellsDefine.Kasha.GetSpellEntity();
                }
            }

            return null;
        }

        public static SpellEntity SetsuCombo()
        {
            var lastGcd = ActionManager.LastSpellId;
            //Single Target
            if (lastGcd == SpellsDefine.Hakaze)
            {
                if (SpellsDefine.Yukikaze.IsUnlock())
                {
                    return SpellsDefine.Yukikaze.GetSpellEntity();
                }
            }

            return null;
        }


        public static SpellEntity GetFuko()
        {
            if (SpellsDefine.Fuko.IsUnlock())
            {
                return SpellsDefine.Fuko.GetSpellEntity();
            }

            return SpellsDefine.Fuga.GetSpellEntity();
        }
    }
}