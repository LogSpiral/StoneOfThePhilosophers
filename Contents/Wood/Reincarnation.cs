using Terraria;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers.Contents.Wood;
public class Reincarnation : ModBuff
{
    public override void Update(Player player, ref int buffIndex)
    {
        player.lifeRegen += 40;
    }
}
