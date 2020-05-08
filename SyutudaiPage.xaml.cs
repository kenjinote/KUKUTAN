using System;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace KUKUTAN
{
    /// <summary>
    /// SyutudaiPage.xaml の相互作用ロジック
    /// </summary>
    public partial class SyutudaiPage : Page
    {
        private DispatcherTimer _timer1; // タイマー変数
        private Window window; // キーイベント登録・解除用
        private int TimeCount; // 現在の秒数
        private const int LimitSec = 10; // 問題時間は1分間（60秒）

        public SyutudaiPage()
        {
            InitializeComponent();
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            if (_timer1 != null && _timer1.IsEnabled)
            {
                _timer1.Stop();
            }
            NavigationService.Navigate(new KekkaPage());
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            TimeCount++;

            timetext.Text = "";
            timetext.Inlines.Add(new Run(TimeCount.ToString()) { FontSize = 25 });
            timetext.Inlines.Add("秒");

            //＜１分経過したら終了する＞
            if (TimeCount == LimitSec)
            {
                _timer1.Stop();

                // おわりページに遷移する
                NavigationService.Navigate(new FinishPage());
            }
        }

        // キーボード入力のイベント
        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (　// 数字を入力した場合
                e.Key == Key.D0 || e.Key == Key.D1 || e.Key == Key.D2 || e.Key == Key.D3 || e.Key == Key.D4 ||
                e.Key == Key.D5 || e.Key == Key.D6 || e.Key == Key.D7 || e.Key == Key.D8 || e.Key == Key.D9 ||
                e.Key == Key.NumPad0 || e.Key == Key.NumPad1 || e.Key == Key.NumPad2 || e.Key == Key.NumPad3 || e.Key == Key.NumPad4 ||
                e.Key == Key.NumPad5 || e.Key == Key.NumPad6 || e.Key == Key.NumPad7 || e.Key == Key.NumPad8 || e.Key == Key.NumPad9
                )
            {
                // 未入力のセルに0を入力しようとしたときは入力を無視する
                if ((e.Key == Key.D0 || e.Key == Key.NumPad0) && text3.Content.ToString().Length == 0)
                {
                    return;
                }
                // 2桁未満のときのみ数値入力を許容する
                if (text3.Content.ToString().Length < 2)
                {
                    string keyCodeString = e.Key.ToString();
                    text3.Content += keyCodeString[keyCodeString.Length - 1].ToString();
                }
            }
            // スペースキー、マイナスキー、バックスペースキー、デリートキーを入力した場合は、セルの入力をクリアする
            else if (e.Key == Key.Space || e.Key == Key.OemMinus || e.Key == Key.Subtract || e.Key == Key.Back || e.Key == Key.Delete)
            {
                text3.Content = "";
            }
            // エンターキーを入力した場合は
            else if (e.Key == Key.Enter)
            {
                // 数字未入力の場合は何もしない
                if (text3.Content.ToString() == "") return;

                // 入力値が正解の場合
                if (text3.correct)
                {
                    // 正解の音を鳴らす
                    new SoundPlayer(Properties.Resources.CBC).Play();

                    // 正解数をカウントアップ
                    //mainWindow.OKCount++;

                }
                // 不正解の場合は、回答をクリアする
                else
                {
                    new SoundPlayer(Properties.Resources.BUBU).Play();
                    text3.Content = "";
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            window = Window.GetWindow(this);
            window.KeyDown += HandleKeyDown;

        //Label3.Text = CStr(0) '残り時間
            Module1.mondai_count = 0;
            Module1.collect_count = 0; // 正解数
            Module1.mistake_count = 0; // 不正解数
            Module1.matigaikaisu = 0;

            switch(Module1.keishiki)
            {
                case 0: // もじ+すうじ＋こたえ
                    Label5.Visibility = Visibility.Visible; // すうじ
                    break;
                case 1: // もじ＋すうじ
                    break;
                case 2: // もじのみ
                    break;
                case 3: // すうじのみ
                    break;
                case 4: // 段ごとのテスト
                    break;
                case 5: // ８１問テストチャレンジ
                    break;
                case 6: // 穴埋
                    break;
                case 7: // 実力判定
                    break;
                case 8: // ふくしゅうモード
                    break;
            }

            TimeCount = 0;
            // タイマのインスタンスを生成
            _timer1 = new DispatcherTimer();
            // インターバルを設定
            _timer1.Interval = new TimeSpan(0, 0, 1);
            // タイマメソッドを設定
            _timer1.Tick += new EventHandler(Timer1_Tick);
            // タイマを開始
            _timer1.Start();
        }

        private void Page_Unloaded_1(object sender, RoutedEventArgs e)
        {
            window.KeyDown -= HandleKeyDown;
        }
    }
}
