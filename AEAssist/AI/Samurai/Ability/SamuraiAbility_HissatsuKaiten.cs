using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;
using Common.Logging.Configuration;
using ff14bot.Managers;

namespace AEAssist.AI.Samurai.Ability
{
    public class SamuraiAbility_HissatsuKaiten : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.HissatsuKaiten.IsUnlock())
            {
                return -20;
            }

            if (Core.Me.HasMyAura(AurasDefine.Kaiten) || SpellsDefine.HissatsuKaiten.RecentlyUsed())
            {
                return -10;
            }
            
            if (ActionResourceManager.Samurai.Kenki < 20)
            {
                return -1;
            }

            if (MovementManager.IsMoving)
            {
                return -2;
            }

            if (!AIRoot.Instance.Is2ndAbilityTime())
            {
                return -3;
            }

            if (SamuraiSpellHelper.SenCounts() == 1 &&
                SamuraiSpellHelper.NeedsDot())
            {
                    return 0;
            }

            if (SamuraiSpellHelper.SenCounts() == 3)
            {
                    return 1;
            }
            
            //AOE
            if (SamuraiSpellHelper.SenCounts() == 2)
            {
                if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 3))
                {
                    return 2;
                }
            }
            
            // if (SamuraiSpellHelper.SenCounts() == 2)
            // {
            //     if (!ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Ka))
            //     {
            //         if (ActionManager.LastSpellId == SpellsDefine.Kasha ||
            //             ActionManager.LastSpellId == SpellsDefine.Oka)
            //         {
            //             return 2;
            //         }
            //     }
            //     if (!ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Getsu))
            //     {
            //         if (ActionManager.LastSpellId == SpellsDefine.Gekko ||
            //             ActionManager.LastSpellId == SpellsDefine.Mangetsu)
            //         {
            //             return 2;
            //         }
            //     }
            //     if (!ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Setsu))
            //     {
            //         if (ActionManager.LastSpellId == SpellsDefine.Yukikaze)
            //         {
            //             return 2;
            //         }
            //     }
            // }

            return -1;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.HissatsuKaiten.GetSpellEntity();
            if (spell == null) return null;
            if (await spell.DoAbility())
                return spell;
            return null;
        }
    }
}