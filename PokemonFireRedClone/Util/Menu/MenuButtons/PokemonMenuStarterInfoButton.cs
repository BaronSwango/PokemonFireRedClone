using System;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class PokemonMenuStarterInfoButton : PokemonMenuInfoButton
    {

        public PokemonMenuStarterInfoButton(CustomPokemon pokemon)
            : base(pokemon)
        {
            State = ButtonState.SELECTED;
            bounce = true;
        }

        public override void UpdateInfoPositions(GameTime gameTime)
        {
            pokemonAssets.Name.SetPosition(new Vector2(BackgroundInUse.Position.X + 120, BackgroundInUse.Position.Y + 84));
            pokemonAssets.Level.SetPosition(new Vector2(pokemonAssets.Name.Position.X + 32, pokemonAssets.Name.Position.Y + pokemonAssets.Name.SourceRect.Height + 8)); 
            if (pokemonAssets.Gender != null)
                pokemonAssets.Gender.SetPosition(new Vector2(BackgroundInUse.Position.X + BackgroundInUse.SourceRect.Width - 28 - pokemonAssets.Gender.SourceRect.Width, pokemonAssets.Level.Position.Y));
            pokemonAssets.MaxHP.SetPosition(new Vector2(BackgroundInUse.Position.X + BackgroundInUse.SourceRect.Width - 21 - pokemonAssets.MaxHP.SourceRect.Width, BackgroundInUse.Position.Y + BackgroundInUse.SourceRect.Height - 16 - pokemonAssets.MaxHP.SourceRect.Height));
            pokemonAssets.CurrentHP.SetPosition(new Vector2(BackgroundInUse.Position.X + BackgroundInUse.SourceRect.Width - 101 - pokemonAssets.CurrentHP.SourceRect.Width, pokemonAssets.MaxHP.Position.Y));
            pokemonAssets.HPBar.Position = new Vector2(BackgroundInUse.Position.X + 120 - ((1 - pokemonAssets.HPBar.Scale.X) / 2 * pokemonAssets.HPBar.SourceRect.Width), BackgroundInUse.Position.Y + 164);
            if (!spritePositioned)
            {
                menuSprite.Position = new Vector2(BackgroundInUse.Position.X + 24 - (menuSprite.SourceRect.Width / 4), BackgroundInUse.Position.Y + 56);
                spritePositioned = true;
            }
            if (State == ButtonState.SELECTED)
            {
                menuSprite.IsActive = false;
                float counterSpeed = (float)(gameTime.ElapsedGameTime.TotalMilliseconds * 8);
                counter += counterSpeed;
                if (counter > 1000)
                {
                    if (bounce)
                    {
                        menuSprite.SpriteSheetEffect.CurrentFrame.X = 0;
                        menuSprite.Position = new Vector2(BackgroundInUse.Position.X + 24 - (menuSprite.SourceRect.Width / 4), BackgroundInUse.Position.Y + 72);
                        bounce = false;
                    }
                    else
                    {
                        menuSprite.SpriteSheetEffect.CurrentFrame.X = 1;
                        menuSprite.Position = new Vector2(BackgroundInUse.Position.X + 24 - (menuSprite.SourceRect.Width / 4), BackgroundInUse.Position.Y + 56);
                        bounce = true;
                    }
                    counter = 0;
                }
            }
            else
            {
                menuSprite.IsActive = true;
                bounce = true;
                menuSprite.Position = new Vector2(BackgroundInUse.Position.X + 24 - (menuSprite.SourceRect.Width / 4), BackgroundInUse.Position.Y + 56);
            }
        }

        protected override void LoadBackground()
        {
            base.LoadBackground();
            BackgroundUnselected.Path = "Menus/PokemonMenu/PokemonMenuMainButton";
            BackgroundSelected.Path = "Menus/PokemonMenu/PokemonMenuMainButtonSelected";
        }
    }
}
