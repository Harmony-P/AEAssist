﻿#pragma checksum "..\..\..\View\GeneralSettingView.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "1E803A76D09CB411031DFC4E0F8E0445FF28AF498BDB08A459CE25960E7A147A"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using AEAssist;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace AEAssist.View {
    
    
    /// <summary>
    /// GeneralSettingView
    /// </summary>
    public partial class GeneralSettingView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 47 "..\..\..\View\GeneralSettingView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox SwitchLan;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\View\GeneralSettingView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ShowOverlay;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\View\GeneralSettingView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox ShowGameLog;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\View\GeneralSettingView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox ShowAbilityDebug;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\View\GeneralSettingView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox ShowBattleTime;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\View\GeneralSettingView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox UseHotkey;
        
        #line default
        #line hidden
        
        
        #line 83 "..\..\..\View\GeneralSettingView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Hotkey_Stop;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\..\View\GeneralSettingView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Hotkey_CloseBuff;
        
        #line default
        #line hidden
        
        
        #line 130 "..\..\..\View\GeneralSettingView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Dex_ChoosePotion;
        
        #line default
        #line hidden
        
        
        #line 135 "..\..\..\View\GeneralSettingView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Str_ChoosePotion;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/AEAssist;component/view/generalsettingview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\GeneralSettingView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.SwitchLan = ((System.Windows.Controls.ComboBox)(target));
            
            #line 48 "..\..\..\View\GeneralSettingView.xaml"
            this.SwitchLan.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.SwitchLan_OnSelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ShowOverlay = ((System.Windows.Controls.Button)(target));
            
            #line 58 "..\..\..\View\GeneralSettingView.xaml"
            this.ShowOverlay.Click += new System.Windows.RoutedEventHandler(this.ShowOverlay_OnClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ShowGameLog = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 4:
            this.ShowAbilityDebug = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 5:
            this.ShowBattleTime = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 6:
            this.UseHotkey = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 7:
            this.Hotkey_Stop = ((System.Windows.Controls.ComboBox)(target));
            
            #line 84 "..\..\..\View\GeneralSettingView.xaml"
            this.Hotkey_Stop.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.Hotkey_Stop_OnSelectionChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.Hotkey_CloseBuff = ((System.Windows.Controls.ComboBox)(target));
            
            #line 89 "..\..\..\View\GeneralSettingView.xaml"
            this.Hotkey_CloseBuff.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.Hotkey_CloseBuff_OnSelectionChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.Dex_ChoosePotion = ((System.Windows.Controls.ComboBox)(target));
            
            #line 131 "..\..\..\View\GeneralSettingView.xaml"
            this.Dex_ChoosePotion.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.Dex_ChoosePotion_OnSelectionChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            this.Str_ChoosePotion = ((System.Windows.Controls.ComboBox)(target));
            
            #line 136 "..\..\..\View\GeneralSettingView.xaml"
            this.Str_ChoosePotion.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.Str_ChoosePotion_OnSelectionChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

