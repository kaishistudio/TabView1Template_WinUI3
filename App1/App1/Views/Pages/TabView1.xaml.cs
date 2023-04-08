using App1.ViewModels;
using App1.Views.Windows;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App1.Templates.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TabView1 : Page
    {
        TabView1ViewModel tabView1View;
        MainWindow mainWindow;
        public TabView1()
        {
            tabView1View = new TabView1ViewModel(mainWindow);
            this.InitializeComponent();
            Shares.Data.RequestedThemeList.Add(root);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = e.Parameter as object[];
            mainWindow = param[0] as MainWindow;
            tabView1View = new TabView1ViewModel(mainWindow);
            if (param.Length > 1)
            {
                tabView1View.AddTabToTabs(param[1] as TabViewItem);
            }
            base.OnNavigatedTo(e);
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            tabView1View.SqliteInit();
            new SettingsViewModel().ThemeInit(mainWindow, mainWindow.root);
        }
    }
}
