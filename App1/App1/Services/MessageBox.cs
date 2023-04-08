using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Services
{
    internal static class MessageBox
    {
        public async static void Show(string title, string content)
        {
            ContentDialog cd = new ContentDialog();
            cd.XamlRoot = Shares.Data.AppServices.root.XamlRoot;
            cd.Title = title;
            cd.CloseButtonText = "OK";
            cd.Content = content;
            await cd.ShowAsync();
        }
       
        public async static void Show(string content)
        {
            ContentDialog cd = new ContentDialog();
            cd.XamlRoot = Shares.Data.AppServices.root.XamlRoot;
            cd.Title = "";
            cd.CloseButtonText = "OK";
            cd.Content = content;
            await cd.ShowAsync();
        }  
        public async static void Show(XamlRoot root,string title, string content)
        {
            ContentDialog cd = new ContentDialog();
            cd.XamlRoot = root;
            cd.Title = title;
            cd.CloseButtonText = "OK";
            cd.Content = content;
            await cd.ShowAsync();
        }
        public async static void Show(XamlRoot root,string content)
        {
            ContentDialog cd = new ContentDialog();
            cd.XamlRoot = root;
            cd.Title = "";
            cd.CloseButtonText = "OK";
            cd.Content = content;
            await cd.ShowAsync();
        }
    }
}
