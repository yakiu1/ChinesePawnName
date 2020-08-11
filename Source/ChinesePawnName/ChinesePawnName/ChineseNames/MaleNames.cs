using ChinesePawnName.ChineseLib;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ChinesePawnName.ChineseNames
{
    class MaleNames
    {
        public string[] 姓 = "AllFamilyNames".Translate().ToString().Split(',');

        public string[] 名 = "AllMaleNameWord".Translate().ToString().Split(',');

        public NameTriple GetChineseMaleName(Pawn pawn)
        {
            // RimWorld.PawnColumnWorker_Label
            //var re1 = new RegExp("^[\u4E00-\\u9fa5]*$")     //漢字的範圍
            int allChineseChar = 0x9fa5 - 0x4E00;

            var nameRand = new Random(Guid.NewGuid().GetHashCode());

            string randNameChar = Verse.Rand.Value > 0.1f ?
                NormalWords.AllNormalWords[nameRand.Next(NormalWords.AllNormalWords.Length)] :
                UnicodeToString(@"\u" + Convert.ToString(0x4E00 + nameRand.Next(allChineseChar), 16));

            string[] 所有男姓 = new MaleNames().姓;

            string[] 所有男名稱 = new MaleNames().名;

            string 姓 = "";
            string 名 = "";

            var rand = new Random(Guid.NewGuid().GetHashCode());

            名 = Verse.Rand.Value > 0.5f ?
                所有男名稱[rand.Next(所有男名稱.Length)].Substring(0, 1) + randNameChar
                : randNameChar + 所有男名稱[rand.Next(所有男名稱.Length)].Substring(0, 1);



            姓 = 所有男姓[rand.Next(所有男姓.Length)];


            if (Verse.Rand.Value < 0.7f)
            {

                if (Verse.Rand.Value < 0.6f)
                {
                    return NickNameGenerator.GetNormalNickName(new Verse.NameTriple(名, "", 姓), Gender.Male, pawn);
                }
                else if (Verse.Rand.Value < 0.7f)
                {
                    return NickNameGenerator.GetStackNickName(new Verse.NameTriple(名, "", 姓), Gender.Male);
                }
                if (Verse.Rand.Value < 0.74f)
                {
                    NameTriple temp = NickNameGenerator.GetNormalNickName(new Verse.NameTriple(名, "", 姓), Gender.Male, pawn);
                    return new NameTriple(temp.First, temp.First + NickNameGenerator.GetPawnOwnNickName(pawn), temp.Last);
                }
                else
                {
                    NameTriple temp = NickNameGenerator.GetNormalNickName(new Verse.NameTriple(名, "", 姓), Gender.Male, pawn);
                    return new NameTriple(temp.First, NickNameGenerator.GetAnywayNickName姓(Gender.Male) + 姓, temp.Last); ;
                }
            }
            else
            {
                return new Verse.NameTriple(名, 名, 姓);
            }

        }

        private string UnicodeToString(string srcText)
        {
            string dst = "";
            string src = srcText;
            int len = srcText.Length / 6;

            for (int i = 0; i <= len - 1; i++)
            {
                string str = "";
                str = src.Substring(0, 6).Substring(2);
                src = src.Substring(6);
                byte[] bytes = new byte[2];
                bytes[1] = byte.Parse(int.Parse(str.Substring(0, 2), System.Globalization.NumberStyles.HexNumber).ToString());
                bytes[0] = byte.Parse(int.Parse(str.Substring(2, 2), System.Globalization.NumberStyles.HexNumber).ToString());
                dst += Encoding.Unicode.GetString(bytes);
            }
            return dst;
        }
    }
}
