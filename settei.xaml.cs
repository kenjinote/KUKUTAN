using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KUKUTAN
{
    /// <summary>
    /// settei.xaml の相互作用ロジック
    /// </summary>
    public partial class settei : Page
    {
        public settei()
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

        private void savebutton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("保存しました"/*, "九九タン"*/);
        }

        private void savebutton_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            for (int i = 1; i <= 20; i++)
            {
                var textbox = this.FindName("textbox" + i.ToString()) as TextBox;
                if (textbox != null )
                {
                    textbox.FontSize = textbox.ActualHeight / 1.5;
                }
             }
        }
    }
}
