using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEAssist.AI.Sage.GCD
{
    public class SageGcdPhlegma : IAIHandler
    {
        private bool IfRaidBuffs()
        {
            List<uint> raidbuffs = new List<uint>
            {
                AurasDefine.BattleLitany,
                AurasDefine.Brotherhood,
                //AurasDefine.ArcaneCircle,
                AurasDefine.BattleVoice,
                //AurasDefine.SearingLight,
                AurasDefine.Embolden,
                AurasDefine.Divination,
                AurasDefine.VulnerabilityTrickAttack,
                AurasDefine.Divination
            };
            if (Core.Me.HasAnyAura(raidbuffs))
            {
                return true;
            }
            if (Core.Me.CurrentTarget.HasAnyAura(raidbuffs))
            {
                return true;
            }
            return false;
        }
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Phlegma.IsUnlock())
            {
                return -20;
            }
            if (!ActionManager.CanCastOrQueue(GetPhlegma().SpellData, Core.Me.CurrentTarget))
            {
                LogHelper.Debug("Can't Cast Phlegma distance maybe too much?");
                return -6;
            }

            var spell = GetPhlegma();
            var charges = spell.SpellData.Charges;

            LogHelper.Debug("Current Phlegma Charge is: " + charges);
            
            if (charges == 0)
            {
                LogHelper.Debug("Phlegma has 0 charges meaning is not ready so skip it.");
                return -1;
            }
            
            if (IfRaidBuffs())
            {
                return 1;
            }
            // If we are not moving check how many charges left for phlegma; don't waste it keep it for movement.
            if (MovementManager.IsMoving) return 2;
            if (spell.IsMaxChargeReady(0.3f))
            {
                LogHelper.Debug("即将溢出");
                return 3;
            }
            //if (!(phlegmaCharges < 2) && !(phlegmaChargesII < 2) && !(phlegmaChargesIII < 2)) return 3;
            LogHelper.Debug("Not wasting Phlegma while standing still saving it for movement cast.");
            return -1;
        }
        private static SpellEntity GetPhlegma()
        {
            if (SpellsDefine.PhlegmaIII.IsUnlock())
            {
                if (!ActionManager.HasSpell(SpellsDefine.PhlegmaII))
                {
                    return null;
                }

                return SpellsDefine.PhlegmaIII.GetSpellEntity();
            }
            
            if (SpellsDefine.PhlegmaII.IsUnlock())
            {
                if (!ActionManager.HasSpell(SpellsDefine.Phlegma))
                {
                    return null;
                }

                return SpellsDefine.PhlegmaII.GetSpellEntity();
            }

            return SpellsDefine.Phlegma.GetSpellEntity();
        }

        public async Task<SpellEntity> Run()
        {
            if (DataBinding.Instance.UseAOE)
            {
                var aoeChecker = TargetHelper.CheckNeedUseAOE(12, 5, ConstValue.SageAOECount);
                if (aoeChecker)
                {
                    var spellData = GetPhlegma();
                    if (spellData == null)
                    {
                        LogHelper.Error("Failed to get spell returning null;");
                        return null;
                    }
                    LogHelper.Debug("Doing Phlegma AOE");
                    if (await spellData.DoGCD()) return spellData;
                }
            }

            var spell = GetPhlegma();
            if (spell == null) return null;
            var ret = await spell.DoGCD();
            return ret ? spell : null;
        }
    }
}