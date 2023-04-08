using App1.Services;
using App1.ViewModels;
using App1.Views.Pages;
using App1.Views.Pages;
using App1.Views.Pages.ListPage;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App1
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static void AddUserPage(string name, string iconcode,Symbol symbol, Type type)
        {
            var page = new Models.PageInfo()
            {
                Name = name,
                IconCode = iconcode,
                PageType = type,
                IconSymbol = symbol,
            };
            if (Shares.Data.List_Pages.FirstOrDefault(o=>o.Name==name)==null)
                Shares.Data.List_Pages.Add(page);
        }
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            UserDataInit();
            Shares.Data.MainWindow = new Views.Windows.MainWindow(null);
            Shares.Data.MainWindow.Activate();
        }


        //介绍：这里是用户数据自定义模板，可以方便的部署你的应用配置。
        //用法：0.删除Page中的示例Page：
        //      1.添加你的新页面到Views\Pages文件夹
        //      2.添加你的ViewModel到Views\ViewModels文件夹（可选）
        //      3.到下面AddUserPage添加你的页面信息
        //一切工作完成！然后配置好你的商店名称与图片，就可以发布了！
        //模板内文件说明：
        //
        void UserDataInit()
        {
            //sqlite数据库设置
            Shares.Data.UserDBTableName = "Table1";//数据库表名称
            Shares.Data.UserDBColumns =new List<string>() { "Name","Url"};//表列表

            //用户页面设置
            //页面名称
            //icon文字图标代码（代码可从以下网址获取）
            //https://learn.microsoft.com/zh-cn/uwp/api/windows.ui.xaml.controls.symbol?view=winrt-22621
            //页面的type
            AddUserPage("Welcome", "\xE10F",Symbol.Home, typeof(WelcomePage));
            AddUserPage("List", "\xE14C",Symbol.List, typeof(ListDetailPage));
        }
    }
}
