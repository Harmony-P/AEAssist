﻿using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;

namespace AEAssist.AI.RedMage.Ability
{
    public class RedMageAbility_Fleche : IAIHandler
    {
        uint spell;

        public int Check(SpellEntity lastSpell)
        {
            spell = SpellsDefine.Fleche;
            if (!spell.IsReady())
                return -1;
            //LogHelper.Debug("NO10:" + spell.ToString());
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoAbility()) return spell.GetSpellEntity();

            return null;
        }
    }

}