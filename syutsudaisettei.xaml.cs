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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null)
                return;
            if (button.Content.ToString().Contains("②"))
            {
                NavigationService.Navigate(new sokudosettei());
                return;
            }
            if (button.Content.ToString().Contains("⑤"))
            {
                NavigationService.Navigate(new SecondPage());
                return;
            }


            // 数字文字設定ページに遷移
            NavigationService.Navigate(new zyunzyosettei());            
        }
    }
}
