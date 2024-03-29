﻿using System;
using System.Collections.Generic;

namespace AEAssist
{
    public class TriggerMgr
    {
        public static TriggerMgr Instance = new TriggerMgr();

        public HashSet<Type> AllActionType = new HashSet<Type>();

        public Dictionary<Type, TriggerAttribute> AllAttrs = new Dictionary<Type, TriggerAttribute>();

        public HashSet<Type> AllCondType = new HashSet<Type>();

        public Dictionary<string, Type> Name2Type = new Dictionary<string, Type>();

        public TriggerMgr()
        {
            var baseType = typeof(ITriggerBase);
            var condType = typeof(ITriggerCond);
            var actionType = typeof(ITriggerAction);
            foreach (var type in GetType().Assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;
                if (!baseType.IsAssignableFrom(type))
                    continue;

                if (condType.IsAssignableFrom(type))
                    AllCondType.Add(type);
                else
                    AllActionType.Add(type);

                var attrs = type.GetCustomAttributes(typeof(TriggerAttribute), false);
                var attr = attrs[0] as TriggerAttribute;
                AllAttrs[type] = attr;

                Name2Type[attr.Name] = type;
            }
        }
    }
}