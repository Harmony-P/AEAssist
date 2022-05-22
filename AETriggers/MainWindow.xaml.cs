﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AETriggers;
using Microsoft.Win32;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Trigger = AEAssist.Trigger;

namespace AEAssist
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public class TriggerTypeData
        {
            public string Name { get; set; }
            public string Tooltip { get; set; }
        }

        private RenameWindow _renameWindow;
        
        public MainWindow()
        {
            InitializeComponent();
            /*var currCulture = CultureInfo.CurrentCulture;
            if (currCulture.Name.Contains("zh-CN"))
            {
                Load.Content = "加载Excel表";
                Export.Content = "导出";
            }*/

            var list1 = new List<TriggerTypeData>();
            foreach (var v in TriggerMgr.Instance.AllCondType)
            {
                var attr = TriggerMgr.Instance.AllAttrs[v];
                list1.Add(new TriggerTypeData()
                {
                    Name = attr.Name,
                    Tooltip = attr.Tooltip
                });
            }
            Conds.ItemsSource = list1;


            var list2 = new List<TriggerTypeData>();
            foreach (var v in TriggerMgr.Instance.AllActionType)
            {
                var attr = TriggerMgr.Instance.AllAttrs[v];
                list2.Add(new TriggerTypeData()
                {
                    Name = attr.Name,
                    Tooltip = attr.Tooltip
                });
            }
            Actions.ItemsSource = list2;

            JobComboBox.SelectedValue = Jobs.Any;

            _renameWindow = new RenameWindow();
            
            Entry.Init();
        }
        
        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            var TriggerLine = DataBinding.Instance.Export();
            if (TriggerLine == null)
                return;
            var openFile = new SaveFileDialog();
            openFile.Filter = "Json(*.json)|*.json";
            openFile.InitialDirectory = new DirectoryInfo("../").FullName;
            openFile.FileName =
                $"[{TriggerLine.TargetJob}] [{TriggerLine.Name}] [{TriggerLine.Author}].json";
            var ret = openFile.ShowDialog();
            if (!ret.HasValue || !ret.Value)
                return;
            TriggerHelper.SaveTriggerLine(TriggerLine, openFile.FileName);
            MessageBox.Show("Export Success!");
        }

        private string ListToString(string[] pa)
        {
            var str = string.Empty;
            if (pa != null && pa.Length > 0)
                foreach (var v in pa)
                    str += v + ",";

            return $"[{str}]";
        }

        /*private void RefreshUI()
        {
            var TriggerLine = Entry.TriggerLine;
            var AllExcelData = Entry.AllExcelData;
            if (TriggerLine == null)
            {
                AuthorName.Content = string.Empty;
                Name.Content = string.Empty;
                TargetZone.Content = string.Empty;
                TargetJob.Content = string.Empty;
                return;
            }

            AuthorName.Content = TriggerLine.Author;
            Name.Content = TriggerLine.Name;
            TargetZone.Content = $"{TriggerLine.CurrZoneId} | {TriggerLine.SubZoneId}";
            TargetJob.Content = TriggerLine.TargetJob;
        }*/
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            DragMove();
        }

        private void Selector_OnSelected(object sender, EventArgs eventArgs)
        {
            DataBinding.Instance.TargetJob = JobComboBox.SelectedValue.ToString();
        }

        private void AddConditionOrActionBtn_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void AddGrouIdBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var newId = IdTextBox.Text;
            if (string.IsNullOrEmpty(newId) || DataBinding.Instance.GroupIds.Contains(newId))
            {
                MessageBox.Show("Repeat or Null!");
                return;
            }
            DataBinding.Instance.GroupIds.Add(newId);
            DataBinding.Instance.AllGroupData[newId] = new DataBinding.GroupData();
        }

        private void IdTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var newId = IdTextBox.Text;
                if (string.IsNullOrEmpty(newId) || DataBinding.Instance.GroupIds.Contains(newId))
                {
                    MessageBox.Show("Repeat or Null!");
                    return;
                }
                DataBinding.Instance.GroupIds.Add(newId);
                DataBinding.Instance.AllGroupData[newId] = new DataBinding.GroupData();
                // reset text box value after enter.
                IdTextBox.Text = "";
            }
        }
        

        // private void Main_OnMouseDown(object sender, MouseButtonEventArgs e)
        // {
        //     if (e.ChangedButton != MouseButton.Right)
        //     {
        //         _chooseTriggerWindow.Hide();
        //         return;
        //     }
        //
        //     var mousePos = e.GetPosition(this);
        //    var screenPos = this.PointToScreen(mousePos);
        //     _chooseTriggerWindow.Show(screenPos);
        // }

        private void AuthorTextBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void TriggerOnClickHandler(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem == null)
                return;

            if (string.IsNullOrEmpty(DataBinding.Instance.CurrChoosedId))
            {
                MessageBox.Show("Pls choose a group on left");
                return;
            }

            if (!DataBinding.Instance.AllGroupData.TryGetValue(DataBinding.Instance.CurrChoosedId, out var group))
            {
                return;
            }

            group.AddTrigger(menuItem.Header.ToString());
        }

        private void GroupIdRename_EventHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var stackPanel = button.Parent as StackPanel;
                var textBlock = stackPanel.Children[0] as TextBlock;

                var oldId = textBlock.Text;
                 var mousePos =  Mouse.GetPosition(this);
                var screenPos = this.PointToScreen(mousePos);
                _renameWindow.Display(screenPos,v=>Rename(oldId,v));
                //todo: get newId,
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        void Rename(string oldId,string newId)
        {
            bool currChoosed = false;
            if (DataBinding.Instance.CurrChoosedId == oldId)
            {
                DataBinding.Instance.CurrChoosedId = string.Empty;
                CondsListView.ItemsSource = null;
                currChoosed = true;
            }

            var oldIndex = DataBinding.Instance.GroupIds.IndexOf(oldId);

            DataBinding.Instance.GroupIds.Remove(oldId);
            var groupData =  DataBinding.Instance.AllGroupData[oldId];
            DataBinding.Instance.AllGroupData.Remove(oldId);
            DataBinding.Instance.GroupIds.Insert(oldIndex,newId);
            DataBinding.Instance.AllGroupData[newId] = groupData;
            if (currChoosed)
            {
                DataBinding.Instance.CurrChoosedId = newId;
                CondsListView.ItemsSource = groupData.CondTriggers;
                ActionsListView.ItemsSource = groupData.ActionTriggers;
            }

        }

        private void GroupIdDel_EventHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var stackPanel = button.Parent as StackPanel;
                var textBlock = stackPanel.Children[0] as TextBlock;

                var delId = textBlock.Text;
                if (DataBinding.Instance.CurrChoosedId == delId)
                {
                    DataBinding.Instance.CurrChoosedId = string.Empty;
                    CondsListView.ItemsSource = null;
                    ActionsListView.ItemsSource = null;
                }

                DataBinding.Instance.GroupIds.Remove(delId);
                DataBinding.Instance.AllGroupData.Remove(delId);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void ShowGroup_EventHandler(object sender, MouseButtonEventArgs e)
        {
            var TextBlock = sender as TextBlock;
            var id = TextBlock.Text;
            DataBinding.Instance.CurrChoosedId = id;
            var groupData = DataBinding.Instance.AllGroupData[id];
            CondsListView.ItemsSource = groupData.CondTriggers;
            ActionsListView.ItemsSource = groupData.ActionTriggers;
        }

        private void LoadTriggerLine_OnClick(object sender, RoutedEventArgs e)
        {
            var openFile = new OpenFileDialog();
            openFile.Filter = "Json(*.json)|*.json";
            openFile.InitialDirectory = Environment.CurrentDirectory;
            openFile.Multiselect = false;
            var ret = openFile.ShowDialog();

            if (!ret.HasValue || !ret.Value)
                return;
            var file = openFile.FileName;
            {
                var str = "";
                TriggerLine line = null;
                (str, line) = TriggerHelper.LoadTriggerLine(file);
                if (str != null && line == null) MessageBox.Show(str);

                if (line != null) DataBinding.Instance.Load(line);
            }
        }

        private void Cond_DeleteTriggerBehavior_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = CondsListView.SelectedValue as DataBinding.Trigger;
            CondsListView.SelectedValue = null;
            DataBinding.Instance.AllGroupData[DataBinding.Instance.CurrChoosedId].CondTriggers.Remove(selectedItem);
        }
        
        private void Action_DeleteTriggerBehavior_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = ActionsListView.SelectedValue as DataBinding.Trigger;
            ActionsListView.SelectedValue = null;
            DataBinding.Instance.AllGroupData[DataBinding.Instance.CurrChoosedId].ActionTriggers.Remove(selectedItem);
        }

        private void ListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            foreach (var v in e.RemovedItems)
            {
                var trigger = v as DataBinding.Trigger;
                trigger.DelButton = Visibility.Hidden;
            }

            foreach (var v in e.AddedItems)
            {
                var trigger = v as DataBinding.Trigger;
                trigger.DelButton = Visibility.Visible;
            }
        }
    }
}