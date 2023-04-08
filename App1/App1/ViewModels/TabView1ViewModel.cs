using App1.Views.Pages;
using App1.Views.Pages.ListPage;
using App1.Views.Windows;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using App1.Views.Pages;
using App1.Services;
using CommunityToolkit.WinUI.Helpers;

namespace App1.ViewModels
{
    internal class TabView1ViewModel
    {
        SettingsViewModel settingsViewModel;
        MainWindow mainWindow;
        public string AppName
        {
            get { return SystemInformation.Instance.ApplicationName; }
            set { }
        }
        /// <summary>
        /// viewmodel初始化
        /// </summary>
        /// <param name="param"></param>
        public TabView1ViewModel(MainWindow window)
        {
            mainWindow = window;
            settingsViewModel = new SettingsViewModel();
            App.AddUserPage("Settings", "\xE115", Symbol.Setting, typeof(SettingsPage));
        }
        /// <summary>
        /// 添加一个tab
        /// </summary>
        /// <param name="header"></param>
        public void CreateNewTVI(string header, Symbol symbol, object content)
        {
            var newTab = new TabViewItem()
            {
                IconSource = new SymbolIconSource()
                {
                    Symbol = symbol
                },
                Header = header,
                Content = content
            };
            Tabs.TabItems.Add(newTab);
            Tabs.SelectedItem = newTab;
        }

        TabView Tabs;
        /// <summary>
        /// tabview初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TabViewWindowingSamplePage_Loaded(object sender, RoutedEventArgs e)
        {
            Tabs = sender as TabView;
            if (WaitTabItem != null)
            {
                AddTabToTabs(WaitTabItem);
            }
            else
            {
                CreateNewTVI("Welcome", Symbol.Home, new WelcomePage());
            }
            settingsViewModel.ThemeInit();
            UpdateTitleBar();
        }
        /// <summary>
        /// 添加按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void Tabs_AddTabButtonClick(TabView sender, object args)
        {
            MenuFlyout mf = new MenuFlyout();
            foreach (var m in Shares.Data.List_Pages)
            {
                MenuFlyoutItem mfi = new MenuFlyoutItem() { Text = m.Name };
                mfi.Click += (ss, ee) =>
                {
                    Frame frame = new Frame();
                    frame.Navigate(m.PageType);
                    CreateNewTVI(m.Name, m.IconSymbol, frame);
                };
                mf.Items.Add(mfi);
            }
            mf.ShowAt(CustomDragRegion);
        }
        public async void SqliteInit()
        {
            await SqliteService.CreatOrReadDBByName("Data", Shares.Data.UserDBTableName, Shares.Data.UserDBColumns);
            var t = "";
            foreach (var d in Shares.Data.UserDBColumns)
            {
                t += d + ",";
            }
            if (t.Length > 0) t = t.Substring(0, t.Length - 1);
            for (int i = 0; i < 10; i++)
            {
                SqliteService.EditDBByCommand($@"INSERT INTO {Shares.Data.UserDBTableName} ({t}) VALUES ('{i}','{i * i}');");
            }

        }
        Grid CustomDragRegion;
        /// <summary>
        /// titlebar初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CustomDragRegion_Loaded(object sender, RoutedEventArgs e)
        {
            CustomDragRegion = sender as Grid;
            UpdateTitleBar();
        }
        /// <summary>
        /// 更新titlebar属性
        /// </summary>
        public void UpdateTitleBar()
        {
            if (CustomDragRegion != null && mainWindow != null)
            {
                mainWindow.SetTitleBar(CustomDragRegion);
                CustomDragRegion.MinWidth = 188;
            }
        }
        Grid ShellTitleBarInset;
        /// <summary>
        /// ShellTitleBarInset初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ShellTitleBarInset_Loaded(object sender, RoutedEventArgs e)
        {
            ShellTitleBarInset = sender as Grid;
        }
        /// <summary>
        /// tabitem选择变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void Tabs_TabItemsChanged(TabView sender, Windows.Foundation.Collections.IVectorChangedEventArgs args)
        {
            // If there is only one tab left, disable dragging and reordering of Tabs.
            if (sender.TabItems.Count == 1)
            {
                sender.CanReorderTabs = false;
                sender.CanDragTabs = false;
            }
            else
            {
                sender.CanReorderTabs = true;
                sender.CanDragTabs = true;
            }
            UpdateTitleBar();
        }

        TabViewItem WaitTabItem = null;
        /// <summary>
        /// 增加tabitem到tabview
        /// </summary>
        /// <param name="tab"></param>
        public void AddTabToTabs(TabViewItem tab)
        {
            if (Tabs != null)
                Tabs.TabItems.Add(tab);
            else
                WaitTabItem = tab;
        }
        /// <summary>
        /// tabitem拖出窗口事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        // Create a new Window once the Tab is dragged outside.
        public void Tabs_TabDroppedOutside(TabView sender, TabViewTabDroppedOutsideEventArgs args)
        {
            Tabs.TabItems.Remove(args.Tab);
            var newWindow = new MainWindow(args.Tab);
            newWindow.Activate();
        }
        /// <summary>
        /// tabitem关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>

        public void Tabs_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);
        }
    }
}
