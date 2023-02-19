using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.UI;

namespace StoneOfThePhilosophers.Contents
{


    /// <summary>
    /// DXTsT自制的粒子ID表
    /// 制作时间：2017/1/31
    /// 版权所有：DXTsT & 四十九落星制作组
    /// 
    /// 说明：以下字段带有（！）标识符的说明此粒子效果会在黑暗中自发光
    /// 带有（.）标识符说明此粒子效果会高亮显示但是不会发光
    /// 其余Dust全部都不会发光！
    /// </summary>
    public static class MyDustId
    {
        /// <summary>
        /// brown dirt
        /// </summary>
        public const int BrownDirt = 0;
        /// <summary>
        /// grey stone
        /// </summary>
        public const int GreyStone = 1;
        /// <summary>
        /// thick green grass
        /// </summary>
        public const int GreenGrass = 2;
        /// <summary>
        /// thin green grass
        /// </summary>
        public const int ThinGreenGrass = 3;
        /// <summary>
        /// grey pebbles
        /// </summary>
        public const int GreyPebble = 4;
        /// <summary>
        /// red blood
        /// </summary>
        public const int RedBlood = 5;
        /// <summary>
        /// (!)orange fire, emits orange light !WARNING
        /// </summary>
        public const int Fire = 6;
        /// <summary>
        /// brown wood
        /// </summary>
        public const int Wood = 7;
        /// <summary>
        /// purple gems
        /// </summary>
        public const int PurpleGems = 8;
        /// <summary>
        /// orange gems
        /// </summary>
        public const int OrangeGems = 9;
        /// <summary>
        /// yellow gems
        /// </summary>
        public const int YellowGems = 10;
        /// <summary>
        /// white gems
        /// </summary>
        public const int WhiteGems = 11;
        /// <summary>
        /// red gems
        /// </summary>
        public const int RedGems = 12;
        /// <summary>
        /// cyan gems
        /// </summary>
        public const int CyanGems = 13;
        /// <summary>
        /// purple corruption particle with no gravity
        /// </summary>
        public const int CorruptionParticle = 14;
        /// <summary>
        /// (!)white amd blue magic fx, emits pale blue light
        /// </summary>
        public const int BlueMagic = 15;
        /// <summary>
        /// bluish white clouds like hermes boots
        /// </summary>
        public const int WhiteClouds = 16;
        /// <summary>
        /// thin grey material
        /// </summary>
        public const int ThinGrey = 17;
        /// <summary>
        /// thin sickly green material
        /// </summary>
        public const int SicklyGreen = 18;
        /// <summary>
        /// thin yellow material
        /// </summary>
        public const int ThinYellow = 19;
        /// <summary>
        /// (!)white lingering, emits cyan light
        /// </summary>
        public const int WhiteLingering = 20;
        /// <summary>
        /// (!)purple lingering, emits purple light
        /// </summary>
        public const int PurpleLingering = 21;
        /// <summary>
        /// brown material
        /// </summary>
        public const int Brown = 22;
        /// <summary>
        /// orange material
        /// </summary>
        public const int orange = 23;
        /// <summary>
        /// thin brown material
        /// </summary>
        public const int ThinBrown = 24;
        /// <summary>
        /// copper
        /// </summary>
        public const int Copper = 25;
        /// <summary>
        /// iron
        /// </summary>
        public const int iron = 26;
        /// <summary>
        /// (!)purple fx, emits bright purple light
        /// </summary>
        public const int PurpleLight = 27;
        /// <summary>
        /// dull copper
        /// </summary>
        public const int DullCopper = 28;
        /// <summary>
        /// (!)dark blue, emits pale pink light !WARNING
        /// </summary>
        public const int DarkBluePinkLight = 29;
        /// <summary>
        /// silver material
        /// </summary>
        public const int Silver = 30;
        /// <summary>
        /// yellowish white cloud material
        /// </summary>
        public const int Smoke = 31;
        /// <summary>
        /// yellow sand
        /// </summary>
        public const int Sand = 32;
        /// <summary>
        /// water, highly transparent
        /// </summary>
        public const int Water = 33;
        /// <summary>
        /// (!)red fx, emits red light !WARNING
        /// </summary>
        public const int RedLight = 35;
        /// <summary>
        /// muddy pale material
        /// </summary>
        public const int MuddyPale = 36;
        /// <summary>
        /// dark grey material
        /// </summary>
        public const int DarkGrey = 37;
        /// <summary>
        /// muddy brown material
        /// </summary>
        public const int MuddyBrown = 38;
        /// <summary>
        /// bright green jungle grass
        /// </summary>
        public const int JungleGrass = 39;
        /// <summary>
        /// bright green thin grass
        /// </summary>
        public const int ThinGrass = 40;
        /// <summary>
        /// (!)dark blue wandering circles, emits bright cyan light !WARNING
        /// </summary>
        public const int BlueCircle = 41;
        /// <summary>
        /// thin teal material
        /// </summary>
        public const int ThinTeal = 42;
        /// <summary>
        /// (!)bright green spores that lingers for a while, emits light green light
        /// </summary>
        public const int GreenSpores = 44;
        /// <summary>
        /// (!)light blue circles, emits purple light
        /// </summary>
        public const int LightBlueCircle = 45;
        /// <summary>
        /// green material with no gravity
        /// </summary>
        public const int GreenMaterial = 46;
        /// <summary>
        /// thin cyan grass
        /// </summary>
        public const int CyanGrass = 47;
        /// <summary>
        /// pink water, highly transparent
        /// </summary>
        public const int PinkWater = 52;
        /// <summary>
        /// grey material
        /// </summary>
        public const int GreyMaterial = 53;
        /// <summary>
        /// black material
        /// </summary>
        public const int BlackMaterial = 54;
        /// <summary>
        /// (!)bright orange thick fx, emits yellow light
        /// </summary>
        public const int OrangeFx = 55;
        /// <summary>
        /// (!)cyan fx, emits pale blue light
        /// </summary>
        public const int CyanFx = 56;
        /// <summary>
        /// (!)small yellow hallowed fx, emis yellow light
        /// </summary>
        public const int YellowHallowFx = 57;
        /// <summary>
        /// (!)hot and pale pink magic fx, emits pink light
        /// </summary>
        public const int PinkMagic = 58;
        /// <summary>
        /// (!)blue torch, emits pure blue light !WARNING
        /// </summary>
        public const int BlueTorch = 59;
        /// <summary>
        /// (!)red torch, emits pure red light !WARNING
        /// </summary>
        public const int RedTorch = 60;
        /// <summary>
        /// (!)green torch, emits pure green light !WARNING
        /// </summary>
        public const int GreenTorch = 61;
        /// <summary>
        /// (!)purple torch, emits purple light !WARNING
        /// </summary>
        public const int PurpleTorch = 62;
        /// <summary>
        /// (!)white torch, emits bright white light !WARNING
        /// </summary>
        public const int WhiteTorch = 63;
        /// <summary>
        /// (!)yellow torch, emits deep yellow light !WARNING
        /// </summary>
        public const int YellowTorch = 64;
        /// <summary>
        /// (!)demon torch, emits pulsating pink/purple light !WARNING
        /// </summary>
        public const int DemonTorch = 65;
        /// <summary>
        /// (!)White transparent !WARNING
        /// </summary>
        public const int WhiteTransparent = 66;
        /// <summary>
        /// (!)cyan ice crystals, emits cyan light
        /// </summary>
        public const int CyanIce = 67;
        /// <summary>
        /// (.)dark cyan ice crystals, emits very faint blue light, glows in disabled gravity
        /// </summary>
        public const int DarkCyanIce = 68;
        /// <summary>
        /// thin pink material
        /// </summary>
        public const int ThinPink = 69;
        /// <summary>
        /// (.)thin transparent purple material, emits faint purple light, glows in disabled gravity
        /// </summary>
        public const int TransparentPurple = 70;
        /// <summary>
        /// (!)transparent pink fx, emits faint pink light
        /// </summary>
        public const int TransparentPinkFx = 71;
        /// <summary>
        /// (!)solid pink fx, emits faint pink light
        /// </summary>
        public const int SolidPinkFx = 72;
        /// <summary>
        /// (!)solid bright pink fx, emits pink light
        /// </summary>
        public const int BrightPinkFx = 73;
        /// <summary>
        /// (!)solid bright green fx, emits green light
        /// </summary>
        public const int BrightGreenFx = 74;
        /// <summary>
        /// (!)green cursed torch !WARNING
        /// </summary>
        public const int CursedFire = 75;
        /// <summary>
        /// snowfall, lasts a long time
        /// </summary>
        public const int Snow = 76;

