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

        // 1の段 ボタンが押されたとき
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Module1.dan = 1; // 1の段

            // 出題設定画面に遷移
            NavigationService.Navigate(new syutsudaisettei());
        }

        // 2の段 ボタンが押されたとき
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Module1.dan = 2; // 2の段

            // 出題設定画面に遷移
            NavigationService.Navigate(new syutsudaisettei());
        }

        // 3の段 ボタンが押されたとき
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Module1.dan = 3; // 3の段

            // 出題設定画面に遷移
            NavigationService.Navigate(new syutsudaisettei());
        }

        // 4の段 ボタンが押されたとき
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            Module1.dan = 4; // 4の段

            // 出題設定画面に遷移
            NavigationService.Navigate(new syutsudaisettei());
        }

        // 5の段 ボタンが押されたとき
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            Module1.dan = 5; // 5の段

            // 出題設定画面に遷移
            NavigationService.Navigate(new syutsudaisettei());
        }

        // 6の段 ボタンが押されたとき
        private void button6_Click(object sender, RoutedEventArgs e)
        {
            Module1.dan = 6; // 6の段

            // 出題設定画面に遷移
            NavigationService.Navigate(new syutsudaisettei());
        }

        // 7の段 ボタンが押されたとき
        private void button7_Click(object sender, RoutedEventArgs e)
        {
            Module1.dan = 7; // 7の段

            // 出題設定画面に遷移
            NavigationService.Navigate(new syutsudaisettei());
        }

        // 8の段 ボタンが押されたとき
        private void button8_Click(object sender, RoutedEventArgs e)
        {
            Module1.dan = 8; // 8の段

            // 出題設定画面に遷移
            NavigationService.Navigate(new syutsudaisettei());
        }

        // 9の段 ボタンが押されたとき
        private void button9_Click(object sender, RoutedEventArgs e)
        {
            Module1.dan = 9; // 9の段

            // 出題設定画面に遷移
            NavigationService.Navigate(new syutsudaisettei());
        }
    }
}
