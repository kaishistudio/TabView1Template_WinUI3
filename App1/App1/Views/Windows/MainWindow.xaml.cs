using App1.Services;
using App1.Templates.Views;
using App1.Views;
using App1.Views.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App1.Views.Windows
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WindowEx
    {
        public MainWindow(object param)
        {
            this.InitializeComponent();
            Title = AppWindow.Title;
            AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
            ExtendsContentIntoTitleBar = true;
            Shares.Data.RequestedThemeList.Add(frame);
            if (param != null)
            {
                if(param is TabViewItem) {
                    frame.Navigate(typeof(TabView1), new object[] { this,param as TabViewItem });
                }
            }
            else
            {
                //frame.Navigate(typeof(Hamburger2), null);//ºº±¤²Ëµ¥2
                frame.Navigate(typeof(TabView1), new object[] { this});
            }
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            Shares.Data.AppServices = new AppServices(Shares.Data.MainWindow, Shares.Data.MainWindow.root);
            Shares.Data.RequestedThemeList.Add(Shares.Data.MainWindow.root);
        }
    }
}