        /// <summary>
        /// thin grey material
        /// </summary>
        public const int ThinGrey1 = 77;
        /// <summary>
        /// thin copper material
        /// </summary>
        public const int ThinCopper = 78;
        /// <summary>
        /// thin yellow material
        /// </summary>
        public const int ThinYellow1 = 79;
        /// <summary>
        /// ice block material
        /// </summary>
        public const int IceBlock = 80;
        /// <summary>
        /// iron material
        /// </summary>
        public const int Iron = 81;
        /// <summary>
        /// silty material
        /// </summary>
        public const int Silty = 82;
        /// <summary>
        /// sickly green material
        /// </summary>
        public const int SicklyGreen1 = 83;
        /// <summary>
        /// bluish grey material
        /// </summary>
        public const int BluishGrey = 84;
        /// <summary>
        /// thin sandy materiial
        /// </summary>
        public const int ThinSandy = 85;
        /// <summary>
        /// (!)transparent pink material, emits pink light
        /// </summary>
        public const int PinkTrans = 86;
        /// <summary>
        /// (!)transparent yellow material, emits yellow light
        /// </summary>
        public const int YellowTrans = 87;
        /// <summary>
        /// (!)transparent blue material, emits blue light
        /// </summary>
        public const int BlueTrans = 88;
        /// <summary>
        /// (!)transparent green material, emits green light
        /// </summary>
        public const int GreenTrans = 89;
        /// <summary>
        /// (!)transparent red material, emits red light
        /// </summary>
        public const int RedTrans = 90;
        /// <summary>
        /// (!)transparent white material, emits white light
        /// </summary>
        public const int WhiteTrans = 91;
        /// <summary>
        /// (!)transparent cyan material, emits cyan light
        /// </summary>
        public const int CyanTrans = 92;
        /// <summary>
        /// thin dark green grass
        /// </summary>
        public const int DarkGrass = 93;
        /// <summary>
        /// thin pale dark green grass
        /// </summary>
        public const int PaleDarkGrass = 94;
        /// <summary>
        /// thin dark red grass
        /// </summary>
        public const int DarkRedGrass = 95;
        /// <summary>
        /// thin blackish green grass
        /// </summary>
        public const int BlackGreenGrass = 96;
        /// <summary>
        /// thin dark red grass
        /// </summary>
        public const int DarkRedGrass1 = 97;
        /// <summary>
        /// purple water, highly transparent
        /// </summary>
        public const int PurpleWater = 98;
        /// <summary>
        /// cyan water, highly transparent
        /// </summary>
        public const int CyanWater = 99;
        /// <summary>
        /// pink water, highly transparent
        /// </summary>
        public const int PinkWater1 = 100;
        /// <summary>
        /// cyan water, highly transparent
        /// </summary>
        public const int CyanWater1 = 101;
        /// <summary>
        /// orange water, highly transparent
        /// </summary>
        public const int OrangeWater = 102;
        /// <summary>
        /// dark blue water, highly transparent
        /// </summary>
        public const int DarkBlueWater = 103;
        /// <summary>
        /// hot pink water, highly transparent
        /// </summary>
        public const int HotPinkWater = 104;
        /// <summary>
        /// red water, highly transparent
        /// </summary>
        public const int RedWater = 105;
        /// <summary>
        /// (.)transparent red/green/blue material, glows in the dark
        /// </summary>
        public const int RgbMaterial = 106;
        /// <summary>
        /// (!)short green powder, emits green light
        /// </summary>
        public const int GreenFXPowder = 107;
        /// <summary>
        /// light pale purple round material
        /// </summary>
        public const int PurpleRound = 108;
        /// <summary>
        /// black material
        /// </summary>
        public const int BlackMaterial1 = 109;
        /// <summary>
        /// (.)bright green bubbles, emits very faint green light
        /// </summary>
        public const int GreenBubble = 110;
        /// <summary>
        /// (.)bright cyan bubbles, emits very faint cyan light
        /// </summary>
        public const int CyanBubble = 111;
        /// <summary>
        /// (.)bright pink bubbles, emits very faint pink light
        /// </summary>
        public const int PinkBubble = 112;
        /// <summary>
        /// (.)blue ice crystals, glows in the dark
        /// </summary>
        public const int BlueIce = 113;
        /// <summary>
        /// (.)bright pink/yellow bubbles, emits very faint pink light
        /// </summary>
        public const int PinkYellowBubble = 114;
        /// <summary>
        /// red grass
        /// </summary>
        public const int RedGrass = 115;
        /// <summary>
        /// blueish green grass
        /// </summary>
        public const int BlueGreenGrass = 116;
        /// <summary>
        /// red grass
        /// </summary>
        public const int RedGrass1 = 117;
        /// <summary>
        /// purple gems
        /// </summary>
        public const int PurpleGems1 = 118;
        /// <summary>
        /// pink gems
        /// </summary>
        public const int PinkGems = 119;
        /// <summary>
        /// pale pink gems
        /// </summary>
        public const int PalePinkGems = 120;
        /// <summary>
        /// thin grey material
        /// </summary>
        public const int ThinGrey2 = 121;
        /// <summary>
        /// thin iron material
        /// </summary>
        public const int ThinIron = 122;
        /// <summary>
        /// hot pink bubble material
        /// </summary>
        public const int HotPinkBubble = 123;
        /// <summary>
        /// yellowish white bubbles
        /// </summary>
        public const int YellowWhiteBubble = 124;
        /// <summary>
        /// thin red material
        /// </summary>
        public const int ThinRed = 125;
        /// <summary>
        /// thin grey material
        /// </summary>
        public const int ThinGrey3 = 126;
        /// <summary>
        /// (!)reddish orange fire, emits orange light
        /// </summary>
        public const int OrangeFire = 127;
        /// <summary>
        /// green gems
        /// </summary>
        public const int GreenGems = 128;
        /// <summary>
        /// thin brown material
        /// </summary>
        public const int ThinBrown1 = 129;
        /// <summary>
        /// (!)trailing red falling fireworks, emits red light
        /// </summary>
        public const int TrailingRed = 130;
        /// <summary>
        /// (!)trailing green rising fireworks, emits green light
        /// </summary>
        public const int TrailingGreen = 131;
        /// <summary>
        /// (!)trailing cyan falling fireworks, emits cyan light
        /// </summary>
        public const int TrailingCyan = 132;
        /// <summary>
        /// (!)trailing yellow falling fireworks, emits cyan light
        /// </summary>
        public const int TrailingYellow = 133;
        /// <summary>
        /// trailing pink falling fireworks
        /// </summary>
        public const int TrailingPink = 134;
        /// <summary>
        /// (!)cyan ice torch, emits cyan light !WARNING
        /// </summary>
        public const int IceTorch = 135;
        /// <summary>
        /// red material
        /// </summary>
        public const int Red = 136;
        /// <summary>
        /// bright blue/cyan material
        /// </summary>
        public const int BrightCyan = 137;
        /// <summary>
        /// bright orange/brown material
        /// </summary>
        public const int BrightOrange = 138;
        /// <summary>
        /// cyan confetti
        /// </summary>
        public const int CyanConfetti = 139;
        /// <summary>
        /// green confetti
        /// </summary>
        public const int GreenConfetti = 140;
        /// <summary>
        /// pink confetti
        /// </summary>
        public const int PinkConfetti = 141;
        /// <summary>
        /// yellow confetti
        /// </summary>
        public const int YellowConfetti = 142;
        /// <summary>
        /// light grey stone
        /// </summary>
        public const int LightGreyStone = 143;
        /// <summary>
        /// vivid copper stone
        /// </summary>
        public const int CopperStone = 144;
        /// <summary>
        /// pink stone
        /// </summary>
        public const int PinkStone = 145;
        /// <summary>
        /// green/brown material mix
        /// </summary>
        public const int GreenBrown = 146;
        /// <summary>
        /// orange material
        /// </summary>
        public const int Orange = 147;
        /// <summary>
        /// desaturated red material
        /// </summary>
        public const int RedDesaturated = 148;
        /// <summary>
        /// white material
        /// </summary>
        public const int White = 149;
        /// <summary>
        /// black/yellow/bluishwhite material
        /// </summary>
        public const int BlackYellowBluishwhite = 150;
        /// <summary>
        /// thin white material
        /// </summary>
        public const int ThinWhite = 151;
        /// <summary>
        /// (!)bright orange bubbles !WARNING
        /// </summary>
        public const int OrangeBubble = 152;
        /// <summary>
        /// bright orange bubble material
        /// </summary>
        public const int OrangeBubbleMaterial = 153;
        /// <summary>
        /// pale blue thin material
        /// </summary>
        public const int BlueThin = 154;
        /// <summary>
        /// thin dark brown material
        /// </summary>
        public const int DarkBrown = 155;
        /// <summary>
        /// (!)bright blue/white bubble material, emits pale blue light
        /// </summary>
        public const int BlueWhiteBubble = 156;
        /// <summary>
        /// (.)thin green fx, glows in the dark
        /// </summary>
        public const int GreenFx = 157;
        /// <summary>
        /// (!)orange fire, emits orange light !WARNING
        /// </summary>
        public const int OrangeFire1 = 158;
        /// <summary>
        /// (!)flickering yellow fx, emits yellow light !WARNING
        /// </summary>
        public const int YellowFx = 159;
        /// <summary>
        /// (!)shortlived cyan fx, emits bright cyan light
        /// </summary>
        public const int CyanShortFx = 160;
        /// <summary>
        /// cyan material
        /// </summary>
        public const int CyanMaterial = 161;
        /// <summary>
        /// (!)shortlived orange fx, emits bright orange light
        /// </summary>
        public const int OrangeShortFx = 162;
        /// <summary>
        /// (.)bright green thin material, glows in the dark
        /// </summary>
        public const int BrightGreen = 163;
        /// <summary>
        /// (!)flickering pink fx, emits hot pink light !WARNING
        /// </summary>
        public const int PinkFx = 164;
        /// <summary>
        /// white/blue bubble material
        /// </summary>
        public const int WhiteBlueBubble = 165;
        /// <summary>
        /// thin bright pink material
        /// </summary>
        public const int PinkThinBright = 166;
        /// <summary>
        /// thin green material
        /// </summary>
        public const int ThinGreen = 167;
        /// <summary>
        /// !bright pink bubbles !WARNING
        /// </summary>
        public const int PinkBrightBubble = 168;
        /// <summary>
        /// (!)yellow fx, emits deep yellow light !WARNING
        /// </summary>
        public const int YellowFx1 = 169;
        /// <summary>
        /// (.)thin orange fx, emits faint white light
        /// </summary>
        public const int Ichor = 170;
        /// <summary>
        /// bright purple bubble material
        /// </summary>
        public const int PurpleBubble = 171;
        /// <summary>
        /// (.)light blue particles, emits faint blue light
        /// </summary>
        public const int BlueParticle = 172;
        /// <summary>
        /// (!)shortlived purple fx, emits bright purple light
        /// </summary>
        public const int PurpleShortFx = 173;
        /// <summary>
        /// (!)bright orange bubble material, emits reddish orange light
        /// </summary>
        public const int OrangeFire2 = 174;
        /// <summary>
        /// (.)shortlived white fx, glows in the dark
        /// </summary>
        public const int WhiteShortFx = 175;
        /// <summary>
        /// light blue particles
        /// </summary>
        public const int LightBlueParticle = 176;
        /// <summary>
        /// light pink particles
        /// </summary>
        public const int LightPinkParticle = 177;
        /// <summary>
        /// light green particles
        /// </summary>
        public const int LightGreenParticle = 178;
        /// <summary>
        /// light purple particles
        /// </summary>
        public const int LightPurpleParticle = 179;
        /// <summary>
        /// (!)light cyan particles, glows in the dark
        /// </summary>
        public const int LightCyanParticle = 180;
        /// <summary>
        /// (.)light cyan/pink bubble material, glows in the dark
        /// </summary>
        public const int CyanPinkBubble = 181;
        /// <summary>
        /// (.)light red bubble material, barely emits red light
        /// </summary>
        public const int RedBubble = 182;
        /// <summary>
        /// (.)transparent red bubble material, glows in the dark
        /// </summary>
        public const int RedTransBubble = 183;
        /// <summary>
        /// sickly pale greenish grey particles that stay in place
        /// </summary>
        public const int GreenishGreyParticle = 184;
        /// <summary>
        /// (!)light cyan crystal material, emits cyan light
        /// </summary>
        public const int CyanCrystal = 185;
        /// <summary>
        /// pale dark blue smoke
        /// </summary>
        public const int DarkBlueSmoke = 186;
        /// <summary>
        /// (!)light cyan particles, emits cyan light
        /// </summary>
        public const int LightCyanParticle1 = 187;
        /// <summary>
        /// bright green bubbles
        /// </summary>
        public const int GreenBubble1 = 188;
        /// <summary>
        /// thin orange material
        /// </summary>
        public const int OrangeMaterial = 189;
        /// <summary>
        /// thin gold material
        /// </summary>
        public const int GoldMaterial = 190;
        /// <summary>
        /// black flakes
        /// </summary>
        public const int BlackFlakes = 191;
        /// <summary>
        /// snow material
        /// </summary>
        public const int SnowMaterial = 192;
        /// <summary>
        /// green material
        /// </summary>
        public const int GreenMaterial1 = 193;
        /// <summary>
        /// thin brown material
        /// </summary>
        public const int BrownMaterial = 194;
        /// <summary>
        /// thin black material
        /// </summary>
        public const int BlackMaterial2 = 195;
        /// <summary>
        /// thin green material
        /// </summary>
        public const int ThinGreen1 = 196;
        /// <summary>
        /// (.)thin bright cyan material, glows in the dark
        /// </summary>
        public const int BrightCyanMaterial = 197;
        /// <summary>
        /// black/white particles
        /// </summary>
        public const int BlackWhiteParticle = 198;
        /// <summary>
        /// pale purple/black/grey particles
        /// </summary>
        public const int PurpleBlackGrey = 199;
        /// <summary>
        /// pink particles
        /// </summary>
        public const int PinkParticle = 200;
        /// <summary>
        /// light pink particles
        /// </summary>
        public const int LightPinkParticle1 = 201;
        /// <summary>
        /// light cyan particles
        /// </summary>
        public const int LightCyanParticle2 = 202;
        /// <summary>
        /// grey particles
        /// </summary>
        public const int GreyParticle = 203;
        /// <summary>
        /// (.)white particles, glows in the dark
        /// </summary>
        public const int WhiteParticle = 204;
        /// <summary>
        /// (.)thin pink material, barely emits pink light
        /// </summary>
        public const int ThinPinkMaterial = 205;
        /// <summary>
        /// (!)shortlived cyan fx, emits bright blue light
        /// </summary>
        public const int CyanShortFx1 = 206;
        /// <summary>
        /// thin brown material
        /// </summary>
        public const int BrownMaterial1 = 207;
        /// <summary>
        /// orange stone
        /// </summary>
        public const int OrangeStone = 208;
        /// <summary>
        /// pale green stone
        /// </summary>
        public const int PaleGreenStone = 209;
        /// <summary>
        /// off white material
        /// </summary>
        public const int OffWhite = 210;
        /// <summary>
        /// bright blue particles
        /// </summary>
        public const int BrightBlueParticle = 211;
        /// <summary>
        /// white particles
        /// </summary>
        public const int WhiteParticle1 = 212;
        /// <summary>
        /// (.)shortlived tiny white fx, barely emits white light
        /// </summary>
        public const int WhiteShortFx1 = 213;
        /// <summary>
        /// thin pale brown material
        /// </summary>
        public const int Thin = 214;
        /// <summary>
        /// thin khaki material
        /// </summary>
        public const int ThinKhaki = 215;
        /// <summary>
        /// pale pink material
        /// </summary>
        public const int Pale = 216;
        /// <summary>
        /// cyan particles
        /// </summary>
        public const int Cyan = 217;
        /// <summary>
        /// hot pink particles
        /// </summary>
        public const int Hot = 218;
        /// <summary>
        /// (!)trailing red flying fireworks, emits orange light
        /// </summary>
        public const int TrailingRed1 = 219;
        /// <summary>
        /// (!)trailing green flying fireworks, emits green light
        /// </summary>
        public const int TrailingGreen1 = 220;
        /// <summary>
        /// (!)trailing blue flying fireworks, emits pale blue light
        /// </summary>
        public const int TrailingBlue = 221;
        /// <summary>
        /// (!)trailing yellow flying fireworks, emits yellow light
        /// </summary>
        public const int TrailingYellow1 = 222;
        /// <summary>
        /// (.)trailing red flying fireworks, glows in the dark
        /// </summary>
        public const int TrailingRed2 = 223;
        /// <summary>
        /// thin blue material
        /// </summary>
        public const int ThinBlue = 224;
        /// <summary>
        /// orange material
        /// </summary>
        public const int OrangeMaterial1 = 225;
        /// <summary>
        /// 
        /// </summary>
        public const int ElectricCyan = 226;

