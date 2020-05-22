using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KUKUTAN
{
    /// <summary>
    /// KekkaPage.xaml の相互作用ロジック
    /// </summary>
    public partial class KekkaPage : Page
    {
        public KekkaPage()
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
			short t, i;

			Label3.Content = string.Format("{0}秒", Module1.syutu_count - 1);
			if (Module1.review_mode == true)
			{
				Label13.Visibility = Visibility.Hidden;
				Label14.Visibility = Visibility.Hidden;
				Label15.Visibility = Visibility.Hidden;
			}
			else if (Module1.mondai_count - Module1.mistake_count == Module1.syutsudaimondaisu + 1) // 成績を確認して合格を表示する
			{
				Label13.Visibility = Visibility.Visible;
				Label14.Visibility = Visibility.Hidden;
				Label15.Visibility = Visibility.Hidden;
			}
			else if (Module1.mondai_count - Module1.mistake_count > Module1.syutsudaimondaisu + 1 - 4)
			{
				Label13.Visibility = Visibility.Hidden;
				Label14.Visibility = Visibility.Visible;
				Label15.Visibility = Visibility.Hidden;
			}
			else
			{
				Label13.Visibility = Visibility.Hidden;
				Label14.Visibility = Visibility.Hidden;
				Label15.Visibility = Visibility.Visible;
			}


			Command3.Visibility = Visibility.Hidden;
			Module1.huseikaicheck = false;

            for (i = 1; i <= 9; i++)
            {
                for (t = 1; t <= 9; t++)
                {
                    if (Module1.huseikaidankakunin[i, t] == true)
                    {
                        Command3.Visibility = Visibility.Visible;
                        Module1.huseikaicheck = true;
                        break;
                    }
                }
                Module1.suuzikakunin[i] = false;
            }

			if (Module1.softversion != 0)
			{
				if (Module1.huseikaicheck == true)
				{
					//mistakekakunin();
				}
			}


            switch (Module1.keishiki)
            {
                case 0:
                    {
                        Label5.Content = "①れんしゅう　（もじ＋すうじ＋こたえ）";
                        break;
                    }
                case 1:
                    {
                        switch (Module1.sokudo)
                        {
                            case 0:
                                {
                                    Label5.Content = "②れんしゅう　もじ＋すうじ（ゆっくり）";
                                    t = 2;
                                    break;
                                }

                            case 1:
                                {
                                    Label5.Content = "②れんしゅう　もじ＋すうじ（ふつう）";
                                    t = 3;
                                    break;
                                }

                            case 2:
                                {
                                    Label5.Content = "②れんしゅう　もじ＋すうじ（オリジナル）";
                                    t = 4;
                                    break;
                                }
                        }
                        break;
                    }
                case 2:
                    {
                        Label5.Content = "③れんしゅう　もじ";
                        break;
                    }
                case 3:
                    {
                        Label5.Content = "④れんしゅう　すうじ";
                        break;
                    }
                case 4:
                    {
                        Label5.Content = "⑤段ごとのテスト";
                        break;
                    }
                case 5:
                    {
                        Label5.Content = "⑥テスト　81問チャレンジ";
                        break;
                    }
                case 6:
                    {
                        Label5.Content = "⑦あなうめ";
                        break;
                    }
                case 7:
                    {
                        Label5.Content = "実力判定モード";
                        break;
                    }
                case 8:
                    {
                        Label5.Content = "ふくしゅうモード";
                        break;
                    }
            }

            switch (Module1.dan)
            {
                case 10:
                    {
                        Label9.Content = "段：ランダム";
                        break;
                    }

                default:
                    {
                        Label9.Content = Module1.dan + "の段";
                        break;
                    }
            }

            switch (Module1.zyunzyo)
            {
                case 0:
                    {
                        Label10.Content = "じゅんじょ：じゅんばん";
                        break;
                    }

                case 1:
                    {
                        Label10.Content = "じゅんじょ：ぎゃくじゅん";
                        break;
                    }

                case 2:
                    {
                        Label10.Content = "じゅんじょ：ランダム";
                        break;
                    }
            }

            if (Module1.keishiki == 4)
                Label10.Content = "-";

            if (Module1.keishiki == 5)
                Label9.Content = "-";

            if (Module1.keishiki == 7)
            {
                Label9.Content = "-";
                Label10.Content = "-";
            }

            if (Module1.review_mode == true)
            {
                Label1.Content = "けっか（ふくしゅうモード）";
                Label6.Content = "-";
                Label11.Content = "-";
                Label18.Content = "-";
            }
            else
            {
                Label1.Content = "けっか";
                Label6.Content = Module1.collect_count;
                Label11.Content = Module1.syutsudaimondaisu + "問";
                Label18.Content = Module1.mistake_count;
            }

            if (Module1.softversion != 0)
            {
                //Call savedata();
                //Call sinkyukakunin();
                //Call zituryokuhantei
           }
        }

		private void Command2_Click(object sender, RoutedEventArgs e)
		{
            Module1.mondai_count = 0;
            Module1.collect_count = 0;
            Module1.mistake_count = 0;
            Module1.review_mode = false;
            for(int i = 1; i <= 9; i++)
            {
                for(int t = 1; t <= 9; t++)
                {
                    Module1.siyousuuzikakunin[i, t] = false;
                }
            }
            if( Module1.softversion == 0)
            {
                while (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            }
            else
            {
                // 登録ページを表示
                //TourokuPage.Show()
            }
        }

        private void Command3_Click(object sender, RoutedEventArgs e)
		{
            Module1.matigaicount = 0;

            for (int s = 1; s <= 3; s++)
            {
                for (short i = 1; i <= 9; i++)
                {
                    for (short t = 1; t <= 9; t++)
                    {
                        if (Module1.huseikaidankakunin[i, t] == true)
                        {
                            Module1.matigaicount++;
                            Module1.matigaimi[Module1.matigaicount] = i;
                            Module1.matigaihou[Module1.matigaicount] = t;
                        }
                    }
                }
            }

            Module1.review_mode = true;

            while (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
            NavigationService.Navigate(new SecondPage());



            /*

                    Dim i, t As Object
                    Dim s As Short


                    matigaicount = 0

                    For s = 1 To 3
                        For i = 1 To 9
                            For t = 1 To 9
                                If huseikaidankakunin(i, t) = True Then
                                    matigaicount = matigaicount + 1
                                    matigaimi(matigaicount) = i
                                    matigaihou(matigaicount) = t
                                End If
                            Next 
                        Next 
                    Next 

                    review_mode = True
                    SecondPage.Show()
                    Me.Close()


            */
        }
    }
}
