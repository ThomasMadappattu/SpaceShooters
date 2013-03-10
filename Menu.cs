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
    // This class is responsible for the game menu .The game menu is implemented using  
    // the sprite class . The corresponding menu items are displayed and buttons are   
    // updated when the arrow keys are pressed.
    //</summary>

    class Menu 
    {
        SpriteFont spriteFont;
        Sprite testButtonleft = new Sprite();
        Sprite testButtonright = new Sprite();
        Sprite testBackground = new Sprite();
        public int downInc = 0;
        int leftInc = -40;
        int menuX=10;
        int menuY=-60;
        bool  mpauseStatus;
        bool showHighScore = false;
        Color colorPlay, colorHisc, colorHelp, colorExit, colorResume;
        const int SELECT_PLAY = 1;
        const int SELECT_HIGHSCORE = 2;
        const int SELECT_HELP = 3;
        const int SELECT_EXIT = 4;
        const int SELECT_RESUME = 5;
        const string FONT_ASSET = @"Fonts\GameFont";
        const string BUTTON_SPRITE_ASSET1= @"Textures\button";
        const string BUTTON_SPRITE_ASSET2= @"Textures\button1";
        const string MENU_SPRITE_ASSET = @"Textures\menu";
        const string MENU_SELECT_SOUND = "menu_select3 2";
        public int select=0;
        //<summary>
        //load the graphics and the fonts
        //</summary>
        public void Load(ContentManager content)
        {
            spriteFont=content.Load<SpriteFont>(FONT_ASSET);
            testButtonleft.Load(content, BUTTON_SPRITE_ASSET1);
            testButtonright.Load(content, BUTTON_SPRITE_ASSET2);
            testBackground.Load(content, MENU_SPRITE_ASSET);
        }
        //<summary>
        // Checks which key is pressed and returns it to the user .Updates the position of the
        // button sprites.
        //</summary>
        public  KeyboardState Update(GameTime gameTime,KeyboardState prevKeystate,int gameStatus)
        {
            KeyboardState curKeystate=Keyboard.GetState();
            if (gameStatus == 1)
            {
                mpauseStatus = true;

            }
            else
            {
                mpauseStatus = false;


            }
                    
            if (curKeystate.IsKeyDown(Keys.Down)&&prevKeystate.IsKeyDown(Keys.Down)==false)
            {
                SoundPlayer.soundBank.PlayCue(MENU_SELECT_SOUND);
                if (downInc < 180)
                    downInc += 60;
                else if (mpauseStatus == false)
                    downInc = 0;
                else
                    downInc = -60;
                if (downInc == 60)
                    leftInc = -80;
                else if (downInc == 0)
                    leftInc = -60;
                else
                    leftInc = 0;
                
                
            }
            if (curKeystate.IsKeyDown(Keys.Up) && prevKeystate.IsKeyDown(Keys.Up) == false)
            
                 {

                    SoundPlayer.soundBank.PlayCue(MENU_SELECT_SOUND);
                    if (downInc > 0 && mpauseStatus == false)
                        downInc -= 60;
                    else if (downInc > -60 && mpauseStatus == true)
                        downInc -= 60;
                    else downInc = 180;

                    if (downInc == 60)
                        leftInc = -60;
                    else if (downInc == 0)
                        leftInc = -40;
                    else
                        leftInc = 0;


                }

                if (curKeystate.IsKeyDown(Keys.Enter) && prevKeystate.IsKeyDown(Keys.Enter) == false)
                {
                    SoundPlayer.soundBank.PlayCue(MENU_SELECT_SOUND);
                    if (downInc == 0)
                    {
                        select = SELECT_PLAY;
                    }
                    else if (downInc == 60)
                    {
                        select = SELECT_HIGHSCORE;
                    }
                    else if (downInc == 120)
                    {
                        select = SELECT_HELP;
                    }
                    else if (downInc == 180)
                    {
                        select = SELECT_EXIT;
                    }
                    else if (downInc == -60)
                    {
                        select = SELECT_RESUME;
                    }
                
               
                }
                if (curKeystate.IsKeyDown(Keys.Escape) && prevKeystate.IsKeyDown(Keys.Escape) == false&&mpauseStatus==true)
                {
                    if(showHighScore==false)
                    select = SELECT_RESUME;
                  
                }
                prevKeystate = curKeystate;
                testButtonleft.Update(gameTime);
                testButtonright.Update(gameTime);
                testBackground.Update(gameTime);
                return (prevKeystate);
        }
        //<summary>
        //Draw the menu and buttons . Also sets the font color for drawing the current selection
        //</summary>
        //<param name="myBatch"> Represents the required sprite batch
        public void Draw(SpriteBatch myBatch)
        {
            
                colorExit = colorHelp = colorHisc = colorPlay = colorResume = Color.Green;
                if (downInc == -60)
                    colorResume = Color.LawnGreen;
                else if (downInc == 0)
                    colorPlay = Color.LawnGreen;
                else if (downInc == 60)
                    colorHisc = Color.LawnGreen;
                else if (downInc == 120)
                    colorHelp = Color.LawnGreen;
                else if (downInc == 180)
                    colorExit = Color.LawnGreen;
                testBackground.Draw(myBatch);
                testButtonleft.SpritePosition = new Vector2(255 + leftInc + menuX, 255 + downInc + menuY);
                testButtonleft.Scale = (0.25f);
                testButtonleft.Draw(myBatch);
                testButtonright.SpritePosition = new Vector2(455 - leftInc + menuX, 255 + downInc + menuY);
                testButtonright.Scale = (0.25f);
                testButtonright.Draw(myBatch);
                if (mpauseStatus == true)
                {
                    myBatch.DrawString(spriteFont, "RESUME", new Vector2(310 + menuX, 200 + menuY), colorResume, 0, new Vector2(0, 0), 2, 0, 1);
                }
                myBatch.DrawString(spriteFont, "NEW GAME", new Vector2(282 + menuX, 260 + menuY), colorPlay, 0, new Vector2(0, 0), 2, 0,1);
                myBatch.DrawString(spriteFont, "HIGH SCORES", new Vector2(253 + menuX, 320 + menuY), colorHisc, 0, new Vector2(0, 0), 2, 0, 1);
                myBatch.DrawString(spriteFont, "HELP", new Vector2(337 + menuX, 380 + menuY), colorHelp, 0, new Vector2(0, 0), 2, 0, 1);
                myBatch.DrawString(spriteFont, "EXIT", new Vector2(340 + menuX, 440 + menuY), colorExit, 0, new Vector2(0, 0), 2, 0, 1);

            }
           
        
    }
    
}
