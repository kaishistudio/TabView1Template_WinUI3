using App1.Models;
using App1.Services;
using App1.Templates.Views;
using App1.ViewModels;
using App1.Views.Pages;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Shares
{
    internal class Data
    {
        public static Views.Windows.MainWindow MainWindow;
        public static AppServices AppServices;
        public static List<object> RequestedThemeList = new() { };//需要设置mica或ayclic效果的控件加入这里
        public static Grid TitleBar;
        public static TextBlock Tb_Header;
        public static Grid G_Frames;
        public static SplitView SplitView;
        public static Grid G_Pane;
        public static ListDetailsView ListDetailsView;
        public static ListView ListView_PageList;
        public static List<PageInfo> List_Pages=new ();
        public static string UserDBTableName;
        public static List<string> UserDBColumns;
    }
}
