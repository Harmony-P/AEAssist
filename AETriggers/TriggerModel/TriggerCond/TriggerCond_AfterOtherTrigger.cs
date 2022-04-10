﻿using System;

namespace AETriggers.TriggerModel
{
    [Trigger(name: "AfterOtherTrigger", remark: "某组触发器触发后过了多久")]
    public class TriggerCond_AfterOtherTrigger : ITriggerCond
    {
        public string TriggerId;
        public int Time;

        public void WriteFromJson(string[] values)
        {
            this.TriggerId = values[0];
#if Trigger
            if (!Entry.AllExcelData.ContainsKey(this.TriggerId))
            {
                throw new Exception($"不存在组Id: {values[0]}!\n");
            }
#endif

            if (!int.TryParse(values[1], out var time))
            {
                throw new Exception($"{values[1]}格式错误!\n");
            }

            this.Time = time;
            if (Time < 0)
            {
                throw new Exception("参数配置了小于0的值");
            }
        }
    }
}