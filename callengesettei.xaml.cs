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
    /// callengesettei.xaml の相互作用ロジック
    /// </summary>
    public partial class callengesettei : Page
    {
        public callengesettei()
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Module1.zyunzyo = 0; // じゅんばん

            // スタートボタン画面 に遷移
            NavigationService.Navigate(new SecondPage());
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Module1.zyunzyo = 1; // ぎゃくじゅん

            // スタートボタン画面 に遷移
            NavigationService.Navigate(new SecondPage());
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Module1.zyunzyo = 2; // ランダム

            // スタートボタン画面 に遷移
            NavigationService.Navigate(new SecondPage());
        }
    }
}
