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
    /// sokudosettei.xaml の相互作用ロジック
    /// </summary>
    public partial class sokudosettei : Page
    {
        public sokudosettei()
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

        // ゆっくり ボタンが押されたとき
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Module1.sokudo = 0; // ゆっくり

            // 順序設定画面に遷移
            NavigationService.Navigate(new zyunzyosettei());
        }

        // ふつう ボタンが押されたとき
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Module1.sokudo = 1; // ふつう

            // 順序設定画面に遷移
            NavigationService.Navigate(new zyunzyosettei());
        }

        // オリジナル ボタンが押されたとき
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Module1.sokudo = 2; // オリジナル

            // 順序設定画面に遷移
            NavigationService.Navigate(new zyunzyosettei());
        }
    }
}
