﻿using AEAssist.View;
using PropertyChanged;

namespace AEAssist
{
    [AddINotifyPropertyChangedInterface]
    public class DebugCenter
    {
        public static DebugCenter Intance = new DebugCenter();

        public string Msg { get; set; } = "Debug";

        public void ShowMsg(string msg, bool check)
        {
            Msg = msg;
            if (!check)
                UIHelper.RfreshCurrOverlay();
        }
    }
}