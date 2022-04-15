using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PokemonMenuInfoButton
    {
        public Image BackgroundSelected;
        public Image BackgroundUnselected;
        public Image BackgroundSwitchSelected;
        public Image BackgroundSwitchUnselected;

        CustomPokemon pokemon;
        protected Image PokemonName;
        protected Image PokemonLevel;

        public PokemonMenuInfoButton(CustomPokemon pokemon)
        {
            this.pokemon = pokemon;
        }


        public void LoadContent()
        {
            LoadBackground();

            loadPokemonInfo();
        }

        public void UnloadContent()
        {
            BackgroundUnselected.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PokemonName.Draw(spriteBatch);
            PokemonLevel.Draw(spriteBatch);
        }

        public virtual void UpdateInfoPositions()
        {
            PokemonName.Position = new Vector2(BackgroundUnselected.Position.X + 116, BackgroundUnselected.Position.Y + 20);
            PokemonLevel.Position = new Vector2(PokemonName.Position.X + 32, PokemonName.Position.Y + PokemonName.SourceRect.Height + 8);
        }

        protected virtual void LoadBackground()
        {
            BackgroundSelected = new Image();
            BackgroundUnselected = new Image();
            BackgroundSwitchSelected = new Image();
            BackgroundSwitchUnselected = new Image();
            BackgroundUnselected.Path = "Menus/PokemonMenu/PokemonMenuButton";

        }

        void loadPokemonInfo()
        {
            //Initialize info images
            PokemonName = new Image();
            PokemonLevel = new Image();

            //Update info text
            PokemonName.FontName = "Fonts/PokemonFireRedSmall";
            PokemonName.UseFontColor = true;
            PokemonName.FontColor = Color.White;
            PokemonName.Text = pokemon.Name;
            PokemonLevel.FontName = "Fonts/PokemonFireRedSmall";
            PokemonLevel.UseFontColor = true;
            PokemonLevel.FontColor = Color.White;
            PokemonLevel.Text = "Lv" + pokemon.Level;

            //LoadContent
            PokemonName.LoadContent();
            PokemonLevel.LoadContent();
        }
 
    }
}
