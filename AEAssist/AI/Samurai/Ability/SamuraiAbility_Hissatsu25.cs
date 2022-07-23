using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;
using ff14bot;
using ff14bot.Objects;

namespace AEAssist.AI.Samurai.Ability
{
    public class SamuraiAbility_Hissatsu25 : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.HissatsuShinten.IsUnlock())
            {
                return -10;
            }
            
            if (!Core.Me.HasMyAura(AurasDefine.Fugetsu) && ActionResourceManager.Samurai.Kenki < 80)
            {
                return -3;
            }
            
            if (SpellsDefine.HissatsuShinten.RecentlyUsed() || SpellsDefine.HissatsuKyuten.RecentlyUsed())
            {
                return -5;
            }

            if (ActionResourceManager.Samurai.Kenki < 45)
            {
                return -1;
            }

            return 0;
        }

        private static SpellEntity GetSpell()
        {
            if (SpellsDefine.HissatsuKyuten.IsUnlock())
            {
                if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 3))
                {
                    return SpellsDefine.HissatsuKyuten.GetSpellEntity();
                }
            }

            return SpellsDefine.HissatsuShinten.GetSpellEntity();
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