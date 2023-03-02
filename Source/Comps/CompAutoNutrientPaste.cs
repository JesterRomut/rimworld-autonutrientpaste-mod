using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;
using UnityEngine.Diagnostics;
using Verse;

namespace AutoNutrientPasteCommand
{
    public class AutoNutrientPasteManager: GameComponent
    {
        private static AutoNutrientPasteManager _instance;

        public List<Building_NutrientPasteDispenser> activeDispensers = new List<Building_NutrientPasteDispenser>();

        public AutoNutrientPasteManager()
        {
            _instance = this;
        }

        public AutoNutrientPasteManager(Game game)
        : this()
        {
        }

        public static AutoNutrientPasteManager Main
        {
            get
            {
                if (_instance == null)
                {
                    throw new NullReferenceException("Accessing AutoNutrientPasteManager before it was constructed.");
                }
                return _instance;
            }
        }

        public bool IsActive(Building_NutrientPasteDispenser dispenser) => activeDispensers.Contains(dispenser);

        public void PurgeActives()
        {
            activeDispensers.RemoveAll((Building_NutrientPasteDispenser d) => d.DestroyedOrNull());
        }

        public void MakeActive(Building_NutrientPasteDispenser dispenser)
        {
            PurgeActives();
            activeDispensers.Add(dispenser);
        }

        public void MakeInactive(Building_NutrientPasteDispenser dispenser)
        {
            PurgeActives();
            activeDispensers.Remove(dispenser);
        }

        public void ToggleActive(Building_NutrientPasteDispenser dispenser)
        {
            if (IsActive(dispenser))
            {
                MakeInactive(dispenser);
            }
            else
            {
                MakeActive(dispenser);
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref activeDispensers, "activeDispensers", LookMode.Reference, this);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                if (activeDispensers == null)
                {
                    activeDispensers = new List<Building_NutrientPasteDispenser>();
                }
            }
        }

        public void DispenseAll(Building_NutrientPasteDispenser dispenser)
        {
            while (true)
            {
                Thing thing = dispenser.TryDispenseFood();
                if (thing == null)
                {
                    break;
                }
                if (!GenPlace.TryPlaceThing(thing, dispenser.InteractionCell, dispenser.Map, ThingPlaceMode.Near))
                {
                    break;
                }
            }
        }

        public override void GameComponentTick()
        {
            base.GameComponentTick();

            foreach(Building_NutrientPasteDispenser dispenser in activeDispensers) {
                if (!dispenser.CanDispenseNow)
                {
                    continue;
                }

                DispenseAll(dispenser);
            }
        }
    }
}
