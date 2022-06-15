﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AEAssist.AI.Monk.GCD;
using AEAssist.Helper;
using ff14bot.Enums;

namespace AEAssist.AI.Monk
{
    [Job(ClassJobType.Monk)]
    public class Monk_AIPriority : IAIPriorityQueue
    {
        public List<IAIHandler> GCDQueue { get; } = new List<IAIHandler>()
        {
            new MonkGCD_PerfectBalanceGCD(),
            new MonkGCD_BaseGCD(),
        };

        public List<IAIHandler> AbilityQueue { get; } = new List<IAIHandler>()
        {

        };
        public async Task<bool> UsePotion()
        {
            return await PotionHelper.ForceUsePotion(SettingMgr.GetSetting<GeneralSettings>().StrPotionId);
        }
    }
}