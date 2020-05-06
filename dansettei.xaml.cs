using System.Windows.Controls;

namespace KUKUTAN
{
    /// <summary>
    /// dansettei.xaml の相互作用ロジック
    /// </summary>
    public partial class dansettei : Page
    {
        public dansettei()
        {
            InitializeComponent();
        }

        private void backbutton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            while (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new SecondPage());
        }
    }
}
