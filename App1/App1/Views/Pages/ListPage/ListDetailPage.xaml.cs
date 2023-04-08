using App1.ViewModels;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App1.Views.Pages.ListPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ListDetailPage : Page
    {
        public ListDetailPage()
        {
            this.InitializeComponent();
            Shares.Data.ListDetailsView = ListDetailsViewControl;
            ListDetailsViewModel.Init();
            ListDetailsViewControl.ItemsSource = ListDetailsViewModel.Source;
        }

        private void ListDetailsViewControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListDetailsViewModel.ListDetailsViewControl_SelectionChanged(sender);
           
        }
    }
    
}