        /// <summary>
        /// (!)Lunar fire!!!
        /// </summary>
        public const int CyanLunarFire = 229;
        /// <summary>
        /// (!)flickering Purple fx, emits Purple light !WARNING
        /// </summary>
        public const int PurpleFx = 230;
    }
    public static class VirtualDreamFactorMethods
    {

        public static T Sum<T, TValue>(T[] values) where T : IVector<T, TValue>
        {
            var vec = values[0];
            for (int n = 1; n < values.Length; n++)
                vec.Add(values[n]);
            return vec;
        }
        public static float Lerp(this float t, float from, float to, bool clamp = false)
        {
            if (clamp)
            {
                t = MathHelper.Clamp(t, 0, 1);
            }
            return MathHelper.Lerp(from, to, t);
        }


        /// <summary>
        /// 阿汪超喜欢用的插值函数，获得一个先迅速增加再慢慢变小的插值
        /// </summary>
        /// <param name="value">丢进去的变量，取值范围一般是[0,maxTimeWhen]</param>
        /// <param name="maxTimeWhen">什么时候插值结束呢</param>
        /// <returns>自己画函数图像去，真的像是一个小山丘一样(</returns>
        public static float HillFactor2(this float value, float maxTimeWhen = 1)
        {
            //return Clamp((center - Math.Abs(center - value)) / center / whenGetMax, 0, 1);
            return (1 - (float)Math.Cos(MathHelper.TwoPi * Math.Sqrt(value / maxTimeWhen))) * 0.5f;
        }
        /// <summary>
        /// 阿汪超喜欢用的插值函数，获得一个先迅速增加再慢慢变小的插值
        /// </summary>
        /// <param name="value">丢进去的变量，取值范围一般是[0,maxTimeWhen]</param>
        /// <param name="maxTimeWhen">什么时候插值结束呢</param>
        /// <returns>自己画函数图像去，真的像是一个小山丘一样(</returns>
        public static float HillFactor(this float value, float maxTimeWhen = 1)
        {
            //return Clamp((center - Math.Abs(center - value)) / center / whenGetMax, 0, 1);
            return (float)Math.Sin(MathHelper.Pi * Math.Sqrt(value / maxTimeWhen));
        }
        /// <summary>
        /// 阿汪超喜欢用的插值函数，获得一个先上后下的插值
        /// </summary>
        /// <param name="value">丢进去的变量，取值范围一般是[0,2*center]</param>
        /// <param name="center">中间值，或者说最大值点</param>
        /// <param name="whenGetMax">决定丢进去的值与最大值的比值为多少时第一次达到最大值(1)，一般取(0,0.5f]</param>
        /// <returns>自己画函数图像去，不是三角形就是梯形(</returns>
        public static float SymmetricalFactor2(this float value, float center, float whenGetMax)
        {
            //return Clamp((center - Math.Abs(center - value)) / center / whenGetMax, 0, 1);
            return value.SymmetricalFactor(center, whenGetMax * center * 2);
        }

