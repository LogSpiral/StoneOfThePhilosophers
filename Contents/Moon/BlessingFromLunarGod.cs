using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace StoneOfThePhilosophers.Contents.Moon;

public class BlessingFromLunarGod : ModBuff
{
    public override void Update(Player player, ref int buffIndex)
    {
        for (int l = 0; l < Player.MaxBuffs; l++)
        {
            int num24 = player.buffType[l];
            if (Main.debuff[num24] && player.buffTime[l] > 0 && (num24 < 0 || !BuffID.Sets.NurseCannotRemoveDebuff[num24]))
            {
                player.DelBuff(l);
                l = -1;
            }
        }
    }
}
