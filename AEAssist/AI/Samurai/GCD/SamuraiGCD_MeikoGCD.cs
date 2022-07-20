using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;
using ff14bot.Managers;

namespace AEAssist.AI.Samurai.GCD
{
    public class SamuraiGCD_MeikoGCD : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (Core.Me.HasMyAura(AurasDefine.MeikyoShisui) || SpellsDefine.MeikyoShisui.RecentlyUsed())
            {
                return 1;
            }
            return -5;
        }
        //Ka -- hana
        //Setsu -- yuki
        //Getsu -- lunar 

        private static SpellEntity GetSpell()
        {
            if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 3))
            {
                if (!ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Getsu))
                {
                    return SpellsDefine.Mangetsu.GetSpellEntity();
                }
                if (!ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Ka))
                {
                    return SpellsDefine.Oka.GetSpellEntity();
                }
            }
            else
            {
                if (!ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Getsu))
                {
                    return SpellsDefine.Gekko.GetSpellEntity();
                }
                if (!ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Ka))
                {
                    return SpellsDefine.Kasha.GetSpellEntity();
                }
                if (!ActionResourceManager.Samurai.Sen.HasFlag(ActionResourceManager.Samurai.Iaijutsu.Setsu))
                {
                    return SpellsDefine.Yukikaze.GetSpellEntity();
                }
            }
            return SpellsDefine.Gekko.GetSpellEntity();
        }


        public async Task<SpellEntity> Run()
        {
            var spell = GetSpell();
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret)
                return spell;
            return null;
        }
    }
}