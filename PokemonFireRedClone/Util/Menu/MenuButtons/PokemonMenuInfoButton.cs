using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PokemonMenuInfoButton
    {
        public enum ButtonState { UNSELECTED, SELECTED, SWITCH_UNSELECTED, SWITCH_SELECTED, FAINT_UNSELECETED, FAINT_SELECTED }

        public Image BackgroundSelected;
        public Image BackgroundUnselected;
        public Image BackgroundSwitchSelected;
        public Image BackgroundSwitchUnselected;
        public Image BackgroundFaintSelected;
        public Image BackgroundFaintUnselected;
        public ButtonState State;

        protected Image menuSprite;
        protected Vector2 originalMenuSpritePos;
        protected PokemonAssets pokemonAssets;
        protected CustomPokemon pokemon;

        protected float counter;
        protected bool bounce;
        protected bool spritePositioned;

        public Image BackgroundInUse
        {
            get
            {
                switch (State)
                {
                    case ButtonState.UNSELECTED:
                        return BackgroundUnselected;
                    case ButtonState.SELECTED:
                        return BackgroundSelected;
                    case ButtonState.SWITCH_SELECTED:
                        return BackgroundSwitchSelected;
                    case ButtonState.SWITCH_UNSELECTED:
                        return BackgroundSwitchUnselected;
                    case ButtonState.FAINT_UNSELECETED:
                        return BackgroundFaintUnselected;
                    default:
                        return BackgroundFaintSelected;
                }
            }
            private set { }
        }

        public PokemonMenuInfoButton(CustomPokemon pokemon)
        {
            this.pokemon = pokemon;
            pokemonAssets = new PokemonAssets(pokemon, true);
            State = ButtonState.UNSELECTED;
        }


        public void LoadContent()
        {
            LoadBackground();
            pokemonAssets.LoadContent("Fonts/PokemonFireRedSmall", Color.White, Color.Gray);

            menuSprite = pokemon.Pokemon.MenuSprite;
            menuSprite.Effects = "SpriteSheetEffect";
            menuSprite.LoadContent();
            menuSprite.SpriteSheetEffect.AmountOfFrames = new Vector2(2, 1);
            menuSprite.SpriteSheetEffect.CurrentFrame = Vector2.Zero;
            menuSprite.SpriteSheetEffect.SwitchFrame = 120;
            menuSprite.IsActive = true;
        }

        public void UnloadContent()
        {
            BackgroundUnselected.UnloadContent();
            pokemonAssets.UnloadContent();
            menuSprite.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            menuSprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            pokemonAssets.Draw(spriteBatch);
            menuSprite.Draw(spriteBatch);
        }

        public virtual void UpdateInfoPositions(GameTime gameTime)
        {
            pokemonAssets.Name.SetPosition(new Vector2(BackgroundInUse.Position.X + 116, BackgroundInUse.Position.Y + 24));
            pokemonAssets.Level.SetPosition(new Vector2(pokemonAssets.Name.Position.X + 32, pokemonAssets.Name.Position.Y + pokemonAssets.Name.SourceRect.Height + 8));
            if (pokemonAssets.Gender != null)
                pokemonAssets.Gender.SetPosition(new Vector2(BackgroundInUse.Position.X + (BackgroundInUse.SourceRect.Width / 2), pokemonAssets.Level.Position.Y));
            pokemonAssets.MaxHP.SetPosition(new Vector2(BackgroundInUse.Position.X + BackgroundInUse.SourceRect.Width - 21 - pokemonAssets.MaxHP.SourceRect.Width, pokemonAssets.Level.Position.Y));
            pokemonAssets.CurrentHP.SetPosition(new Vector2(BackgroundInUse.Position.X + BackgroundInUse.SourceRect.Width - 101 - pokemonAssets.CurrentHP.SourceRect.Width, pokemonAssets.Level.Position.Y));
            pokemonAssets.HPBar.Position = new Vector2(BackgroundInUse.Position.X + 384 - ((1 - pokemonAssets.HPBar.Scale.X) / 2 * pokemonAssets.HPBar.SourceRect.Width), BackgroundInUse.Position.Y + 36);
            if (!spritePositioned)
            {
                menuSprite.Position = new Vector2(BackgroundInUse.Position.X + 36 - (menuSprite.SourceRect.Width / 4), BackgroundInUse.Position.Y + 48 - (menuSprite.SourceRect.Height / 2));
                spritePositioned = true;
            }
            if (State == ButtonState.SELECTED)
            {
                menuSprite.IsActive = false;
                float counterSpeed = (float) (gameTime.ElapsedGameTime.TotalMilliseconds * 8);
                counter += counterSpeed;
                if (counter > 1000)
                {
                    if (bounce)
                    {
                        menuSprite.SpriteSheetEffect.CurrentFrame.X = 1;
                        menuSprite.Position = new Vector2(BackgroundInUse.Position.X + 36 - (menuSprite.SourceRect.Width / 4), BackgroundInUse.Position.Y + 32 - (menuSprite.SourceRect.Height / 2));
                        bounce = false;
                    }
                    else
                    {
                        menuSprite.SpriteSheetEffect.CurrentFrame.X = 0;
                        menuSprite.Position = new Vector2(BackgroundInUse.Position.X + 36 - (menuSprite.SourceRect.Width / 4), BackgroundInUse.Position.Y + 48 - (menuSprite.SourceRect.Height / 2));
                        bounce = true;
                    }
                    counter = 0;
                }
            } else
            {
                menuSprite.IsActive = true;
                bounce = true;
                menuSprite.Position = new Vector2(BackgroundInUse.Position.X + 20 - (menuSprite.SourceRect.Width / 4), BackgroundInUse.Position.Y + 48 - (menuSprite.SourceRect.Height / 2));
            }

        }

        protected virtual void LoadBackground()
        {
            BackgroundSelected = new Image
            {
                Path = "Menus/PokemonMenu/PokemonMenuButtonSelected"
            };
            BackgroundUnselected = new Image
            {
                Path = "Menus/PokemonMenu/PokemonMenuButton"
            };
            BackgroundSwitchSelected = new Image();
            BackgroundSwitchUnselected = new Image();
        }
        

    }
}
