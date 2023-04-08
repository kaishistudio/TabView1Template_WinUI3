using App1.Models;
using App1.Services;
using App1.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace App1.ViewModels
{
    internal class ListDetailsViewModel : ObservableRecipient
    {
        public static ObservableCollection<ListModel> Source = new() { };
        public static ObservableCollection<ListModel> DetailSource = new() { };
        public static void Init()
        {
            Source = ReadSqliteData();
        }
        public static ObservableCollection<ListModel> ReadSqliteData()
        {
            ObservableCollection<ListModel> list = new ObservableCollection<ListModel>();

            var query = SqliteService.ReadTableData(Shares.Data.UserDBTableName, Shares.Data.UserDBColumns, "", "");
            while (query.Read())
            {
                list.Insert(0, new ListModel { Title = int.Parse(query.GetString(0)), Url = int.Parse(query.GetString(1)) });
            }

            return list;
        }
        public static void ListDetailsViewControl_SelectionChanged(object sender)
        {
            try
            {
                var model = Source[(sender as ListDetailsView).SelectedIndex];
                DetailSource.Clear();
                DetailSource.Add(model);
            }
            catch { }
        }

    }
}