        /// <summary>
        /// 阿汪超喜欢用的插值函数，获得一个先上后下的插值
        /// </summary>
        /// <param name="value">丢进去的变量，取值范围一般是[0,2*center]</param>
		/// <param name="center">中间值，或者说最大值点</param>
		/// <param name="whenGetMax">决定丢进去的值为多少时第一次达到最大值(1)，一般取(0,center]</param>
		/// <returns>自己画函数图像去，不是三角形就是梯形(</returns>
        public static float SymmetricalFactor(this float value, float center, float whenGetMax)
        {
            return MathHelper.Clamp((center - Math.Abs(center - value)) / whenGetMax, 0, 1);
        }
        public static Color GetLerpValue(this float factor, params Color[] values)
        {
            if (factor <= 0)
            {
                return values[0];
            }
            else if (factor >= 1)
            {
                return values[values.Length - 1];
            }
            else
            {
                int c = values.Length - 1;
                int tier = (int)(c * factor);
                return Color.Lerp(values[tier], values[tier + 1], c * factor % 1);
            }
        }
        public static Vector4 GetLerpValue(this float factor, params Vector4[] values)
        {
            if (factor <= 0)
            {
                return values[0];
            }
            else if (factor >= 1)
            {
                return values[values.Length - 1];
            }
            else
            {
                int c = values.Length - 1;
                int tier = (int)(c * factor);
                return Vector4.Lerp(values[tier], values[tier + 1], c * factor % 1);
            }
        }
        public static float GetLerpValue(this float factor, params float[] values)
        {
            if (factor <= 0)
            {
                return values[0];
            }
            else if (factor >= 1)
            {
                return values[values.Length - 1];
            }
            else
            {
                int c = values.Length - 1;
                int tier = (int)(c * factor);
                return MathHelper.Lerp(values[tier], values[tier + 1], c * factor % 1);
            }
        }

