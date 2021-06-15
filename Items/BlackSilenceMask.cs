using Terraria.ModLoader;

namespace Furioso.Items
{
    public class BlackSilenceMask : EquipTexture
    {
        public override bool DrawHead()
        {
            return true;
        }
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
            drawAltHair = true;
        }
    }
}
