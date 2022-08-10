using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class SaveMenu : Menu
    {

        private bool exited;
        private double exitCounter;

        public Image InfoTitlesBackground;
        public PokemonText SaveRegion;
        [XmlElement("Info")]
        public List<PokemonText> InfoTitles;
        public Image MenuBackground;
        public Image Arrow;
        public TextBox SaveDialogue;

        /* InfoTitles:
         * In front of InfoTitlesBackground:
         *        Town name
         * - Player      PlayerName
         * - Badges      # Badges
         * - PokeDex     # Pokedex
         * - Time        # Time
         * 
         * DISPLAY MENUS AFTER TEXTBOX TRANSITION
         * In front of MenuBackground
         * - Yes (MenuItem) (Continue TextBox Dialogue and save the player's game)
         * - No (MenuItem) (Back to main menu)
         * 
         * TextBox Dialogue Page 1: Would you like to save the game?
         * If yes is selected:
         *   If there is currently a file:
         *    TextBox Dialogue Page 2:
         *      Line 1: There is already a saved file.
         *      Line 2: Is it okay to overwrite it?
         *      DISPLAY SAME MENU AS BEFORE
         *      If yes is selected:
         *        TextBox Dialogue Page 3: [PlayerName] saved the game.
         *        Automatically unload menu after a few seconds
         *      else:
         *        Exit to MainMenu
         *   else
         *    (Page += 2) TextBox Dialogue Page 3: [PlayerName] saved the game.
         * else:
         *   Exit to MainMenu
         *    
         */

        protected override void AlignMenuItems()
        {
            Vector2 dimensions = Vector2.Zero;
            Vector2 playerPos = ((GameplayScreen)ScreenManager.Instance.CurrentScreen).Player.Sprite.Position;

            InfoTitlesBackground.Position = new Vector2(playerPos.X - (ScreenManager.Instance.Dimensions.X / 2) + 40,
                playerPos.Y - (ScreenManager.Instance.Dimensions.Y / 2) + 52);

            SaveRegion.SetPosition(new Vector2(InfoTitlesBackground.Position.X + 116, InfoTitlesBackground.Position.Y + 20));

            InfoTitles[0].SetPosition(new Vector2(InfoTitlesBackground.Position.X + 264, InfoTitlesBackground.Position.Y + 76));
            InfoTitles[1].SetPosition(new Vector2(InfoTitles[0].Position.X, InfoTitles[0].Position.Y + 76));
            for (int i = 2; i < InfoTitles.Count; i++)
            {
                InfoTitles[i].SetPosition(new Vector2(InfoTitles[0].Position.X, InfoTitles[i-1].Position.Y + 56));
            }

            MenuBackground.Position = new Vector2(playerPos.X + 200, playerPos.Y + 20);

            foreach (MenuItem item in Items)
            {
                item.PokemonText.SetPosition(new Vector2(MenuBackground.Position.X + 60, MenuBackground.Position.Y + dimensions.Y + 32));

                dimensions += new Vector2(item.PokemonText.SourceRect.Width,
                    item.PokemonText.SourceRect.Height);
            }

        }


        public override void LoadContent()
        {
            exited = false;
            exitCounter = 0;
            InfoTitlesBackground.LoadContent();
            SaveRegion.LoadContent();
            Arrow.LoadContent();

            MenuBackground.LoadContent();

            Player player = ((GameplayScreen)ScreenManager.Instance.CurrentScreen).Player;
            SaveDialogue.Dialogue[3].Image.Text = Player.PlayerJsonObject.Name + "   saved   the   game.";
            SaveDialogue.LoadContent(ref player);

            // format time to include days to hours
            var tsTime = TimeSpan.FromHours(Player.PlayerJsonObject.Time + Player.ElapsedTime);

            InfoTitles[0].Image.Text = Player.PlayerJsonObject.Name;
            InfoTitles[1].Image.Text = Player.PlayerJsonObject.Badges.ToString();
            InfoTitles[2].Image.Text = Player.PlayerJsonObject.Pokedex.ToString();
            InfoTitles[3].Image.Text = $"{tsTime.Hours + (tsTime.Days * 24):0}:{tsTime.Minutes:00}";

            foreach (PokemonText image in InfoTitles)
                image.LoadContent();

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            InfoTitlesBackground.UnloadContent();
            SaveRegion.UnloadContent();
            Arrow.UnloadContent();
            foreach (PokemonText image in InfoTitles)
                image.UnloadContent();
            MenuBackground.UnloadContent();
            SaveDialogue.UnloadContent(ref ((GameplayScreen)ScreenManager.Instance.CurrentScreen).Player);
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            SaveDialogue.Update(gameTime);
            if (SaveDialogue.Page == 3)
            {
                ((GameplayScreen)ScreenManager.Instance.CurrentScreen).Player.Save();
                exited = true;
                exitCounter += gameTime.ElapsedGameTime.TotalSeconds;
                if (exitCounter >= 2)
                {
                    ((GameplayScreen)ScreenManager.Instance.CurrentScreen).MenuManager.MenuName = "MainMenu";
                    ((GameplayScreen)ScreenManager.Instance.CurrentScreen).MenuManager.UnloadContent();
                }
            }
            if (!SaveDialogue.IsTransitioning && !exited)
            {
                AlignMenuItems();
                if (InputManager.Instance.KeyPressed(Keys.S))
                    ItemNumber++;
                else if (InputManager.Instance.KeyPressed(Keys.W))
                    ItemNumber--;

                if (ItemNumber < 0)
                    ItemNumber = 0;
                else if (ItemNumber > 1)
                    ItemNumber = 1;

                for (int i = 0; i < Items.Count; i++)
                {
                    if (i == ItemNumber)
                    {
                        Items[i].PokemonText.Image.IsActive = true;
                        Arrow.Position = new Vector2(Items[i].PokemonText.Position.X - Arrow.SourceRect.Width,
                            Items[i].PokemonText.Position.Y + (Items[i].PokemonText.SourceRect.Height / 4) - 2);
                    }
                    else
                        Items[i].PokemonText.Image.IsActive = false;

                }
                base.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SaveDialogue.Draw(spriteBatch);
            InfoTitlesBackground.Draw(spriteBatch);
            SaveRegion.Draw(spriteBatch);
            foreach (PokemonText image in InfoTitles)
                image.Draw(spriteBatch);
            
            if (!SaveDialogue.IsTransitioning && !exited)
            {
                MenuBackground.Draw(spriteBatch);
                Arrow.Draw(spriteBatch);
                base.Draw(spriteBatch);
            }
        }

        public override void Yes()
        {
            if (!SaveDialogue.IsTransitioning)
            {
                if (!File.Exists("Load/Gameplay/Player.json"))
                    SaveDialogue.Page = 2;
                SaveDialogue.IsTransitioning = true;
                if (SaveDialogue.Page == 2)
                    Items.Clear();
            }
        }

    }
}
