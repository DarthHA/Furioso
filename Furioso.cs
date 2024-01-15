using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using Color = Microsoft.Xna.Framework.Color;

namespace Furioso
{
    public class Furioso : Mod
    {
        private int time = 0, iconFrame = 0; // 声明了关于动态封面的变量
        private Asset<Texture2D>[] icon = new Asset<Texture2D>[10]; // icon贴图

        // 部分特效
        public static Effect MookEffect;
        public static Effect WheelEffect;
        public static Effect WheelEffect2;
        public static Effect DurandalEffect;
        public static Effect DurandalEffect2;
        public static Effect CrystalEffect;
        public static Effect CrystalEffect2;

        public override void PostSetupContent()
        {
            MookEffect = ModContent.Request<Effect>("Furioso/Effects/Content/MookEffect").Value;
            WheelEffect = ModContent.Request<Effect>("Furioso/Effects/Content/WheelEffect").Value;
            WheelEffect2 = ModContent.Request<Effect>("Furioso/Effects/Content/WheelEffect2").Value;
            DurandalEffect = ModContent.Request<Effect>("Furioso/Effects/Content/DurandalEffect").Value;
            DurandalEffect2 = ModContent.Request<Effect>("Furioso/Effects/Content/DurandalEffect2").Value;
            CrystalEffect = ModContent.Request<Effect>("Furioso/Effects/Content/CrystalEffect").Value;
            CrystalEffect2 = ModContent.Request<Effect>("Furioso/Effects/Content/CrystalEffect2").Value;
            DamageValue.BlackSilenceCardCounter = 0;
            base.PostSetupContent();
        }

        public override void Load()
        {
            On_Main.DrawMenu += Main_DrawMenu; // On掉原版的UI
            // 动态封面的切换
            for (int i = 0; i < icon.Length; i++)
            {
                icon[i] = ModContent.Request<Texture2D>($"Furioso/Icons/Icon_{i + 1}");
            }
        }

        public override void Unload()
        {
            SkillData.Unload();
            MookEffect = null;
            WheelEffect = null;
            WheelEffect2 = null;
            DurandalEffect = null;
            DurandalEffect2 = null;
            CrystalEffect = null;
            CrystalEffect2 = null;
            On_Main.DrawMenu -= Main_DrawMenu;
        }

