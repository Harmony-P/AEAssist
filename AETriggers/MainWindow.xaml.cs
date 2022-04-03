﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AETriggers.TriggerModel;
using Microsoft.Win32;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Trigger = AETriggers.TriggerModel.Trigger;

namespace AETriggers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            
            Entry.Init();
        }

        private TriggerLine TriggerLine;

        private struct ExcelData
        {
            public string groupId;
            public string type;
            public string valueType;
            public string[] valueParams;
        }

        private Dictionary<string, List<ExcelData>> AllExcelData = new Dictionary<string, List<ExcelData>>();

        private void Load_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Excel(*.xlsx)|*.xlsx|Excel(*.xls)|*.xls";
            openFile.InitialDirectory = Environment.CurrentDirectory;
            openFile.Multiselect = false;
            var ret = openFile.ShowDialog();
            
            if (!ret.HasValue || !ret.Value)
                return;
            var file = openFile.FileName;
            IWorkbook workbook;
            string fileExt = Path.GetExtension(file).ToLower();
            try
            {
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read,System.IO.FileShare.ReadWrite))
                {
                    if (fileExt == ".xlsx")
                    {
                        workbook = new XSSFWorkbook(fs);
                    }
                    else if (fileExt == ".xls")
                    {
                        workbook = new HSSFWorkbook(fs);
                    }
                    else
                    {
                        workbook = null;
                    }

                    if (workbook == null)
                    {
                        MessageBox.Show("加载表格失败!");
                        return;
                    }

                    ISheet sheet = workbook.GetSheetAt(0);

                    // for (int i = 0; i < sheet.LastRowNum; i++)
                    // {
                    //     var row = sheet.GetRow(i);
                    //     foreach (var v in row.Cells)
                    //     {
                    //         if (v != null && !string.IsNullOrEmpty(v.ToString()))
                    //             MessageBox.Show($"{v.RowIndex} {v.ColumnIndex} : {v.ToString()}");
                    //     }
                    // }
                    
                    
                    
                    var authorName = sheet.GetRow(0).GetCell(1).ToString();
                    var version = sheet.GetRow(1).GetCell(1).ToString();
                    var targetDuty = sheet.GetRow(2).GetCell(1).ToString();
                    var job = sheet.GetRow(3).GetCell(1).ToString();

                    this.TriggerLine = new TriggerLine
                    {
                        Version = version,
                        Author = authorName,
                        TargetDuty = targetDuty,
                        TargetJob = job,
                    };

                    AllExcelData.Clear();
                    for (int i = 6; i < sheet.LastRowNum; i++)
                    {
                        var row = sheet.GetRow(i);
                        var cell = row.GetCell(1);
                        var notData = cell != null && cell.ToString().StartsWith("#");
                        if(notData)
                            continue;
                        var groupId = row.GetCell(2)?.ToString();
                        var type = row.GetCell(3)?.ToString();
                        var valueType = row.GetCell(4)?.ToString();
                        
                        if(groupId == null || type == null || valueType == null)
                            continue;
                        
                        var valueParams = new string[3];
                        valueParams[0] = row.GetCell(5)?.ToString();
                        valueParams[1] = row.GetCell(6)?.ToString();
                        valueParams[1] = row.GetCell(7)?.ToString();

                        if (!AllExcelData.TryGetValue(groupId, out var list))
                        {
                            list = new List<ExcelData>();
                            AllExcelData[groupId] = list;
                        }

                        list.Add(new ExcelData
                        {
                            groupId = groupId,
                            type = type,
                            valueType = valueType,
                            valueParams = valueParams
                        });
                    }

                    var loadRet = LoadExcelData();

                    if (loadRet) MessageBox.Show("成功加载数据 ");
                    else
                    {
                        this.TriggerLine = null;
                        this.AllExcelData.Clear();
                    }

                    RefreshUI();
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }

        }
        
        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            if (TriggerLine == null)
                return;
            SaveFileDialog openFile = new SaveFileDialog();
            openFile.Filter = "Json(*.json)|*.json";
            openFile.InitialDirectory = Environment.CurrentDirectory;
            openFile.FileName =
                $"[{TriggerLine.Author}][{TriggerLine.Version}]{TriggerLine.TargetDuty}_{TriggerLine.TargetJob}.json";
            var ret = openFile.ShowDialog();
            if (!ret.HasValue || !ret.Value)
                return;
            TriggerHelper.SaveTriggerLine(triggerLine: TriggerLine, openFile.FileName);
            MessageBox.Show("导出成功!");

        }

        private bool LoadExcelData()
        {
            if (this.TriggerLine == null || this.AllExcelData.Count == 0)
                return false;
            foreach (var v in AllExcelData)
            {
                var trigger = new Trigger();
                TriggerLine.Triggers.Add(trigger);
                
                foreach (var data in v.Value)
                {
                    var strs = data.valueType.Split(':');
                    var category = strs[0];
                    var typeName = strs[1];

                    var type = TriggerMgr.Instance.Name2Type[typeName];
                    var instance = Activator.CreateInstance(type) as ITriggerBase;

                    try
                    {
                        instance.WriteFromJson(data.valueParams);

                        if (instance is ITriggerCond)
                        {
                            trigger.TriggerConds.Add(instance as ITriggerCond);
                        }
                        else
                        {
                            trigger.TriggerActions.Add(instance as ITriggerAction);
                        }
                    }
                    catch (Exception e)
                    {
                        var pre = $"Type: {data.valueType} Params : [{ListToString(data.valueParams)}]\n ";
                        MessageBox.Show( pre+ e.ToString());
                    }

                }
            }

            return true;
        }

        string ListToString(string[] pa)
        {
            var str = string.Empty;
            if (pa != null && pa.Length > 0)
                foreach (var v in pa)
                {
                    str += v + ",";
                }

            return $"[{str}]";
        }

        private void RefreshUI()
        {
            if (TriggerLine == null)
            {
                this.AuthorName.Content = string.Empty;
                this.Version.Content = string.Empty;
                this.TargetDuty.Content = string.Empty;
                this.TargetJob.Content = string.Empty;
                return;
            }

            this.AuthorName.Content = TriggerLine.Author;
            this.Version.Content = TriggerLine.Version;
            this.TargetDuty.Content = TriggerLine.TargetDuty;
            this.TargetJob.Content = TriggerLine.TargetJob;
        }

        private void LoadJson_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Json(*.json)|*.json";
            openFile.InitialDirectory = Environment.CurrentDirectory;
            openFile.Multiselect = false;
            var ret = openFile.ShowDialog();
            
            if (!ret.HasValue || !ret.Value)
                return;
            var file = openFile.FileName;
            this.TriggerLine = TriggerHelper.LoadTriggerLine(file);
            MessageBox.Show("加载成功!");
        }
    }
}