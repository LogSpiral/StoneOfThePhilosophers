﻿using LogSpiralLibrary.CodeLibrary.Utilties;
using Terraria.ModLoader;
using Terraria;

namespace StoneOfThePhilosophers.Contents.Water;

public class TidalErosion : ModBuff
{
    public override void Update(NPC npc, ref int buffIndex)
    {
        for (int n = 0; n < 3; n++)
            Dust.NewDustPerfect(npc.Center, MyDustId.Water).velocity *= 2;

    }
    public override void Update(Player player, ref int buffIndex)
    {
        player.statDefense -= 5;
    }
}
public class TidalErosionGBNPC : GlobalNPC
{
    public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
    {
        if (npc.HasBuff<TidalErosion>())
        {
            modifiers.FinalDamage += 10;
            for (int n = 0; n < 10; n++)
            {
                Dust.NewDustPerfect(npc.Center, MyDustId.Water).velocity *= 6;
            }
        }
    }
}
