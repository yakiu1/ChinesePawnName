using ChinesePawnName.ChineseLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ChinesePawnName
{
    class NickNameGenerator
    {

        /// <summary>
        /// 前後綴點綴產生暱稱
        /// </summary>
        /// <param name="name">角色原名</param>
        /// <param name="gender">角色性別</param>
        /// <returns></returns>
        public static Verse.NameTriple GetNormalNickName(Verse.NameTriple name, Gender gender = Gender.None, Pawn pawn = null)
        {

            string 小名 = "";
            string 前綴 = "";
            string 後綴 = "";

            #region 名稱產生
            // 前後規則 
            // 前綴稱號 小、阿、大
            // 後綴稱號 哥or姐 弟or妹
            // 寵物就直接用前綴了
            if (Verse.Rand.Value >= 0.5f || pawn.RaceProps.Animal)
            {
                if (Verse.Rand.Value >= 0.66f)
                {
                    前綴 = "小";
                }
                else if (Verse.Rand.Value >= 0.33f)
                {
                    前綴 = "阿";
                }
                else
                {
                    前綴 = "大";
                }

            }
            else
            {

                if (gender == Gender.Female)
                {
                    if (Verse.Rand.Value >= 0.5f)
                    {

                        後綴 = "姐";
                    }
                    else
                    {

                        後綴 = "妹";
                    }

                }
                else
                {
                    if (Verse.Rand.Value >= 0.5f)
                    {

                        後綴 = "哥";
                    }
                    else if (Verse.Rand.Value >= 0.3f)
                    {
                        後綴 = "弟";
                    }
                    else
                    {
                        後綴 = "狗";
                    }
                }
            }
            // 取姓取名則一
            // 取姓
            // 取名
            if (Verse.Rand.Value >= 0.5f)
            {
                小名 = 前綴 + name.Last + 後綴;
            }
            else
            {
                Random rand = new Random();
                int cut = rand.Next(0, 2);
                小名 = 前綴 + name.First.Substring(cut, 1) + 後綴;
            }
            #endregion

            return new Verse.NameTriple(name.First, 小名, name.Last);
        }

        // 取得疊字小名
        public static Verse.NameTriple GetStackNickName(Verse.NameTriple name, Gender gender = Gender.None)
        {
            string 小名 = "";
            if (Verse.Rand.Value >= 0.5f)
            {
                小名 = name.First.Substring(0, 1) + name.First.Substring(0, 1);
            }
            else
            {
                小名 = name.First.Substring(1, 1) + name.First.Substring(1, 1);
            }
            return new Verse.NameTriple(name.First, 小名, name.Last);
        }

        // 取得人物特性綴詞
        public static string GetPawnOwnNickName(Pawn pawn)
        {
            string result = "";

            if (pawn.ageTracker.AgeBiologicalYears >= 50 && pawn.gender == Gender.Male)
            {
                result = Verse.Rand.Value > 0.2f ? "老頭" : "老伯";
            }
            if (pawn.ageTracker.AgeBiologicalYears >= 50 && pawn.gender == Gender.Female)
            {
                result = Verse.Rand.Value > 0.2f ? "奶奶" : "老巫婆";
            }


            return result;
        }

        /// <summary>
        /// 只看性別直接給綽號
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
        public static string GetAnywayNickName姓(Gender gender)
        {
            string[] MaleNickName = "AllMaleAnyway".Translate().ToString().Split(',');
            string[] FemaleNickName = "AllFemaleAnyway".Translate().ToString().Split(',');

            if (gender == Gender.Female)
            {
                Random rand = new Random();
                return FemaleNickName[rand.Next(FemaleNickName.Length)];
            }
            else
            {
                Random rand = new Random();
                return MaleNickName[rand.Next(MaleNickName.Length)];
            }



        }



        //=======================以下尚開發中=====================
        // 取得諧音小名 不穩定 暫不使用
        // To Get homophonic NickName (But It's Proformace is not good), It's bad to foreach an unsort data.
        public static Verse.NameTriple GetHomophonicNickName(Verse.NameTriple name, Gender gender = Gender.None)
        {
            string 小名 = "";
            List<string> 男單詞庫 = new List<string> { "菊花", "博起", "華哥兒", "番薯", "芭樂",
                "小熊維尼", "鎖鏈殺手", "滷肉飯","師傅","屌炸天","小淫蟲","淫魔","送飯的","節節八八"};

            List<string> 女單詞庫 = new List<string> { "番薯", "芭樂", "辣台妹", "罡妹", "正咩", "北妻", "水餃", "冰淇淋", "滷肉飯", "隨意包盧肌考尻", "夏天妹", "和平使者", "便宜" };

            if (gender == Gender.Female)
            {
                女單詞庫.Shuffle();
                bool ok = false;
                女單詞庫.ForEach(word =>
                {
                    if (ok == false)
                    {
                        小名 = "";

                        // 單字中所有字詞 
                        List<string> wordWords = new List<string>();
                        for (int i = 0; i < word.Length; i++)
                        {
                            wordWords.Add(word.Substring(i, 1));
                        }
                        // 諧音轉換器
                        wordWords.ForEach(w =>
                        {
                            if (findWord(w, name.First.Substring(0, 1)))
                            {
                                小名 = 小名 + name.First.Substring(0, 1);
                                ok = true;
                                Log.Message(小名);
                            }
                            else if (findWord(w, name.First.Substring(1, 1)))
                            {
                                小名 = 小名 + name.First.Substring(1, 1);
                                ok = true;
                                Log.Message(小名);
                            }
                            else
                            {
                                小名 = 小名 + w;
                            }
                        });

                    }
                });


                return ok ? new Verse.NameTriple(name.First, 小名, name.Last) : new Verse.NameTriple(name.First, "", name.Last);

            }
            else
            {
                男單詞庫.Shuffle();
                bool ok = false;
                男單詞庫.ForEach(word =>
                {
                    if (ok == false)
                    {
                        小名 = "";

                        // 單字中所有字詞
                        List<string> wordWords = new List<string>();
                        for (int i = 0; i < word.Length; i++)
                        {
                            wordWords.Add(word.Substring(i, 1));
                        }
                        // 諧音轉換器
                        wordWords.ForEach(w =>
                        {
                            if (findWord(w, name.First.Substring(0, 1)))
                            {
                                小名 = 小名 + name.First.Substring(0, 1);
                                ok = true;
                                Log.Message(小名);
                            }
                            else if (findWord(w, name.First.Substring(1, 1)))
                            {
                                小名 = 小名 + name.First.Substring(1, 1);
                                ok = true;
                                Log.Message(小名);
                            }
                            else
                            {
                                小名 = 小名 + w;
                            }
                        });
                    }

                });
                return ok ? new Verse.NameTriple(name.First, 小名, name.Last) : new Verse.NameTriple(name.First, "", name.Last);
            }
        }


        private static bool findWord(string _string, string _string2)
        {
            string temp1 = "";
            string temp2 = "";

            // cuz some word in last will crash,so -20.
            for (int i = 0; i < AllChineseData.allChart.Length - 20; i++)
            {
                if (AllChineseData.allChart[i].Split('0')[0] == _string)
                {
                    temp1 = AllChineseData.allChart[i].Split('0')[1];
                }

                if (AllChineseData.allChart[i].Split('0')[0] == _string2)
                {
                    temp2 = AllChineseData.allChart[i].Split('0')[1];
                }
            }

            return temp1 == temp2;
        }

    }
}
