﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AEAssist.AI;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist
{
    public class RotationManager
    {
        public static RotationManager Instance = new RotationManager();

        public Dictionary<ClassJobType, IRotation> AllRotations = new Dictionary<ClassJobType, IRotation>();

        public DefaultRotation DefaultRotation = new DefaultRotation();

        private ClassJobType _classJobType;

        public void Init()
        {
            AllRotations.Clear();
            var baseType = typeof(IRotation);
            foreach (var type in GetType().Assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;
                if (!baseType.IsAssignableFrom(type))
                    continue;
                if (type == typeof(DefaultRotation))
                    continue;
                var attrs = type.GetCustomAttributes(typeof(RotationAttribute), false);
                if (attrs.Length == 0)
                {
                    LogHelper.Error($"Rotation class [{type}] need RotationAttribute");
                    continue;
                }

                var attr = attrs[0] as RotationAttribute;
                AllRotations[attr.ClassJobType] = Activator.CreateInstance(type) as IRotation;
                LogHelper.Info("Load Rotation: " + attr.ClassJobType);
            }
        }

        public void CheckChangeJob()
        {
            if (_classJobType != Core.Me.CurrentJob)
            {
                _classJobType = Core.Me.CurrentJob;
                GetRotation().Init();
            }
        }

        private IRotation GetRotation()
        {
            if (AllRotations.TryGetValue(Core.Me.CurrentJob, out var job)) return job;

            return DefaultRotation;
        }

        public Task<bool> Rest()
        {
            return GetRotation().Rest();
        }

        public Task<bool> PreCombatBuff()
        {
            return GetRotation().PreCombatBuff();
        }

        public Task<bool> Pull()
        {
            return GetRotation().Pull();
        }

        public Task<bool> Heal()
        {
            CountDownHandler.Instance.Update();
            TargetMgr.Instance.Update();
            return AIRoot.Instance.Update();
        }

        public Task<bool> CombatBuff()
        {
            return GetRotation().CombatBuff();
        }

        public Task<bool> Combat()
        {
            return GetRotation().Combat();
        }

        public Task<bool> PullBuff()
        {
            return GetRotation().PullBuff();
        }

        public SpellData GetBaseGCDSpell()
        {
            return GetRotation().GetBaseGCDSpell();
        }
    }
}