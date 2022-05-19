﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using AEAssist.Helper;
using Buddy.Overlay;
using ff14bot;
using ff14bot.Enums;

namespace AEAssist.View.OverlayManager
{
    public class OverlayManager
    {
        public static OverlayManager Instance = new OverlayManager();

        private readonly Dictionary<ClassJobType, List<OverlayUIComponent>> AllOverlays =
            new Dictionary<ClassJobType, List<OverlayUIComponent>>();

        private ClassJobType _classJobType;
        
        private HashSet<OverlayUIComponent> _existOverlays = new HashSet<OverlayUIComponent>();

        private HashSet<OverlayUIComponent> _delSet = new HashSet<OverlayUIComponent>();

        public void Init()
        {
            var baseType = typeof(OverlayUIComponent);
            AllOverlays.Clear();
            foreach (var type in GetType().Assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;
                if (!baseType.IsAssignableFrom(type))
                    continue;
                var attrs = type.GetCustomAttributes(typeof(JobAttribute), false);
                if (attrs.Length == 0) continue;

                var attr = attrs[0] as JobAttribute;
                if(!AllOverlays.ContainsKey(attr.ClassJobType))
                    AllOverlays.Add(attr.ClassJobType, new List<OverlayUIComponent>());
                AllOverlays[attr.ClassJobType].Add(Activator.CreateInstance(type) as OverlayUIComponent);
            }
        }

        public void SwitchJob()
        {
            if (!Core.OverlayManager.IsActive)
                return;
            var currJob = Core.Me.CurrentJob;
            if (currJob == _classJobType)
                return;

            Close();
            Open();
        }

        public void SwitchOverlay()
        {
            if (!Core.OverlayManager.IsActive)
                return;
            if (_existOverlays.Count>0)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        void Open()
        {
            var currJob = Core.Me.CurrentJob;
            _classJobType = currJob;
            if (AllOverlays.TryGetValue(currJob, out var window))
            {
                foreach (var v in window)
                {
                    LogHelper.Info("AddOverlay   "+ v.GetType().Name);
                    if (_existOverlays.Add(v))
                        Core.OverlayManager.AddUIComponent(v);
                }
            }
        }

        public void Close()
        {
            _delSet.Clear();

            foreach (var v in _existOverlays)
            {
                LogHelper.Info("RemoveOverlay   "+ v.GetType().Name);
                Core.OverlayManager.RemoveUIComponent(v);
                _delSet.Add(v);
            }
            
            foreach (var v in _delSet)
            {
                _existOverlays.Remove(v);
            }
        }
        

        public void RefreshOverlay()
        {
            foreach (var v in _existOverlays)
            {
                v.Control.Refresh();
            }
        }
    }
}