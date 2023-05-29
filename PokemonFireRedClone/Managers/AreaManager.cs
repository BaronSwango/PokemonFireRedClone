using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class AreaManager
    {

        private readonly Image background;
        private PokemonText areaText;
        private int imageOffset;
        private readonly Counter counter;
        private bool dropdown;
        private Area newArea;

        private bool isLoaded;
        public bool IsTransitioning;

        public AreaManager()
        {
            background = new()
            {
                Path = "Gameplay/NewArea"
            };
            dropdown = true;
            counter = new Counter(1820);
        }

        public void LoadContent(List<Area> areas, Player player)
        {
            if (!background.IsLoaded)
            {
                background.LoadContent();
            }

            foreach (Area area in areas)
            {
                if (area.Contains(player.Sprite.Position))
                {
                    Player.PlayerJsonObject.CurrentArea = area;
                    Player.PlayerJsonObject.AreaName = area.Name;
                    break;
                }
            }

            areaText = new PokemonText(Player.PlayerJsonObject.AreaName, "Fonts/PokemonFireRedDialogue", new(113, 113, 113), new(218, 218, 212));
            areaText.LoadContent();

            isLoaded = true;
        }

        public void UnloadContent()
        {
            if (isLoaded)
            {
                background.UnloadContent();
            }
        }

        public void Update(GameTime gameTime, List<Area> areas, Player player)
        {
            foreach (Area area in areas)
            {
                if (area.PlayerEntered(player))
                {
                    if (IsTransitioning)
                    {
                        if (dropdown)
                        {
                            dropdown = false;
                        }

                        counter.Finish();
                        newArea = area;
                    }
                    else
                    {
                        IsTransitioning = true;
                        areaText = new PokemonText(area.Name, "Fonts/PokemonFireRedDialogue", new(113, 113, 113), new(218, 218, 212));
                        areaText.LoadContent();
                    }

                    Player.PlayerJsonObject.CurrentArea = area;
                    Player.PlayerJsonObject.AreaName = area.Name;
                    Console.WriteLine(area.Name);
                }
            }

            if (IsTransitioning)
            {
                Transition(gameTime, player);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsTransitioning && isLoaded)
            {
                background.Draw(spriteBatch);
                areaText.Draw(spriteBatch);
            }
        }

        public void Reset()
        {
            if (isLoaded)
            {
                IsTransitioning = false;
                imageOffset = 0;
                dropdown = true;
                counter.Reset();
                areaText.UnloadContent();

                if (newArea != null)
                {
                    IsTransitioning = true;
                    areaText = new PokemonText(newArea.Name, "Fonts/PokemonFireRedDialogue", new(113, 113, 113), new(218, 218, 212));
                    areaText.LoadContent();
                    newArea = null;
                }
            }
        }

        private void Transition(GameTime gameTime, Player player)
        {
            int speed = (int)(gameTime.ElapsedGameTime.TotalMilliseconds * 0.55);
            Vector2 screenPos = new(player.Sprite.Position.X - (ScreenManager.Instance.Dimensions.X / 2) + 64, player.Sprite.Position.Y - (ScreenManager.Instance.Dimensions.Y / 2) - 48 + imageOffset);

            background.Position = screenPos;
            areaText.SetPosition(new(background.Position.X + (background.SourceRect.Width / 2) - (areaText.SourceRect.Width / 2), background.Position.Y + 4));

            if (imageOffset + speed >= background.SourceRect.Height && dropdown)
            {
                imageOffset = background.SourceRect.Height;
                dropdown = false;
            }
            else if (dropdown)
            {
                imageOffset += speed;
            }

            if (!dropdown && !counter.Finished)
            {
                counter.Update(gameTime);
            }

            if (!dropdown && counter.Finished)
            {
                imageOffset -= speed;
            }

            if (!dropdown && imageOffset <= 0)
            {
                Reset();
            }
        }
    }
}
