using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace SGAmod
{
    public static class SGAUtils
    {
        public static Vector3 ToVector3(this Vector2 vector, bool keepz = false)
        {
            return new Vector3(vector.X, vector.Y, 0);
        }
        public static bool IsValidEnemy(this NPC npc)
        {
            return npc.active && !npc.dontTakeDamage && !npc.townNPC && !npc.friendly;
        }
        public static List<NPC> ClosestEnemies(Vector2 Center, float maxdist, Vector2 Center2 = default, List<Point> AddedWeight = default, bool checkWalls = true, bool checkCanChase = true)
        {
            maxdist *= maxdist;
            if (Center2 == default)
                Center2 = Center;
            if (AddedWeight == default)
                AddedWeight = new List<Point>();

            List<NPC> closetnpcs = new List<NPC>();
            for (int i = 0;i < Main.maxNPCs;i++)
            {
                NPC npc = Main.npc[i];
                float distvectx = (Center2.X - npc.Center.X) * (Center2.X - npc.Center.X);
                float distvectY = (Center2.Y - npc.Center.Y) * (Center2.Y - npc.Center.Y);
                float squaredDist = Math.Abs(distvectx + distvectY);
                if (Main.npc[i].active)
                {
                    bool colcheck = !checkWalls || (Collision.CheckAABBvLineCollision(Main.npc[i].position, new Vector2(Main.npc[i].width, Main.npc[i].height), Main.npc[i].Center, Center) && Collision.CanHitLine(Main.npc[i].Center, 0, 0, Center, 0, 0));
                    if (Main.npc[i].IsValidEnemy() && (!checkCanChase || Main.npc[i].CanBeChasedBy()) && colcheck && squaredDist < maxdist)
                    {
                        closetnpcs.Add(Main.npc[i]);
                    }
                }
            }

            Func<NPC, float> sortbydistance = delegate (NPC npc)
            {
                float distvectX = (Center2.X - npc.Center.X) * (Center2.X - npc.Center.X);
                float distvecty = (Center2.Y - npc.Center.Y) * (Center2.Y - npc.Center.Y);
                float squaredDist = Math.Abs(distvectX + distvecty);

                float score = squaredDist;
                Point weightedscore = AddedWeight.FirstOrDefault(npcid => npcid.X == npc.whoAmI);
                score += weightedscore != default ? weightedscore.Y * Math.Abs(weightedscore.Y) : 0;

                if (weightedscore != default && weightedscore.Y >= 1000000)
                {
                    score = 100000000;
                }
                return score;
            };

            if (closetnpcs.Count < 1)
                return null;
            else
            {
                closetnpcs = closetnpcs.ToArray().OrderBy(sortbydistance).ToList();
                if (AddedWeight != default)
                    closetnpcs.RemoveAll(npc => (int)sortbydistance(npc) == 100000000);
                return closetnpcs;
            }
        }
    }
}
