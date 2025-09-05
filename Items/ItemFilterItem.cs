using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Initializers;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MechTransfer.Items
{
    [Autoload(false)]
    public class ItemFilterItem : ModItem
    {
        public override string Texture => (GetType().Namespace + "." + Name).Replace('.', '/');

        public delegate bool MatchCondition(Item item);

        public int recipeItem = -1;
        public int Rarity = 0;
        public bool expert = false;
        public bool master = false;
        public Texture2D categoryTexture = null;
        public float customCategoryScale = 0;
        public string TextureName = "BaseFilterItem";

        private MatchCondition matchCondition;

        protected override bool CloneNewInstances => true;

        public override string Name { get; }

        public ItemFilterItem(string name, MatchCondition matchCondition)
        {
            Name = name;
            this.matchCondition = matchCondition;
        }

        public override LocalizedText DisplayName
        {
            get
            {
                if (Main.halloween && Name == "DyeFilterItem") //Easter egg name handling
                    return Language.GetText("Mods.MechTransfer.Items.DyeFilterItem.EasterEggName");
                return base.DisplayName;
            }
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = Rarity;
            Item.expert = expert;
            Item.master = master;
        }

        public bool MatchesItem(Item item)
        {
            return matchCondition(item);
        }

        public override void AddRecipes()
        {
            if (recipeItem != -1)
            {
                Recipe r = CreateRecipe();
                r.AddIngredient(Mod.Find<ModItem>("AnyFilterItem").Item.type, 1);
                r.AddIngredient(recipeItem, 1);
                r.AddTile(TileID.WorkBenches);
                r.Register();
            }
        }

        private void SetColor()
        {
            int rareType = Item.rare;
            Color newColor = Color.White;
            ModRarity modRare = RarityLoader.GetRarity(Rarity);

            if (modRare != null)
            {
                newColor = modRare.RarityColor;
            }

            else if (expert)
            {
                newColor.R = (byte)Main.DiscoR;
                newColor.G = (byte)Main.DiscoG;
                newColor.B = (byte)Main.DiscoB;
            }

            else if (master)
            {
                float colorOffset = ((float)Main.mouseTextColor) / 255f;
                newColor = new Color((byte)(255f * colorOffset), (int)(byte)(Main.masterColor * 200f * colorOffset), 0, 255);
            }

            else
            {
                newColor = ItemRarity.GetColor(rareType);
            }

            Item.color = newColor;
        }

        public void SetCategoryTexture(int type)
        {
            ModItem modItem = ModContent.GetModItem(type);
            if (modItem != null)
            {
                SetCategoryTexture(modItem.Texture);
            }

            else
            {
                SetCategoryTexture($"Terraria/Images/Item_{type}");
            }
        }

        public void SetCategoryTexture(string textureName)
        {
            if (textureName == null)
                return;

            try 
            {
                categoryTexture = ModContent.Request<Texture2D>(textureName, AssetRequestMode.ImmediateLoad).Value;
            }

            catch
            {

            }
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            SetColor();

            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            SetColor();

            return false;
        }

        private void Draw(SpriteBatch spriteBatch, Vector2 position, Color drawColor, Vector2 origin, float scale, float rotation)
        {
            Texture2D filterTexture = ModContent.Request<Texture2D>((GetType().Namespace + "." + TextureName).Replace('.', '/'), AssetRequestMode.ImmediateLoad).Value;
            Color filterColor = new Color(((float)drawColor.R / 255f) * ((float)Item.color.R / 255f),
                                          ((float)drawColor.G / 255f) * ((float)Item.color.G / 255f),
                                          ((float)drawColor.B / 255f) * ((float)Item.color.B / 255f)); //If you use "itemColor", the item in the frame may flicker and behave strangely in low light.

            spriteBatch.Draw(filterTexture, position, null, filterColor, rotation, origin, scale, 0, 0);

            if (categoryTexture != null)
            {
                int categoryHeight = categoryTexture.Height;
                int categoryWidth = categoryTexture.Width;

                float drawOffset = 21 - 16;
                float categoryScale = customCategoryScale == 0 ? scale * (26f / (float)Math.Max(categoryWidth, categoryHeight)) : scale * customCategoryScale;

                spriteBatch.Draw(categoryTexture, position, null, drawColor, rotation, new Vector2(categoryWidth / 2 - (drawOffset), categoryHeight / 2 - (drawOffset)), categoryScale, 0, 0);
            }
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Draw(spriteBatch, position, drawColor, origin, scale, 0);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Vector2 position = Item.position - Main.screenPosition + new Vector2(Item.width / 2 - 0.5f, Item.height / 2 - 0.5f);
            Draw(spriteBatch, position, lightColor, new Vector2(Item.width / 2, Item.height / 2), scale, rotation);
        }
    }
}