using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;
using ff14bot;

namespace AEAssist.AI.Samurai.Ability
{
    public class SamuraiAbility_TrueNorth : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (AIRoot.Instance.CloseBurst)
                return -5;

            if (!AEAssist.DataBinding.Instance.UseTrueNorth)
                return -10;

            if (!SpellsDefine.TrueNorth.IsReady())
                return -1;

            if (Core.Me.HasAura(AurasDefine.TrueNorth)
                || SpellsDefine.TrueNorth.RecentlyUsed())
                return -9;


            //alway use in the second half of GCD
            if (!AIRoot.Instance.Is2ndAbilityTime())
                return -11;

            if (ActionManager.LastSpellId == SpellsDefine.Jinpu)
            {
                if (!Core.Me.CurrentTarget.IsBehind)
                {
                    return 1;
                }
            }

            if (ActionManager.LastSpellId == SpellsDefine.Shifu)
            {
                if (!Core.Me.CurrentTarget.IsFlanking)
                {
                    return 2;
                }
            }

            return -5;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.TrueNorth.GetSpellEntity();
            if (spell == null) return null;
            if (await spell.DoAbility())
                return spell;
            return null;
        }
    }
}