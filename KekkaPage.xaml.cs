using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KUKUTAN
{
    /// <summary>
    /// KekkaPage.xaml の相互作用ロジック
    /// </summary>
    public partial class KekkaPage : Page
    {
        public KekkaPage()
        {
            InitializeComponent();
        }

        private void backbutton_Click(object sender, RoutedEventArgs e)
        {
            while (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}
