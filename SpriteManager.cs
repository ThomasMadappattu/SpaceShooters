using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace SpaceShooter
{

    class SpriteManager
    {
        public List<Sprite> gameSprites = new List<Sprite>();
        Sprite collisionSprite;
        public bool flag = false;
        float currentTimeImmune = 0;

        const int ALIVE = 0;
        public void Add(Sprite sprite)
        {
            gameSprites.Add(sprite);
        }

        public virtual void DrawSprites(SpriteBatch spriteBatch)
        {
            foreach (Sprite sprite in gameSprites)
            {
                sprite.Draw(spriteBatch);
            }
        }

        public virtual void UpdateSprites(GameTime gameTime)
        {
            CheckCollisions();
            if (Player.immune)
            {
                currentTimeImmune += gameTime.ElapsedGameTime.Milliseconds;
                if (currentTimeImmune > 3000)
                {
                    currentTimeImmune = 0;
                    Player.immune = false;
                }
            }
            foreach (Sprite sprite in gameSprites)
            {

                sprite.Update(gameTime);
            }
        }
        public virtual bool Remove(Sprite sprite)
        {
            return gameSprites.Remove(sprite);
        }

        public virtual void CheckCollisions()
        {
            foreach (Sprite sprite in gameSprites)
            {
                if (sprite.GetType() == typeof(Player))
                {
                    collisionSprite = sprite;
                    break;
                }
            }
            int rectHeight = (int)(((Player)collisionSprite).Scale * ((Player)collisionSprite).SpriteSourceRectangle.Height);
            int rectWidth = (int)(((Player)collisionSprite).Scale * ((Player)collisionSprite).SpriteSourceRectangle.Width);
            int xPos = (int)((Player)collisionSprite).SpritePosition.X;
            int yPos = (int)((Player)collisionSprite).SpritePosition.Y;
            Rectangle playerRect = new Rectangle(xPos, yPos, rectWidth, rectHeight);
            if (((Player)collisionSprite).Visible)
            {
                foreach (Sprite sprite in ((Player)collisionSprite).bulletSprites)
                {
                    Rectangle bulletRect = new Rectangle((int)sprite.SpritePosition.X, (int)sprite.SpritePosition.Y, sprite.SpriteSourceRectangle.Width, sprite.SpriteSourceRectangle.Height);

                    foreach (Sprite gsprite in gameSprites)
                    {
                        Rectangle alienRect = new Rectangle((int)gsprite.SpritePosition.X, (int)gsprite.SpritePosition.Y, gsprite.SpriteSourceRectangle.Width, gsprite.SpriteSourceRectangle.Height);

                        if (gsprite.GetType() != typeof(Player))
                        {
                            if (alienRect.Intersects(bulletRect) && ((Bullet)sprite).Visible)
                            {
                                if ((gsprite.GetType() == typeof(Alien)) && (int)(((Alien)gsprite).state) == ALIVE)
                                {
                                    ((Alien)gsprite).Explode();
                                    ((Bullet)sprite).Visible = false;
                                    ((Alien)gsprite).SpriteSourceRectangle = new Rectangle(0, 0, 0, 0);
                                }
                            }
                            if (gsprite.GetType() == typeof(Alien))
                            {
                                Alien tempAlien = (Alien)gsprite;
                                Rectangle alienBulletRect = new Rectangle((int)tempAlien.bulletSprite.SpritePosition.X, (int)tempAlien.bulletSprite.SpritePosition.Y, (int)tempAlien.bulletSprite.SpriteSourceRectangle.Width, (int)tempAlien.bulletSprite.SpriteSourceRectangle.Height);
                                if (alienBulletRect.Intersects(bulletRect) && tempAlien.bulletSprite.bulletImmune == false && ((Bullet)sprite).Visible)
                                {
                                    ((Alien)gsprite).bulletSprite.Visible = false;
                                    SpaceGame.playerScore += (5 + ((SpaceGame.gameLevel - 1) * 5));
                                }
                            }
                        }
                    }
                }
                foreach (Sprite sprites in gameSprites)
                {
                    if (sprites.GetType() == typeof(Alien))
                    {
                        Rectangle alienRectangle = new Rectangle((int)sprites.SpritePosition.X, (int)sprites.SpritePosition.Y, sprites.SpriteSourceRectangle.Width, sprites.SpriteSourceRectangle.Height);
                        Alien tempAlien = (Alien)sprites;
                        Rectangle bulletRectangle = new Rectangle((int)tempAlien.bulletSprite.SpritePosition.X, (int)tempAlien.bulletSprite.SpritePosition.Y, tempAlien.bulletSprite.SpriteSourceRectangle.Width, tempAlien.bulletSprite.SpriteSourceRectangle.Height);

                        if (playerRect.Intersects(alienRectangle) && !Player.immune)
                        {
                            //if (tempAlien.bulletSprite.Visible)
                            if (((Player)collisionSprite).Visible == true)
                            {
                                if (tempAlien.Visible)
                                {
                                    ((Player)collisionSprite).SpriteSourceRectangle = new Rectangle(0, 0, 0, 0);
                                    ((Player)collisionSprite).Visible = false;
                                    // ((Player)collisionSprite).Die();
                                    ((Player)collisionSprite).Explode();
                                    playerRect = Rectangle.Empty;
                                    Player.immune = true;

                                }
                            }
                        }
                        if (playerRect.Intersects(bulletRectangle) && !Player.immune)
                        {
                            if (tempAlien.bulletSprite.Visible)
                            {
                                ((Player)collisionSprite).SpriteSourceRectangle = new Rectangle(0, 0, 0, 0);
                                ((Player)collisionSprite).Visible = false;
                                ((Player)collisionSprite).Explode();
                                // ((Player)collisionSprite).Die();
                                playerRect = Rectangle.Empty;
                                Player.immune = true;

                            }
                        }
                    }
                }
            }
        }
    }
}
