using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KUKUTAN
{
	/// <summary>
	/// SecondPage.xaml の相互作用ロジック
	/// </summary>
	public partial class SecondPage : Page
    {
        public SecondPage()
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

        private void start_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CountDownPage(new SyutudaiPage()));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
			// 設定ファイルから値を読み込む
			short[] byou = new short[10] { 135, 150, 120, 90, 90, 90, 90, 243, 90, 180 };
			short[] mondaisu = new short[10] { 27, 27, 27, 27, 27, 27, 27, 81, 27, 81 };
			// 読み込みたいCSVファイルのパスを指定して開く
			using (StreamReader sr = new StreamReader(@"settei.csv"))
			{
				int i = 0;
				// 末尾まで繰り返す
				while (!sr.EndOfStream)
				{
					// CSVファイルの一行を読み込む
					string line = sr.ReadLine();
					// "" を取り除く
					line = line.Replace("\"", "");
					// 読み込んだ一行をカンマ毎に分けて配列に格納する
					string[] values = line.Split(',');
					// 問題数を読み込む
					if (values.Length >= 1)
					{
						mondaisu[i] = short.Parse(values[0]);
					}
					// 秒数を読み込む
					if (values.Length >= 2)
					{
						byou[i] = short.Parse(values[1]);
					}
					i++;
				}
			}

			int t = 0;
            switch (Module1.keishiki)
            {
				case 0:
					keishiki_label.Content = "①れんしゅう　（もじ＋すうじ＋こたえ）";
					t = 0;
					break;
				case 1:
					switch (Module1.sokudo)
					{
						case 0:
							keishiki_label.Content = "②れんしゅう　もじ＋すうじ（ゆっくり）";
							t = 1;
							break;
						case 1:
							keishiki_label.Content = "②れんしゅう　もじ＋すうじ（ふつう）";
							t = 2;
							break;
						case 2:
							keishiki_label.Content = "②れんしゅう　もじ＋すうじ（オリジナル）";
							t = 3;
							break;
					}
					break;
				case 2:
					keishiki_label.Content = "③れんしゅう　もじ";
					t = 4;
					break;
				case 3:
					keishiki_label.Content = "④れんしゅう　すうじ";
					t = 5;
					break;
				case 4:
					keishiki_label.Content = "⑤段ごとのテスト";
					t = 6;
					break;
				case 5:
					keishiki_label.Content = "⑥テスト　81問チャレンジ";
					t = 7;
					break;
				case 6:
					keishiki_label.Content = "⑦あなうめ";
					if (Module1.dan != 10)
					{
						t = 8;
					}
					else
					{
						t = 9;
					}
					break;
				case 7:
					keishiki_label.Content = "実力判定モード";
					break;
				case 8:
					keishiki_label.Content = "ふくしゅうモード";
					break;
			}

			// 秒数を確定する
			if (Module1.review_mode)
			{
				Module1.syutsudaimondaisu = Module1.matigaicount;
				Module1.SEC = (short)(Module1.syutsudaimondaisu * 10);
				if (Module1.SEC > 999)
				{
					Module1.SEC = 999;
				}
			}
			else if (Module1.keishiki == 7)
			{
				Module1.syutsudaimondaisu = 81;
				Module1.SEC = 600;
			}
			else if (Module1.keishiki == 8)
			{
				for (int i = 0; i < Module1.hukusyudatami.Length; i++)
				{
					Module1.hukusyudatami[i] = 0;
				}
			}
			else
			{
				Module1.SEC = byou[t];
			}

			switch (Module1.dan)
			{
				case 10:
					dan_label.Content = "段：ランダム";
					break;
				default:
					dan_label.Content = Module1.dan + "の段";
					break;
			}

			switch (Module1.zyunzyo)
			{
				case 0:
					zyunzyo_label.Content = "じゅんじょ：じゅんばん";
					break;
				case 1:
					zyunzyo_label.Content = "じゅんじょ：ぎゃくじゅん";
					break;
				case 2:
					zyunzyo_label.Content = "じゅんじょ：ランダム";
					break;
			}

			if (Module1.keishiki == 4)
			{
				zyunzyo_label.Content = "-";
			}

			if (Module1.keishiki == 5)
			{
				dan_label.Content = "-";
			}

			if (Module1.review_mode)
			{
				dan_label.Content = "ふくしゅうモード";
				zyunzyo_label.Content = "-";
			}

			if (Module1.keishiki == 7 || Module1.keishiki == 8)
			{
				dan_label.Content = "-";
				zyunzyo_label.Content = "-";
			}

			if (Module1.review_mode)
			{
				backbutton.Visibility = Visibility.Hidden;
			}
			else
			{
				backbutton.Visibility = Visibility.Visible;
			}

			// 間違えた問題の初期化
			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < 10; j++)
				{
					Module1.huseikaidankakunin[i, j] = false;
				}
			}
		}
	}
}
