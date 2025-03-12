using System.Collections.Generic;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class Tile
    {
        public string State;
        public string ID;
        public Entity Entity;
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
        public void Update(Player player, List<NPC> npcs)
        {
            HandleEntity(player);

            foreach (NPC npc in npcs)
            {
                HandleEntity(npc);
            }
        }

        private void HandleEntity(Entity entity)
        {
            Rectangle tileRect = new((int)Position.X, (int)Position.Y,
                SourceRect.Width, SourceRect.Height - 20);
            Rectangle entityRect = new((int)entity.Sprite.Position.X, (int)entity.Sprite.Position.Y, entity.Sprite.SourceRect.Width, 84); 

            if (entityRect.Intersects(tileRect)) 
            {
                if (entity is not NPC && (State == "Solid" || (Entity != null && Entity != entity)))
                {
                    entity.Collide(tileRect);

                    /*
                    * TODO: Animation of player collision (goes through walking cycle slowly, can't move until it's done)
                    * TODO: handle when player collides with NPC and NPC moves (continue to play collision animation but also not allowed to move until animation is complete)
                    * TODO: Cancel sprinting when player collides
                    * TODO: handle sign and textbox interaction (interacting with sign/npc cancels collision animation)
                    * TODO: Opening menu cancels collision animation
                    * TODO: WATCH SCREEN RECORDING ON EXACT LOGISTICS OF COLLISION ANIMATION
                    */
                    
                }
                else if (Entity != entity)
                {
                    if (ID == "[1:1]")
                    {
                        TileAnimationManager.Instance.AddAnimation(entity, new GrassTileAnimation(entity, this));
                        // EntityAnimationManager.Instance.StartAnimation((IAnimatable) entity, new GrassTileAnimation(entity, this));
                    }
                    Entity = entity;
                }
            } 
            else if (Entity == entity)
            {
                Entity = null;
            }
        }
    }
}