        public static float GetLerpValue_Loop(this float factor, params float[] values)
        {
            if (factor <= 0 || factor >= 1)
            {
                return values[0];
            }
            else
            {
                int c = values.Length;
                int tier = (int)(c * factor);
                return MathHelper.Lerp(values[tier], values[(tier + 1) == c ? 0 : (tier + 1)], c * factor % 1);
            }
        }
        public static Vector2 GetLerpValue_Loop(this float factor, params Vector2[] values)
        {
            if (factor <= 0 || factor >= 1)
            {
                return values[0];
            }
            else
            {
                int c = values.Length;
                int tier = (int)(c * factor);
                return Vector2.Lerp(values[tier], values[(tier + 1) == c ? 0 : (tier + 1)], c * factor % 1);
            }
        }
        public static Vector4 GetLerpValue_Loop(this float factor, params Vector4[] values)
        {
            if (factor <= 0 || factor >= 1)
            {
                return values[0];
            }
            else
            {
                int c = values.Length;
                int tier = (int)(c * factor);
                return Vector4.Lerp(values[tier], values[(tier + 1) == c ? 0 : (tier + 1)], c * factor % 1);
            }
        }
        public static Vector2 GetLerpValue(this float factor, params Vector2[] values)
        {
            if (factor <= 0)
            {
                return values[0];
            }
            else if (factor >= 1)
            {
                return values[values.Length - 1];
            }
            else
            {
                int c = values.Length - 1;
                int tier = (int)(c * factor);
                return Vector2.Lerp(values[tier], values[tier + 1], c * factor % 1);
            }
        }
    }
    public static class StoneOfThePhilosophersHelper
    {
        public static UIElement SetSize(this UIElement uie, Vector2 size, float precentWidth = 0, float precentHeight = 0)
        {
            uie.SetSize(size.X, size.Y, precentWidth, precentHeight);
            return uie;
        }
        public static UIElement SetSize(this UIElement uie, float width, float height, float precentWidth = 0, float precentHeight = 0)
        {
            uie.Width.Set(width, precentWidth);
            uie.Height.Set(height, precentHeight);
            return uie;
        }
        public static object Call(params object[] args)
        {
            string funcName = (string)args[0];
            switch (funcName)
            {
                case "SymmetricFactor":
                    {
                        float value = (float)args[1];
                        float whenGetMax = (float)args[2];
                        return MathHelper.Clamp((0.5f - Math.Abs(value - 0.5f)) / whenGetMax, 0, 1);
                    }
                case "SmoothSymmetricFactor":
                    {
                        return MathHelper.SmoothStep(0, 1, (float)Call("SymmetricFactor", (float)args[1], (float)args[2]));
                    }
            }
            return null;
        }
        public static float SmoothSymmetricFactor(this float value, float whenGetMax)
        {
            return MathHelper.SmoothStep(0, 1, SymmetricFactor(value, whenGetMax));
        }
        public static float SymmetricFactor(this float value, float whenGetMax)
        {
            return MathHelper.Clamp((0.5f - Math.Abs(value - 0.5f)) / whenGetMax, 0, 1);
        }

