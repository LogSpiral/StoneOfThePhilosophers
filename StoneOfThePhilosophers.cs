using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers
{
    public class StoneOfThePhilosophers : Mod
    {
        public static Effect VertexDraw => vertexDraw ??= ModContent.Request<Effect>("StoneOfThePhilosophers/Effects/VertexDraw", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        static Effect vertexDraw;

    }
}