using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Composition;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel;
using Windows.Graphics;
using Windows.System;
using WinRT.Interop;
using Microsoft.Extensions.Configuration;
using WinRT;
using System.Drawing;
using WinUIEx;
using System.IO;
using Microsoft.UI.Xaml.Controls;
using System.Text.RegularExpressions;
using Windows.Storage;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Windows.ApplicationModel.DataTransfer;
using System.Diagnostics;
using Microsoft.UI.Xaml.Media.Animation;

namespace App1.Services;

//需要引入Pinvoke.User32、CommunityToolkit.WinUI、WinUIEx包

public class AppServices
{
    public Grid root;
    public Window window;
    public AppServices(Window window,Grid root)
    {
        this.root = root;
        this.window = window;
    }
    /// <summary>
    /// App名称
    /// </summary>
    public string ApplicationName = SystemInformation.Instance.ApplicationName;
    /// <summary>
    /// App版本
    /// </summary>
    public PackageVersion ApplicationVersion = SystemInformation.Instance.ApplicationVersion;
    /// <summary>
    /// App是否第一次运行
    /// </summary>
    public bool IsFirstRun = SystemInformation.Instance.IsFirstRun;
    /// <summary>
    /// 得到屏幕工作区高度
    /// </summary>
    public double ScreenWorkAreaHeight = DisplayArea.Primary.WorkArea.Height;
    /// <summary>
    /// 得到屏幕工作区宽度
    /// </summary>
    public double ScreenWorkAreaWidth = DisplayArea.Primary.WorkArea.Width;
    /// <summary>
    /// 得到屏幕高度
    /// </summary>
    public double ScreenHeight = DisplayArea.Primary.OuterBounds.Height;
    /// <summary>
    /// 得到屏幕宽度
    /// </summary>
    public double ScreenWidth = DisplayArea.Primary.OuterBounds.Width;
    /// <summary>
    /// 设置TitleBar透明
    /// </summary>
    /// <param name="window"></param>
    public void SetTitleBarTransparent()
    {
        var res = Application.Current.Resources;
        res["WindowCaptionBackground"] = Colors.Transparent;
        res["WindowCaptionForeground"] = Colors.Black;

        var activeWindow = Win32ApiService.GetActiveWindow();
        if (GetHwnd() == activeWindow)
        {
            Win32ApiService.SendMessage(GetHwnd(), Win32ApiService.WM_ACTIVATE, Win32ApiService.WA_INACTIVE, IntPtr.Zero);
            Win32ApiService.SendMessage(GetHwnd(), Win32ApiService.WM_ACTIVATE, Win32ApiService.WA_ACTIVE, IntPtr.Zero);
        }
        else
        {
            Win32ApiService.SendMessage(GetHwnd(), Win32ApiService.WM_ACTIVATE, Win32ApiService.WA_ACTIVE, IntPtr.Zero);
            Win32ApiService.SendMessage(GetHwnd(), Win32ApiService.WM_ACTIVATE, Win32ApiService.WA_INACTIVE, IntPtr.Zero);
        }

    }
    /// <summary>
    /// 获得应用运行时改变的主题
    /// </summary>
    /// <returns></returns>
    public ElementTheme GetSavedRequestedTheme()
    {
        var Theme = ApplicationData.Current.LocalSettings.Values.ContainsKey("ElementTheme")? ApplicationData.Current.LocalSettings.Values["ElementTheme"].ToString():"Default";
        return (ElementTheme)Enum.Parse(typeof(ElementTheme), Theme == "" ? "Default" : Theme);
    }
    /// <summary>
    /// 应用运行时改变主题
    /// </summary>
    /// <returns></returns>
    public async Task SetRequestedThemeAsync( ElementTheme Theme)
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
    /// <summary>
    /// 设置应用主题,需要在窗口加载前调用，在app.xaml.cs中使用。
    /// </summary>
    /// <param name="app"></param>
    /// <param name="theme"></param>
    public void SetAppTheme(App app, ApplicationTheme theme)
    {
        app.RequestedTheme = theme;
    }
    /// <summary>
    /// 设置剪切板文字
    /// </summary>
    /// <param name="t"></param>
    public void SetClipBoard(string t)
    {
        DataPackage dp = new DataPackage();
        dp.SetText(t);
        Clipboard.SetContent(dp);
    }
    /// <summary>
    /// 打开商店作者链接 如KS.STUDIO
    /// </summary>
    /// <param name="publishername"></param>
    public async void StartStorepublisherUri(string publishername)
    {
        await Launcher.LaunchUriAsync(new Uri(@"ms-windows-store://publisher/?name=" + publishername));
    }
    /// <summary>
    /// 打开商店产品链接 如9NKD6C5B3434
    /// </summary>
    /// <param name="productid"></param>
    public async void StartStoreProductUri(string productid)
    {
        await Launcher.LaunchUriAsync(new Uri(@"ms-windows-store://pdp/?productid=" + productid));
    }
    /// <summary>
    /// 显示一个通知 
    /// </summary>
    /// <param name="text"></param>
    public void ShowPopup( string text, int seconds)
    {
        Popup popup = new Popup()
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(0, 40, 0, 0),
            RenderTransform = new CompositeTransform() { TranslateX = 0 - root.ActualWidth },

        };
        var g = new Grid()
        {
            Background = new SolidColorBrush((Windows.UI.Color)Application.Current.Resources["SystemAccentColor"]),
            CornerRadius = new CornerRadius(4),
        };
        g.Children.Add(new TextBlock()
        {
            Text = text,
            Margin = new Thickness(10),
            TextWrapping = TextWrapping.Wrap,
            MaxWidth = root.ActualWidth
        });
        popup.Child = g;
        root.Children.Add(popup);
        popup.Opened += (s, e) =>
        {
            var storyBoard = new Storyboard();
            var extendAnimation = new DoubleAnimation { Duration = new Duration(TimeSpan.FromSeconds(0.08)), From = 0 - root.ActualWidth, To = 10, EnableDependentAnimation = true };
            Storyboard.SetTarget(extendAnimation, popup);
            Storyboard.SetTargetProperty(extendAnimation, "(UIElement.RenderTransform).(CompositeTransform.TranslateX)");
            storyBoard.Children.Add(extendAnimation);
            storyBoard.Begin();
            storyBoard.Completed += async (ss, ee) =>
            {
                await Task.Delay(1000 * seconds);
                var storyBoard = new Storyboard();
                var extendAnimation = new DoubleAnimation { Duration = new Duration(TimeSpan.FromSeconds(0.08)), From = 10, To = root.ActualWidth, EnableDependentAnimation = true };
                Storyboard.SetTarget(extendAnimation, popup);
                Storyboard.SetTargetProperty(extendAnimation, "(UIElement.RenderTransform).(CompositeTransform.TranslateX)");
                storyBoard.Children.Add(extendAnimation);
                storyBoard.Begin();
                storyBoard.Completed += (ss, ee) =>
                {
                    popup.IsOpen = false;
                };
            };
        };
        popup.IsOpen = true;
    }
    /// <summary>
    /// 设置窗口为上次打开的位置和大小，如果失败设置默认值。
    /// </summary>
    /// <param name="window"></param>
    /// <param name="DefautWidth"></param>
    /// <param name="DefaultHeight"></param>
    public void SetWindowRectFromSave( int DefautWidth, int DefaultHeight,string key)
    {
        try
        {
            var save = ApplicationData.Current.LocalSettings.Values[key].ToString();
            var savesz = Array.ConvertAll<string, double>(save.Split('#'), s => double.Parse(s) < 0 ? 0 : double.Parse(s));
            SetWindowRect( new Windows.Graphics.RectInt32((int)savesz[0], (int)savesz[1], (int)savesz[2] + 15, (int)savesz[3] + 10));
        }
        catch
        {
            SetWindowSize( DefautWidth, DefaultHeight);
        }
    }
    public void SaveWindowRect( double left, double top, double width, double height,string key)
    {
        ApplicationData.Current.LocalSettings.Values[key] = $"{left}#{top}#{width}#{height}";
    }
    /// <summary>
    /// 窗口最大化(App._Window_Hamburg)
    /// </summary>
    public void WindowMax()
    {
        PInvoke.User32.ShowWindow(GetHwnd(), PInvoke.User32.WindowShowStyle.SW_MAXIMIZE);
    }
    /// <summary>
    /// 窗口最小化(App._Window_Hamburg)
    /// </summary>
    public void WindowMin()
    {
        PInvoke.User32.ShowWindow(GetHwnd(), PInvoke.User32.WindowShowStyle.SW_MINIMIZE);
    }
    /// <summary>
    /// 窗口恢复
    /// </summary>
    public void WindowNormal()
    {
        PInvoke.User32.ShowWindow(GetHwnd(), PInvoke.User32.WindowShowStyle.SW_RESTORE);
    }
    /// <summary>
    /// 以窗口最近一次的大小和状态显示窗口
    /// </summary>
    public void WindowLastRect()
    {
        PInvoke.User32.ShowWindow(GetHwnd(), PInvoke.User32.WindowShowStyle.SW_SHOWNOACTIVATE);
    }
    /// <summary>
    /// 窗口隐藏
    /// </summary>
    public void WindowHide()
    {
        PInvoke.User32.ShowWindow(GetHwnd(), PInvoke.User32.WindowShowStyle.SW_HIDE);
    }
    /// <summary>
    /// 窗口显示
    /// </summary>
    public void WindowShow()
    {
        PInvoke.User32.ShowWindow(GetHwnd(), PInvoke.User32.WindowShowStyle.SW_SHOW);
    }
    /// <summary>
    /// 窗口置顶
    /// </summary>
    public void WindowIsAlwaysOnTop( bool IsAlwaysOnTop)
    {
        WindowManager.Get(window).IsAlwaysOnTop = IsAlwaysOnTop;
    }
    /// <summary>
    /// 窗口是否可改变大小
    /// </summary>
    public void WindowIsResizable( bool IsResizable)
    {
        WindowManager.Get(window).IsResizable = IsResizable;
    }
    /// <summary>
    /// 窗口全屏
    /// </summary>
    public void WindowFullScreen( bool IsFullScreen)
    {
        if (IsFullScreen)
            GetAppWindowForCurrentWindow().SetPresenter(AppWindowPresenterKind.FullScreen);
        else
            GetAppWindowForCurrentWindow().SetPresenter(AppWindowPresenterKind.Default);
    }
    /// <summary>
    /// 是否显示最大化按钮
    /// </summary>
    public void WindowIsMaximizable( bool a)
    {
        WindowManager.Get(window).IsMaximizable = a;
    }
    /// <summary>
    /// 是否显示最小化按钮
    /// </summary>
    public void WindowIsMinimizable( bool a)
    {
        WindowManager.Get(window).IsMinimizable = a;
    }
    /// <summary>
    /// 设置窗口大小（左上宽高）
    /// </summary>
    /// <param name="rect"></param>
    public void SetWindowRect( RectInt32 rect)
    {
        GetAppWindowForCurrentWindow().MoveAndResize(rect);
    }
    /// <summary>
    /// 设置窗口尺寸，位置在屏幕当中
    /// </summary>
    /// <param name="window"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void SetWindowSize( double width, double height)
    {
        window.SetWindowSize(width, height);
    }
    /// <summary>
    /// 得到窗口宽度
    /// </summary>
    public double WindowWidth()
    {
        return WindowManager.Get(window).Width;
    }
    /// <summary>
    /// 得到窗口高度
    /// </summary>
    public double WindowHeight()
    {
        return WindowManager.Get(window).Height;
    }
    /// <summary>
    /// 得到窗口左部
    /// </summary>
    public double WindowLeft()
    {
        PInvoke.RECT rect;
        PInvoke.User32.GetWindowRect(GetHwnd(), out rect);
        return rect.left;
    }
    /// <summary>
    /// 得到窗口顶部
    /// </summary>
    public double WindowTop()
    {
        PInvoke.RECT rect;
        PInvoke.User32.GetWindowRect(GetHwnd(), out rect);
        return rect.top;
    }
    /// <summary>
    /// 得到窗口右部
    /// </summary>
    public double WindowRight()
    {
        PInvoke.RECT rect;
        PInvoke.User32.GetWindowRect(GetHwnd(), out rect);
        return rect.right;
    }
    /// <summary>
    /// 得到窗口底部
    /// </summary>
    public double WindowBottom()
    {
        PInvoke.RECT rect;
        PInvoke.User32.GetWindowRect(GetHwnd(), out rect);
        return rect.bottom;
    }
    /// <summary>
    /// 得到鼠标坐标
    /// </summary>
    public Point GetCursor()
    {
        PInvoke.POINT p;
        PInvoke.User32.GetCursorPos(out p);
        return p;
    }
    /// <summary>
    /// 设置窗口宽度
    /// </summary>
    public double SetWindowWidth( double w)
    {
        return WindowManager.Get(window).Width = w;
    }
    /// <summary>
    /// 设置窗口高度
    /// </summary>
    public double SetWindowHeight( double h)
    {
        return WindowManager.Get(window).Height = h;
    }
    /// <summary>
    /// 得到窗口句柄
    /// </summary>
    /// <returns></returns>
    public nint GetHwnd()
    {
        return WinRT.Interop.WindowNative.GetWindowHandle(window);
    }
    /// <summary>
    ///  LoadIcon("Images/windowIcon.ico");ico文件属性设置为内容。
    /// </summary>
    /// <param name="iconName"></param>
    public void LoadIcon( string iconName)
    {
        GetAppWindowForCurrentWindow().SetIcon(Path.Combine(AppContext.BaseDirectory, iconName));
    }
    /// <summary>
    /// AppWindow
    /// </summary>
    /// <returns></returns>
    public AppWindow GetAppWindowForCurrentWindow()
    {
        WindowId myWndId = Win32Interop.GetWindowIdFromWindow(GetHwnd());
        return AppWindow.GetFromWindowId(myWndId);
    }
    /// <summary>
    /// 设置App标题
    /// </summary>
    public void SetAppTitle( string title)
    {
        var m_appWindow = GetAppWindowForCurrentWindow();
        if (m_appWindow != null)
        {
            m_appWindow.Title = title;
        }
    }
    /// <summary>
    /// 窗口内容是否拓展到标题栏
    /// </summary>
    /// <param name="window"></param>
    /// <param name="Isextend"></param>
    public void SetExtendstoTitleBar( bool Isextend)
    {
        window.ExtendsContentIntoTitleBar = Isextend;
    }
    WindowsSystemDispatcherQueueHelper m_wsdqHelper; // See below for implementation.
    MicaController m_backdropController;
    SystemBackdropConfiguration m_configurationSource;
    /// <summary>
    /// 设置窗口亚克力效果
    /// </summary>
    public bool TrySetSystemBackdropMica()
    {
        if (Microsoft.UI.Composition.SystemBackdrops.MicaController.IsSupported())
        {
            m_wsdqHelper = new WindowsSystemDispatcherQueueHelper();
            m_wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

            // Create the policy object.
            m_configurationSource = new SystemBackdropConfiguration();

            // Initial configuration state.
            m_configurationSource.IsInputActive = true;
            SetConfigurationSourceTheme();

            m_backdropController = new Microsoft.UI.Composition.SystemBackdrops.MicaController();

            // Enable the system backdrop.
            // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
            m_backdropController.AddSystemBackdropTarget(window.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
            m_backdropController.SetSystemBackdropConfiguration(m_configurationSource);
            return true; // succeeded
        }

        return false; // Mica is not supported on this system
    }
    void SetConfigurationSourceTheme()
    {
        switch (((FrameworkElement)window.Content).ActualTheme)
        {
            case ElementTheme.Dark: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Dark; break;
            case ElementTheme.Light: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Light; break;
            case ElementTheme.Default: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Default; break;
        }
    }
    class WindowsSystemDispatcherQueueHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        struct DispatcherQueueOptions
        {
            internal int dwSize;
            internal int threadType;
            internal int apartmentType;
        }

        [DllImport("CoreMessaging.dll")]
        private static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object dispatcherQueueController);

        object m_dispatcherQueueController = null;
        public void EnsureWindowsSystemDispatcherQueueController()
        {
            if (Windows.System.DispatcherQueue.GetForCurrentThread() != null)
            {
                // one already exists, so we'll just use it.
                return;
            }

            if (m_dispatcherQueueController == null)
            {
                DispatcherQueueOptions options;
                options.dwSize = Marshal.SizeOf(typeof(DispatcherQueueOptions));
                options.threadType = 2;    // DQTYPE_THREAD_CURRENT
                options.apartmentType = 2; // DQTAT_COM_STA

                CreateDispatcherQueueController(options, ref m_dispatcherQueueController);
            }
        }
    }

}
