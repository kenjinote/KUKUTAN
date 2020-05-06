using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KUKUTAN
{
    /// <summary>
    /// TopPage.xaml の相互作用ロジック
    /// </summary>
    public partial class TopPage : Page
    {
        public TopPage()
        {
            InitializeComponent();
        }

        private void exitbutton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow) Application.Current.MainWindow;
            mainWindow.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 出題設定に遷移
            NavigationService.Navigate(new syutsudaisettei());
        }

        private void anaumebutton_Click(object sender, RoutedEventArgs e)
        {
            // 段設定に遷移
            NavigationService.Navigate(new dansettei());            
        }

        private void challenge81button_Click(object sender, RoutedEventArgs e)
        {
            // チャレンジ設定に遷移
            NavigationService.Navigate(new callengesettei());            
        }

        private void settingsbutton_Click(object sender, RoutedEventArgs e)
        {
            frmLogin frm = new frmLogin();
            frm.ShowDialog();
            if (frm.pass)
            {
                NavigationService.Navigate(new settei());                
            }
        }
    }
}
