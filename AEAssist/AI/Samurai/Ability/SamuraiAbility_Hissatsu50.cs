using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;
using ff14bot;
using ff14bot.Objects;

namespace AEAssist.AI.Samurai.Ability
{
    public class SamuraiAbility_Hissatsu50 : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.HissatsuGuren.IsUnlock())
            {
                return -10;
            }

            if (!SpellsDefine.HissatsuGuren.IsReady())
            {
                return -5;
            }

            if (ActionResourceManager.Samurai.Kenki < 25 && !SpellsDefine.Ikishoten.RecentlyUsed())
            {
                return -1;
            }

            return 0;
        }

        private static SpellEntity GetSpell()
        {
            var tar = Core.Me.CurrentTarget as Character;
            if (SpellsDefine.HissatsuSenei.IsUnlock())
            {
                if (!TargetHelper.CheckNeedUseAOETest(10, 10, 2))
                {
                    return SpellsDefine.HissatsuSenei.GetSpellEntity();
                }
            }

            return SpellsDefine.HissatsuGuren.GetSpellEntity();
        }
        
        public async Task<SpellEntity> Run()
        {
            var spell = GetSpell();
            if (spell == null) return null;
            if (await spell.DoAbility())
                return spell;
            return null;
        }
    }
}