using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using RimWorld;
using Verse;

namespace GT_Animation
{
    public class Graphic_Animated : Graphic_Collection
    {
        private int currentFrame = 0;

        private int ticksPerFrame = 15;

        private int ticksPrev = 0;

        private bool randomized = false;

        private bool initFromComps = false;

        public override Material MatSingle
        {
            get
            {
                return this.subGraphics[currentFrame].MatSingle;
            }
        }

        public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
        {
            if(thingDef == null)
            {
                Log.Error("Graphic_Animated with null thingDef", false);
                return;
            }
            if(this.subGraphics == null)
            {
                Log.Error("Graphic_Animated has no subgraphics", false);
                return;
            }
            if (thingDef.HasComp(typeof(CompGTAnimation)) && !this.initFromComps)
            {
                CompGTAnimation comp = thing.TryGetComp<CompGTAnimation>();
                this.ticksPerFrame = comp.Props.frameSpeed;
                this.randomized = comp.Props.randomized;
                this.initFromComps = true;
            }
            int ticksCurrent = Find.TickManager.TicksGame;
            if(ticksCurrent >= this.ticksPrev + this.ticksPerFrame)
            {
                this.ticksPrev = ticksCurrent;
                if (this.randomized)
                {
                    this.currentFrame = Rand.Range(0, this.subGraphics.Length - 1);
                }
                else
                {
                    this.currentFrame++;
                }
            }
            if(this.currentFrame >= this.subGraphics.Length)
            {
                this.currentFrame = 0;
            }

            Graphic graphic = this.subGraphics[this.currentFrame];
            Graphics.DrawMesh(MeshPool.plane10, loc, rot.AsQuat, graphic.MatSingle, 0);
        }
    }
}
