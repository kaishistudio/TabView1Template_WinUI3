using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel;
using App1.Helpers;
using App1.Services;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using WinUIEx;
using App1.Views;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using App1.Views.Windows;

namespace App1.ViewModels
{
    internal class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 是否亚克力
        /// </summary>
        private bool _IsAcrylic;
        public bool IsAcrylic
        {
            get { return _IsAcrylic; }
            set
            {
                _IsAcrylic = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAcrylic)));
            }
        }
        /// <summary>
        /// 默认主题选择
        /// </summary>
        private bool _IsDefaultSelected = true;

        public bool IsDefaultSelected
        {
            get { return _IsDefaultSelected; }
            set
            {
                _IsDefaultSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDefaultSelected)));
            }
        }
        /// <summary>
        /// 浅色主题选择
        /// </summary>
        private bool _IsLightSelected = false;

        public bool IsLightSelected
        {
            get { return _IsLightSelected; }
            set
            {
                _IsLightSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLightSelected)));
            }
        }
        /// <summary>
        /// 深色主题选择
        /// </summary>
        private bool _IsDarkSelected = false;

        public bool IsDarkSelected
        {
            get { return _IsDarkSelected; }
            set
            {
                _IsDarkSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDarkSelected)));
            }
        }
        /// <summary>
        /// 获取版本信息
        /// </summary>
        private string _versionDescription;
        public string VersionDescription
        {
            get { return GetVersionDescription(); }
            set
            {
                _versionDescription = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VersionDescription)));
            }
        }
        /// <summary>
        /// 主题切换命令
        /// </summary>
        public ICommand SwitchThemeCommand;
        public SettingsViewModel()
        {
            //初始化按钮选择
            IsDefaultSelected = GetSavedRequestedTheme() == ElementTheme.Default ? true : false;
            IsDarkSelected = GetSavedRequestedTheme() == ElementTheme.Dark ? true : false;
            IsLightSelected = GetSavedRequestedTheme() == ElementTheme.Light ? true : false;
            //初始化亚克力按钮选择
            IsAcrylic = ApplicationData.Current.LocalSettings.Values.ContainsKey("IsAcrylic") ? ApplicationData.Current.LocalSettings.Values["IsAcrylic"].ToString() == "True" ? true :false :  false;
            SwitchThemeCommand = new RelayCommand<ElementTheme>(async (param) =>
            {
                foreach (var g in Shares.Data.RequestedThemeList)
                {
                    await SetRequestedThemeAsync(g, param);
                }
                SaveRequestedTheme(param);
            });
        }
        /// <summary>
        /// 亚克力按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CheckBox_IsAcrylic_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
            {
                Shares.Data.MainWindow.Backdrop = new AcrylicSystemBackdrop();
                ApplicationData.Current.LocalSettings.Values["IsAcrylic"] = "True";
            }
            else
            {
                Shares.Data.MainWindow.Backdrop = new MicaSystemBackdrop();
                ApplicationData.Current.LocalSettings.Values["IsAcrylic"] = "False";
            }
        }
        /// <summary>
        /// 主题初始化
        /// </summary>
        public async void ThemeInit()
        {
            if (Shares.Data.RequestedThemeList != null)
            {
                foreach (var obj in Shares.Data.RequestedThemeList)
                {
                    await SetRequestedThemeAsync(obj, GetSavedRequestedTheme());
                }
            }
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("IsAcrylic"))
            {
                var isac = ApplicationData.Current.LocalSettings.Values["IsAcrylic"].ToString();
                if (isac == "True")
                {
                    Shares.Data.MainWindow.Backdrop = new AcrylicSystemBackdrop();
                }
                else
                {
                    Shares.Data.MainWindow.Backdrop = new MicaSystemBackdrop();
                }
            }
            else
            {
                Shares.Data.MainWindow.Backdrop = new MicaSystemBackdrop();
            }
        }
        public async void ThemeInit(MainWindow window,Grid g)
        {
            await SetRequestedThemeAsync(g, GetSavedRequestedTheme());
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("IsAcrylic"))
            {
                var isac = ApplicationData.Current.LocalSettings.Values["IsAcrylic"].ToString();
                if (isac == "True")
                {
                    window.Backdrop = new AcrylicSystemBackdrop();
                }
                else
                {
                    window.Backdrop = new MicaSystemBackdrop();
                }
            }
            else
            {
                window.Backdrop = new MicaSystemBackdrop();
            }
        }
        /// <summary>
        /// 获取版本信息
        /// </summary>
        /// <returns></returns>
        private static string GetVersionDescription()
        {
            Version version;

            if (RuntimeHelper.IsMSIX)
            {
                var packageVersion = Package.Current.Id.Version;

                version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
            }
            else
            {
                version = Assembly.GetExecutingAssembly().GetName().Version!;
            }

            return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
        /// <summary>
        /// 获得应用运行时改变的主题
        /// </summary>
        /// <returns></returns>
        public ElementTheme GetSavedRequestedTheme()
        {
            var Theme = ApplicationData.Current.LocalSettings.Values.ContainsKey("ElementTheme") ? ApplicationData.Current.LocalSettings.Values["ElementTheme"].ToString() : "Default";
            return (ElementTheme)Enum.Parse(typeof(ElementTheme), Theme == "" ? "Default" : Theme);
        }
        /// <summary>
        /// 应用运行时改变主题
        /// </summary>
        /// <returns></returns>
        public async Task SetRequestedThemeAsync(object root, ElementTheme Theme)
        {
            if (root is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = Theme;
            }

            await Task.CompletedTask;
        }
        /// <summary>
        /// 保存应用运行时改变主题
        /// </summary>
        /// <returns></returns>
        public void SaveRequestedTheme(ElementTheme Theme)
        {
            ApplicationData.Current.LocalSettings.Values["ElementTheme"] = Theme.ToString();
        }
        
    }
}
