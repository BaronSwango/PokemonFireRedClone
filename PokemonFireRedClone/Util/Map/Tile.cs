using System.Xml.Serialization;

using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class Tile
    {

        public string State;
        public string ID;
        [XmlIgnore]
        public Rectangle SourceRect { get; private set; }
        [XmlIgnore]
        public Vector2 Position{ get; private set; }

        public void LoadContent(Vector2 position, Rectangle sourceRect, string State)
        {
            Position = position;
            SourceRect = sourceRect;
            this.State = State;
        }

        //TODO: Play sound when colliding
        //TODO: 
        public void Update(ref Player player)
        {
            if (State == "Solid")
            {
                Rectangle tileRect = new((int)Position.X, (int)Position.Y,
                    SourceRect.Width, SourceRect.Height - 20);
                Rectangle playerRect = new((int)player.Sprite.Position.X, (int)player.Sprite.Position.Y,
                    player.Sprite.SourceRect.Width, player.Sprite.SourceRect.Height);

                if (playerRect.Intersects(tileRect))
                {
                    if (player.State == Entity.MoveState.Left)
                        player.Sprite.Position.X = tileRect.Right;
                    else if (player.State == Entity.MoveState.Right)
                        player.Sprite.Position.X = tileRect.Left - player.Sprite.SourceRect.Width;
                    else if (player.State == Entity.MoveState.Up)
                        player.Sprite.Position.Y = tileRect.Bottom;
                    else
                        player.Sprite.Position.Y = tileRect.Top - player.Sprite.SourceRect.Height;

                    player.State = Entity.MoveState.Idle;
                    player.Colliding = true;
                }
            }
        }

    }

}