        private void Main_DrawMenu(On_Main.orig_DrawMenu orig, Main self, GameTime gameTime)
        {
            #region 动态封面UI
            // 以下两行为获取Main.MenuUI的UIState集
            FieldInfo uiStateField = Main.MenuUI.GetType().GetField("_history", BindingFlags.NonPublic | BindingFlags.Instance);
            List<UIState> _history = (List<UIState>)uiStateField.GetValue(Main.MenuUI);
            UIState myMod = new UIState();
            // 使用for遍历UIState集，寻找UIMods类的实例
            for (int x = 0; x < _history.Count; x++)
            {
                myMod = _history[x];
                // 检测当前UIState的类名全称是否是ModLoader的UIMods
                if (myMod.ToString() == "Terraria.ModLoader.UI.UIMods")
                {
                    // 以下两行为获取UIMods的UI部件集
                    List<UIElement> elements = (List<UIElement>)myMod.GetType().GetField("Elements", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(myMod);
                    List<UIElement> uiElements = (List<UIElement>)elements[0].GetType().GetField("Elements", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(elements[0]);
                    UIPanel uiPanel = (UIPanel)uiElements[0];
                    // 由之前 了解模组选择页面的构成 一节可知，包含了 包含UIList部件的UIPanel 的UIElement第一个被UIMods包含，故此UIElement位于UIMods的部件集的0号索引处
                    // 以下两行用于获取UIElement的UI部件集
                    List<UIElement> myModUIPanel = uiPanel.GetType().GetField("Elements", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(uiPanel) as List<UIElement>;
                    UIList uiList = (UIList)myModUIPanel[0];
                    List<UIElement> myUIModItem;
                    // 遍历uiList包含的子部件，寻找我们mod的UIModItem部件
                    for (int i = 0; i < uiList._items.Count; i++)
                    {
                        //反射获取mod实例，检测其是否是我们的mod
                        if (uiList._items[i].GetType().GetField("_mod", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(uiList._items[i]).ToString() == Name)
                        {
                            // 以下两行为获取我们mod的UIModItem的UI部件集
                            myUIModItem = (List<UIElement>)uiList._items[i].GetType().GetField("Elements", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(uiList._items[i]);

                            float _modIconAdjust = (ModContent.Request<Texture2D>("Furioso/icon").Value == null ? 0 : 85);
                            UIElement badUnloader = myUIModItem.Find((UIElement e) => e.ToString() == "Terraria.ModLoader.UI.UIHoverImage" && e.Top.Pixels == 3);
                            // 遍历UIModItem的UI部件集
                            for (int j = 0; j < myUIModItem.Count; j++)
                            {
                                // 修改此UI部件的贴图和模组名
                                if (myUIModItem[j] is UIImage)
                                {
                                    if (myUIModItem[j].Width.Pixels == 80 && myUIModItem[j].Height.Pixels == 80)
                                    {
                                        (myUIModItem[j] as UIImage).SetImage(icon[iconFrame]);
                                    }
                                }
                            }
                            uiList._items[i].GetType().GetField("Elements", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(uiList._items[i], myUIModItem);
                        }
                    }
                    myModUIPanel[0] = uiList;
                    uiPanel.GetType().GetField("Elements", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(uiPanel, myModUIPanel);
                    uiElements[0] = uiPanel;
                    elements[0].GetType().GetField("Elements", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(elements[0], uiElements);
                    myMod.GetType().GetField("Elements", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(myMod, elements);
                    _history[x] = myMod;
                    uiStateField.SetValue(Main.MenuUI, _history);
                }
            }
            // 计时器递增
            time++;
            // 如果计时器取余6为0（计时器的值能被6整除）（此处的6用于控制动画速度）
            if (time % 10 == 0)
            {
                // 帧图记录器的值+1
                iconFrame++;
                // 重置计时器的值为0
                time = 0;
            }
            // 如果帧图记录器的值大于21（因为贴图后缀数字最大为21）
            if (iconFrame > 9)
                // 重置帧图记录器的值为1
                iconFrame = 0;
            orig(self, gameTime);
            #endregion
        }
    }

    public class GoldenDisplayNameSystem : ModSystem
    {
        // 由于反复反射开销较大且可读性低，而实际使用中又需要多多用到反射，所以这里存储两个变量
        private static Type _uiModItemType;
        private static MethodInfo _drawMethod;

        // 用于滚动颜色效果
        private static RenderTarget2D _renderTarget;

        public override void Load()
        {
            // 服务器上是没UI的，自然没必要挂
            if (Main.dedServ)
            {
                return;
            }

            // 在主线程上运行，否则会报错
            Main.QueueMainThreadAction(() =>
            {
                // 文字内容与长宽
                string text = Mod.DisplayName + " v" + Mod.Version;
                var size = ChatManager.GetStringSize(FontAssets.MouseText.Value, text, Vector2.One).ToPoint();

                // 实例化 RenderTarget，这里 width 和 height 使用 size 的值就行了
                _renderTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, size.X, size.Y);

                Main.spriteBatch.Begin();

                // 设置 RenderTarget 为我们的
                Main.graphics.GraphicsDevice.SetRenderTarget(_renderTarget);
                Main.graphics.GraphicsDevice.Clear(Color.Transparent);

                // 绘制字，注意别写成带描边的了，不然整个字就糊了
                ChatManager.DrawColorCodedString(Main.spriteBatch, FontAssets.MouseText.Value, text, Vector2.Zero,
                    Color.White, 0f, Vector2.Zero, Vector2.One);

                // 还原
                Main.spriteBatch.End();
                Main.graphics.GraphicsDevice.SetRenderTarget(null);
            });

            // 由于原版中 UIModItem 类是内部(internal)的，无法直接使用 typeof(UIModItem) 获取其 Type 实例
            // 所以我们要通过从程序集中寻找的方法来获取
            // typeof(Main).Assembly 是原版的程序集，调用其 GetTypes() 方法并找到第一个名为 UIModItem 的 Type
            // 即可获取到我们想要修改的类的 Type 实例
            _uiModItemType = typeof(Main).Assembly.GetTypes().First(t => t.Name == "UIModItem");

            // 接下来反射获取我们要修改的方法
            _drawMethod = _uiModItemType.GetMethod("Draw", BindingFlags.Instance | BindingFlags.Public);

            // 若方法获取成功(一般来说，只要写得没问题都是成功的)，则调用 HookEndpointManager.Add 添加 RuntimeDetour
            if (_drawMethod is not null)
            {
                // Add 相当于添加 On 命名空间的委托
                MonoModHooks.Add(_drawMethod, DrawHook);
            }
        }

        public override void Unload()
        {
            // 上面已经排除了服务器情况，这里服务器肯定是没挂上的
            if (Main.dedServ)
            {
                return;
            }

            if (_renderTarget is not null)
            {
                _renderTarget = null;
            }
        }

        // 这个委托表示原方法，包含一个类实例，以及相应方法的传入参数
        // UIModItem 是内部的，无法直接获取，这里可直接用 object 替代
        public delegate void DrawDelegate(object uiModItem, SpriteBatch sb);

        // 等同于一个 On 命名空间委托，在里面按照 On 的逻辑写就行了
        private void DrawHook(DrawDelegate orig, object uiModItem, SpriteBatch sb)
        {
            // 一定要记得调用原方法，不然你UI就没了
            orig.Invoke(uiModItem, sb);

            // RenderTarget为null，自然不需要进行了，因为不可能绘制出来
            if (_renderTarget is null)
            {
                return;
            }

            // 反射获取 _modName，后面修改以及绘制需要用到
            // 找不到 _modName 就报错
            if (_uiModItemType.GetField("_modName", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(uiModItem) is not UIText modName)
            {
                throw new Exception("There is something wrong!(by Furioso)");
            }

            // 确保是修改自己Mod的名字 (不过你想改别的我也不拦你)
            if (!modName.Text.Contains(Mod.DisplayName))
            {
                return;
            }

            // 加载所需的资源
            var texture = ModContent.Request<Texture2D>("Furioso/Images/Golden");
            var shader = ModContent.Request<Effect>("Furioso/Effects/Content/Golden", AssetRequestMode.ImmediateLoad).Value;

            // 传入 Shader 所需的参数
            shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * 0.25f);
            Main.instance.GraphicsDevice.Textures[1] = texture.Value; // 传入调色板

            // 为什么y要-2呢？因为原版就这么写的。金色字实现的原理实际上是覆盖原版，所以要保证重合
            var position = modName.GetDimensions().Position() - new Vector2(0f, 2f);


            // 重新开启 SpriteBatch 以应用 Shader
            sb.End();
            // 这个Begin传参可以确保无关参数不被修改，以避免奇怪的错误
            // (而且很方便，不用去找原版都用了哪些参数)
            sb.Begin(SpriteSortMode.Immediate, sb.GraphicsDevice.BlendState, sb.GraphicsDevice.SamplerStates[0],
                sb.GraphicsDevice.DepthStencilState, sb.GraphicsDevice.RasterizerState, shader, Main.UIScaleMatrix);

            // 将 _renderTarget 进行绘制
            sb.Draw(_renderTarget, position, Color.White);
            // 如果不使用 _renderTarget，就用一般的绘制字符串方法吧
            // ChatManager.DrawColorCodedString(sb, FontAssets.MouseText.Value, modName.Text, position, Color.White, 0f, Vector2.Zero, Vector2.One);

            // 重新开启 SpriteBatch 以去除 Shader
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, sb.GraphicsDevice.BlendState, sb.GraphicsDevice.SamplerStates[0],
                sb.GraphicsDevice.DepthStencilState, sb.GraphicsDevice.RasterizerState, null, Main.UIScaleMatrix);
        }
    }
}
