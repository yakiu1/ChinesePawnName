using ChinesePawnName.ChineseNames;
using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;

namespace ChinesePawnName
{
    [StaticConstructorOnStartup]
    public class PatchMain
    {
        public static Harmony instance;
        static PatchMain()
        {
            instance = new Harmony("Gon.ChinesePawnName");
            instance.PatchAll(Assembly.GetExecutingAssembly());
            Log.Message("中文名稱Mod加載完成");
        }
    }

    [HarmonyPatch(typeof(PawnBioAndNameGenerator), "TryGetRandomUnusedSolidName")]
    class QuestUtility_Patch2
    {
        [HarmonyPrefix]
        static bool Prefix(ref Verse.Name __result, Gender gender, string requiredLastName = null)
        {
            Log.Message("patch2");
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
            Log.Message("patch3");
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

    //[HarmonyPatch(typeof(PawnUtility), "GiveNameBecauseOfNuzzle")]
    //class GonGiveNameBecauseOfNuzzle
    //{
    //    [HarmonyPrefix]
    //    static bool Prefix(Pawn namer, Pawn namee)
    //    {
    //        Log.Message("patch4");
    //        FemaleNames nameGenF = new FemaleNames();
    //        MaleNames nameGenM = new MaleNames();

    //        string value = (namee.Name == null) ? namee.LabelIndefinite() : namee.Name.ToStringFull;
    //        NameTriple tempName = namee.gender == Gender.Male ? nameGenM.GetChineseMaleName(namee) : nameGenF.GetChineseFemaleName(namee);

    //        namee.Name = new NameSingle(tempName.Nick);
    //        if (namer.Faction == Faction.OfPlayer)
    //        {
    //            Messages.Message("MessageNuzzledPawnGaveNameTo".Translate(namer.Named("NAMER"), value, namee.Name.ToStringFull, namee.Named("NAMEE")), namee, MessageTypeDefOf.NeutralEvent);
    //        }
    //        return false;
    //    }
    //}

    [HarmonyPatch(typeof(NameTriple), "ToStringFull", MethodType.Getter)]
    class GonGenChineseNameLabel
    {
        [HarmonyPrefix]
        static bool GetPawnLabel(ref string __result, ref NameTriple __instance)
        {
            Log.Message("patch5");
            __result = __instance.Last + __instance.First + "(" + __instance.Nick + ")";
            return false;
        }
    }
}
