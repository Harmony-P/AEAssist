﻿using System.Windows.Media;
using ff14bot.Helpers;

namespace AEAssist
{
    public static class LogHelper
    {
        public static void Debug(string msg)
        {
            Logging.Write($"[AEAssist DEBUG] {msg}");
        }
        
        public static void Info(string msg)
        {
            Logging.Write(Colors.Green,$"[AEAssist Info] {msg}");
        }

        public static void Error(string msg)
        {
            Logging.Write(Colors.DarkRed, $"[AEAssist Error] {msg}");
        }
    }
}