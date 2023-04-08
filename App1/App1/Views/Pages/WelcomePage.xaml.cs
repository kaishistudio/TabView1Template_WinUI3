using App1.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App1.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            this.InitializeComponent();
            AppInit();
        }
        void AppInit()
        {
            var storyBoard=AnimationServices.Animation_TranslateXY(TimeSpan.FromSeconds(0.8),200,0,0,0, g_title);
            storyBoard.Completed += (ss, ee) =>
            {
                g_subtitle.Visibility = Visibility.Visible;
                var storyBoard = AnimationServices.Animation_ScaleXY(TimeSpan.FromSeconds(0.4), 0.2, 1.5, 0.2, 1.5, g_subtitle);
                storyBoard.Completed += (ss, ee) =>
                {
                    g_subtitle.Visibility = Visibility.Visible;
                    var storyBoard = AnimationServices.Animation_Rotation(TimeSpan.FromSeconds(0.4), 0,360, g_subtitle.ActualWidth/2, g_subtitle.ActualHeight/2, g_subtitle);
                    storyBoard.Completed += (ss, ee) =>
                    {
                        g_subtitle.Visibility = Visibility.Visible;
                        var storyBoard = AnimationServices.Animation_SkewXY(TimeSpan.FromSeconds(0.4), 0, -30, 0, -30, g_title.ActualWidth / 2, g_title.ActualHeight / 2, g_title);
                        storyBoard.Completed += (ss, ee) =>
                        {
                            g_subtitle.Visibility = Visibility.Visible;
                            var storyBoard = AnimationServices.Animation_SkewXY(TimeSpan.FromSeconds(0.4), -30, 0, -30,0,  g_title.ActualWidth / 2, g_title.ActualHeight / 2, g_title);
                           
                        };
                    };
                };
            };
        }
    }
}
