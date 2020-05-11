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
    /// syutsudaisettei.xaml の相互作用ロジック
    /// </summary>
    public partial class syutsudaisettei : Page
    {
        public syutsudaisettei()
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

        // ①すうじ＋もじ＋こたえ ボタンが押されたとき
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Module1.keishiki = 0; // すうじ＋もじ＋こたえ

            // 数字文字設定ページに遷移
            NavigationService.Navigate(new zyunzyosettei());
        }

        // ②すうじ＋もじ ボタンが押されたとき
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Module1.keishiki = 1; // すうじ＋もじ

            // 速度設定画面へ遷移
            NavigationService.Navigate(new sokudosettei());
        }

        // ③もじのみ ボタンが押されたとき
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Module1.keishiki = 2; // もじのみ

            // 数字文字設定ページに遷移
            NavigationService.Navigate(new zyunzyosettei());
        }

        // ④すうじのみ ボタンが押されたとき
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            Module1.keishiki = 3; // すうじのみ

            // 数字文字設定ページに遷移
            NavigationService.Navigate(new zyunzyosettei());
        }

        // ⑤段テスト ボタンが押されたとき
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            Module1.keishiki = 4; // 段テスト

            // スタートボタンページへ遷移
            NavigationService.Navigate(new SecondPage());
        }
    }
}
