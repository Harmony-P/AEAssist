using AEAssist.AI.Samurai.Ability;
using AEAssist.AI.Samurai.GCD;
using AEAssist.Helper;
using ff14bot.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEAssist.AI.Samurai
{
    [Job(ClassJobType.Samurai)]
    public class Samurai_AIPriorityQueue : IAIPriorityQueue
    {
        public List<IAIHandler> GCDQueue { get; } = new List<IAIHandler>
        {
            // // new SamuraiGCD_AoERotations(),
            // new SamuraiGCD_OddMinuteBurst(),
            // // new SamuraiGCD_EvenMinutesBurst(),
            // // new SamuraiGCD_Fillers(),
            // new SamuraiGCD_MidareSetsugekka(),
            // new SamuraiGCD_KaeshiSetsugekka(),
            // new SamuraiGCD_Higanbana(),
            // new SamuraiGCD_CoolDownPhase(),
            
            //SAM -- Harmony Version
            new SamuraiGCD_KaeshiIaijutsu(),
            new SamuraiGCD_Iaijutsu(),
            new SamuraiGCD_MeikoGCD(),
            new SamuraiGCD_BaseGCD(),
        };

        public List<IAIHandler> AbilityQueue { get; } = new List<IAIHandler>
        {
            // new SamuraiAbility_HissatsuShinten(),
            // // new SamuraiAbility_HissatsuKyuten(),
            // // new SamuraiGCD_Dot(),
            // // new SamuraiGCD_OgiNamikiriCombo(),
            // // new SamuraiAbility_HissatsuKaiten(),
            // // new SamuraiAbility_KaeshiSetsugekka(),
            // // new SamuraiAbility_HissatsuSenei(),
            // new SamuraiAbility_MeikyoShisui(),
            // // new SamuraiAbility_Ikishoten(),
            // // new SamuraiAbility_Shoha(),
            // // new SamuraiAbility_TsubameGaeshi()
            
            //SAM -- Harmony Version
            new SamuraiAbility_Meikyo(),
            new SamuraiAbility_HissatsuKaiten(),
            new SamuraiAbility_IkishotenHV(),
            new SamuraiAbility_Hissatsu50(),
            new SamuraiAbility_Hissatsu25(),
            new SamuraiAbility_Shoha(),
            new SamuraiAbility_TrueNorth(),
            
        };

        public Task<bool> UsePotion()
        {
            return PotionHelper.ForceUsePotion(SettingMgr.GetSetting<GeneralSettings>().StrPotionId);
        }
    }
}