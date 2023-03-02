using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld;
using Unity.Jobs;
using UnityEngine;
using Verse;
using Verse.AI;

namespace AutoNutrientPasteCommand
{
    static partial class HarmonyPatches
    {
        [HarmonyPatch(typeof(Building_NutrientPasteDispenser))]
        [HarmonyPatch("GetGizmos")]
        [HarmonyPatch(new Type[] {})]
        [HarmonyPostfix]
        private static IEnumerable<Gizmo> DispenserGizmoPatch(IEnumerable<Gizmo> value, Building_NutrientPasteDispenser __instance)
        {
            Command_Toggle gizmoToggle = new Command_Toggle();
            gizmoToggle.icon = Startup.gizmoIcon;
            gizmoToggle.defaultLabel = "AutoMakeNutrientPasteLabel".Translate();
            gizmoToggle.defaultDesc = "AutoMakeNutrientPasteDesc".Translate();
            gizmoToggle.isActive = () => AutoNutrientPasteManager.Main.IsActive(__instance);
            gizmoToggle.toggleAction = delegate
            {
                AutoNutrientPasteManager.Main.ToggleActive(__instance);
            };
            yield return gizmoToggle;

            foreach (Gizmo item in value)
            {
                yield return item;
            }
        }
    }
}