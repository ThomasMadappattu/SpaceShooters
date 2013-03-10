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
    class Sprite
    {
        private Texture2D spriteTexture;
        private Vector2 spritePositon = new Vector2(0, 0);
        private Rectangle spriteRectangle = new Rectangle();
        private Rectangle spriteSourceRectangle = new Rectangle();
        private float scaleFactor = 1.0f;
        private float spriteSpeed = 1.0f;
        private Vector2 spriteDirection = new Vector2();
        public float rotation, depth;
        public Vector2 origin;
        // <summary>
        //
        //</summary>
        //<param name = "theContentManager"> reference
        public virtual  void Load(ContentManager theContentManager, string assetName)
        {
            spriteTexture = theContentManager.Load<Texture2D>(assetName);
            spriteSourceRectangle.X = 0;
            spriteSourceRectangle.Y = 0;
            spriteSourceRectangle.Width = (int)(spriteTexture.Width) ;
            spriteSourceRectangle.Height = (int)(spriteTexture.Height) ;
            spriteRectangle.Width = (int)(spriteTexture.Width* scaleFactor);
            spriteRectangle.Height = (int)(spriteTexture.Height * scaleFactor);
            spriteRectangle.X = (int)this.SpritePosition.X;
            spriteRectangle.Y = (int)this.SpritePosition.Y;
            
            

        }


        public virtual void Draw(SpriteBatch theSpriteBatch)
        {
            
            theSpriteBatch.Draw(spriteTexture, spritePositon, spriteSourceRectangle, Color.White,0.0f,Vector2.Zero,scaleFactor,SpriteEffects.None,0);
        }
        
        public Rectangle SpriteSourceRectangle
        {
            get
            {
                return spriteSourceRectangle;
            }
            set
            {
                spriteSourceRectangle = value;
            }


        }
        public Vector2 SpritePosition
        {
            get
            {
                return spritePositon;
            }
            set
            {

                spritePositon = value;
            }

        }
        public float Scale
        {
            get
            {
                return scaleFactor;
            }
            set
            {
                scaleFactor = value;
                spriteRectangle.X = (int)this.SpritePosition.X;
                spriteRectangle.Y = (int)this.SpritePosition.Y;
                spriteRectangle.Width =(int) scaleFactor * spriteSourceRectangle.Width;
                spriteRectangle.Height =(int) scaleFactor * spriteSourceRectangle.Height;
            }
        }
        public Vector2 SpriteOrigin
        {
            set
            {
                origin = value;
            }
            get
            {
                return origin;
            }

        }
        public float SpriteRotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
            }


        }
        public float SpriteDepth
        {
            get
            {
                return depth;
            }
            set
            {
                depth = value;
            }
        }
        public virtual void Update(GameTime gameTime, Vector2 direction)
        {
            spritePositon += direction * spriteSpeed * (float)(gameTime.ElapsedGameTime.TotalSeconds);

        }
        public virtual void Update(GameTime gameTime)
        {
            spritePositon += spriteDirection * spriteSpeed * (float)(gameTime.ElapsedGameTime.TotalSeconds);

        }
        public void Update(GameTime gameTime, Vector2 speed, Vector2 direction)
        {
            spritePositon += direction * speed * (float)(gameTime.ElapsedGameTime.TotalSeconds);

        
        }

        
        public float Speed
        {
            get 
            { 
                return spriteSpeed; 
            }
            set
            {             
                spriteSpeed = value; 
            }
        }
        public Vector2 Direction
        {
            get 
            { 
                return spriteDirection; 
            }
            set 
            { 
                spriteDirection = value; 
            }
        }
        public Rectangle SpriteRectangle
        {
            get
            {
                return spriteRectangle;
            }
        }
        public float Width
        {
            get
            {

                return spriteTexture.Width;
            }
        }
        public float Height
        {
            get
            {
                return spriteTexture.Height;
            
            }
         
        }
    }
    
}
