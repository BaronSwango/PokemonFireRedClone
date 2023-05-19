using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
	public class PokemonListMenu : Menu
	{
        private readonly Dictionary<int, PokemonText> pokemonNames;
        private readonly Dictionary<int, List<Image>> pokemonOwned;
        private readonly List<int> currentShownIndices;
        public Image Arrow;
		public Image PokemonListBackground;
        public Image PokedexBackground;

        public Image ArrowUp;
        public Image ArrowDown;
        private float arrowDownOriginalY;
        private float arrowUpOriginalY;
        private float arrowOffset;
        private bool increase;

        private bool autoScroll;
        private const int scrollWaitTime = 300;
        private const int autoScrollSpeed = 10;
        private Counter scrollTimer;

        private static int SavedIndex
        {
            get { return ((PokedexScreen)ScreenManager.Instance.CurrentScreen).SavedSearchIndex; }
            set { ((PokedexScreen)ScreenManager.Instance.CurrentScreen).SavedSearchIndex = value; }
        }

        private static int NumItemsBeforeSavedIndex
        {
            get { return ((PokedexScreen)ScreenManager.Instance.CurrentScreen).NumItemsBeforeSavedIndex; }
            set { ((PokedexScreen)ScreenManager.Instance.CurrentScreen).NumItemsBeforeSavedIndex = value; }
        }

        private static bool IsTransitioning
        {
            get { return ((PokedexScreen)ScreenManager.Instance.CurrentScreen).MenuManager.IsTransitioning; }
            set { }
        }
        
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
            PokedexBackground.LoadContent();
            Arrow.LoadContent();

            PokedexBackground.Position.Y = 64;

            InitializePokemonList();

            if (Items.Count > 10)
            {
                ArrowDown.LoadContent();
            }

            AlignMenuItems();

            Arrow.Position = new Vector2(Items[currentShownIndices[ItemNumber] - 1].PokemonText.Position.X - Arrow.SourceRect.Width,
                                    Items[currentShownIndices[ItemNumber] - 1].PokemonText.Position.Y - 8);
            if (ArrowDown.IsLoaded)
            {
                arrowUpOriginalY = 44;
                arrowDownOriginalY = ScreenManager.Instance.Dimensions.Y - ArrowDown.SourceRect.Height - 44;
                ArrowDown.Position = new(ScreenManager.Instance.Dimensions.X - ArrowDown.SourceRect.Width - 124, arrowDownOriginalY);
            }

        }

        public override void UnloadContent()
        {
            PokemonListBackground.UnloadContent();
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

            if (ArrowUp.IsLoaded)
            {
                ArrowUp.UnloadContent();
            }

            if (ArrowDown.IsLoaded)
            {
                ArrowDown.UnloadContent();
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
                {
                    ItemNumber++;
                }
                else if (InputManager.Instance.KeyPressed(Keys.W) && ItemNumber > 0)
                {
                    ItemNumber--;
                }
                
            }

            if (!IsTransitioning)
            {
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

                if (currentShownIndices[ItemNumber] > 7 || currentShownIndices[0] != 1)
                {
                    if (!ArrowUp.IsLoaded)
                    {
                        ArrowUp.LoadContent();
                        ArrowUp.Position = new(ArrowDown.Position.X, arrowUpOriginalY);
                    }
                }

                AnimateArrows(gameTime);

                if ((InputManager.Instance.KeyPressed(Keys.E) && !string.IsNullOrEmpty(Items[currentShownIndices[ItemNumber] - 1].LinkType)) || InputManager.Instance.KeyPressed(Keys.Q))
                {
                    NumItemsBeforeSavedIndex = ItemNumber;
                    ItemNumber = currentShownIndices[ItemNumber] - 1;
                    SavedIndex = ItemNumber + 1;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            PokemonListBackground.Draw(spriteBatch);
            PokedexBackground.Draw(spriteBatch);
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

            if (!IsTransitioning && ArrowUp.IsLoaded && (currentShownIndices[ItemNumber] > 7 || !currentShownIndices.Contains(1)))
            {
                ArrowUp.Draw(spriteBatch);
            }

            if (!IsTransitioning && ArrowDown.IsLoaded && (currentShownIndices[ItemNumber] < Items.Count - 3 && !currentShownIndices.Contains(Items.Count)))
            { 
                ArrowDown.Draw(spriteBatch);
            }
        }

        private void InitializePokemonList()
        {
            int maxIndexNum = 0;

            foreach (string s in Player.PlayerJsonObject.Pokedex.Keys)
            {
                Pokemon mon = PokemonManager.Instance.GetPokemon(s);

                pokemonNames.Add(mon.Index, new PokemonText(mon.Name.ToUpper(), "Fonts/PokemonFireRedDialogue", new Color(0,0,0), new Color(224, 216, 192)));

                if (Player.PlayerJsonObject.Pokedex[s])
                {
                    List<Image> types = new();

                    Image pokeball = new()
                    {
                        Path = "Menus/PokedexMenu/OwnedPokeball"
                    };

                    types.Add(pokeball);

                    foreach (Type type in mon.Types)
                    {
                        types.Add(TypeProperties.ImageOf(type));
                    }

                    pokemonOwned.Add(mon.Index, types);
                }

                if (mon.Index > maxIndexNum)
                {
                    maxIndexNum = mon.Index;
                }
            }

            for (int i = 1; i <= maxIndexNum; i++)
            {
                Items.Add(new MenuItem("", new PokemonText("No" + i.ToString().PadLeft(3, '0'), "Fonts/PokemonFireRedSmall",
                    new Color(0, 0, 0), new Color(224, 216, 192))));

                if (!pokemonNames.ContainsKey(i))
                {
                    pokemonNames.Add(i, new PokemonText("-----", "Fonts/PokemonFireRedDialogue", new Color(0, 0, 0), new Color(224, 216, 192)));
                }
                else
                {
                    Items[i - 1].LinkType = "Menu";
                    Items[i - 1].LinkID = "Load/Menus/PokemonDetailsMenu.xml";
                    Items[i - 1].MenuName = "PokemonDetailsMenu";
                }
            }

            for (int i = SavedIndex - NumItemsBeforeSavedIndex; i <= maxIndexNum && currentShownIndices.Count < 10; i++)
            {
                currentShownIndices.Add(i);
                Items[i - 1].PokemonText.LoadContent();
                pokemonNames[i].LoadContent();

                if (i == SavedIndex)
                {
                    ItemNumber = currentShownIndices.Count - 1;
                }

                if (pokemonOwned.ContainsKey(i))
                {
                    foreach (Image image in pokemonOwned[i])
                    {
                        image.LoadContent();
                    }
                }
            }
        }

        private void AnimateArrows(GameTime gameTime)
        {
            float speed = (float)(48 * gameTime.ElapsedGameTime.TotalSeconds);

            if (arrowOffset >= 12)
            {
                increase = false;
            }
            else if (arrowOffset <= 0)
            {
                increase = true;
            }

            if (increase)
            {
                arrowOffset += speed;
            }
            else
            {
                arrowOffset -= speed;
            }

            if (arrowOffset % 4 < 1)
            {
                if (ArrowUp.IsLoaded)
                {
                    ArrowUp.Position.Y = arrowUpOriginalY + (int)arrowOffset;
                }

                ArrowDown.Position.Y = arrowDownOriginalY - (int)arrowOffset;
            }
        }
    }
}

