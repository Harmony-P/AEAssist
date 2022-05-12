﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AEAssist.AI;
using AEAssist;
using AEAssist.Define;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Managers;

namespace AEAssist.Helper
{
    internal class PotionHelper
    {
        public static List<PotionData> DexPotions { get; set; } = new List<PotionData>();
        public static List<PotionData> StrPotions { get; set; } = new List<PotionData>();
        public static List<PotionData> MindPotions { get; set; } = new List<PotionData>();

        public static void Init()
        {
            if (DexPotions == null)
                DexPotions = new List<PotionData>();
            DexPotions.Clear();
            DexPotions.Add(new PotionData
            {
                ID = 36110,
                Name = "Grade 6 Dex/6级巧力之幻药"
            });
            DexPotions.Add(new PotionData
            {
                ID = 36105,
                Name = "Grade 5 Dex/5级巧力之幻药"
            });

            DexPotions.Add(new PotionData
            {
                ID = 31894,
                Name = "Grade 4 Dex/4级巧力之幻药"
            });

            DexPotions.Add(new PotionData
            {
                ID = 29493,
                Name = "Grade 3 Dex/3级巧力之幻药"
            });


            if (StrPotions == null)
                StrPotions = new List<PotionData>();
            StrPotions.Add(new PotionData
            {
                ID = 36109,
                Name = "Grade 6 Str/6级刚力之幻药"
            });
            StrPotions.Add(new PotionData
            {
                ID = 36104,
                Name = "Grade 5 Str/5级刚力之幻药"
            });
            StrPotions.Add(new PotionData
            {
                ID = 31893,
                Name = "Grade 4 Str/4级刚力之幻药"
            });
            StrPotions.Add(new PotionData
            {
                ID = 29492,
                Name = "Grade 3 Str/3级刚力之幻药"
            });
            
            // TODO: This need to be fixed and find out correct ID and translate to CN.
            if (MindPotions == null)
                MindPotions = new List<PotionData>();
            MindPotions.Add(new PotionData
            {
                ID = 0000,
                Name = "Grade 6 Mind/Todo:cnd name?"
            });
            MindPotions.Add(new PotionData
            {
                ID = 0000,
                Name = "Grade 5 Mind/Todo:cnd name?"
            });
            MindPotions.Add(new PotionData
            {
                ID = 0000,
                Name = "Grade 4 Mind/Todo:cnd name?"
            });
            MindPotions.Add(new PotionData
            {
                ID = 0000,
                Name = "Grade 3 Mind/Todo:cnd name?"
            });
            
        }

        public static async Task<bool> UsePotion(int potionRawId)
        {
            if (!SettingMgr.GetSetting<GeneralSettings>().UsePotion)
                return false;
            if (AIRoot.Instance.CloseBurst)
                return false;
            return await ForceUsePotion(potionRawId);
        }

        public static async Task<bool> ForceUsePotion(int potionRawId)
        {
            var item = InventoryManager.FilledSlots.FirstOrDefault(s => s.RawItemId == potionRawId);
            if (item == null)
                return false;
            if (!item.CanUse(Core.Me)) return false;
            LogHelper.Info($@"Using Item >>> {item.Name}");
            for (var i = 0; i < 15; i++)
            {
                item.UseItem(Core.Me);
                await Coroutine.Wait(100, () => false);
                if (item == null || !item.CanUse())
                    return true;
            }

            return false;
        }

        internal static bool CheckPotion(int potionRawId)
        {
            var item = InventoryManager.FilledSlots.FirstOrDefault(s => s.RawItemId == potionRawId);

            if (item == null || !item.CanUse(Core.Me)) return false;

            return true;
        }

        // public static void DebugAllItems()
        // {
        //     foreach (var v in InventoryManager.FilledSlots)
        //     {
        //         LogHelper.Info($"道具 {v.RawItemId} {v.Name} {v.Item.ChnName}");
        //     }
        // }

        public static int CheckNum(int potionId)
        {
            var num = 0;
            foreach (var v in InventoryManager.FilledSlots)
                if (v.RawItemId == potionId)
                    num = (int) (num + v.Item.ItemCount());

            return num;
        }
    }
}