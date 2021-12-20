﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class Tile
    {

        Vector2 position;
        Rectangle sourceRect;
        string state;

        public Rectangle SourceRect
        {
            get { return sourceRect; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public Vector2 Center
        {
            get
            { return new Vector2(position.X + (SourceRect.Width / 2), position.Y + (SourceRect.Height / 2)); }
        }


        /*
         * Find position of tile
         * FInd Height znd width of tile
         * Find center position of tile
         *
         *
         */

        public Tile()
        {
        }

        public void LoadContent(Vector2 position, Rectangle sourceRect, string state)
        {
            this.position = position;
            this.sourceRect = sourceRect;
            this.state = state;
        }

        public void UnloadContent()
        {

        }

        //TODO: Play sound and slow down animation when colliding
        public void Update(GameTime gameTime, ref Player player)
        {
            if (state == "Solid")
            {
                Rectangle tileRect = new Rectangle((int) Position.X, (int) Position.Y, 
                    sourceRect.Width, sourceRect.Height-20);
                Rectangle playerRect = new Rectangle((int) player.Image.Position.X, (int) player.Image.Position.Y,
                    player.Image.SourceRect.Width, player.Image.SourceRect.Height);

                if (playerRect.Intersects(tileRect))
                {
                    if (player.state == Player.State.MoveLeft)
                        player.Image.Position.X = (int) tileRect.Right;
                    else if (player.state == Player.State.MoveRight)
                        player.Image.Position.X = (int) tileRect.Left - player.Image.SourceRect.Width;
                    else if (player.state == Player.State.MoveUp)
                        player.Image.Position.Y = tileRect.Bottom;
                    else
                        player.Image.Position.Y = tileRect.Top - player.Image.SourceRect.Height;
                    player.state = Player.State.Idle;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

    }

}
