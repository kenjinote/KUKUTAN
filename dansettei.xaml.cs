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

        private void button1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Module1.dan = 1; // 1段

            // スタートボタン画面 に遷移
            NavigationService.Navigate(new SecondPage());
        }

        private void button2_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Module1.dan = 2; // 2段

            // スタートボタン画面 に遷移
            NavigationService.Navigate(new SecondPage());
        }

        private void button3_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Module1.dan = 3; // 3段

            // スタートボタン画面 に遷移
            NavigationService.Navigate(new SecondPage());
        }

        private void button4_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Module1.dan = 4; // 4段

            // スタートボタン画面 に遷移
            NavigationService.Navigate(new SecondPage());
        }

        private void button5_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Module1.dan = 5; // 5段

            // スタートボタン画面 に遷移
            NavigationService.Navigate(new SecondPage());
        }

        private void button6_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Module1.dan = 6; // 6段

            // スタートボタン画面 に遷移
            NavigationService.Navigate(new SecondPage());
        }

        private void button7_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Module1.dan = 7; // 7段

            // スタートボタン画面 に遷移
            NavigationService.Navigate(new SecondPage());
        }

        private void button8_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Module1.dan = 8; // 8段

            // スタートボタン画面 に遷移
            NavigationService.Navigate(new SecondPage());
        }

        private void button9_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Module1.dan = 9; // 9段

            // スタートボタン画面 に遷移
            NavigationService.Navigate(new SecondPage());
        }

        private void button_random_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Module1.dan = 10; // ランダム

            // スタートボタン画面 に遷移
            NavigationService.Navigate(new SecondPage());
        }
    }
}
