using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class BagMenu : Menu
    {
        private bool isBagMenuLoaded;
        public Image Arrow;
        public Image Cancel;

        public List<PokemonText> ItemAmounts;

        protected override void AlignMenuItems()
        {
            float dimensionY = 0;

            for (int i = 0; i < Items.Count; i++)
            {
                if (i < Items.Count - 1)
                {
                    ItemAmounts[i].SetPosition(new(948, 96 + dimensionY));
                }
                Items[i].PokemonText.SetPosition(new(548, 76 + dimensionY));
                Items[i].Image.Position = new(172 - Items[i].Image.SourceRect.Width / 2, 624 - Items[i].Image.SourceRect.Height / 2);
                Items[i].Description[0].SetPosition(new(252, 536));
                dimensionY += Items[i].PokemonText.Image.SourceRect.Height + 8;
            }
        }

        public override void LoadContent()
        {
            LoadMenuItems();
            Arrow.LoadContent();
            isBagMenuLoaded = true;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            Arrow.UnloadContent();
            Cancel.UnloadContent();

            for (int i = 0; i < Items.Count; i++)
            {
                if (i < Items.Count - 1)
                {
                    ItemAmounts[i].UnloadContent();
                }

                foreach (PokemonText desc in Items[i].Description)
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

            for (int i = 0; i < Items.Count; i++)
            {
                if (i < Items.Count - 1)
                {
                    ItemAmounts[i].Draw(spriteBatch);
                }

                Items[i].PokemonText.Draw(spriteBatch);
            }

            Arrow.Draw(spriteBatch);
        }

        public void LoadMenuItems()
        {
            if (isBagMenuLoaded)
            {
                if (Cancel.IsLoaded)
                {
                    Cancel.UnloadContent();
                }

                base.UnloadContent();

                for (int i = 0; i < Items.Count; i++)
                {
                    if (i < Items.Count - 1)
                    {
                        ItemAmounts[i].UnloadContent();
                    }

                    foreach (PokemonText desc in Items[i].Description)
                    {
                        desc.UnloadContent();
                    }
                }

                Items.Clear();
                ItemAmounts.Clear();
            }

            Dictionary<string, int> items = new();

            switch (BagMenuManager.CurrentPage)
            {
                case BagMenuManager.BagPage.ITEMS:
                    if (Player.PlayerJsonObject.ItemCounts != null)
                    {
                        foreach (KeyValuePair<string, int> counts in Player.PlayerJsonObject.ItemCounts)
                        {
                            Item item = ItemManager.Instance.GetItem(counts.Key);
                            if (item.ItemType != ItemType.KEY_ITEM && item.ItemType != ItemType.TM && item.ItemType != ItemType.HM && item.ItemType != ItemType.POKE_BALL)
                            {
                                Items.Add(new MenuItem("Item", new PokemonText(item.Name, "Fonts/PokemonFireRedDialogue", new Color(113, 113, 113), new Color(218, 218, 218)))
                                {
                                    Image = item.Icon,
                                    Description = new List<PokemonText>
                                    {
                                        new(item.Description, "Fonts/PokemonFireRedDialogue", new Color(255, 255, 255), new Color(113, 113, 113))
                                    }
                                });
                            }
                            ItemAmounts.Add(new PokemonText("×    " + counts.Value.ToString(), "Fonts/PokemonFireRedSmall", new Color(113, 113, 113), new Color(218, 218, 218)));
                        }
                    }
                    break;
                case BagMenuManager.BagPage.KEY_ITEMS:
                    if (Player.PlayerJsonObject.ItemCounts != null)
                    {
                        foreach (KeyValuePair<string, int> counts in Player.PlayerJsonObject.ItemCounts)
                        {
                            Item item = ItemManager.Instance.GetItem(counts.Key);
                            if (item.ItemType == ItemType.KEY_ITEM)
                            {
                                Items.Add(new MenuItem("Item", new PokemonText(item.Name, "Fonts/PokemonFireRedDialogue", new Color(113, 113, 113), new Color(218, 218, 218)))
                                {
                                    Image = item.Icon,
                                    Description = new List<PokemonText>
                                    {
                                        new(item.Description, "Fonts/PokemonFireRedDialogue", new Color(255, 255, 255), new Color(113, 113, 113))
                                    }
                                });
                            }
                            ItemAmounts.Add(new PokemonText("×    " + counts.Value.ToString(), "Fonts/PokemonFireRedSmall", new Color(113, 113, 113), new Color(218, 218, 218)));
                        }
                    }
                    break;
                case BagMenuManager.BagPage.POKE_BALLS:
                    if (Player.PlayerJsonObject.ItemCounts != null)
                    {
                        foreach (KeyValuePair<string, int> counts in Player.PlayerJsonObject.ItemCounts)
                        {
                            Item item = ItemManager.Instance.GetItem(counts.Key);
                            if (item.ItemType == ItemType.POKE_BALL)
                            {
                                Items.Add(new MenuItem("Item", new PokemonText(item.Name, "Fonts/PokemonFireRedDialogue", new Color(113, 113, 113), new Color(218, 218, 218)))
                                {
                                    Image = item.Icon,
                                    Description = new List<PokemonText>
                                    {
                                        new(item.Description, "Fonts/PokemonFireRedDialogue", new Color(255, 255, 255), new Color(113, 113, 113))
                                    }
                                });
                            }
                            ItemAmounts.Add(new PokemonText("×    " + counts.Value.ToString(), "Fonts/PokemonFireRedSmall", new Color(113, 113, 113), new Color(218, 218, 218)));
                        }
                    }
                    break;
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

            for (int i = 0; i < Items.Count; i++)
            {
                if (i < Items.Count - 1)
                {
                    ItemAmounts[i].LoadContent();
                }

                foreach (PokemonText desc in Items[i].Description)
                {
                    desc.LoadContent();
                }
            }

            base.LoadContent();
        }
    }
}