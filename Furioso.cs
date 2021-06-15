using Furioso.Items;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace Furioso
{
    public class Furioso : Mod
	{

        public static Effect MookEffect;

        public static Effect WheelEffect;
        public static Effect WheelEffect2;
        public static Effect DurandalEffect;
        public static Effect DurandalEffect2;
        public static Effect CrystalEffect;
        public static Effect CrystalEffect2;

        public override void PostSetupContent()
        {
            MookEffect = GetEffect("Effects/Content/MookEffect");
            WheelEffect = GetEffect("Effects/Content/WheelEffect");
            WheelEffect2 = GetEffect("Effects/Content/WheelEffect2");
            DurandalEffect = GetEffect("Effects/Content/DurandalEffect");
            DurandalEffect2 = GetEffect("Effects/Content/DurandalEffect2");
            CrystalEffect = GetEffect("Effects/Content/CrystalEffect");
            CrystalEffect2 = GetEffect("Effects/Content/CrystalEffect2");
            FuriosoList.Load();
            base.PostSetupContent();
        }


        public override void Load()
        {
            AddEquipTexture(new BlackSilenceHandsOn(), null, EquipType.HandsOn, "BlackSilenceHandsOn", "Furioso/Items/BlackSilenceHandsOn", "", "");
            AddEquipTexture(new BlackSilenceHandsOff(), null, EquipType.HandsOff, "BlackSilenceHandsOff", "Furioso/Items/BlackSilenceHandsOff", "", "");
            AddEquipTexture(new BlackSilenceMask(), null, EquipType.Face, "BlackSilenceMask", "Furioso/Items/BlackSilenceMask", "", "");
        }
        public override void Unload()
        {
            FuriosoList.Unload();
            MookEffect = null;
            WheelEffect = null;
            WheelEffect2 = null;
            DurandalEffect = null;
            DurandalEffect2 = null;
            CrystalEffect = null;
            CrystalEffect2 = null;
        }

    }


}