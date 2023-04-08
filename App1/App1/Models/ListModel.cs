using ABI.System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace App1.Models
{
    public class ListModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        
        private int _Title ;
        public int Title
        {
            set
            {
                _Title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
            }
            get
            {
                return _Title;
            }
        }
       
        private int _Url ;
        public int Url
        {
            set
            {
               
            }
            get
            {
                return _Title*_Title;
            }
        }
        public string Way
        {
            set
            {
            }
            get
            {
                return $"{_Title}*{_Title}={Math.Pow(_Title, 2)}";
            }
        }

    }
}
