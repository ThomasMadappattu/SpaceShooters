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
    class Alien : AnimatedSprite
    {
        public  enum AlienState { ALIVE, EXPLODED,FIRE, DIE };
        public static bool[] path=new bool[20] ;
        public int currentPath;
        public AlienState state = AlienState.ALIVE;
        public Bullet bulletSprite= new Bullet();
        ContentManager mContentManager;
        
        AnimatedSprite explosionSprite = new AnimatedSprite(7,6);
        double prevGameTime=0;
        double currentTime = 0;
        double curPlayerTime = 0;
        // double prevExplosionTime = 0;
        public bool Visible = true;
        const string EXPLOSION_TEXTURE= @"Textures\explosion";
        const int MAX_X = 800;
        const int MAX_Y = 600;
        const int BULLET_TYPE = 0;
        const float BULLET_SPEED =220;
        const int NORMAL_BULLET = 2;
        const int IMMUNE_BULLET = 0;
        float prevPlayerX;
        public int alienType;

        bool fireBullet = false;
        public Alien():base()
        {
        }
        public Alien(int numberOfFrames, int framesPerSec) : base(numberOfFrames, framesPerSec)
        {
            bulletSprite.Speed = 200.0f;
          
        
        }

        public override void Load(ContentManager content, string asset)
        {
            mContentManager = content;
            explosionSprite.Load(mContentManager, EXPLOSION_TEXTURE);
            explosionSprite.SpriteSourceRectangle = new Rectangle(0, 0, 65, 65);
            bulletSprite.Load(mContentManager, BULLET_TYPE);
        
            base.Load(content, asset);
        }
        public override void Update(GameTime gameTime)
        {
            int x = RandomGenerator.getNextRandomNumber(0, 2000);
            int condition = x % 16;//condition variable  to fire immune green bullets
            
            if (this.alienType!=0)
            {
                //code to fire immune green bullets if player opts to play without
                //horizontal movement 
                float change;
                if (curPlayerTime == 0)
                {
                    prevPlayerX = SpaceGame.playerX;
                }
                curPlayerTime += gameTime.ElapsedRealTime.Milliseconds;

                if (curPlayerTime > 1000)
                {
                    change = prevPlayerX - SpaceGame.playerX;
                    if (change < 0)
                        change *= -1;
                    if (change < 50)
                        fireBullet = true;
                    else
                        fireBullet = false;
                    curPlayerTime = 0;

                }
                if (fireBullet == true)
                    condition = x % 2;
            }
            else
            {
                condition = 1;
            }
            //if condition == 0 green bullet is fired     
           
            
                if (condition == 0 && bulletSprite.Visible == false && alienType>=1)
                {
                   
                    
                        bulletSprite = new Bullet();
                        bulletSprite.Load(mContentManager, IMMUNE_BULLET);
                    


                }
         
            else if (bulletSprite.Visible == false)
            {
                bulletSprite = new Bullet();
                bulletSprite.Load(mContentManager, NORMAL_BULLET);
            }

            
            UpdateBullet(gameTime);
            if (state == AlienState.EXPLODED)
            {
                if (currentTime == 0)
                    SoundPlayer.soundBank.PlayCue("explosion1");
                
                currentTime += gameTime.ElapsedGameTime.Milliseconds;
                explosionSprite.Update(gameTime);
                 
               
                if (currentTime > 1000)
                {
                 
                    state = AlienState.DIE;
                   
                    currentTime = 0; 
                   
                }
            }
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {

            int pathIndex;
            int spriteX;
            int rand;
            
            
                if (state == AlienState.ALIVE)
                {

                    if (this.SpritePosition.Y >= MAX_Y)
                    {
                        pathIndex = this.currentPath;
                        
                       
                        rand = RandomGenerator.getNextRandomNumber(0, 10);
                        if (path[rand] == false)
                        {
                            int SpriteY;
                            SpriteY = RandomGenerator.getNextRandomNumber(-200, 20);
                            path[rand] = true;
                            spriteX = rand * 80;
                            this.currentPath = rand;
                            path[pathIndex] = false;

                            this.SpritePosition = new Vector2(spriteX, SpriteY);

                            this.Speed = RandomGenerator.getNextRandomNumber(200, 450);
                        }
                    }

                    if (this.SpritePosition.X >= MAX_X-50)
                    {
                        if (alienType == 3)
                        {
                            this.Direction = new Vector2(-1, 0);
                            
                        }
                        else
                        {
                            int x = RandomGenerator.getNextRandomNumber(-200, 200);
                            this.SpritePosition = new Vector2(x, this.SpritePosition.Y);

                        } 
                        this.Speed = RandomGenerator.getNextRandomNumber(200, 450);
                    }
                    if (alienType == 3 && this.SpritePosition.X < 0)
                    {
                        this.Direction = new Vector2(1, 0);
                        
                        this.Speed = RandomGenerator.getNextRandomNumber(200, 450);
                        
                    }
                    base.Draw(spriteBatch);
                }
                else if (state == AlienState.EXPLODED)
                {
                    
                    explosionSprite.SpritePosition = this.SpritePosition;
                    explosionSprite.PlayOnlyOnce = true;
                    explosionSprite.Draw(spriteBatch);
                   
                    
                }
                else if (state == AlienState.DIE)
                {
                    SpaceGame.playerScore += (10 + ((SpaceGame.gameLevel - 1) * 5));
                    if (this.alienType == 2)
                        SpaceGame.playerScore += (25*SpaceGame.gameLevel);
                  

                        Visible = true;
                        state = AlienState.ALIVE;
                        this.SpritePosition = new Vector2(this.SpritePosition.X, 20);
                        this.SpriteSourceRectangle = new Rectangle(0, 0, 68, 40);
                        explosionSprite.Reset(); 
                    
                }
                
                bulletSprite.Draw(spriteBatch);      
            

        }
        private void ShootBullet()
        {
            
            bulletSprite.Fire(this.SpritePosition+ new Vector2(30,25), this.Speed+BULLET_SPEED, new Vector2(0, 1));
            
        }
        private void UpdateBullet(GameTime theGameTime)
        {
            double curTime = theGameTime.TotalGameTime.TotalMilliseconds;
            
            double randTime = RandomGenerator.getNextRandomNumber(2000);
            if ((((curTime - prevGameTime) > 2000 + randTime) || fireBullet == true) && state != AlienState.EXPLODED && this.SpritePosition.Y > 0 && Visible == true&&this.bulletSprite.Visible==false)
            {

                prevGameTime = curTime;
                ShootBullet();
                
               
                
            }
           // bulletSprite.Visible = true;
           
            bulletSprite.Update(theGameTime);
                                          
        }
        public void Explode()
        {
            Visible = false;
            state = AlienState.EXPLODED;
        }
        
    }
}
