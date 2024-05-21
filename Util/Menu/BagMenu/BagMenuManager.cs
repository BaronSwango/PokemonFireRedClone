using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
	public class BagMenuManager
	{
		public enum BagPage { ITEMS = 1, KEY_ITEMS, POKE_BALLS }

		public static BagPage CurrentPage;
		private static int itemNumberItems, itemNumberKeyItems, itemNumberPokeBalls;
		private readonly MenuManager menu;
		private bool isTransitioning;

		private readonly Image background;
		private readonly PokemonText page;
		private readonly Image bag;
		private readonly Image bagShadow;

		private readonly Image arrowRight;
		private readonly Image arrowLeft;
		private float arrowOffset;
		private bool increase;
		private float arrowRightOriginalX;
		private float arrowLeftOriginalX;

		private BagAnimation bagAnimation;
		private BagMenuDisplayAnimation displayAnimation;

		private BagRotateAnimation rotateAnimation;


		public BagMenuManager()
		{
			if (CurrentPage == 0)
			{
				CurrentPage = BagPage.ITEMS;
			}

			background = new Image
			{
				Path = "Menus/BagMenu/BagMenuBackground"
			};

			bag = new Image
			{
				Path = "Menus/BagMenu/Bag",
				Effects = "SpriteSheetEffect"
			};

			bagShadow = new Image
			{
				Path = "Menus/BagMenu/BagShadow"
			};

			arrowRight = new Image
			{
				Path = "Menus/BagMenu/ArrowRight"
			};

			arrowLeft = new Image
			{
				Path = "Menus/BagMenu/ArrowLeft"
			};

			page = CurrentPage switch
			{
				BagPage.ITEMS => new PokemonText("ITEMS", "Fonts/PokemonFireRedDialogue", Color.White, new Color(113, 113, 113)),
				BagPage.KEY_ITEMS => new PokemonText("KEY   ITEMS", "Fonts/PokemonFireRedDialogue", Color.White, new Color(113, 113, 113)),
				BagPage.POKE_BALLS => new PokemonText("POKé   BALLS", "Fonts/PokemonFireRedDialogue", Color.White, new Color(113, 113, 113)),
				_ => new PokemonText("ITEMS", "Fonts/PokemonFireRedDialogue", Color.White, new Color(113, 113, 113)),
			};

			menu = new MenuManager("BagMenu");
		}

		public void LoadContent()
		{
			background.LoadContent();

			bag.LoadContent();
			bag.SpriteSheetEffect.AmountOfFrames = new(4, 1);
			bag.SpriteSheetEffect.CurrentFrame.X = (int)CurrentPage;
			bag.Position = new(116, 200);
			bagAnimation = new BagAnimation(bag);
			displayAnimation = new BagMenuDisplayAnimation();
			rotateAnimation = new BagRotateAnimation(bag);

			bagShadow.LoadContent();
			bagShadow.Position = new(100, 380);

			page.LoadContent();
			page.SetPosition(new(336 - page.SourceRect.Width / 2, 72));

			switch (CurrentPage)
			{
				case BagPage.ITEMS:
					arrowRight.LoadContent();
					break;
				case BagPage.KEY_ITEMS:
					arrowLeft.LoadContent();
					arrowRight.LoadContent();
					break;
				case BagPage.POKE_BALLS:
					arrowLeft.LoadContent();
					break;
			}

			if (arrowLeft.IsLoaded)
			{
				arrowLeft.Position = new(bag.Position.X - 48, bag.Position.Y + 116);
				arrowLeftOriginalX = arrowLeft.Position.X;
			}

			if (arrowRight.IsLoaded)
			{
				arrowRight.Position = new(bag.Position.X + bag.SourceRect.Width / 4 + 12, bag.Position.Y + 116);
				arrowRightOriginalX = arrowRight.Position.X;
			}

			menu.LoadContent("Load/Menus/BagMenu.xml");
			
			switch(CurrentPage) {
				case BagPage.ITEMS:
					menu.Menu.ItemNumber = itemNumberItems;
					break;
				case BagPage.KEY_ITEMS:
					menu.Menu.ItemNumber = itemNumberKeyItems;
					break;
				case BagPage.POKE_BALLS:
					menu.Menu.ItemNumber = itemNumberPokeBalls;
					break;
			}
		}

		public void UnloadContent()
		{
			switch(CurrentPage) {
				case BagPage.ITEMS:
					itemNumberItems = menu.Menu.ItemNumber;
					break;
				case BagPage.KEY_ITEMS:
					itemNumberKeyItems = menu.Menu.ItemNumber;
					break;
				case BagPage.POKE_BALLS:
					itemNumberPokeBalls = menu.Menu.ItemNumber;
					break;
			}
			background.UnloadContent();
			bag.UnloadContent();
			bagShadow.UnloadContent();
			page.UnloadContent();
			menu.UnloadContent();
			displayAnimation.UnloadContent();

			if (arrowRight.IsLoaded)
			{
				arrowRight.UnloadContent();
			}

			if (arrowLeft.IsLoaded)
			{
				arrowLeft.UnloadContent();
			}
		}

		public void Update(GameTime gameTime)
		{
			if (((InputManager.Instance.KeyPressed(Keys.S) && menu.Menu.ItemNumber < menu.Menu.Items.Count - 1) 
				|| InputManager.Instance.KeyPressed(Keys.W) && menu.Menu.ItemNumber > 0) && !isTransitioning) {
				rotateAnimation.Reset();
				rotateAnimation.AnimationStage = BagRotateAnimation.Stage.HALF_FORWARD;
			} 

			if (!isTransitioning) {
				menu.Update(gameTime);
			}

			Transition(gameTime);
			bag.Update(gameTime);
			rotateAnimation.Animate(gameTime);

			if (InputManager.Instance.KeyPressed(Keys.Q))
			{
				ScreenManager.Instance.ChangeScreens("GameplayScreen");
			}

			if (InputManager.Instance.KeyPressed(Keys.D) || InputManager.Instance.KeyPressed(Keys.A))
			{
				switch (CurrentPage)
				{
					case BagPage.ITEMS:
						if (InputManager.Instance.KeyPressed(Keys.D))
						{
							CurrentPage = BagPage.KEY_ITEMS;
							displayAnimation.Reset();
							bagAnimation.Reset();
							isTransitioning = true;
							itemNumberItems = menu.Menu.ItemNumber;
							((BagMenu)menu.Menu).LoadMenuItems();
							menu.Menu.ItemNumber = itemNumberKeyItems;
						}

						break;

					case BagPage.KEY_ITEMS:
						CurrentPage = InputManager.Instance.KeyPressed(Keys.A) ? BagPage.ITEMS : BagPage.POKE_BALLS;
						displayAnimation.Reset();
						bagAnimation.Reset();
						isTransitioning = true;
						itemNumberKeyItems = menu.Menu.ItemNumber;
						((BagMenu)menu.Menu).LoadMenuItems();
						menu.Menu.ItemNumber = CurrentPage == BagPage.ITEMS ? itemNumberItems : itemNumberPokeBalls;
						break;

					case BagPage.POKE_BALLS:
						if (InputManager.Instance.KeyPressed(Keys.A))
						{
							CurrentPage = BagPage.KEY_ITEMS;
							displayAnimation.Reset();
							bagAnimation.Reset();
							isTransitioning = true;
							itemNumberPokeBalls = menu.Menu.ItemNumber;
							((BagMenu)menu.Menu).LoadMenuItems();
							menu.Menu.ItemNumber = itemNumberKeyItems;
						}

						break;
				}
			}

			AnimateArrows(gameTime);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			background.Draw(spriteBatch);
			bagShadow.Draw(spriteBatch);
			bag.Draw(spriteBatch);

			if (!isTransitioning)
			{
				page.Draw(spriteBatch);
				menu.Draw(spriteBatch);

				if (CurrentPage == BagPage.ITEMS || CurrentPage == BagPage.KEY_ITEMS)
				{
					arrowRight.Draw(spriteBatch);
				}

				if (CurrentPage == BagPage.KEY_ITEMS || CurrentPage == BagPage.POKE_BALLS)
				{
					arrowLeft.Draw(spriteBatch);
				}
			}
			else
			{
				displayAnimation.Draw(spriteBatch);
			}
		}

		private void Transition(GameTime gameTime)
		{
			if (isTransitioning)
			{
				if (bagAnimation.Animate(gameTime))
				{
					switch (CurrentPage)
					{
						case BagPage.ITEMS:
							bag.SpriteSheetEffect.CurrentFrame.X = 1;
							page.UpdateText("ITEMS");
							page.SetPosition(new(336 - page.SourceRect.Width / 2, 72));

							if (!arrowRight.IsLoaded)
							{
								arrowRight.LoadContent();
								arrowRight.Position = new(bag.Position.X + bag.SourceRect.Width + 12, bag.Position.Y + 116);
								arrowRightOriginalX = arrowRight.Position.X;
							}

							break;

						case BagPage.KEY_ITEMS:
							bag.SpriteSheetEffect.CurrentFrame.X = 2;
							page.UpdateText("KEY   ITEMS");
							page.SetPosition(new(336 - page.SourceRect.Width / 2, 72));

							if (!arrowRight.IsLoaded)
							{
								arrowRight.LoadContent();
								arrowRight.Position = new(bag.Position.X + bag.SourceRect.Width + 12, bag.Position.Y + 116);
								arrowRightOriginalX = arrowRight.Position.X;
							}

							if (!arrowLeft.IsLoaded)
							{
								arrowLeft.LoadContent();
								arrowLeft.Position = new(bag.Position.X - 48, bag.Position.Y + 116);
								arrowLeftOriginalX = arrowLeft.Position.X;
							}

							break;

						case BagPage.POKE_BALLS:
							bag.SpriteSheetEffect.CurrentFrame.X = 3;
							page.UpdateText("POKé   BALLS");
							page.SetPosition(new(336 - page.SourceRect.Width / 2, 72));

							if (!arrowLeft.IsLoaded)
							{
								arrowLeft.LoadContent();
								arrowLeft.Position = new(bag.Position.X - 48, bag.Position.Y + 116);
								arrowLeftOriginalX = arrowLeft.Position.X;
							}

							break;
					}
					menu.Update(gameTime);
				}

				if (!displayAnimation.Animate(gameTime)) return;

				isTransitioning = false;
			}
		}

		private void AnimateArrows(GameTime gameTime)
		{
			if (!isTransitioning)
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
					if (arrowLeft.IsLoaded)
					{
						arrowLeft.Position.X = arrowLeftOriginalX - (int)arrowOffset;
					}

					arrowRight.Position.X = arrowRightOriginalX + (int)arrowOffset;
				}
			}
		}
	}
}