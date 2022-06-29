using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class OverlayMenu
    {

        protected int ItemNumber;

        public Image Background;
        public Image Arrow;
        [XmlElement("Items")]
        public List<MenuItem> Items;
        public bool IsOpen;


        public virtual void AlignMenuItems(Vector2 backgroundPos)
        {
            Background.Position = backgroundPos;
            Arrow.Position.X = -Arrow.SourceRect.Width;
            float dimensionY = Background.Position.Y + Background.SourceRect.Height - 88;

            for (int i = Items.Count - 1; i >= 0; i--)
            {
                Items[i].Image.Position = new Vector2(Background.Position.X + 60, dimensionY);
                dimensionY -= Items[i].Image.SourceRect.Height + 8;
            }
        }

        public virtual void LoadContent()
        {
            if (!Background.IsLoaded)
            {
                Background.LoadContent();
                Arrow.LoadContent();

                foreach (MenuItem i in Items)
                    i.Image.LoadContent();
            }

            ItemNumber = 0;
            IsOpen = true;
        }

        public virtual void UnloadContent()
        {
            Background.UnloadContent();
            Arrow.UnloadContent();

            foreach (MenuItem i in Items)
                i.Image.UnloadContent();

            IsOpen = false;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (IsOpen)
            {
                Background.Draw(spriteBatch);
                Arrow.Draw(spriteBatch);
                foreach (MenuItem i in Items)
                    i.Image.Draw(spriteBatch);
            }
        }


    }
}
