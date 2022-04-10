﻿using System;

namespace AETriggers.TriggerModel
{
    [Trigger(name: "SwitchDOT", remark: "是否允许使用DOT")]
    public class TriggerAction_SwitchDOT : ITriggerAction
    {
        public bool value;

        public void WriteFromJson(string[] values)
        {
            if (!int.TryParse(values[0], out var va))
            {
                throw new Exception($"{values[0]}格式错误!\n");
            }

            this.value = va == 1;
        }
    }
}