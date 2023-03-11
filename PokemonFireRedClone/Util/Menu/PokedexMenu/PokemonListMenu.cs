using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace PokemonFireRedClone
{
	public class PokemonListMenu : Menu
	{
        private readonly Dictionary<int, PokemonText> pokemonNames;
        private readonly Dictionary<int, List<Image>> pokemonOwned;
        private readonly List<int> currentShownIndices;
        public Image Arrow;
		public Image PokemonListBackground;
        

        public PokemonListMenu()
        {
            pokemonNames = new();
            pokemonOwned = new();
            currentShownIndices = new();
        }

        protected override void AlignMenuItems()
        {
            float dimensionY = 0;

            foreach (int i in currentShownIndices)
            {
                Items[i - 1].PokemonText.SetPosition(new Vector2(PokemonListBackground.Position.X + 176, PokemonListBackground.Position.Y + dimensionY + 88));
                dimensionY += Items[i - 1].PokemonText.SourceRect.Height + 30;

                pokemonNames[i].SetPosition(new(Items[i - 1].PokemonText.Position.X + Items[i - 1].PokemonText.SourceRect.Width + 84,
                        Items[i - 1].PokemonText.Position.Y - 20));

                if (pokemonOwned.ContainsKey(i))
                {
                    pokemonOwned[i][0].Position = new(Items[i - 1].PokemonText.Position.X + Items[i - 1].PokemonText.SourceRect.Width + 18,
                        pokemonNames[i].Position.Y + 4);

                    int dimensionX = 0;

                    for (int j = 1; j < pokemonOwned[i].Count; j++)
                    {
                        pokemonOwned[i][j].Position = new(pokemonNames[i].Position.X + 256 + dimensionX, pokemonNames[i].Position.Y + 4);
                        dimensionX += pokemonOwned[i][j].SourceRect.Width;
                    }
                }
            }
        }

        public override void LoadContent()
        {
            PokemonListBackground.LoadContent();
            Arrow.LoadContent();
            InitializePokemonList();

            AlignMenuItems();

            Arrow.Position = new Vector2(Items[0].PokemonText.Position.X - Arrow.SourceRect.Width,
                                    Items[0].PokemonText.Position.Y - 8);
        }

        public override void UnloadContent()
        {
            PokemonListBackground.UnloadContent();
            Arrow.UnloadContent();

            foreach (int i in currentShownIndices)
            {
                Items[i - 1].PokemonText.UnloadContent();
                pokemonNames[i].UnloadContent();

                if (pokemonOwned.ContainsKey(i))
                {
                    foreach (Image image in pokemonOwned[i])
                    {
                        image.UnloadContent();
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {

            if ((ItemNumber == 3 && InputManager.Instance.KeyPressed(Keys.W) && currentShownIndices[0] > 1)
                || (ItemNumber == 6 && InputManager.Instance.KeyPressed(Keys.S) && currentShownIndices[^1] < Items.Count))
            {
                int delIndex = ItemNumber == 3 ? currentShownIndices[^1] : currentShownIndices[0];

                Items[delIndex - 1].PokemonText.UnloadContent();
                pokemonNames[delIndex].UnloadContent();

                if (pokemonOwned.ContainsKey(delIndex))
                {
                    foreach (Image image in pokemonOwned[delIndex])
                    {
                        image.UnloadContent();
                    }
                }

                for (int i = 0; i < currentShownIndices.Count; i++)
                {
                    if (ItemNumber == 3)
                    {
                        currentShownIndices[i]--;
                    }
                    else
                    {
                        currentShownIndices[i]++;
                    }
                }

                int createIndex = ItemNumber == 3 ? currentShownIndices[0] : currentShownIndices[^1];

                if (Items[createIndex - 1].PokemonText.IsLoaded)
                {
                    Items[createIndex - 1].PokemonText.ReloadText();
                    pokemonNames[createIndex].ReloadText();

                    if (pokemonOwned.ContainsKey(createIndex))
                    {
                        foreach (Image image in pokemonOwned[createIndex])
                        {
                            image.ReloadTexture();
                        }
                    }
                }
                else
                {
                    Items[createIndex - 1].PokemonText.LoadContent();
                    pokemonNames[createIndex].LoadContent();

                    if (pokemonOwned.ContainsKey(createIndex))
                    {
                        foreach (Image image in pokemonOwned[createIndex])
                        {
                            image.LoadContent();
                        }
                    }
                }

                AlignMenuItems();
            }
            else
            {
                if (InputManager.Instance.KeyPressed(Keys.S) && ItemNumber < 9)
                    ItemNumber++;
                else if (InputManager.Instance.KeyPressed(Keys.W) && ItemNumber > 0)
                    ItemNumber--;
            }

            for (int i = 0; i < currentShownIndices.Count; i++)
            {
                if (i == ItemNumber)
                {
                    Items[currentShownIndices[i] - 1].PokemonText.Image.IsActive = true;
                    Arrow.Position = new Vector2(Items[currentShownIndices[i] - 1].PokemonText.Position.X - Arrow.SourceRect.Width,
                        Items[currentShownIndices[i] - 1].PokemonText.Position.Y - 8);
                }
                else
                {
                    Items[currentShownIndices[i] - 1].PokemonText.Image.IsActive = false;
                }

            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            PokemonListBackground.Draw(spriteBatch);
            Arrow.Draw(spriteBatch);

            foreach (int i in currentShownIndices)
            {
                Items[i - 1].PokemonText.Draw(spriteBatch);
                pokemonNames[i].Draw(spriteBatch);

                if (pokemonOwned.ContainsKey(i))
                {
                    foreach (Image image in pokemonOwned[i])
                    {
                        image.Draw(spriteBatch);
                    }
                }
            }
        }


        private void InitializePokemonList()
        {
            int maxIndexNo = 0;

            foreach (string s in Player.PlayerJsonObject.PokemonSeen)
            {
                Pokemon mon = PokemonManager.Instance.GetPokemon(s);

                pokemonNames.Add(mon.Index, new PokemonText(mon.Name.ToUpper(), "Fonts/PokemonFireRedDialogue", new Color(0,0,0), new Color(224, 216, 192)));

                if (Player.PlayerJsonObject.PokemonOwned.Contains(s))
                {
                    List<Image> types = new();

                    Image pokeball = new()
                    {
                        Path = "Menus/PokedexMenu/OwnedPokeball"
                    };

                    types.Add(pokeball);

                    foreach (Type type in mon.Types)
                        types.Add(TypeProperties.ImageOf(type));

                    pokemonOwned.Add(mon.Index, types);
                }

                if (mon.Index > maxIndexNo)
                {
                    maxIndexNo = mon.Index;
                }
            }

            for (int i = 1; i <= maxIndexNo; i++)
            {
                Items.Add(new MenuItem("PokedexEntry", new PokemonText("No" + i.ToString().PadLeft(3, '0'), "Fonts/PokemonFireRedSmall",
                    new Color(0, 0, 0), new Color(224, 216, 192))));

                if (!pokemonNames.ContainsKey(i))
                {
                    pokemonNames.Add(i, new PokemonText("-----", "Fonts/PokemonFireRedDialogue", new Color(0, 0, 0), new Color(224, 216, 192)));
                }

                if (currentShownIndices.Count < 10)
                {
                    currentShownIndices.Add(i);
                    Items[i - 1].PokemonText.LoadContent();
                    pokemonNames[i].LoadContent();

                    if (pokemonOwned.ContainsKey(i))
                    {
                        foreach (Image image in pokemonOwned[i])
                        {
                            image.LoadContent();
                        }
                    }
                }
            }
        }
    }
}

