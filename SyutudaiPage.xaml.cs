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
        private input active_text;

        public SyutudaiPage()
        {
            InitializeComponent();
        }

        private void Command1_Click(object sender, RoutedEventArgs e)
        {
            if (_timer1 != null && _timer1.IsEnabled)
            {
                _timer1.Stop();
            }
            NavigationService.Navigate(new KekkaPage());
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Label3.Content = Module1.syutu_count.ToString();

            // 時間制限に達したら
            if (Module1.syutu_count == Module1.SEC)
            {
                _timer1.Stop();

                // おわりページに遷移する
                NavigationService.Navigate(new FinishPage());
            }

            Module1.syutu_count++;
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
                if ((e.Key == Key.D0 || e.Key == Key.NumPad0) && active_text.Content.ToString().Length == 0)
                {
                    return;
                }
                // 2桁未満のときのみ数値入力を許容する
                if (active_text.Content.ToString().Length < 2)
                {
                    string keyCodeString = e.Key.ToString();
                    active_text.Content += keyCodeString[keyCodeString.Length - 1].ToString();
                }
            }
            // スペースキー、マイナスキー、バックスペースキー、デリートキーを入力した場合は、セルの入力をクリアする
            else if (e.Key == Key.Space || e.Key == Key.OemMinus || e.Key == Key.Subtract || e.Key == Key.Back || e.Key == Key.Delete)
            {
                active_text.Content = "";
            }
            // エンターキーを入力した場合は
            else if (e.Key == Key.Enter)
            {
                // 数字未入力の場合は何もしない
                if (active_text.Content.ToString() == "") return;

                if (active_text.correct)
                {
                    // 入力値が正解の場合
                    Module1.matigaikaisu = 0;

                    if (Module1.keishiki == 1)
                    {
                        Label6.Visibility = Visibility.Hidden;
                    }

                    if (Module1.review_mode == true && Module1.keishiki != 0)
                    {
                        Label6.Visibility = Visibility.Hidden;
                    }

                    // 正解の音を鳴らす
                    new SoundPlayer(Properties.Resources.CBC).Play();

                    if (Module1.huseikaicheck == true)
                    {
                        Module1.huseikaicheck = false; // 不正解のカウント初期化
                    }
                    else
                    {
                        Module1.collect_count = (short)(Module1.collect_count + 1); // 正解数をカウントアップ
                    }

                    Module1.mondai_count = (short)(Module1.mondai_count + 1); // 出題数をカウントアップ

                    if (Module1.mondai_count == (short)(Module1.syutsudaimondaisu + 1))　// 出題数が目標に達したら終了
                    {
                        // タイマーストップ
                        if (_timer1 != null && _timer1.IsEnabled)
                        {
                            _timer1.Stop();
                        }

                        // おわりページに遷移する
                        NavigationService.Navigate(new FinishPage());
                    }
                    else
                    {
                        Label1.Content = Module1.mondai_count; // 出題数表示を更新
                        hyouji(); // 次の問題を出題する
                    }
                }
                else
                {
                    // 不正解のとき

                    // 不正解の音を鳴らす
                    new SoundPlayer(Properties.Resources.BUBU).Play();

                    Module1.matigaikaisu++; // もじ＋すうじの場合、1回間違えたらもじで答表示、2回間違えたらすうじで薄く答を表示

                    if (Module1.keishiki == 1 || Module1.review_mode == true)
                    {
                        if (Module1.matigaikaisu == 1)
                        {
                            Label6.Visibility = Visibility.Visible;
                        }
                        else if (Module1.matigaikaisu > 1)
                        {
                            active_text.ShowHint = true;
                        }
                    }

                    active_text.Content = ""; // ShowHint設定の後行う必要がある。

                    if (Module1.huseikaicheck == false) // 不正解数を増やすかどうかを確認
                    {
                        Module1.huseikaicheck = true;
                        Module1.mistake_count++;
                    }

                    Module1.huseikaidankakunin[Module1.X, Module1.Y] = true;

                    if (Module1.keishiki != 7)
                    {
                        return;
                    }
                    else if (Module1.keishiki == 7)
                    {
                        // 実力判定のみ間違えても次の問題を表示する

                        if (Module1.huseikaicheck == true) Module1.huseikaicheck = false; // 不正解のカウント初期化
                        Module1.mondai_count++;

                        if (Module1.mondai_count == (short)(Module1.syutsudaimondaisu + 1)) // 出題数が目標に達したら終了
                        {
                            // タイマーストップ
                            if (_timer1 != null && _timer1.IsEnabled)
                            {
                                _timer1.Stop();
                            }

                            // おわりページに遷移する
                            NavigationService.Navigate(new FinishPage());
                        }
                        else
                        {
                            Label1.Content = Module1.mondai_count; // 出題数表示を更新
                            hyouji(); // 次の問題を出題する
                        }
                    }
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            window = Window.GetWindow(this);
            window.KeyDown += HandleKeyDown;

            Label3.Content = "0"; // 残り時間
            Module1.mondai_count = 0;
            Module1.collect_count = 0; // 正解数
            Module1.mistake_count = 0; // 不正解数
            Module1.matigaikaisu = 0;

            Text3.Background = new SolidColorBrush(Colors.White);

            Label7.Content = "×";

            switch (Module1.keishiki)
            {
                case 0: // すうじ＋もじ＋こたえ
                    Label5.Visibility = Visibility.Visible;
                    Label6.Visibility = Visibility.Visible;
                    Label7.Visibility = Visibility.Visible;
                    Label8.Visibility = Visibility.Visible;
                    Label9.Visibility = Visibility.Hidden;
                    Text1.Visibility = Visibility.Visible;
                    Text2.Visibility = Visibility.Visible;
                    // 答えを表示
                    Text3.ShowHint = true;
                    active_text = Text3;
                    Text1.Background = new SolidColorBrush(Colors.Black);
                    Text2.Background = new SolidColorBrush(Colors.Black);
                    Text3.Background = new SolidColorBrush(Colors.White);
                    Text1.Foreground = new SolidColorBrush(Colors.Yellow);
                    Text2.Foreground = new SolidColorBrush(Colors.Yellow);
                    Text3.Foreground = new SolidColorBrush(Colors.Black);
                    break;
                case 1: // すうじ＋もじ
                    Label5.Visibility = Visibility.Visible;
                    Label6.Visibility = Visibility.Hidden;
                    Label7.Visibility = Visibility.Visible;
                    Label8.Visibility = Visibility.Visible;
                    Label9.Visibility = Visibility.Hidden;
                    Text1.Visibility = Visibility.Visible;
                    Text2.Visibility = Visibility.Visible;
                    active_text = Text3;
                    Text1.Background = new SolidColorBrush(Colors.Black);
                    Text2.Background = new SolidColorBrush(Colors.Black);
                    Text3.Background = new SolidColorBrush(Colors.White);
                    Text1.Foreground = new SolidColorBrush(Colors.Yellow);
                    Text2.Foreground = new SolidColorBrush(Colors.Yellow);
                    Text3.Foreground = new SolidColorBrush(Colors.Black);
                    break;
                case 2: // もじ
                    Label5.Visibility = Visibility.Hidden;
                    Label6.Visibility = Visibility.Hidden;
                    Label7.Visibility = Visibility.Hidden;
                    Label8.Visibility = Visibility.Hidden;
                    Label9.Visibility = Visibility.Visible;
                    Text1.Visibility = Visibility.Hidden;
                    Text2.Visibility = Visibility.Hidden;
                    active_text = Text3;
                    Text1.Background = new SolidColorBrush(Colors.Black);
                    Text2.Background = new SolidColorBrush(Colors.Black);
                    Text3.Background = new SolidColorBrush(Colors.White);
                    Text1.Foreground = new SolidColorBrush(Colors.Yellow);
                    Text2.Foreground = new SolidColorBrush(Colors.Yellow);
                    Text3.Foreground = new SolidColorBrush(Colors.Black);
                    break;
                case 3: // すうじ
                    Label5.Visibility = Visibility.Hidden;
                    Label6.Visibility = Visibility.Hidden;
                    Label7.Visibility = Visibility.Visible;
                    Label8.Visibility = Visibility.Visible;
                    Label9.Visibility = Visibility.Hidden;
                    Text1.Visibility = Visibility.Visible;
                    Text2.Visibility = Visibility.Visible;
                    active_text = Text3;
                    Text1.Background = new SolidColorBrush(Colors.Black);
                    Text2.Background = new SolidColorBrush(Colors.Black);
                    Text3.Background = new SolidColorBrush(Colors.White);
                    Text1.Foreground = new SolidColorBrush(Colors.Yellow);
                    Text2.Foreground = new SolidColorBrush(Colors.Yellow);
                    Text3.Foreground = new SolidColorBrush(Colors.Black);
                    break;
                case 4: // 段ごとテスト
                    Label5.Visibility = Visibility.Hidden;
                    Label6.Visibility = Visibility.Hidden;
                    Label7.Visibility = Visibility.Visible;
                    Label8.Visibility = Visibility.Visible;
                    Label9.Visibility = Visibility.Hidden;
                    Text1.Visibility = Visibility.Visible;
                    Text2.Visibility = Visibility.Visible;
                    active_text = Text3;
                    Text1.Background = new SolidColorBrush(Colors.Black);
                    Text2.Background = new SolidColorBrush(Colors.Black);
                    Text3.Background = new SolidColorBrush(Colors.White);
                    Text1.Foreground = new SolidColorBrush(Colors.Yellow);
                    Text2.Foreground = new SolidColorBrush(Colors.Yellow);
                    Text3.Foreground = new SolidColorBrush(Colors.Black);
                    break;
                case 5: // 81問チャレンジ
                    Label5.Visibility = Visibility.Hidden;
                    Label6.Visibility = Visibility.Hidden;
                    Label7.Visibility = Visibility.Visible;
                    Label8.Visibility = Visibility.Visible;
                    Label9.Visibility = Visibility.Hidden;
                    Text1.Visibility = Visibility.Visible;
                    Text2.Visibility = Visibility.Visible;
                    active_text = Text3;
                    Text1.Background = new SolidColorBrush(Colors.Black);
                    Text2.Background = new SolidColorBrush(Colors.Black);
                    Text3.Background = new SolidColorBrush(Colors.White);
                    Text1.Foreground = new SolidColorBrush(Colors.Yellow);
                    Text2.Foreground = new SolidColorBrush(Colors.Yellow);
                    Text3.Foreground = new SolidColorBrush(Colors.Black);
                    break;
                case 6: // 穴埋
                    Label5.Visibility = Visibility.Hidden;
                    Label6.Visibility = Visibility.Hidden;
                    Label7.Visibility = Visibility.Visible;
                    Label8.Visibility = Visibility.Visible;
                    Label9.Visibility = Visibility.Hidden;
                    Text1.Visibility = Visibility.Visible;
                    Text2.Visibility = Visibility.Visible;
                    active_text = Text2;
                    Text1.Background = new SolidColorBrush(Colors.Black);
                    Text2.Background = new SolidColorBrush(Colors.White);
                    Text3.Background = new SolidColorBrush(Colors.Black);
                    Text1.Foreground = new SolidColorBrush(Colors.Yellow);
                    Text2.Foreground = new SolidColorBrush(Colors.Black);
                    Text3.Foreground = new SolidColorBrush(Colors.Yellow);
                    break;
                case 7: // 実力判定
                    Label5.Visibility = Visibility.Hidden;
                    Label6.Visibility = Visibility.Hidden;
                    Label7.Visibility = Visibility.Visible;
                    Label8.Visibility = Visibility.Visible;
                    Label9.Visibility = Visibility.Hidden;
                    Text1.Visibility = Visibility.Visible;
                    Text2.Visibility = Visibility.Visible;
                    active_text = Text3;
                    Text1.Background = new SolidColorBrush(Colors.Black);
                    Text2.Background = new SolidColorBrush(Colors.Black);
                    Text3.Background = new SolidColorBrush(Colors.White);
                    Text1.Foreground = new SolidColorBrush(Colors.Yellow);
                    Text2.Foreground = new SolidColorBrush(Colors.Yellow);
                    Text3.Foreground = new SolidColorBrush(Colors.Black);
                    break;
                case 8: // ふくしゅう
                    Label5.Visibility = Visibility.Visible;
                    Label6.Visibility = Visibility.Hidden;
                    Label7.Visibility = Visibility.Visible;
                    Label8.Visibility = Visibility.Visible;
                    Label9.Visibility = Visibility.Hidden;
                    Text1.Visibility = Visibility.Visible;
                    Text2.Visibility = Visibility.Visible;
                    active_text = Text3;
                    Text1.Background = new SolidColorBrush(Colors.Black);
                    Text2.Background = new SolidColorBrush(Colors.Black);
                    Text3.Background = new SolidColorBrush(Colors.White);
                    Text1.Foreground = new SolidColorBrush(Colors.Yellow);
                    Text2.Foreground = new SolidColorBrush(Colors.Yellow);
                    Text3.Foreground = new SolidColorBrush(Colors.Black);
                    break;
            }

            if (Module1.review_mode == true && Module1.anaumekakunin == true)
            {
                Label5.Visibility = Visibility.Hidden;
                Label6.Visibility = Visibility.Hidden;
                Label7.Visibility = Visibility.Visible;
                Label8.Visibility = Visibility.Visible;
                Label9.Visibility = Visibility.Hidden;
                Text1.Visibility = Visibility.Visible;
                Text2.Visibility = Visibility.Visible;
                active_text = Text2;
                Text1.Background = new SolidColorBrush(Colors.Black);
                Text2.Background = new SolidColorBrush(Colors.White);
                Text3.Background = new SolidColorBrush(Colors.Black);
                Text1.Foreground = new SolidColorBrush(Colors.Yellow);
                Text2.Foreground = new SolidColorBrush(Colors.Black);
                Text3.Foreground = new SolidColorBrush(Colors.Yellow);
            }

            //出題：「1問目」の表示

            Module1.mondai_count = 1; // 出題数

            Label1.Content = Module1.mondai_count.ToString(); // 現在出題数

            Module1.syutu_count = 1; // 出題時間

            hyouji();

            if (Module1.review_mode == true) // ふくしゅうモードの場合は戻るボタンを表示しない
            {
                Command1.Visibility = Visibility.Hidden;
                Command2.Visibility = Visibility.Visible;
            }
            else
            {
                Command1.Visibility = Visibility.Visible;
                Command2.Visibility = Visibility.Hidden;
            }

            // タイマのインスタンスを生成
            _timer1 = new DispatcherTimer();
            // インターバルを設定
            _timer1.Interval = new TimeSpan(0, 0, 1);
            // タイマメソッドを設定
            _timer1.Tick += new EventHandler(Timer1_Tick);
            // タイマを開始
            _timer1.Start();
        }

        private void hyouji()
        {
            if (Module1.review_mode == true)
            {
                Module1.X = Module1.matigaimi[Module1.mondai_count];
                Module1.Y = Module1.matigaihou[Module1.mondai_count];
                Module1.ANS = (short)(Module1.X * Module1.Y);
            }
            else if (Module1.keishiki == 8)
            {
                Module1.X = Module1.hukusyudatami[Module1.mondai_count];
                Module1.Y = Module1.hukusyudatahou[Module1.mondai_count];
                Module1.ANS = (short)(Module1.X * Module1.Y);
            }
            else
            {
                SakumonModule.Sakumon();
            }

            // こたえを設定
            if (active_text == Text1)
            {
                active_text.anser = Module1.X;
            }
            else if (active_text == Text2)
            {
                active_text.anser = Module1.Y;
            }
            else if (active_text == Text3)
            {
                active_text.anser = Module1.ANS;
            }

            // 形式によってヒントを表示するかどうか設定する。（※ここで間違った時のヒントは出題時にクリアされる）
            if (Module1.keishiki == 0)
            {
                active_text.ShowHint = true;
            }
            else
            {
                active_text.ShowHint = false;
            }

            Text1.Content = "";
            Text2.Content = "";
            Text3.Content = ""; // 入力答え表示の初期化

            switch (Module1.keishiki)
            {
                case 6: // あなうめのとき
                    var brush = Text2.Background as SolidColorBrush;
                    if (brush.Color == Colors.White)
                    {
                        Text1.Content = Module1.X.ToString();
                        Text3.Content = Module1.ANS.ToString();
                        Label5.Content = Module1.kakeyomikata[Module1.X, Module1.Y];
                        Label6.Content = Module1.kakeyomikatakotae[Module1.X, Module1.Y];
                        Label9.Content = Module1.kakeyomikata[Module1.X, Module1.Y];
                    }
                    else
                    {
                        Text2.Content = Module1.Y.ToString();
                        Text3.Content = Module1.ANS.ToString();
                        Label5.Content = Module1.kakeyomikata[Module1.X, Module1.Y];
                        Label6.Content = Module1.kakeyomikatakotae[Module1.X, Module1.Y];
                        Label9.Content = Module1.kakeyomikata[Module1.X, Module1.Y];
                    }
                    break;
                default:
                    Label5.Content = Module1.kakeyomikata[Module1.X, Module1.Y];
                    Label6.Content = Module1.kakeyomikatakotae[Module1.X, Module1.Y];
                    Label9.Content = Module1.kakeyomikata[Module1.X, Module1.Y];
                    Text1.Content = Module1.X.ToString();
                    Text2.Content = Module1.Y.ToString();                    
                    break;
            }

            if (Module1.review_mode == true && Module1.anaumekakunin == true)
            {
                Text1.Content = "";
                Text2.Content = "";
                Text3.Content = "";
                var brush = Text2.Background as SolidColorBrush;
                if (brush.Color == Colors.White)
                {
                    Text1.Content = Module1.X.ToString();
                    Text3.Content = Module1.ANS.ToString();
                    Label5.Content = Module1.kakeyomikata[Module1.X, Module1.Y];
                    Label6.Content = Module1.kakeyomikatakotae[Module1.X, Module1.Y];
                    Label9.Content = Module1.kakeyomikata[Module1.X, Module1.Y];
                }
                else
                {
                    Text2.Content = Module1.Y.ToString();
                    Text3.Content = Module1.ANS.ToString();
                    Label5.Content = Module1.kakeyomikata[Module1.X, Module1.Y];
                    Label6.Content = Module1.kakeyomikatakotae[Module1.X, Module1.Y];
                    Label9.Content = Module1.kakeyomikata[Module1.X, Module1.Y];
                }
            }

            Module1.anaumekakunin = false;
        }

        private void Page_Unloaded_1(object sender, RoutedEventArgs e)
        {
            window.KeyDown -= HandleKeyDown;
        }

        private void Command2_Click(object sender, RoutedEventArgs e)
        {
            if (_timer1 != null && _timer1.IsEnabled)
            {
                _timer1.Stop();
            }

            var res = MessageBox.Show("ふくしゅうモードをおわります。おわってもよいですか？", "確認", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes)
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Module1.huseikaidankakunin[i, j] = false;
                    }
                }
                Module1.mistake_count = 0;

                // おわりページに遷移する
                NavigationService.Navigate(new FinishPage());

            }
            else
            {
                if (_timer1 != null && _timer1.IsEnabled)
                {
                    _timer1.Start();
                }
            }
        }
    }
}