        public static void VertexDraw(CustomVertexInfo[] vertexs, Texture2D baseTex, Texture2D aniTex, Vector2 uTime = default, bool trailing = false)
        {
            Effect effect = StoneOfThePhilosophers.VertexDraw;
            if (effect == null) return;
            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            if (trailing)
            {
                List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();
                for (int i = 0; i < vertexs.Length - 2; i += 2)
                {
                    triangleList.Add(vertexs[i]);
                    triangleList.Add(vertexs[i + 2]);
                    triangleList.Add(vertexs[i + 1]);

                    triangleList.Add(vertexs[i + 1]);
                    triangleList.Add(vertexs[i + 2]);
                    triangleList.Add(vertexs[i + 3]);
                }
                vertexs = triangleList.ToArray();
            }

            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));
            effect.Parameters["uTransform"].SetValue(model * Main.GameViewMatrix.TransformationMatrix * projection);
            effect.Parameters["uTimeX"].SetValue(uTime.X);
            effect.Parameters["uTimeY"].SetValue(uTime.Y);
            Main.graphics.GraphicsDevice.Textures[0] = baseTex;
            Main.graphics.GraphicsDevice.Textures[1] = aniTex;
            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;
            Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.AnisotropicWrap;
            effect.CurrentTechnique.Passes[0].Apply();
            //Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertexs, 0, vertexs.Length / 3);
            Main.graphics.GraphicsDevice.RasterizerState = originalState;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public static void VertexDraw3DPlanes()
        {

        }
    }
    public struct CustomVertexInfo : IVertexType
    {
        private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(new VertexElement[3]
        {
                new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
                new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0)
        });
        public Vector2 Position;
        public Color Color;
        public Vector3 TexCoord;

        public CustomVertexInfo(Vector2 position, Color color, Vector3 texCoord)
        {
            Position = position;
            Color = color;
            TexCoord = texCoord;
        }
        public CustomVertexInfo(Vector2 position, float alpha, Vector3 texCoord)
        {
            Position = position;
            Color = Color.White with { A = (byte)(MathHelper.Clamp(255 * alpha, 0, 255)) };
            TexCoord = texCoord;
        }
        public CustomVertexInfo(Vector2 position, Vector3 texCoord)
        {
            Position = position;
            Color = Color.White;
            TexCoord = texCoord;
        }

        public VertexDeclaration VertexDeclaration
        {
            get
            {
                return _vertexDeclaration;
            }
        }
    }
    public abstract class MagicArea : ModProjectile
    {
        public override string Texture => "StoneOfThePhilosophers/MagicArea_1";//{StarBound.NPCs.Bosses.BigApe.BigApeTools.ApePath}StrawBerryArea

        public Projectile projectile => Projectile;
        public Player player => Main.player[projectile.owner];
        public bool Released => projectile.timeLeft < 12;
        public float Light => Released ? MathHelper.Lerp(1, 0, (12 - projectile.timeLeft) / 12f) : MathHelper.Clamp(projectile.ai[0] / 60f, 0, 1);
        public float Theta => projectile.ai[0] / 60f * MathHelper.TwoPi;
        public float Alpha
        {
            get
            {

                if (projectile.ai[0] >= 120)
                {
                    return -MathHelper.Pi / 3f;
                }
                else if (projectile.ai[0] >= 60)
                {
                    return -(projectile.ai[0] - 60) * (projectile.ai[0] - 60) / 3600f * MathHelper.Pi / 3f;
                }
                return 0;
            }
        }
        public float Beta => MathHelper.SmoothStep(0, 1, projectile.ai[0] / 60f) * projectile.velocity.ToRotation();
        public float Size => Released ? MathHelper.Lerp(96, 144, (12 - projectile.timeLeft) / 12f) : 96;
        public const float dis = 64;
        public virtual int Cycle => 60;
        public virtual Color MainColor => Color.White;
        public virtual bool UseMana => (int)projectile.ai[0] % Cycle == 0;
        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.DamageType = DamageClass.Magic;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.hide = true;
            //vertexInfos[0] = new CustomVertexInfo(default, MainColor, new Vector3(0, 0, 0));
            //vertexInfos[1] = new CustomVertexInfo(default, MainColor, new Vector3(1, 0, 0));
            //vertexInfos[2] = new CustomVertexInfo(default, MainColor, new Vector3(1, 1, 0));
            //vertexInfos[3] = new CustomVertexInfo(default, MainColor, new Vector3(0, 1, 0));
        }
        //public Vector2 GetVec(Vector3 vector, float d = dis, bool negativeTheta = false)
        //{
        //    float x = vector.X;
        //    float y = vector.Y;
        //    float z = vector.Z;
        //    float sA = (float)Math.Sin(Alpha);
        //    float cA = (float)Math.Cos(Alpha);
        //    float sB = (float)Math.Sin(Beta);
        //    float cB = (float)Math.Cos(Beta);
        //    float sT = (float)Math.Sin(Theta) * (negativeTheta ? -1 : 1);
        //    float cT = (float)Math.Cos(Theta);
        //    float value1 = cA * (cT * x - sT * y) - sA * (z + d);
        //    float value2 = sT * x + cT * y;
        //    return l / (l - z) * new Vector2(cB * value1 - sB * value2, sB * value1 + cB * value2);
        //}
        //public CustomVertexInfo[] vertexInfos = new CustomVertexInfo[4];
        public virtual void ShootProj(bool dying = false)
        {

        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            Main.instance.DrawCacheProjsOverWiresUI.Add(index);
            base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
        }
        //public virtual void UpdateVertex()
        //{
        //    for (int n = 0; n < vertexInfos.Length; n++)
        //    {
        //        vertexInfos[n].Position = GetVec(new Vector3(vertexInfos[n].TexCoord.X - 0.5f, vertexInfos[n].TexCoord.Y - 0.5f, 0) * Size) + player.Center;
        //        vertexInfos[n].TexCoord.Z = Light;
        //        if (MainColor != vertexInfos[n].Color)
        //        {
        //            vertexInfos[n].Color = MainColor;
        //        }
        //    }
        //}
        public override bool PreDraw(ref Color lightColor)
        {
            Matrix transform =
            Matrix.CreateScale(2) *
            Matrix.CreateTranslation(-1, -1, -1) *
            new Matrix(Size, 0, 0, 0,
                          0, Size, 0, 0,
                          0, 0, dis, 0,
                          0, 0, 0, 1) *
            Matrix.CreateRotationZ(Theta) *
            Matrix.CreateRotationY(Alpha) *
            Matrix.CreateRotationZ(Beta);
            CustomVertexInfo[] vertexInfos = new CustomVertexInfo[8];
            for (int n = 0; n < 8; n++)
            {
                var vec = new Vector3(n % 2, n / 2 % 2, 0);
                vertexInfos[n].TexCoord = new Vector3(vec.X, vec.Y, Light);
                vertexInfos[n].Color = MainColor;
                vec = Vector3.Transform(vec, transform);
                if (n == 3)
                {
                    transform =
                    Matrix.CreateScale(2) *
                    Matrix.CreateTranslation(-1, -1, -1) *
                    new Matrix(Size * 1.5f, 0, 0, 0,
                                         0, Size * 1.5f, 0, 0,
                                         0, 0, dis * 1.5f, 0,
                                         0, 0, 0, 1) *
                    Matrix.CreateRotationZ(-Theta * 1.5f) *
                    Matrix.CreateRotationY(Alpha) *
                    Matrix.CreateRotationZ(Beta);
                }
                vec += new Vector3(projectile.Center - Main.screenPosition - new Vector2(Main.screenWidth, Main.screenHeight) * .5f, 0);
                vec.Z = (2000 - vec.Z) / 2000f;
                vec /= vec.Z;
                vertexInfos[n].Position = new Vector2(vec.X, vec.Y) + Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) * .5f;
            }
            CustomVertexInfo[] vertexs = new CustomVertexInfo[12];
            for (int n = 0; n < 12; n++)
            {
                int index = (n % 6) switch
                {
                    0 => 0,
                    1 => 2,
                    2 => 1,
                    3 => 1,
                    4 => 2,
                    5 or _ => 3,
                };
                if (n > 5) index += 4;
                vertexs[n] = vertexInfos[index];
            }
            StoneOfThePhilosophersHelper.VertexDraw(vertexs[0..6],
                TextureAssets.Projectile[Type].Value,
                ModContent.Request<Texture2D>("StoneOfThePhilosophers/Style_4").Value,
                new Vector2(Main.GameUpdateCount * -0.09f, Main.GlobalTimeWrappedHourly * 0.6f));
            StoneOfThePhilosophersHelper.VertexDraw(vertexs[6..12],
                ModContent.Request<Texture2D>("StoneOfThePhilosophers/MagicArea_2").Value,
                ModContent.Request<Texture2D>("StoneOfThePhilosophers/Style_4").Value,
                new Vector2(Main.GameUpdateCount * -0.15f, Main.GlobalTimeWrappedHourly));
            return false;
        }
        public override void Kill(int timeLeft)
        {
            //if (projectile.ai[0] >= 60f)
            //{
            //    for (int n = 0; n < 10; n++)
            //    {
            //    }
            //}
        }
        public override void AI()
        {
            projectile.friendly = false;
            //if (projectile.ai[0] == 0) 
            //{

            //}
            projectile.ai[0]++;
            //UpdateVertex();
            if (projectile.owner == Main.myPlayer)
            {
                Vector2 diff = Main.MouseWorld - player.Center;
                diff.Normalize();
                projectile.velocity = diff;
                projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
                projectile.netUpdate = true;
            }
            projectile.Center = player.Center;
            int dir = projectile.direction;
            player.ChangeDir(dir);
            //player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);
            if (projectile.timeLeft > 12)
            {
                if (!player.channel)
                {
                    projectile.timeLeft = 12;
                }
                else
                {
                    projectile.timeLeft = 14;
                    if (UseMana)
                    {
                        if (!player.CheckMana(player.inventory[player.selectedItem].mana, true))
                        {
                            projectile.timeLeft = 12;
                        }
                        if (projectile.ai[0] >= 120)
                            ShootProj();
                    }
                }
            }
            else
            {
                if (projectile.ai[0] > 60f)
                {
                    ShootProj(true);
                }
            }
        }
    }
    public abstract class MagicStone : ModItem
    {
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ManaCrystal, 10);
            recipe.AddTile(TileID.DemonAltar);
            AddOtherIngredients(recipe);
            recipe.Register();
        }
        public virtual void AddOtherIngredients(Recipe recipe)
        {
            recipe.AddIngredient(ItemID.HellstoneBar, 15);
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[item.shoot] < 1;
        }
        public Item item => Item;
        public override void SetDefaults()
        {
            item.DamageType = DamageClass.Magic;
            item.width = 34;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.height = 40;
            item.rare = ItemRarityID.Cyan;
            item.autoReuse = true;
            item.useAnimation = 12;
            item.useTime = 12;
            item.useStyle = 5;
            item.channel = true;
            item.value = 150;
            item.knockBack = 4f;
            item.shootSpeed = 10;
            item.damage = 30;
            item.mana = 15;
        }
    }
}
