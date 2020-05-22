namespace KUKUTAN
{
    class Module1
    {
        public const short SEC_MID = 240; // 時間（残り時間テキストカラー変更１)
        public const short SEC_ALART = 290; // 時間（残り時間テキストカラー変更２）
        public const short softversion = 0; // ソフトのバージョン　0:保存なし、1:保存あり（1人）、2:保存あり（50人）

        public static short mojisuu = 0; // 入力もじ数をカウント

        //[System.Runtime.InteropServices.DllImport("winmm.dll")]
        //public static extern int sndPlaySound(string lpszSoundName, int uFlags); // VBのバージョンが4.0以上の時

        // Public FontType As Short 'フォント種類　0:そろふぉんと、1:ゴシック

        public static short count_down = 0; // 出題前のカウントダウンで使うタイマーのカウント
        public static short syutu_count = 0; // 出題中の残り時間で使うタイマーのカウント
        public static short collect_count = 0; // 正解数のカウント
        public static short mistake_count = 0; // 不正解数のカウント
        public static short mondai_count = 0; // 問題数のカウント
        public static short mcount = 0; // 答え入力時のもじ数のカウント
        public static short X = 0; // 掛けられる数
        public static short Y = 0; // 掛ける数
        public static short Z = 0;
        public static short ANS = 0; // 正解値
        public static bool kidou = false;

        public static short keishiki = 0; // 問題表示形式
                                          // 0:すうじ＋もじ＋こたえ
                                          // 1:すうじ＋もじ
                                          // 2:もじのみ
                                          // 3:すうじのみ
                                          // 4:段テスト
                                          // 5:８１問テストチャレンジ
                                          // 6:穴埋
                                          // 7:実力判定
                                          // 8:ふくしゅうモード

        public static bool review_mode = false; // ふくしゅうモード

        public static short dan = 0; // 段：１～９ 10はランダム
        public static short zyunzyo = 0; // 出題じゅんじょ　0:じゅんばん、1:ぎゃくじゅん　,2:ランダム
        public static string[,] kakeyomikata = new string[10, 10]; // かけざんの読み方
        public static string[,] kakeyomikatakotae = new string[10, 10]; // かけざんの読み方の答
        public static short[,] challengezyunban = new short[10, 10]; // challengeの際のじゅんばん
        public static bool huseikaicheck = false; // 間違えがあったか確認
        public static bool[,] huseikaidankakunin = new bool[10, 10]; // 不正解の段を確認
        public static short SEC = 0; // 出題制限時間
        public static short syutsudaimondaisu = 0; // 出題問題数設定
        public static short[] hukusyudatami = new short[21];
        public static short[] hukusyudatahou = new short[21]; // ふくしゅうする段の実と法を保存する
        public static short sokudo = 0; // 0:ゆっくり 1:ふつう 2:オリジナル
        public static short matigaikaisu = 0; // 問題を間違えた回数を記録
        public static bool[] suuzikakunin = new bool[10]; // 使用したすうじを確認
        public static short[] matigaihou = new short[244];
        public static short[] matigaimi = new short[244];
        public static short matigaicount = 0;
        public static short hyouzicount = 0; // ふくしゅうで出題する問題を選択
        public static bool[,] siyousuuzikakunin = new bool[10, 10]; // 出題したすうじを確認
        public static bool anaumekakunin = false; // あなうめの場合ふくしゅう機能が変更になるため確認する
    }
}
        