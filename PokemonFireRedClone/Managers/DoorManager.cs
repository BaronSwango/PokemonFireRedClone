using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class DoorManager
    {
        private Door door;
        //private double counter;
        private Counter counter;

        [XmlElement("Doors")]
        public List<Door> Doors;
        public bool IsTransitioning;


        public void LoadContent(string ID)
        {
            foreach (Door door in Doors)
            {
                if (door.ID == ID)
                {
                    this.door = door;
                    break;
                }
            }

            ScreenManager.Instance.Image.IsActive = true;
            ScreenManager.Instance.Image.FadeEffect.Increase = true;
            ScreenManager.Instance.Image.FadeEffect.FadeSpeed = 3.5f;
            ScreenManager.Instance.Image.Alpha = 0.0f;

            counter = new Counter(500);
        }

        public void Update(GameTime gameTime, GameplayScreen screen, XmlManager<Map> mapLoader)
        {
            Transition(gameTime, screen, mapLoader);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsTransitioning)
                ScreenManager.Instance.Image.Draw(spriteBatch);
        }

        public void Transition(GameTime gameTime, GameplayScreen screen, XmlManager<Map> mapLoader)
        {
            if (IsTransitioning)
            {
                ScreenManager.Instance.Image.Update(gameTime);
                if (ScreenManager.Instance.Image.Alpha == 1.0f)
                {
                    //if (counter < 500)
                    if (!counter.Finished)
                    {
                        //counter += gameTime.ElapsedGameTime.TotalMilliseconds;
                        counter.Update(gameTime);
                        ScreenManager.Instance.Image.IsActive = false;
                        return;
                    }

                    if (!ScreenManager.Instance.Image.IsActive)
                        ScreenManager.Instance.Image.IsActive = true;

                    screen.Map.UnloadContent();
                    bool prevInside = screen.Map.Inside;

                    screen.Map = mapLoader.Load("Load/Gameplay/Map/" + door.MapName + ".xml");
                    screen.Map.LoadContent();

                    screen.Player.Sprite.Position = door.Coords;
                    Player.PlayerJsonObject.MapName = door.MapName;

                    if (prevInside)
                        screen.AreaManager.IsTransitioning = true;

                    screen.Player.Spawn(ref screen.Map);
                    screen.Player.Destination = screen.Player.PreviousTile = screen.Player.Sprite.Position;

                    foreach (NPC npc in screen.Map.NPCs)
                        npc.Spawn(ref screen.Map);

                    Vector2 playerPos = screen.Player.Sprite.Position;
                    ScreenManager.Instance.Image.Position = new Vector2(playerPos.X - (ScreenManager.Instance.Dimensions.X / 2) + 32,
                    playerPos.Y - (ScreenManager.Instance.Dimensions.Y / 2) + 40);

                    //counter = 0;
                    counter.Reset();
                }
                else if (ScreenManager.Instance.Image.Alpha == 0.0f)
                {
                    ScreenManager.Instance.Image.IsActive = false;
                    IsTransitioning = false;
                }
                else
                {
                    Vector2 playerPos = screen.Player.Sprite.Position;
                    ScreenManager.Instance.Image.Position = new Vector2(playerPos.X - (ScreenManager.Instance.Dimensions.X / 2) + 32,
                    playerPos.Y - (ScreenManager.Instance.Dimensions.Y / 2) + 40);
                }

            }
        }

    }
}
