using System.Xml.Serialization;

using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class Tile
    {

        public string State;
        public string ID;
        public bool ContainsEntity;
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
        public void Update(ref Player player)
        {
            if (State == "Solid" || ContainsEntity)
            {
                Rectangle tileRect = new((int)Position.X, (int)Position.Y,
                    SourceRect.Width, SourceRect.Height - 20);
                Rectangle playerRect = new((int)player.Image.Position.X, (int)player.Image.Position.Y,
                    player.Image.SourceRect.Width, player.Image.SourceRect.Height);

                if (playerRect.Intersects(tileRect))
<<<<<<< HEAD
                    player.Collide(tileRect);
                
=======
                {
                    if (player.State == Player.PlayerState.MoveLeft)
                        player.Image.Position.X = tileRect.Right;
                    else if (player.State == Player.PlayerState.MoveRight)
                        player.Image.Position.X = tileRect.Left - player.Image.SourceRect.Width;
                    else if (player.State == Player.PlayerState.MoveUp)
                        player.Image.Position.Y = tileRect.Bottom;
                    else
                        player.Image.Position.Y = tileRect.Top - player.Image.SourceRect.Height;

                    player.State = Player.PlayerState.Idle;
                    player.Colliding = true;
                }
>>>>>>> parent of eee7c30 (Started work on NPCs)
            }
        }

    }

}
