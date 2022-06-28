using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class SummaryPage
    {
        public CustomPokemon Pokemon;
        protected Image background;
        protected Image pokeImage; 

        readonly Image outline;

        public SummaryPage(CustomPokemon pokemon)
        {
            Pokemon = pokemon;
            outline = new Image
            {
                Path = "Menus/SummaryMenu/Outline"
            };
            pokeImage = pokemon.Pokemon.Front;
            pokeImage.Flip = true;
            background = new Image();
        }

        public virtual void LoadContent()
        {
            outline.LoadContent();
            background.LoadContent();
            pokeImage.LoadContent();
            pokeImage.Position = new Vector2(240 - (pokeImage.SourceRect.Width / 2), 256-(pokeImage.SourceRect.Height/2));
        }

        public virtual void UnloadContent()
        {
            outline.UnloadContent();
            background.UnloadContent();
            pokeImage.UnloadContent();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            outline.Draw(spriteBatch);
            background.Draw(spriteBatch);
            pokeImage.Draw(spriteBatch);
        }
        
    }
}
