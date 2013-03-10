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
    class Bullet:Sprite
    {
        
        const string BULLET_SPRITE_ASSET1 = @"textures\bullet";
        const string BULLET_SPRITE_ASSET2 = @"textures\allienBullet";
        const string BULLET_SPRITE_ASSET3 = @"textures\alienBullet2";
        const int  BULLET_1=0,BULLET_2=1,BULLET_3=2;
        const int MAX_DISTANCE = 800;
        public bool Visible = false;
        int mbulletType;
        public bool bulletImmune;
        
        Vector2 startPosition;
        float spriteSpeed;
        Vector2 spriteDirection;
        public  void Load(ContentManager content,int bulletType)
        {
            mbulletType = bulletType;

            if (bulletType == BULLET_3)
            {
                base.Load(content, BULLET_SPRITE_ASSET3);
                bulletImmune = false;
            }
            else  if (bulletType == BULLET_2)
            {
                base.Load(content, BULLET_SPRITE_ASSET2);
                bulletImmune = false;
            }
            else if(bulletType==BULLET_1)
            {
                
                    base.Load(content,BULLET_SPRITE_ASSET1 );
                    this.Scale = 0.5f;
                    bulletImmune = true;
                
            }
           
        }
        public override void Update(GameTime gameTime)
        {
          

            if (Vector2.Distance(startPosition,base.SpritePosition) > MAX_DISTANCE)
            {
                Visible = false;
            }

            if (Visible == true)
            {
                this.Speed = spriteSpeed;
                
                this.Direction = spriteDirection;
                base.Update(gameTime);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Visible == true)
            {
                base.Draw(spriteBatch);
            }
        }
        public void Fire(Vector2 theStartPosition, float theSpeed, Vector2 theDirection)
        {
            base.SpritePosition = theStartPosition;
            startPosition = theStartPosition;
            spriteSpeed = theSpeed;
          
            spriteDirection  = theDirection;
            Visible = true;
        }
 
        
    
    }
}
