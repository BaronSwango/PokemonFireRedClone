using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PokemonMenuInfoButton
    {
        public enum ButtonState { UNSELECTED, SELECTED, SWITCH_UNSELECTED, SWITCH_SELECTED, FAINT_UNSELECETED, FAINT_SELECTED }

        protected Image MenuSprite;
        protected Vector2 OriginalMenuSpritePos;
        protected PokemonAssets PokemonAssets;
        protected CustomPokemon Pokemon;

        protected float Counter;
        protected bool Bounce;
        protected bool SpritePositioned;

        public Image BackgroundSelected;
        public Image BackgroundUnselected;
        public Image BackgroundSwitchSelected;
        public Image BackgroundSwitchUnselected;
        public Image BackgroundFaintSelected;
        public Image BackgroundFaintUnselected;
        public ButtonState State;


        public Image BackgroundInUse
        {
            get
            {
                return State switch
                {
                    ButtonState.UNSELECTED => BackgroundUnselected,
                    ButtonState.SELECTED => BackgroundSelected,
                    ButtonState.SWITCH_SELECTED => BackgroundSwitchSelected,
                    ButtonState.SWITCH_UNSELECTED => BackgroundSwitchUnselected,
                    ButtonState.FAINT_UNSELECETED => BackgroundFaintUnselected,
                    _ => BackgroundFaintSelected,
                };
            }
            private set { }
        }

        public PokemonMenuInfoButton(CustomPokemon pokemon)
        {
            Pokemon = pokemon;
            PokemonAssets = new PokemonAssets(pokemon, true);
            State = ButtonState.UNSELECTED;
        }


        public void LoadContent()
        {
            LoadBackground();
            PokemonAssets.LoadContent("Fonts/PokemonFireRedSmall", Color.White, Color.Gray);

            MenuSprite = Pokemon.Pokemon.MenuSprite;
            MenuSprite.Effects = "SpriteSheetEffect";
            MenuSprite.LoadContent();
            MenuSprite.SpriteSheetEffect.AmountOfFrames = new Vector2(2, 1);
            MenuSprite.SpriteSheetEffect.CurrentFrame = Vector2.Zero;
            MenuSprite.SpriteSheetEffect.SwitchFrame = 120;
            MenuSprite.IsActive = true;
        }

        public void UnloadContent()
        {
            BackgroundUnselected.UnloadContent();
            PokemonAssets.UnloadContent();
            MenuSprite.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            MenuSprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PokemonAssets.Draw(spriteBatch);
            MenuSprite.Draw(spriteBatch);
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

        public virtual void UpdateInfoPositions(GameTime gameTime)
        {
            PokemonAssets.Name.SetPosition(new Vector2(BackgroundInUse.Position.X + 116, BackgroundInUse.Position.Y + 24));
            PokemonAssets.Level.SetPosition(new Vector2(PokemonAssets.Name.Position.X + 32, PokemonAssets.Name.Position.Y + PokemonAssets.Name.SourceRect.Height + 8));
            if (PokemonAssets.Gender != null)
                PokemonAssets.Gender.SetPosition(new Vector2(BackgroundInUse.Position.X + (BackgroundInUse.SourceRect.Width / 2), PokemonAssets.Level.Position.Y));
            PokemonAssets.MaxHP.SetPosition(new Vector2(BackgroundInUse.Position.X + BackgroundInUse.SourceRect.Width - 21 - PokemonAssets.MaxHP.SourceRect.Width, PokemonAssets.Level.Position.Y));
            PokemonAssets.CurrentHP.SetPosition(new Vector2(BackgroundInUse.Position.X + BackgroundInUse.SourceRect.Width - 101 - PokemonAssets.CurrentHP.SourceRect.Width, PokemonAssets.Level.Position.Y));
            PokemonAssets.HPBar.Position = new Vector2(BackgroundInUse.Position.X + 384 - ((1 - PokemonAssets.HPBar.Scale.X) / 2 * PokemonAssets.HPBar.SourceRect.Width), BackgroundInUse.Position.Y + 36);
            if (!SpritePositioned)
            {
                MenuSprite.Position = new Vector2(BackgroundInUse.Position.X + 36 - (MenuSprite.SourceRect.Width / 4), BackgroundInUse.Position.Y + 48 - (MenuSprite.SourceRect.Height / 2));
                SpritePositioned = true;
            }
            if (State == ButtonState.SELECTED)
            {
                MenuSprite.IsActive = false;
                float counterSpeed = (float) (gameTime.ElapsedGameTime.TotalMilliseconds * 8);
                Counter += counterSpeed;
                if (Counter > 1000)
                {
                    if (Bounce)
                    {
                        MenuSprite.SpriteSheetEffect.CurrentFrame.X = 1;
                        MenuSprite.Position = new Vector2(BackgroundInUse.Position.X + 36 - (MenuSprite.SourceRect.Width / 4), BackgroundInUse.Position.Y + 32 - (MenuSprite.SourceRect.Height / 2));
                        Bounce = false;
                    }
                    else
                    {
                        MenuSprite.SpriteSheetEffect.CurrentFrame.X = 0;
                        MenuSprite.Position = new Vector2(BackgroundInUse.Position.X + 36 - (MenuSprite.SourceRect.Width / 4), BackgroundInUse.Position.Y + 48 - (MenuSprite.SourceRect.Height / 2));
                        Bounce = true;
                    }
                    Counter = 0;
                }
            } else
            {
                MenuSprite.IsActive = true;
                Bounce = true;
                MenuSprite.Position = new Vector2(BackgroundInUse.Position.X + 20 - (MenuSprite.SourceRect.Width / 4), BackgroundInUse.Position.Y + 48 - (MenuSprite.SourceRect.Height / 2));
            }

        }

    }
}
