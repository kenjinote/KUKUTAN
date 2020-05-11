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
    /// zyunzyosettei.xaml の相互作用ロジック
    /// </summary>
    public partial class zyunzyosettei : Page
    {
        public zyunzyosettei()
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

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    Module1.zyunzyo = 0; // じゅんばん

        //    NavigationService.Navigate(new SecondPage());            
        //}

        // じゅんばん ボタンが押されたとき
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Module1.zyunzyo = 0; // じゅんばん

            NavigationService.Navigate(new SecondPage());
        }

        // ぎゃくじゅん ボタンが押されたとき
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Module1.zyunzyo = 1; // ぎゃくじゅん

            NavigationService.Navigate(new SecondPage());

        }

        // ランダム ボタンが押されたとき
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Module1.zyunzyo = 2; // ランダム

            NavigationService.Navigate(new SecondPage());

        }
    }
}
