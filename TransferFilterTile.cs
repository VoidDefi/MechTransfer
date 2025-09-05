using MechTransfer.Items;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MechTransfer.Tiles
{
    [Autoload(false)]
    public class TransferFilterTile : FilterableTile<TransferFilterTileEntity>, ITransferPassthrough
    {
        private Dictionary<int, ItemFilterItem> filterItems = new Dictionary<int, ItemFilterItem>();

        private HashSet<int> Bags;

        public override void PostSetDefaults()
        {
            AddMapEntry(MapColors.Passthrough, GetPlaceItem(0).DisplayName);

            ModContent.GetInstance<TransferAgent>().passthroughs.Add(Type, this);
            ModContent.GetInstance<TransferPipeTile>().connectedTiles.Add(Type);

            base.PostSetDefaults();
        }

        protected override void SetTileObjectData()
        {
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.None, 0, 0);
            base.SetTileObjectData();
        }

        public bool ShouldPassthrough(Point16 location, Item item)
        {
            TransferFilterTileEntity TE;
            if (TryGetEntity(location.X, location.Y, out TE))
            {
                ItemFilterItem filterItem;
                if (filterItems.TryGetValue(TE.item.type, out filterItem))
                {
                    if (Main.tile[location.X, location.Y].TileFrameY == 0)
                        return filterItem.MatchesItem(item);
                    else
                        return !filterItem.MatchesItem(item);
                }
                else
                {
                    if (Main.tile[location.X, location.Y].TileFrameY == 0)
                        return TE.item.type == item.type;
                    else
                        return TE.item.type != item.type;
                }
            }
            return false;
        }

        public override string HoverText(TransferFilterTileEntity entity)
        {
            Tile tile = Main.tile[entity.Position.X, entity.Position.Y];
            if (tile.TileFrameY == 0)
                return Language.GetTextValue("Mods.MechTransfer.UI.Hover.TransferFilterItem");
            else
                return Language.GetTextValue("Mods.MechTransfer.UI.Hover.InverseTransferFilterItem");
        }

        public override int GetDropKind(int Fx, int Fy)
        {
            return Fy / tileObjectData.CoordinateFullHeight;
        }

        public override void PostLoad()
        {
            PlaceItems = new ModItem[2];

            //Filter
            PlaceItems[0] = SimplePrototypeItem.MakePlaceable(Mod, "TransferFilterItem", Type, 16, 16, 0);

            //InverseFilter
            PlaceItems[1] = SimplePrototypeItem.MakePlaceable(Mod, "InverseTransferFilterItem", Type, 16, 16, 1);

            LoadFilters();
        }

        public override void AddRecipes()
        {
            //Filter
            Recipe r = Recipe.Create(PlaceItems[0].Item.type, 1);
            r.AddIngredient(ModContent.ItemType<PneumaticActuatorItem>(), 1);
            r.AddIngredient(ItemID.Actuator, 1);
            r.AddIngredient(ItemID.ItemFrame, 1);
            r.AddTile(TileID.WorkBenches);
            r.Register();
            Recipe r2 = Recipe.Create(PlaceItems[0].Item.type, 1);
            r2.AddIngredient(PlaceItems[1]);
            r2.Register();

            //InverseFilter
            r = Recipe.Create(PlaceItems[1].Item.type, 1);
            r.AddIngredient(ModContent.ItemType<PneumaticActuatorItem>(), 1);
            r.AddIngredient(ItemID.Actuator, 1);
            r.AddIngredient(ItemID.ItemFrame, 1);
            r.AddTile(TileID.WorkBenches);
            r.Register();
            r2 = Recipe.Create(PlaceItems[1].Item.type, 1);
            r2.AddIngredient(PlaceItems[0]);
            r2.Register();

            LoadBagFilter();

            string logFilterTestsEnvVar = Environment.GetEnvironmentVariable("MECHTRANSFER_LOGFILTERTESTS");
            if (logFilterTestsEnvVar != null && logFilterTestsEnvVar.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
                LogFilterTests();
        }

        private ItemFilterItem createFilter(string type, int recipeItem, ItemFilterItem.MatchCondition condition, bool categoryTexture = true, int otherCategoryTexture = -1)
        {
            ItemFilterItem filter = new ItemFilterItem(type + "FilterItem", condition);
            filter.recipeItem = recipeItem;

            if (categoryTexture)
            {
                if (otherCategoryTexture > -1)
                {
                    filter.SetCategoryTexture(otherCategoryTexture);
                }

                else if (recipeItem > -1)
                {
                    filter.SetCategoryTexture(recipeItem);
                }
            }

            Mod.AddContent(filter);

            //Nebula: Easter egg handling is now done at runtime by ItemFilterItem.DisplayName itself
            filterItems.Add(filter.Item.type, filter);

            return filter;
        }

        private void LoadFilters()
        {
            string iconsPath = "MechTransfer/Items/Icons/";

            //Main

            createFilter("Any", -1, x => true).TextureName = "AnyFilter";

            //Rarities

            createFilter("Rarity-Gray", ItemID.GrayPaint, x => x.rare == -1, false).Rarity = -1;
            createFilter("Rarity-White", ItemID.WhitePaint, x => x.rare == 0, false).Rarity = 0;
            createFilter("Rarity-Blue", ItemID.BluePaint, x => x.rare == 1, false).Rarity = 1;
            createFilter("Rarity-Green", ItemID.GreenPaint, x => x.rare == 2, false).Rarity = 2;
            createFilter("Rarity-Orange", ItemID.OrangePaint, x => x.rare == 3, false).Rarity = 3;
            createFilter("Rarity-LightRed", ItemID.RedPaint, x => x.rare == 4, false).Rarity = 4;
            createFilter("Rarity-Pink", ItemID.PinkPaint, x => x.rare == 5, false).Rarity = 5;
            createFilter("Rarity-LightPurple", ItemID.PurplePaint, x => x.rare == 6, false).Rarity = 6;
            createFilter("Rarity-Lime", ItemID.LimePaint, x => x.rare == 7, false).Rarity = 7;
            createFilter("Rarity-Yellow", ItemID.YellowPaint, x => x.rare == 8, false).Rarity = 8;
            createFilter("Rarity-Cyan", ItemID.CyanPaint, x => x.rare == 9, false).Rarity = 9;
            createFilter("Rarity-Red", ItemID.RedPaint, x => x.rare == 10, false).Rarity = 10;
            createFilter("Rarity-Purple", ItemID.PurplePaint, x => x.rare == 11, false).Rarity = 11;
            createFilter("Rarity-Rainbow", ItemID.DemonHeart, x => x.expert == true, false).expert = true;
            createFilter("Rarity-FieryRed", 4924, x => x.master == true, false).master = true;
            createFilter("Rarity-Amber", ItemID.Amber, x => x.rare == -11, false).Rarity = -11;

            //Equipment

            createFilter("Equipable", ItemID.Shackle, x => (x.headSlot >= 0 || x.bodySlot >= 0 || x.legSlot >= 0 || x.accessory || Main.projHook[x.shoot] || x.mountType >= 0 || x.dye > 0 || (x.buffType > 0 && (Main.lightPet[x.buffType] || Main.vanityPet[x.buffType]))), false).SetCategoryTexture(iconsPath + "EquipmentIcon");
            createFilter("Armor", ItemID.WoodBreastplate, x => ((x.headSlot >= 0 || x.bodySlot >= 0 || x.legSlot >= 0) && !x.vanity));
            createFilter("Vanity", ItemID.FamiliarWig, x => (x.vanity));
            createFilter("Accessory", ItemID.Shackle, x => (x.accessory)).customCategoryScale = 0.8f;
            Main.checkHalloween();
            createFilter("Dye", ItemID.SilverDye, x => (x.dye > 0)).customCategoryScale = 0.7f;

            //One use

            createFilter("Ammo", ItemID.MusketBall, x => x.ammo != 0).customCategoryScale = 1f;
            createFilter("Bait", ItemID.ApprenticeBait, x => x.bait > 0).customCategoryScale = 1f;
            createFilter("Money", ItemID.GoldCoin, x => x.type == ItemID.CopperCoin || x.type == ItemID.SilverCoin || x.type == ItemID.GoldCoin || x.type == ItemID.PlatinumCoin).customCategoryScale = 1;
            createFilter("Bag", ItemID.WoodenCrate, x => Bags.Contains(x.type)).customCategoryScale = 0.5f;

            //Many use 

            createFilter("Tool", ItemID.CopperPickaxe, x => x.pick > 0 || x.axe > 0 || x.hammer > 0, false).SetCategoryTexture(iconsPath + "ToolIcon");
            createFilter("Weapon", ItemID.CopperShortsword, x => x.damage > 0 && x.pick == 0 && x.axe == 0 && x.hammer == 0, false).SetCategoryTexture(iconsPath + "WeaponIcon");

            //Weapons

            createFilter("Melee", ItemID.CopperShortsword, x => x.DamageType == DamageClass.Melee);
            createFilter("Magic", ItemID.LesserManaPotion, x => x.DamageType == DamageClass.Magic, true, ItemID.AmethystStaff);
            createFilter("Ranged", ItemID.WoodenBow, x => x.DamageType == DamageClass.Ranged);
            createFilter("Summon", ItemID.SummoningPotion, x => x.DamageType == DamageClass.Summon, true, ItemID.SlimeStaff);
            createFilter("Thrown", ItemID.Shuriken, x => x.DamageType == DamageClass.Throwing).customCategoryScale = 1;

            //Consumes

            createFilter("Consumable", ItemID.PumpkinPie, x => x.consumable, true, ItemID.Glowstick).customCategoryScale = 0.8f;
            createFilter("Material", ItemID.Wood, x => x.material).customCategoryScale = 0.7f;
            createFilter("Potion", ItemID.LesserHealingPotion, x => x.consumable && (x.healLife > 0 || x.healMana > 0 || x.buffType > 0)).customCategoryScale = 0.8f;

            //Places

            createFilter("Tile", ItemID.DirtBlock, x => x.createTile > -1).customCategoryScale = 0.8f;
            createFilter("Wall", ItemID.WoodWall, x => x.createWall > 0).customCategoryScale = 0.5f;
        }

        private void LoadBagFilter()
        {
            Bags =
            [
                ItemID.HerbBag,
                ItemID.CanOfWorms,
                ItemID.Oyster,

                ItemID.GoodieBag,
                ItemID.Present,
                ItemID.BluePresent,
                ItemID.GreenPresent,
                ItemID.YellowPresent,

                ItemID.KingSlimeBossBag,
                ItemID.EyeOfCthulhuBossBag,
                ItemID.EaterOfWorldsBossBag,
                ItemID.BrainOfCthulhuBossBag,
                ItemID.QueenBeeBossBag,
                ItemID.DeerclopsBossBag,
                ItemID.WallOfFleshBossBag,
                ItemID.SkeletronBossBag,
                ItemID.DestroyerBossBag,
                ItemID.TwinsBossBag,
                ItemID.SkeletronPrimeBossBag,
                ItemID.PlanteraBossBag,
                ItemID.GolemBossBag,
                ItemID.FishronBossBag,
                ItemID.CultistBossBag,
                ItemID.MoonLordBossBag,
                ItemID.BossBagBetsy,
                ItemID.FairyQueenBossBag,
                ItemID.QueenSlimeBossBag,
                ItemID.BossBagDarkMage,
                ItemID.BossBagOgre,

                ItemID.LockBox,
                ItemID.ObsidianLockbox,
                ItemID.WoodenCrate,
                ItemID.IronCrate,
                ItemID.GoldenCrate,
                ItemID.JungleFishingCrate,
                ItemID.FloatingIslandFishingCrate,
                ItemID.CorruptFishingCrate,
                ItemID.CrimsonFishingCrate,
                ItemID.HallowedFishingCrate,
                ItemID.DungeonFishingCrate,
                ItemID.FrozenCrate,
                ItemID.OasisCrate,
                ItemID.LavaCrate,
                ItemID.OceanCrate,
                ItemID.WoodenCrateHard,
                ItemID.IronCrateHard,
                ItemID.GoldenCrateHard,
                ItemID.JungleFishingCrateHard,
                ItemID.FloatingIslandFishingCrateHard,
                ItemID.CorruptFishingCrateHard,
                ItemID.CrimsonFishingCrateHard,
                ItemID.HallowedFishingCrateHard,
                ItemID.DungeonFishingCrateHard,
                ItemID.FrozenCrateHard,
                ItemID.OasisCrateHard,
                ItemID.LavaCrateHard,
                ItemID.OceanCrateHard
            ];

            for (int i = 0; i < ItemLoader.ItemCount; i++)
            {
                ModItem item = ItemLoader.GetItem(i);
                if (item != null && item.GetType().GetMethod("ModifyItemLoot").DeclaringType != typeof(ModItem))
                {
                    Bags.Add(i);
                }
            }
        }

        private void LogFilterTests()
        {
            Mod.Logger.Debug("---BEGIN FILTER LISTING---");
            foreach (var item in filterItems)
            {
				Mod.Logger.Debug("----" + item.Value.Name);
                foreach (Item testItem in ContentSamples.ItemsByType.Values)
                {
                    if (item.Value.MatchesItem(testItem))
						Mod.Logger.Debug(testItem.Name);
                }
            }
			Mod.Logger.Debug("---END FILTER LISTING---");
        }
    }
}