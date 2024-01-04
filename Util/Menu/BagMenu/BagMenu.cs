using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class BagMenu : Menu
    {
        public Image Arrow;
        public Image Cancel;

        protected override void AlignMenuItems()
        {
            float dimensionY = 0;

            foreach (MenuItem item in Items)
            {
                item.PokemonText.SetPosition(new(548, 76 + dimensionY));
                item.Image.Position = new(172 - item.Image.SourceRect.Width / 2, 624 - item.Image.SourceRect.Height / 2);
                item.Description[0].SetPosition(new(252, 536));
                dimensionY += item.PokemonText.Image.SourceRect.Height + 8;
            }
        }

        public override void LoadContent()
        {
            foreach (string itemName in Player.PlayerJsonObject.Items)
            {
                Item item = ItemManager.Instance.GetItem(itemName);
                Items.Add(new MenuItem("Item", new PokemonText(item.Name.ToUpper(), "Fonts/PokemonFireRedDialogue", new Color(113, 113, 113), new Color(218, 218, 218)))
                {
                    Image = item.Icon,
                    Description = new List<PokemonText>
                    {
                        new(item.Description, "Fonts/PokemonFireRedDialogue", new Color(255, 255, 255), new Color(113, 113, 113))
                    }
                });
            }

            Items.Add(new MenuItem("Screen", new PokemonText("CANCEL", "Fonts/PokemonFireRedDialogue", new Color(113, 113, 113), new Color(218, 218, 212)))
            {
                LinkID = "GameplayScreen",
                Image = Cancel,
                Description = new List<PokemonText>
                {
                    new("CLOSE   BAG", "Fonts/PokemonFireRedDialogue", new Color(255, 255, 255), new Color(113, 113, 113))
                }
            });

            foreach (MenuItem item in Items)
            {
                foreach (PokemonText desc in item.Description)
                {
                    desc.LoadContent();
                }
            }

            base.LoadContent();
            Arrow.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            Arrow.UnloadContent();

            foreach (MenuItem item in Items)
            {
                foreach (PokemonText desc in item.Description)
                {
                    desc.UnloadContent();
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Instance.KeyPressed(Keys.S))
                ItemNumber++;
            else if (InputManager.Instance.KeyPressed(Keys.W))
                ItemNumber--;


            if (ItemNumber < 0)
                ItemNumber = Items.Count - 1;
            else if (ItemNumber > Items.Count - 1)
                ItemNumber = 0;

            for (int i = 0; i < Items.Count; i++)
            {
                if (i == ItemNumber)
                {
                    Items[i].PokemonText.Image.IsActive = true;
                    Arrow.Position = new Vector2(Items[i].PokemonText.Position.X - Arrow.SourceRect.Width,
                        Items[i].PokemonText.Position.Y + (Items[i].PokemonText.SourceRect.Height / 4) - 2);
                    Items[i].Description[0].Image.IsActive = true;

                }
                else
                {
                    Items[i].PokemonText.Image.IsActive = false;
                    Items[i].Description[0].Image.IsActive = false;
                }

                Items[i].Description[0].Image.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            Items[ItemNumber].Image.Draw(spriteBatch);
            Items[ItemNumber].Description[0].Draw(spriteBatch);

            foreach (MenuItem item in Items)
            {
                item.PokemonText.Draw(spriteBatch);
            }

            Arrow.Draw(spriteBatch);
        }
    }
}

