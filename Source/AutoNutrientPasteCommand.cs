using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace AutoNutrientPasteCommand
{
    [StaticConstructorOnStartup]
    public class Startup
    {
        public static readonly Texture2D gizmoIcon = ContentFinder<Texture2D>.Get("Things/Item/Meal/NutrientPaste/NutrientPaste_c");
        static Startup()
        {
            HarmonyPatches.Init();
        }
    }
}