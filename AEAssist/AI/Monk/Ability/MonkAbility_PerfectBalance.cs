using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using System;
using System.Threading.Tasks;

namespace AEAssist.AI.Monk.Ability
{
    public class MonkAbility_PerfectBalance : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (AIRoot.Instance.CloseBurst)
                return -5;

            if (!SpellsDefine.PerfectBalance.IsUnlock())
            {
                return -10;
            }

            if (!SpellsDefine.PerfectBalance.IsReady())
            {
                return -1;
            }

            if (SpellsDefine.PerfectBalance.RecentlyUsed() || Core.Me.HasAura(AurasDefine.PerfectBalance))
            {
                return -2;
            }

            if (ActionResourceManager.Monk.BlitzTimer != TimeSpan.Zero)
            {
                return -5;
            }

            if (Core.Me.HasAura(AurasDefine.FormlessFist) || MonkSpellHelper.LastSpellWasNadi())
            {
                return -4;
            }

            if (!MonkSpellHelper.InRaptorForm())
            {
                return -3;
            }



            var target = Core.Me.CurrentTarget as Character;

            if (!SpellsDefine.ElixirField.IsUnlock())
            {
                // dmg buff
                if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.DisciplinedFist, 9000))
                {
                    // using dot
                    if (MonkSpellHelper.UsingDot())
                    {
                        // dot timeleft safe
                        if (target.HasMyAuraWithTimeleft(AurasDefine.Demolish, 9000))
                        {
                            AIRoot.GetBattleData<MonkBattleData>().CurrentMonkNadiCombo = MonkNadiCombo.Lunar;
                            return 1;
                        }
                    }
                    // not using dot
                    else
                    {
                        AIRoot.GetBattleData<MonkBattleData>().CurrentMonkNadiCombo = MonkNadiCombo.Lunar;
                        return 1;
                    }
                }

                return -4;
            }
            
            if (!SpellsDefine.RiddleofFire.IsUnlock())
            {
                // dmg buff
                if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.DisciplinedFist, 9000))
                {
                    if (ActionResourceManager.Monk.ActiveNadi == ActionResourceManager.Monk.Nadi.Solar ||
                        ActionResourceManager.Monk.ActiveNadi == ActionResourceManager.Monk.Nadi.Both)
                    {
                        AIRoot.GetBattleData<MonkBattleData>().CurrentMonkNadiCombo = MonkNadiCombo.Lunar;
                    }
                    else if (ActionResourceManager.Monk.ActiveNadi == ActionResourceManager.Monk.Nadi.Lunar)
                    {
                        AIRoot.GetBattleData<MonkBattleData>().CurrentMonkNadiCombo = MonkNadiCombo.Solar;
                    }
                    else if (ActionResourceManager.Monk.ActiveNadi == ActionResourceManager.Monk.Nadi.None)
                    {
                        if (MonkSpellHelper.UsingDot())
                        {
                            // dot timeleft safe
                            if (target.HasMyAuraWithTimeleft(AurasDefine.Demolish, 9000))
                            {
                                AIRoot.GetBattleData<MonkBattleData>().CurrentMonkNadiCombo = MonkNadiCombo.Lunar;
                            }
                            else
                            {
                                AIRoot.GetBattleData<MonkBattleData>().CurrentMonkNadiCombo = MonkNadiCombo.Solar;
                            }
                        }
                        else
                        {
                            AIRoot.GetBattleData<MonkBattleData>().CurrentMonkNadiCombo = MonkNadiCombo.Lunar;
                        }

                    }
                    return 1;
                }

                return -4;
            }

            //ROF + BH --- Even Window
            if (Core.Me.HasAura(AurasDefine.Brotherhood) || SpellsDefine.Brotherhood.CoolDownInGCDs(5) ||
                SpellsDefine.Brotherhood.RecentlyUsed())
            {
                //Pre
                if (!Core.Me.HasAura(AurasDefine.RiddleOfFire))
                {
                    //ROF+BH 1&3
                    if (SpellsDefine.RiddleofFire.AbilityCoolDownInNextXGCDsWindow(0))
                    {
                        if (target.HasMyAuraWithTimeleft(AurasDefine.Demolish, 15000) &&
                            Core.Me.HasMyAuraWithTimeleft(AurasDefine.DisciplinedFist, 10000))
                        {
                            AIRoot.GetBattleData<MonkBattleData>().CurrentMonkNadiCombo = MonkNadiCombo.Lunar;
                            return 3;
                        }

                        AIRoot.GetBattleData<MonkBattleData>().CurrentMonkNadiCombo = MonkNadiCombo.Solar;
                        return 1;
                    }

                    //ROF+BH 2
                    if (SpellsDefine.RiddleofFire.AbilityCoolDownInNextXGCDsWindow(3))
                    {
                        if (target.HasMyAuraWithTimeleft(AurasDefine.Demolish, 15000))
                        {
                            AIRoot.GetBattleData<MonkBattleData>().CurrentMonkNadiCombo = MonkNadiCombo.Solar;
                            AIRoot.GetBattleData<MonkBattleData>().RoFBH2 = true;
                            return 20;
                        }
                    }
                }


                //during rof 
                else
                {
                    //ROF+BH 2
                    if (AIRoot.GetBattleData<MonkBattleData>().RoFBH2)
                    {
                        if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.DisciplinedFist, 10000) &&
                            target.HasMyAuraWithTimeleft(AurasDefine.Demolish, 15000))
                        {
                            AIRoot.GetBattleData<MonkBattleData>().CurrentMonkNadiCombo = MonkNadiCombo.Lunar;
                            return 1;
                        }
                    }
                    else
                    {
                        //ROF+BH 1&3
                        if (ActionResourceManager.Monk.ActiveNadi == ActionResourceManager.Monk.Nadi.Solar ||
                            ActionResourceManager.Monk.ActiveNadi == ActionResourceManager.Monk.Nadi.Both)
                        {
                            AIRoot.GetBattleData<MonkBattleData>().CurrentMonkNadiCombo = MonkNadiCombo.Lunar;
                        }
                        else if (ActionResourceManager.Monk.ActiveNadi == ActionResourceManager.Monk.Nadi.Lunar)
                        {
                            AIRoot.GetBattleData<MonkBattleData>().CurrentMonkNadiCombo = MonkNadiCombo.Solar;
                        }
                        else if (ActionResourceManager.Monk.ActiveNadi == ActionResourceManager.Monk.Nadi.None)
                        {
                            if (!target.HasMyAuraWithTimeleft(AurasDefine.Demolish, 6000))
                            {
                                AIRoot.GetBattleData<MonkBattleData>().CurrentMonkNadiCombo = MonkNadiCombo.Solar;
                            }
                            else
                            {
                                AIRoot.GetBattleData<MonkBattleData>().CurrentMonkNadiCombo = MonkNadiCombo.Lunar;
                            }
                        }


                        return 1;
                    }
                }
            }
            //odd window
            else
            {
                //Pre PB
                if (SpellsDefine.RiddleofFire.AbilityCoolDownInNextXGCDsWindow(2) && !Core.Me.HasAura(AurasDefine.RiddleOfFire))
                {
                    if (target.HasMyAuraWithTimeleft(AurasDefine.Demolish, 15000) &&
                        Core.Me.HasMyAuraWithTimeleft(AurasDefine.DisciplinedFist, 10000))
                    {
                        AIRoot.GetBattleData<MonkBattleData>().CurrentMonkNadiCombo = MonkNadiCombo.Lunar;
                        return 4;
                    }
                }

                if (Core.Me.HasMyAura(AurasDefine.RiddleOfFire))
                {
                    if (target.HasMyAuraWithTimeleft(AurasDefine.Demolish, 15000) &&
                        Core.Me.HasMyAuraWithTimeleft(AurasDefine.DisciplinedFist, 10000))
                    {
                        AIRoot.GetBattleData<MonkBattleData>().CurrentMonkNadiCombo = MonkNadiCombo.Lunar;
                        return 5;
                    }
                }
            }


            return -4;
        }


        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.PerfectBalance.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}