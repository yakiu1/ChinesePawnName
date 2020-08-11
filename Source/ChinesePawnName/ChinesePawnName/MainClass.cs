using ChinesePawnName.ChineseNames;
using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ChinesePawnName
{
    [StaticConstructorOnStartup]
    public class PatchMain
    {
        public static Harmony instance;
        static PatchMain()
        {
            Log.Message("中文名稱Mod加載完成");
            instance = new Harmony("Gon.ChinesePawnName");
            instance.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(PawnBioAndNameGenerator), "TryGetRandomUnusedSolidName")]
    class QuestUtility_Patch2
    {
        [HarmonyPrefix]
        static bool Prefix(ref Verse.Name __result, Gender gender, string requiredLastName = null)
        {
            FemaleNames nameGenF = new FemaleNames();
            MaleNames nameGenM = new MaleNames();
            "Harvest".Translate();
            if (gender == Gender.Female)
            {
                __result = nameGenF.GetChineseFemaleName(null);
            }
            else
            {
                __result = nameGenM.GetChineseMaleName(null);
            }

            // 用Find去找資料 Find.CurrentMap
            return false;
        }
    }

    [HarmonyPatch(typeof(PawnBioAndNameGenerator), "GeneratePawnName")]
    class QuestUtility_Patch3
    {
        [HarmonyPrefix]
        static bool Prefix(ref Verse.Name __result, Pawn pawn, NameStyle style = NameStyle.Full, string forcedLastName = null)
        {
            FemaleNames nameGenF = new FemaleNames();
            MaleNames nameGenM = new MaleNames();
            NameTriple temp = pawn.Name as NameTriple;


            if (pawn.RaceProps.Animal && pawn.Name == null)
            {
                //這裡就走原生的路線
                // __result = new NameTriple("小","動","物") ;
                return true;
            }

            if (pawn.gender == Gender.Female)
            {
                __result = nameGenF.GetChineseFemaleName(pawn);
            }
            else
            {
                __result = nameGenM.GetChineseMaleName(pawn);
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(PawnUtility), "GiveNameBecauseOfNuzzle")]
    class GonGiveNameBecauseOfNuzzle
    {
        [HarmonyPrefix]
        static bool Prefix(Pawn namer, Pawn namee)
        {
            FemaleNames nameGenF = new FemaleNames();
            MaleNames nameGenM = new MaleNames();

            string value = (namee.Name == null) ? namee.LabelIndefinite() : namee.Name.ToStringFull;
            NameTriple tempName = namee.gender == Gender.Male ? nameGenM.GetChineseMaleName(namee) : nameGenF.GetChineseFemaleName(namee);

            namee.Name = new NameSingle(tempName.Nick);
            if (namer.Faction == Faction.OfPlayer)
            {
                Messages.Message("MessageNuzzledPawnGaveNameTo".Translate(namer.Named("NAMER"), value, namee.Name.ToStringFull, namee.Named("NAMEE")), namee, MessageTypeDefOf.NeutralEvent);
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(NameTriple), "ToStringFull", MethodType.Getter)]
    class GonGenChineseNameLabel
    {
        [HarmonyPrefix]
        static bool GetPawnLabel(ref string __result, ref NameTriple __instance)
        {
            __result = __instance.Last + __instance.First + "(" + __instance.Nick + ")";
            return false;
        }
    }

    //[HarmonyPatch(typeof(ITab_Pawn_Character), "FillTab")]
    //class GonGenChineseNameLabel2
    //{
    //    [HarmonyPrefix]
    //    static bool GetPawnLabel(ref Rect rect, Pawn ___PawnToShowInfoAbout, ITab_Pawn_Character __instance)
    //    {
    //             //  RimWorld.ITab_Pawn_Character
    //        Vector2 vector = CharacterCardUtility.PawnCardSize(___PawnToShowInfoAbout);
    //        Pawn temp = ___PawnToShowInfoAbout;

    //        ITab_ContentsBase

    //        CharacterCardUtility.DrawCharacterCard(new Rect(17f, 17f, vector.x, vector.y), temp);
    //        return false;
    //    }
    //  }
}
