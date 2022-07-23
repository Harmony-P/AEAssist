using System;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Samurai.Ability
{
    public class SamuraiAbility_IkishotenHV : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Ikishoten.IsUnlock()) return -1;

            if (!SpellsDefine.Ikishoten.IsReady())
            {
                return -10;
            }
            // if (AIRoot.GetBattleData<SamuraiBattleData>().CurrPhase == SamuraiPhase.EvenMinutesBurstPhase)
            // {

            // }
            //
            // LogHelper.Info("Cooldown for Senei: " + SpellsDefine.HissatsuSenei.GetSpellEntity().Cooldown);
            
            // if (SpellsDefine.HissatsuSenei.GetSpellEntity().Cooldown == TimeSpan.FromMilliseconds(119000))
            // {
            //     return 0;
            // }
            
            if (ActionResourceManager.Samurai.Kenki < 50)
                return 1;
            return -1;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Ikishoten.GetSpellEntity();
            if (spell == null) return null;
            if (await spell.DoAbility())
                return spell;
            return null;
        }
    }
}