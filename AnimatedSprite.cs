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
    //<summary>
    // 
    //
    //</summary>
    class AnimatedSprite:Sprite
    {
        bool isPaused;
        private int frameCount;
        
        private float timePerFrame;
        private int frame;
        private float totalElapsed;
        public  bool DrawSprite=true;
        public bool PlayOnlyOnce = false;
      
        public AnimatedSprite()
        {
            frame = 0;
            totalElapsed = 0;
            isPaused = false;
        }
       
        public AnimatedSprite(int frameCount,int framesPerSec)
        {
            timePerFrame = 1 / (float)framesPerSec;
            this.frameCount = frameCount;
            frame = 0;
            totalElapsed = 0;
            isPaused = false;
        
        }
        
        
           
        public override void Load(ContentManager content, string asset)
        { 
                          
              base.Load(content, asset); 


        }
           
        public override void Update(GameTime gameTime )
        {
          float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
          if (isPaused)
             return;
          totalElapsed += elapsed;
          if (totalElapsed > timePerFrame)
          {
            frame++;
            // Keep the Frame between 0 and the total frames, minus one.
            if (PlayOnlyOnce)
            {
                if (frame >= frameCount)
                    DrawSprite = false;
            }
            else
             frame = frame % frameCount;
            totalElapsed -= timePerFrame;
           }
           base.SpriteSourceRectangle = new Rectangle(this.SpriteSourceRectangle.Width * frame, 0, this.SpriteSourceRectangle.Width, this.SpriteSourceRectangle.Height);
           base.Update(gameTime);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            
            if(DrawSprite)
               base.Draw(spriteBatch);
        
        }
        public void Reset()
        {
            frame = 0;
            totalElapsed = 0f;
        }
        public void Stop()
        {
            this.isPaused = true;
            Reset();
        }
        public void Play()
        {
            this.isPaused = false;
        }  
    
        public bool   Paused
        {
            get 
            { 
                return isPaused;
            }
            set 
            { 
                isPaused = value; 
            }
        }
	
    
    }
}
