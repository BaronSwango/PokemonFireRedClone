using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class TextBoxManager
    {

        // fix when opening pokemon menu in front of sign

        private TextBox textBox;
        private NPC npc;

        [XmlElement("TextBoxes")]
        public List<TextBox> TextBoxes;
        public bool Closed;

        [XmlIgnore]
        public bool IsDisplayed
        {
            get
            {
                if (textBox != null)
                    return textBox.IsDisplayed;
                return false;
            }
            set {
                if (textBox != null)
                    textBox.IsDisplayed = value;
            }
        }

        public void LoadXML()
        {
            XmlManager<TextBoxManager> textBoxLoader = new();
            TextBoxes = textBoxLoader.Load("Load/Gameplay/TextBoxManager.xml").TextBoxes;
        }

        public void LoadContent(string ID, ref Player player)
        {
            LoadXML();

            foreach (TextBox textBox in TextBoxes)
            {
                if (textBox.ID == ID)
                {
                    this.textBox = textBox;
                    this.textBox.LoadContent(ref player);
                    return;
                }
            }

        }

        public void UnloadContent(ref Player player)
        {
            textBox.UnloadContent(ref player);
            textBox = null;

            if (npc != null)
            {
                npc.UpdateMovement = npc.MoveType != NPC.MovementType.STILL;
                npc = null;
            }
        }

        public void Update(GameTime gameTime, ref Map map, ref Player player)
        {

            if (textBox != null)
            {
                textBox.Update(gameTime);
                if (textBox.IsDisplayed && !textBox.IsTransitioning)
                {
                    if ((((InputManager.Instance.KeyDown(Keys.D, Keys.W, Keys.S) && player.Direction == Entity.EntityDirection.Left)
                        || (InputManager.Instance.KeyDown(Keys.W, Keys.A, Keys.D) && player.Direction == Entity.EntityDirection.Down)
                        || (InputManager.Instance.KeyDown(Keys.S, Keys.D, Keys.A) && player.Direction == Entity.EntityDirection.Up)
                        || (InputManager.Instance.KeyDown(Keys.S, Keys.W, Keys.A) && player.Direction == Entity.EntityDirection.Right))
                        && textBox.Type != "NPC")
                        || InputManager.Instance.KeyPressed(Keys.E))
                    {
                        if (InputManager.Instance.KeyPressed(Keys.E) && textBox.Page != textBox.TotalPages)
                        {
                            textBox.IsTransitioning = true;
                            return;
                        }

                        if (npc != null)
                        {
                            if (npc is Trainer trainer)
                            {
                                foreach (CustomPokemon pokemon in trainer.Pokemon)
                                {
                                    pokemon.Create();
                                }

                                ScreenManager.Instance.ChangeScreens("BattleScreen", trainer);
                                return;
                            } 
                            else if (npc.TextBoxReactionType == NPC.TextBoxReaction.SNAP_BACK)
                            {
                                npc.NPCSprite.SetDirection((int) npc.Direction);
                            }
                        }

                        UnloadContent(ref player);
                        Closed = true;
                    }

                }
            } 
            else
            {
                if (player.CanUpdate)
                {
                    Tile currentTile = TileManager.GetCurrentTile(map, player.Sprite, player.Sprite.SourceRect.Width / 2, player.Sprite.SourceRect.Height);

                    if (InputManager.Instance.KeyPressed(Keys.E) && player.State == Entity.MoveState.Idle)
                    {
                        if (TileManager.UpTile(map, currentTile) != null && player.Direction == Entity.EntityDirection.Up)
                        {
                            LoadContent(TileManager.UpTile(map, currentTile).ID, ref player);

                            if (TileManager.UpTile(map, currentTile).Entity != null && TileManager.UpTile(map, currentTile).Entity is NPC npc && !npc.IsMoving)
                            {
                                LoadContent(map.NPCs[map.NPCs.IndexOf(npc)].ID, ref player);

                                if (npc.TextBoxReactionType != NPC.TextBoxReaction.NONE) 
                                {
                                    map.NPCs[map.NPCs.IndexOf(npc)].NPCSprite.SetDirection((int)Entity.EntityDirection.Down);
                                }

                                this.npc = npc;
                                npc.UpdateMovement = false;
                            }
                            
                        }

                        if (TileManager.DownTile(map, currentTile) != null && player.Direction == Entity.EntityDirection.Down)
                        {
                            LoadContent(TileManager.DownTile(map, currentTile).ID, ref player);

                            if (TileManager.DownTile(map, currentTile).Entity != null && TileManager.DownTile(map, currentTile).Entity is NPC npc && !npc.IsMoving)
                            {
                                LoadContent(map.NPCs[map.NPCs.IndexOf(npc)].ID, ref player);
                                
                                if (npc.TextBoxReactionType != NPC.TextBoxReaction.NONE) 
                                {
                                    map.NPCs[map.NPCs.IndexOf(npc)].NPCSprite.SetDirection((int)Entity.EntityDirection.Up);
                                }

                                this.npc = npc;
                                npc.UpdateMovement = false;
                            }
                        }

                        if (TileManager.LeftTile(map, currentTile) != null && player.Direction == Entity.EntityDirection.Left)
                        {
                            LoadContent(TileManager.LeftTile(map, currentTile).ID, ref player);

                            if (TileManager.LeftTile(map, currentTile).Entity != null && TileManager.LeftTile(map, currentTile).Entity is NPC npc && !npc.IsMoving)
                            {
                                LoadContent(map.NPCs[map.NPCs.IndexOf(npc)].ID, ref player);

                                if (npc.TextBoxReactionType != NPC.TextBoxReaction.NONE) 
                                {
                                    map.NPCs[map.NPCs.IndexOf(npc)].NPCSprite.SetDirection((int)Entity.EntityDirection.Right);
                                }

                                this.npc = npc;
                                npc.UpdateMovement = false;
                            }
                        }

                        if (TileManager.RightTile(map, currentTile) != null && player.Direction == Entity.EntityDirection.Right)
                        {
                            LoadContent(TileManager.RightTile(map, currentTile).ID, ref player);

                            if (TileManager.RightTile(map, currentTile).Entity != null && TileManager.RightTile(map, currentTile).Entity is NPC npc && !npc.IsMoving)
                            {
                                LoadContent(map.NPCs[map.NPCs.IndexOf(npc)].ID, ref player);

                                if (npc.TextBoxReactionType != NPC.TextBoxReaction.NONE) 
                                {
                                    map.NPCs[map.NPCs.IndexOf(npc)].NPCSprite.SetDirection((int)Entity.EntityDirection.Left);
                                }

                                this.npc = npc;
                                npc.UpdateMovement = false;
                            }
                        }
                    }
                    else if (currentTile != null && InputManager.Instance.KeyPressed(Keys.W) && TileManager.UpTile(map, currentTile) != null && player.Direction == Entity.EntityDirection.Up)
                        LoadContent(TileManager.UpTile(map, currentTile).ID, ref player);

                    else if (currentTile != null && InputManager.Instance.KeyPressed(Keys.S) && TileManager.DownTile(map, currentTile) != null && player.Direction == Entity.EntityDirection.Down)
                        LoadContent(TileManager.DownTile(map, currentTile).ID, ref player);

                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            textBox?.Draw(spriteBatch);
        }

    }
}
