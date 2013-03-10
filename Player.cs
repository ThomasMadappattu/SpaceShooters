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
    class Player : Sprite
    {
        KeyboardState previousKeyboardState;
        public static bool immune = false;
        const string PLAYER_SPRITE_ASSET = @"textures\playersprite";
        const int PLAYER_X = 340;
        const int PLAYER_Y = 540;
        const int MAX_X = 700;
        const int MIN_X = 0;
        const int BULLET_TYPE = 1;
        const float BULLET_SPEED = 275;
        const int PLAYER_SPEED = 25;
        GameTime mgameTime;
        double currentTime = 0;
        double currentImmuneTime = 0;
        public  int life = 3;
        public List <Bullet> bulletSprites = new List<Bullet>();
        enum PlayerState { ALIVE, EXPLODE, DIE };
        PlayerState playerState = PlayerState.ALIVE;
        AnimatedSprite explosionSprite = new AnimatedSprite(6, 15);
        Sprite[] playerLife = new Sprite[30];
        ContentManager mContentManager;
        public bool Visible=true;
      
        public void Load(ContentManager contentManager)
        {
            mContentManager = contentManager;
            this.SpritePosition = new Vector2(PLAYER_X,PLAYER_Y);
            explosionSprite.Load(contentManager, @"Textures\explo");
            explosionSprite.SpriteSourceRectangle = new Rectangle(0, 0, 55, 52);
            explosionSprite.PlayOnlyOnce = true;
            for (int i = 0; i < life; i++)
            {
                playerLife[i] = new Sprite();
                playerLife[i].SpritePosition = new Vector2(i * 50, 0);
                playerLife[i].Scale = 0.75f;
                playerLife[i].Load(contentManager, @"Textures\playersprite");
            }
           
            base.Load(contentManager,PLAYER_SPRITE_ASSET) ;
            this.Scale = 1.50f;
        }
        public override void Update(GameTime gameTime)
        {
            mgameTime = gameTime;
            KeyboardState curKeyState = Keyboard.GetState();
            
            if (playerState == PlayerState.EXPLODE)
            {
                if (currentTime == 0)
                {
                    SoundPlayer.soundBank.PlayCue("dead");
                }
                currentTime += gameTime.ElapsedGameTime.Milliseconds;
                explosionSprite.Update(gameTime);
                if (currentTime > 500)
                {

                    playerState = PlayerState.DIE;
                    currentTime = 0;

                }
            }

            if (playerState == PlayerState.ALIVE )
            {
                if (this.Visible == true)
                {
                    UpdateMovement(curKeyState);
                }
                else //when a level change occurs to move the alien upwards
                {
                    this.SpritePosition=new Vector2(350,this.SpritePosition.Y);
                    this.Direction=new Vector2(0,-1);
                    this.Speed = 400;
                    
                }
                
            }
           
            UpdateBullet(gameTime, curKeyState);
            /*if (playerState != PlayerState.DIE)
            {
                UpdateMovement(curKeyState);
                UpdateBullet(gameTime, curKeyState);
            }*/
            if(playerState == PlayerState.EXPLODE)
              explosionSprite.Update(gameTime);
            previousKeyboardState = curKeyState;
            for (int i = 0; i < life; i++)
            {
                
                playerLife[i].Update(gameTime);
            }
            base.Update(gameTime); 

                
                
           
        }
    
        private void ShootBullet()
        {

                Vector2 deltaVector = new Vector2(this.SpriteRectangle.Width / 2 + 10, this.SpriteRectangle.Height / 2- 15);
                bool aCreateNew = true;
                foreach (Bullet aBullet in bulletSprites)
                {
                    if (aBullet.Visible == false)
                    {
                        aCreateNew = false;
                        aBullet.Fire(this.SpritePosition+deltaVector,BULLET_SPEED, new Vector2(0, -1));
                        break;
                    }
                }

                if (aCreateNew == true)
                {
                    Bullet aBullet  = new Bullet();
                    aBullet.Load(mContentManager,BULLET_TYPE);
                    aBullet.Fire(this.SpritePosition+deltaVector,BULLET_SPEED, new Vector2(0, -1));
                    bulletSprites.Add(aBullet);
                }
            
        }

        public void UpdateMovement(KeyboardState curKeyState)
        {
            if (curKeyState.IsKeyDown(Keys.A)||curKeyState.IsKeyDown(Keys.Left))
            {
                if (this.SpritePosition.X < 0)
                {
                    this.SpritePosition = new Vector2(0, PLAYER_Y);
                    
                
                }
                else
                {

                    this.Speed = PLAYER_SPEED;
                    this.Direction = new Vector2(-15.0f, 0);
                }
            }
            else if (curKeyState.IsKeyDown(Keys.D)||curKeyState.IsKeyDown(Keys.Right))
            {
                if (this.SpritePosition.X > (MAX_X-20) )
                {
                    this.SpritePosition = new Vector2(MAX_X, PLAYER_Y);
                }
                else
                {

                    this.Speed = PLAYER_SPEED;
                    this.Direction = new Vector2(15.0f, 0);

                }
            }
            else
            {
                this.Speed = 0;

            }
            
        }

        private void UpdateBullet(GameTime theGameTime, KeyboardState aCurrentKeyboardState)
        {
            
            foreach (Bullet aBullet in bulletSprites)
            {
                aBullet.Update(theGameTime);
            }

            if ((aCurrentKeyboardState.IsKeyDown(Keys.Space) == true && previousKeyboardState.IsKeyDown(Keys.Space) == false) || (aCurrentKeyboardState.IsKeyDown(Keys.W) == true && previousKeyboardState.IsKeyDown(Keys.W) == false))
            {

                if (life > 0 && this.Visible == true)
                {
                    
                    SoundPlayer.soundBank.PlayCue("fire");
                    ShootBullet();
                }
            } 
            
        }
        public override void Draw(SpriteBatch spriteBatch)
        {

            bool drawPlayer = true;
            if (Player.immune == true)
            {
                currentImmuneTime += mgameTime.ElapsedRealTime.Milliseconds;
                if (currentImmuneTime >= 50)
                {
                    drawPlayer = false;
                }
                if (currentImmuneTime >= 100)
                {
                    currentImmuneTime = 0;
                    drawPlayer = true;
                }
            }
            
                foreach (Bullet aBullet in bulletSprites)
                {
                    aBullet.Draw(spriteBatch);
                }
                if (drawPlayer == true)
                {
                    base.Draw(spriteBatch);
                }
            
                
                if (playerState == PlayerState.EXPLODE)
                {
                    explosionSprite.SpritePosition = this.SpritePosition + new Vector2(18,3);
                    explosionSprite.PlayOnlyOnce = true;
                    explosionSprite.Draw(spriteBatch);
                    
                   
                }
                else if (playerState == PlayerState.DIE)
                {
                    life--;
                    if (life > 0)
                    {

                        Visible = true;
                        playerState = PlayerState.ALIVE;
                        this.SpritePosition = new Vector2(PLAYER_X,PLAYER_Y);
                        
                        this.SpriteSourceRectangle = new Rectangle(0,0,(int)this.Width,(int)this.Height);
                      
                        explosionSprite = new AnimatedSprite(6,15);
                        explosionSprite.Load(mContentManager, @"Textures\explo");
                        explosionSprite.SpriteSourceRectangle = new Rectangle(0, 0, 55, 52);
                        explosionSprite.PlayOnlyOnce = true;
                    }
                }
            for(int i=0;i<life;i++)

            {
                 playerLife[i].Draw(spriteBatch);
            }               
            
        }
        public void Explode()
        {
            playerState = PlayerState.EXPLODE;
            this.Visible = false;
        }
        public void Die()
        {
            this.playerState = PlayerState.DIE;
            this.Visible = false;
        }

    }
}
