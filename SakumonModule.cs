using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;

namespace KUKUTAN
{
    class SakumonModule
    {
        public static short[,] mondai = new short[3, 3];
        public static void Sakumon() // 作問
        {
            switch (Module1.keishiki)
            {
                case 5: // 81問チャレンジ問題
                    {
                        switch (Module1.zyunzyo) // じゅんじょで場合分け
                        {
                            case 0:
                                {
                                    challengezyunbangyakuzyun();
                                    break;
                                }

                            case 1:
                                {
                                    challengezyunbangyakuzyun();
                                    break;
                                }

                            case 2:
                                {
                                    challengerandom();
                                    break;
                                }
                        }

                        break;
                    }

                case 7 // 実力判定
         :
                    {
                        challengerandom();
                        break;
                    }

                default:
                    {
                        switch (Module1.dan)
                        {
                            case 10:
                                {
                                    danrandomsuuzi(); // 段がランダムの場合(あなうめ)
                                    break;
                                }

                            default:
                                {
                                    switch (Module1.zyunzyo) // じゅんじょで場合分け
                                    {
                                        case 0:
                                            {
                                                zyunbangyakuzyun();
                                                break;
                                            }

                                        case 1:
                                            {
                                                zyunbangyakuzyun();
                                                break;
                                            }

                                        case 2:
                                            {
                                                zyunbanrandom();
                                                break;
                                            }
                                    }

                                    break;
                                }
                        }

                        break;
                    }
            }
        }
        private static void challengezyunbangyakuzyun() // チャレンジじゅんばんぎゃくじゅんの場合
        {
            // 問題生成処理
            // Dim xxx, xx As Object
            // Dim yy As Short
            if (Module1.X > 9)
                Module1.X = 0;

            switch (Module1.zyunzyo) // じゅんじょで場合分け
            {
                case 0:
                    {
                        if (Module1.X == 0)
                            Module1.X = 1; // 初回のみ１から始める
                        if (Module1.Y == 9)
                        {
                            Module1.Y = 0;
                            Module1.X = (short)(Module1.X + 1);
                        }
                        Module1.Y = (short)(Module1.Y + 1);
                        break;
                    }

                case 1:
                    {
                        if (Module1.X == 0)
                            Module1.X = 10; // 初回のみ１から始める
                        if (Module1.Y < 2)
                        {
                            Module1.Y = 10;
                            Module1.X = (short)(Module1.X - 1);
                        }
                        Module1.Y = (short)(Module1.Y - 1);
                        break;
                    }
            }

            Module1.ANS = (short)(Module1.X * Module1.Y);
        }

        private static void challengerandom() // チャレンジrandomの場合
        {
            // 問題生成処理
            short p;
            short q;
            Random rnd = new System.Random();
            do
            {
                p = (short)(rnd.Next(9) + 1);
                q = (short)(rnd.Next(9) + 1);
            }
            while (Module1.challengezyunban[p, q] != Module1.mondai_count);
            Module1.X = p;
            Module1.Y = q;
            Module1.ANS = (short)(Module1.X * Module1.Y);
        }

        private static void danrandomsuuzi()
        {
            short t;
            short i;
            short c; // ランダムの場合
                     // 問題生成処理
            short xx;
            short yy;
            c = 0;
            for (i = 1; i <= 9; i++)
            {
                for (t = 1; t <= 9; t++)
                {
                    if (Module1.siyousuuzikakunin[i, t] == false)
                        c = 1;
                }
            }

            if (c == 0)
            {
                for (i = 1; i <= 9; i++)
                {
                    for (t = 1; t <= 9; t++)
                        Module1.siyousuuzikakunin[i, t] = false;
                }
            }

            Random rnd = new System.Random();
            do
            {
                xx = (short)(rnd.Next(9) + 1);
                yy = (short)(rnd.Next(9) + 1);
            }
            while (Module1.siyousuuzikakunin[xx, yy] != false);

            Module1.siyousuuzikakunin[xx, yy] = true;
            Module1.X = xx;
            Module1.Y = yy;
            Module1.ANS = (short)(Module1.X * Module1.Y);
        }
        private static void zyunbangyakuzyun() // zyunban
        {
            // 問題生成処理

            switch (Module1.zyunzyo) // じゅんじょで場合分け
            {
                case 0:
                    {
                        if (Module1.Y == 9)
                            Module1.Y = 0;
                        Module1.Y = (short)(Module1.Y + 1);
                        break;
                    }

                case 1:
                    {
                        if (Module1.Y < 2)
                            Module1.Y = 10;
                        Module1.Y = (short)(Module1.Y - 1);
                        break;
                    }
            }

            Module1.X = Module1.dan;

            Module1.ANS = (short)(Module1.X * Module1.Y);
        }


        private static void zyunbanrandom()
        {
            // 問題生成処理
            short c, i;
            short yy;
            c = 0;
            for (i = 1; i <= 9; i++)
            {
                if (Module1.suuzikakunin[i] == false)
                    c = 1;
            }

            if (c == 0)
            {
                for (i = 1; i <= 9; i++)
                    Module1.suuzikakunin[i] = false;
            }
            Random rnd = new System.Random();
            do
            {
                yy = (short)(rnd.Next(9) + 1);
            }
            while (Module1.Y == yy || Module1.suuzikakunin[yy] == true);
            Module1.suuzikakunin[yy] = true;
            Module1.X = Module1.dan;
            Module1.Y = yy;
            Module1.ANS = (short)(Module1.X * Module1.Y);
        }
    }
}
