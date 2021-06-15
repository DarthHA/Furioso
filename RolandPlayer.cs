using Furioso.Buffs;
using Furioso.Projectiles.Allas;
using Furioso.Projectiles.Crystal;
using Furioso.Projectiles.OldBoys;
using Furioso.Projectiles.Ranga;
using Furioso.Projectiles.SuperAttack;
using Furioso.Projectiles.Zelkova;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Furioso
{
    public class RolandPlayer : ModPlayer
    {

        //储存
        public bool IsUsingGlove = false;
        public bool IsUsingGloveVanity = false;
        public bool ItemSaved = false;
        public Item[] SavedItem = new Item[10];
        public int[] WeaponCD = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public bool MaskImmune = false;

        public bool Ram = false;
        public int WhatUAreUsing = -1;
        public bool FuriosoAttacking = false;

        public BlackSilenceCounter BlackSilenceCounter = new BlackSilenceCounter();

        public bool CrystalDodge = false;

        public override void PreUpdateMovement()
        {
            if (IsUsingGlove)
            {
                if (Ram && WhatUAreUsing != -1)
                {
                    if (WhatUAreUsing == ModContent.ProjectileType<RangaHeld>())
                    {
                        Projectile ranga = Main.projectile[AnyProj(ModContent.ProjectileType<RangaHeld>())];
                        player.velocity.X = player.direction * (ranga.modProjectile as RangaHeld).vel;
                        player.velocity.Y = -0.01f;
                    }
                    else if (WhatUAreUsing == ModContent.ProjectileType<CrystalHeld>())
                    {
                        Projectile crystal = Main.projectile[AnyProj(ModContent.ProjectileType<CrystalHeld>())];
                        player.velocity.X = player.direction * (crystal.modProjectile as CrystalHeld).vel;
                        player.velocity.Y = -0.01f;
                    }
                    else if (WhatUAreUsing == ModContent.ProjectileType<CrystalHeldSpecial>())
                    {
                        Projectile crystal = Main.projectile[AnyProj(ModContent.ProjectileType<CrystalHeldSpecial>())];
                        player.velocity.X = player.direction * (crystal.modProjectile as CrystalHeldSpecial).vel;
                        player.velocity.Y = -0.01f;
                    }
                    else if (WhatUAreUsing == ModContent.ProjectileType<AllasHeld>())
                    {
                        Projectile allas = Main.projectile[AnyProj(ModContent.ProjectileType<AllasHeld>())];
                        player.velocity = allas.rotation.ToRotationVector2() * (allas.modProjectile as AllasHeld).vel;
                    }
                }
            }
        }

        public override void SetControls()
        {
            if (IsUsingGlove)
            {
                if (Ram)
                {
                    player.controlUp = false;
                    player.controlDown = false;
                    player.controlHook = false;
                    player.controlJump = false;
                    player.controlLeft = false;
                    player.controlMount = false;
                    player.controlRight = false;
                    player.mount.Dismount(player);
                    player.grappling[0] = -1;
                    player.grapCount = 0;
                    player.noFallDmg = true;
                    for (int i = 0; i < 1000; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].aiStyle == 7)
                        {
                            Main.projectile[i].Kill();
                        }
                    }
                }
                else if (FuriosoAttacking)
                {
                    player.controlHook = false;
                    player.controlMount = false;
                    player.mount.Dismount(player);
                    player.grappling[0] = -1;
                    player.grapCount = 0;
                    player.noFallDmg = true;
                    for (int i = 0; i < 1000; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].aiStyle == 7)
                        {
                            Main.projectile[i].Kill();
                        }
                    }
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (IsUsingGlove)
            {
                if (player.IsBlackSilence())
                {
                    player.AddBuff(ModContent.BuffType<BlackSilenceBuff>(), 2);
                }
                if (!player.GetModPlayer<RolandPlayer>().FuriosoAttacking)
                {
                    player.AddBuff(ModContent.BuffType<BlackSilenceCounterBuff>(), 2);
                }

                if (player.itemAnimation == 0)
                {
                    for(int i = 0; i < 10; i++)
                    {
                        if (WeaponCD[i] > 0)
                        {
                            WeaponCD[i]--;
                        }
                    }
                    MaskImmune = false;
                }

                if (Ram || FuriosoAttacking)
                {
                    player.gravControl = false;
                    player.gravControl2 = false;
                    player.gravDir = 1;
                    player.noFallDmg = true;
                }
                if (FuriosoAttacking)
                {
                    player.AddBuff(ModContent.BuffType<FuriosoBuff>(), 2);
                    InfiniteMove();
                }
                if (WhatUAreUsing != ModContent.ProjectileType<CrystalHeld>())
                {
                    CrystalDodge = false;
                }

                if (!ItemSaved)
                {
                    for(int i = 0; i < 10; i++)
                    {
                        SavedItem[i] = player.inventory[i].DeepClone();
                        player.inventory[i].SetDefaults(FuriosoList.WeaponItems[i]);
                    }
                    ItemSaved = true;
                }

                for (int i = 0; i < 10; i++)
                {
                    if (player.inventory[i].IsAir)
                    {
                        if (!player.HasItem(FuriosoList.WeaponItems[i]))
                        {
                            player.inventory[i].SetDefaults(FuriosoList.WeaponItems[i]);
                        }
                    }
                }
            }
            else
            {
                if (ItemSaved)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (player.inventory[i].type == FuriosoList.WeaponItems[i] || player.inventory[i].IsAir)
                        {
                            player.inventory[i] = SavedItem[i].DeepClone();
                        }
                        else
                        {
                            GrabItem(player, SavedItem[i].DeepClone());
                        }
                        SavedItem[i].TurnToAir();
                    }
                    ItemSaved = false;
                }
                QuickStop();
            }
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            if (IsUsingGlove && FuriosoAttacking)
            {
                return false;
            }

            return true;
        }

        public override bool CanBeHitByProjectile(Projectile proj)
        {
            if (IsUsingGlove && FuriosoAttacking)
            {
                return false;
            }
            return true;
        }

        public override void PreSavePlayer()
        {
            if (ItemSaved)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (player.inventory[i].type == FuriosoList.WeaponItems[i] || player.inventory[i].IsAir)
                    {
                        player.inventory[i] = SavedItem[i].DeepClone();
                    }
                    else
                    {
                        GrabItem(player, SavedItem[i].DeepClone());
                    }
                    SavedItem[i].TurnToAir();
                }
                ItemSaved = false;
            }
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            if (IsUsingGlove)
            {
                if (!player.GetModPlayer<RolandPlayer>().FuriosoAttacking)
                {
                    if (AnyProj(ModContent.ProjectileType<OldBoysSwing>()) != -1)
                    {
                        Projectile swing = Main.projectile[AnyProj(ModContent.ProjectileType<OldBoysSwing>())];
                        damage -= (int)(swing.damage * DamageValue.OldBoysBlock);
                        crit = false;
                        BlackSilenceCounter.AddCounter(BlackSilenceCounter.Attack.OldBoys);
                        if (swing.localAI[1] == 0)
                        {
                            swing.localAI[1] = 1;
                            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/OldBoysGuard"), player.Center);
                            Projectile.NewProjectile((npc.Center + player.Center) / 2, Vector2.Zero, ModContent.ProjectileType<OldBoysGuard>(), 0, 0, 0);
                        }
                    }
                }
            }
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (!IsUsingGlove) return true;

            if (FuriosoAttacking)
            {
                return false;
            }
            if (WhatUAreUsing != -1)
            {
                if (!CrystalDodge && WhatUAreUsing == ModContent.ProjectileType<CrystalHeld>())
                {
                    CrystalDodge = true;
                    BlackSilenceCounter.AddCounter(BlackSilenceCounter.Attack.Crystal);
                    player.statLife += damage;
                    if (player.statLife > player.statLifeMax2)
                    {
                        player.statLife = player.statLifeMax2;
                    }
                    player.HealEffect(damage);
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/CrystalEvasion"), player.Center);
                }
                return false;
            }
            if (MaskImmune)
            {
                return false;
            }
            return true;
        }
        public override void ResetEffects()
        {
            if (IsUsingGlove)
            {
                if (WhatUAreUsing != -1)
                {
                    if (AnyProj(WhatUAreUsing) == -1)
                    {
                        Ram = false;
                        WhatUAreUsing = -1;
                    }
                }
                else
                {
                    Ram = false;
                }
                if (FuriosoAttacking)
                {
                    if (AnyProj(ModContent.ProjectileType<SuperAttackHeld>()) == -1)
                    {
                        FuriosoAttacking = false;
                    }
                }
            }
            IsUsingGlove = false;
            IsUsingGloveVanity = false;
        }
        public override void UpdateDead()
        {
            IsUsingGloveVanity = false;
            QuickStop(true);
        }


        public override void ModifyDrawInfo(ref PlayerDrawInfo drawInfo)
        {
            if (IsUsingGlove)
            {
                if (WhatUAreUsing == ModContent.ProjectileType<RangaHeld>())
                {
                    drawInfo.drawHeldProjInFrontOfHeldItemAndBody = true;
                }
            }
        }


        public override void FrameEffects()
        {
            if (IsUsingGloveVanity)
            {
                player.handoff = (sbyte)mod.GetEquipSlot("BlackSilenceHandsOff", EquipType.HandsOff);
                player.handon = (sbyte)mod.GetEquipSlot("BlackSilenceHandsOn", EquipType.HandsOn);
                if (player.IsBlackSilence())
                {
                    if (player.statLife <= player.statLifeMax2 / 2)
                    {
                        player.face = (sbyte)mod.GetEquipSlot("BlackSilenceMask", EquipType.Face);
                    }
                    player.head = 0;
                    player.body = 10;
                    player.legs = 10;
                }
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (IsUsingGlove && player.IsBlackSilence())
            {
                if (Language.ActiveCulture == GameCulture.Chinese)
                {
                    damageSource = PlayerDeathReason.ByCustomReason("一个男人顺着下水道的水流涌了上来。");
                }
                else
                {
                    damageSource = PlayerDeathReason.ByCustomReason("A man drifts down the gutter.");
                }
            }
            return true;
        }


        public void InfiniteMove()
        {
            player.wingTime = player.wingTimeMax;
            player.rocketTime = player.rocketTimeMax;
            if (player.doubleJumpBlizzard)
            {
                player.jumpAgainBlizzard = true;
            }
            if (player.doubleJumpCloud)
            {
                player.jumpAgainCloud = true;
            }
            if (player.doubleJumpFart)
            {
                player.jumpAgainFart = true;
            }
            if (player.doubleJumpSail)
            {
                player.jumpAgainSail = true;
            }
            if (player.doubleJumpSandstorm)
            {
                player.jumpAgainSandstorm = true;
            }
        }

        public void QuickStop(bool ResetCD = false)
        {
            WhatUAreUsing = -1;
            MaskImmune = false;
            CrystalDodge = false;
            FuriosoAttacking = false;
            Ram = false;
            BlackSilenceCounter.Clear();
            if (ResetCD)
            {
                for(int i = 0; i < 10; i++)
                {
                    WeaponCD[i] = 0;
                }
            }
            foreach(Projectile proj in Main.projectile)
            {
                if(proj.active)
                {
                    if (FuriosoList.SpecialProjs.Contains(proj.type))
                    {
                        proj.active = false;
                    }
                }
            }
        }

        private int AnyProj(int type)
        {
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.type == type && proj.owner == player.whoAmI)
                {
                    return proj.whoAmI;
                }
            }
            return -1;
        }

        private void GrabItem(Player player,Item item)
        {
            for(int i = 10; i < 54; i++)
            {
                if (player.inventory[i].IsAir)
                {
                    player.inventory[i] = item.DeepClone();
                    break;
                }
            }
        }
    }
}