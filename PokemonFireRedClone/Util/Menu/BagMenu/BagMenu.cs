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
            foreach (MenuItem item in Items)
            {
                item.PokemonText.SetPosition(new(548, 76));
                item.Image.Position = new(172 - item.Image.SourceRect.Width / 2, 624 - item.Image.SourceRect.Height / 2);
                item.Description[0].SetPosition(new(252, 536));
            }
        }

        public override void LoadContent()
        {
            Items.Add(new MenuItem("Screen", new PokemonText("CANCEL", "Fonts/PokemonFireRedDialogue", new Color(113, 113, 113), new Color(218, 218, 212))));
            Items[0].LinkID = "GameplayScreen";
            Items[0].Image = Cancel;
            Items[0].Description = new List<PokemonText>();
            Items[0].Description.Add(new PokemonText("CLOSE   BAG", "Fonts/PokemonFireRedDialogue", new Color(255, 255, 255), new Color(113, 113, 113)));

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
            base.Draw(spriteBatch);

            foreach (MenuItem item in Items)
            {
                foreach (PokemonText text in item.Description)
                {
                    text.Draw(spriteBatch);
                }
            }

            Arrow.Draw(spriteBatch);
        }
    }
}

