using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
	public class PokemonListMenu : Menu
	{
        private readonly List<Pokemon> pokemon;
        private readonly Dictionary<int, PokemonText> pokemonNames;
        private readonly Dictionary<string, List<Image>> ownedPokemon;
        private readonly List<int> currentShownIndices;
        public Image Arrow;
		public Image PokemonListBackground;
        

        public PokemonListMenu()
        {
            pokemon = new();
            pokemonNames = new();
            ownedPokemon = new();
            currentShownIndices = new();
        }

        protected override void AlignMenuItems()
        {
            float dimensionY = 0;

            foreach (int i in currentShownIndices)
            {
                Items[i-1].PokemonText.SetPosition(new Vector2(PokemonListBackground.Position.X + 176, PokemonListBackground.Position.Y + dimensionY + 88));
                dimensionY += Items[i-1].PokemonText.SourceRect.Height + 30;

                if (pokemonNames.ContainsKey(i))
                {
                    pokemonNames[i].SetPosition(new (Items[i-1].PokemonText.Position.X + Items[i-1].PokemonText.SourceRect.Width + 72,
                        Items[i-1].PokemonText.Position.Y - 20));
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
                }
                else
                {
                    Items[createIndex - 1].PokemonText.LoadContent();
                    pokemonNames[createIndex].LoadContent();
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
            }
        }


        private void InitializePokemonList()
        {
            int maxIndexNo = 0;

            foreach (string s in Player.PlayerJsonObject.PokemonSeen)
            {
                Pokemon mon = PokemonManager.Instance.GetPokemon(s);

                pokemon.Add(mon);
                pokemonNames.Add(mon.Index, new PokemonText(mon.Name.ToUpper(), "Fonts/PokemonFireRedDialogue", new Color(0,0,0), new Color(224, 216, 192)));

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
                }
            }
        }
    }
}

