using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Services
{
    internal class AnimationServices
    {
        /// <summary>
        /// 位移动画 默认TranslateX=0,TranslateY=0
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="fromx"></param>
        /// <param name="tox"></param>
        /// <param name="fromy"></param>
        /// <param name="toy"></param>
        /// <param name="animobject"></param>
        /// <returns></returns>
        public static Storyboard Animation_TranslateXY(TimeSpan duration, double fromx, double tox, double fromy, double toy, Grid animobject)
        {
            animobject.RenderTransform = new CompositeTransform() { TranslateX = 0, TranslateY=0 };
            var storyBoard = new Storyboard();
            var extendAnimation = new DoubleAnimation { Duration = new Duration(duration), From = fromx, To = tox, EnableDependentAnimation = true };
            Storyboard.SetTarget(extendAnimation, animobject);
            Storyboard.SetTargetProperty(extendAnimation, "(UIElement.RenderTransform).(CompositeTransform.TranslateX)");
            storyBoard.Children.Add(extendAnimation);

            var extendAnimation2 = new DoubleAnimation { Duration = new Duration(duration), From = fromy, To = toy, EnableDependentAnimation = true };
            Storyboard.SetTarget(extendAnimation2, animobject);
            Storyboard.SetTargetProperty(extendAnimation2, "(UIElement.RenderTransform).(CompositeTransform.TranslateY)");
            storyBoard.Children.Add(extendAnimation2);

            storyBoard.Begin();
            return storyBoard;
        }
        /// <summary>
        /// 缩放动画 默认ScaleX = 1, ScaleY = 1
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="fromx"></param>
        /// <param name="tox"></param>
        /// <param name="fromy"></param>
        /// <param name="toy"></param>
        /// <param name="animobject"></param>
        public static Storyboard Animation_ScaleXY(TimeSpan duration,double fromx,double tox, double fromy, double toy, Grid animobject)
        {
            animobject.RenderTransform = new CompositeTransform() { TranslateY = 0, ScaleX = 1, ScaleY = 1 };
            var storyBoard = new Storyboard();
            var extendAnimation = new DoubleAnimation { Duration = new Duration(duration), From = fromx, To = tox, EnableDependentAnimation = true };
            Storyboard.SetTarget(extendAnimation, animobject);
            Storyboard.SetTargetProperty(extendAnimation, "(UIElement.RenderTransform).(CompositeTransform.ScaleX)");
            storyBoard.Children.Add(extendAnimation);

            var extendAnimation2 = new DoubleAnimation { Duration = new Duration(duration), From = fromy, To = toy, EnableDependentAnimation = true };
            Storyboard.SetTarget(extendAnimation2, animobject);
            Storyboard.SetTargetProperty(extendAnimation2, "(UIElement.RenderTransform).(CompositeTransform.ScaleY)");
            storyBoard.Children.Add(extendAnimation2);

            storyBoard.Begin();
            return storyBoard;
        }
        /// <summary>
        /// 扭曲动画 分别表示X轴和Y轴倾斜的角度,SkewX顺时针，SkewY逆时针 默认 SkewX=0, SkewY=0
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="fromx"></param>
        /// <param name="tox"></param>
        /// <param name="fromy"></param>
        /// <param name="toy"></param>
        /// <param name="animobject"></param>
        /// <returns></returns>
        public static Storyboard Animation_SkewXY(TimeSpan duration, double fromx, double tox, double fromy, double toy, double centerx, double centery, Grid animobject)
        {
            animobject.RenderTransform = new CompositeTransform() {  SkewX=0, SkewY=0, CenterX = centerx, CenterY = centery };
            var storyBoard = new Storyboard();
            var extendAnimation = new DoubleAnimation { Duration = new Duration(duration), From = fromx, To = tox, EnableDependentAnimation = true };
            Storyboard.SetTarget(extendAnimation, animobject);
            Storyboard.SetTargetProperty(extendAnimation, "(UIElement.RenderTransform).(CompositeTransform.SkewX)");
            storyBoard.Children.Add(extendAnimation);

            var extendAnimation2 = new DoubleAnimation { Duration = new Duration(duration), From = fromy, To = toy, EnableDependentAnimation = true };
            Storyboard.SetTarget(extendAnimation2, animobject);
            Storyboard.SetTargetProperty(extendAnimation2, "(UIElement.RenderTransform).(CompositeTransform.SkewY)");
            storyBoard.Children.Add(extendAnimation2);

            storyBoard.Begin();
            return storyBoard;
        }
        /// <summary>
        /// 旋转动画 默认Rotation=0
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="fromx"></param>
        /// <param name="tox"></param>
        /// <param name="animobject"></param>
        /// <returns></returns>
        public static Storyboard Animation_Rotation(TimeSpan duration, double fromx, double tox,double centerx,double centery,  Grid animobject)
        {
            animobject.RenderTransform = new CompositeTransform() {  Rotation=0,CenterX= centerx, CenterY= centery };
            var storyBoard = new Storyboard();
            var extendAnimation = new DoubleAnimation { Duration = new Duration(duration), From = fromx, To = tox, EnableDependentAnimation = true };
            Storyboard.SetTarget(extendAnimation, animobject);
            Storyboard.SetTargetProperty(extendAnimation, "(UIElement.RenderTransform).(CompositeTransform.Rotation)");
            storyBoard.Children.Add(extendAnimation);

            storyBoard.Begin();
            return storyBoard;
        }
    }
}
