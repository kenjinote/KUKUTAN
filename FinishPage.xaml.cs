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
using System.Windows.Threading;

namespace KUKUTAN
{
    /// <summary>
    /// FinishPage.xaml の相互作用ロジック
    /// </summary>
    public partial class FinishPage : Page
    {
        // タイマのインスタンス
        private DispatcherTimer _timer1;
        public FinishPage()
        {
            InitializeComponent();
            // タイマのインスタンスを生成
            _timer1 = new DispatcherTimer();
            // インターバルを設定
            _timer1.Interval = new TimeSpan(0, 0, 2);
            // タイマメソッドを設定
            _timer1.Tick += new EventHandler(Timer1_Tick);
            // タイマを開始
            _timer1.Start();
        }

        // タイマメソッド
        private void Timer1_Tick(object sender, EventArgs e)
        {
            _timer1.Stop();

            // 結果ページに遷移する
            NavigationService.Navigate(new KekkaPage());
        }
    }
}
