#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using System.IO;

#endregion

namespace SpaceShooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SpaceGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ContentManager content;
        Sprite gameBackground = new Sprite();
        Player playerSprite = new Player();
        private const string FILE_NAME = "Test.data";
        Menu gameMenu = new Menu();
        private SpriteBatch spriteBatch;
        KeyboardState prevKeystate,currentKeystate;
        int x, y;
        SpriteManager spriteManager = new SpriteManager();
        Alien[] alienSprites = new Alien[20];
        const int ALIEN_WIDTH = 68;
        const int ALIEN_HEIGHT = 40;
        SpriteFont spriteFont;
        SpriteFont spriteFont2;
       
        bool pause = false;
        bool gameStarted = false;
        
        bool gameLevelChange = false;
        public static int playerScore = 0;
        const int SELECT_NOT = 0;
        const int SELECT_PLAY = 1;
        const int SELECT_HIGHSCORE = 2;
        const int SELECT_HELP = 3;
        const int SELECT_EXIT = 4;
        const int SELECT_RESUME = 5;
        const int ALIEN_LEVEL1 = 0;
        const int ALIEN_LEVEL2 = 1;
        const int ALIEN_LEVEL3 = 2;
        const int ALIEN_LEVEL4 = 3;
        int gameStatus = 0;
        int playerHighScore = 0;
        public static int gameLevel = 1;
        int maxAlien = 1;
        public static float playerX;
        bool showHighScore = false;
        bool showHelp = false;
        bool soundOnce = true;
        
        public SpaceGame()
        {                
          graphics = new GraphicsDeviceManager(this);
          graphics.ToggleFullScreen();
          content = new ContentManager(Services);
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            string currentHighScore;
            SoundPlayer.init();
            FileStream stream1 = File.Open(FILE_NAME, FileMode.OpenOrCreate, FileAccess.Read);

            BinaryReader r = new BinaryReader(stream1);

            if (r.PeekChar() != -1)
            {
                currentHighScore = r.ReadString();
                playerHighScore = Convert.ToInt32(currentHighScore);
            }
            else
                playerHighScore = 0;
            r.Close();
            stream1.Close();
            base.Initialize();
        }


        /// <summary>
        /// Load your graphics content.  If loadAllContent is true, you should
        /// load content from both ResourceManagementMode pools.  Otherwise, just
        /// load ResourceManagementMode.Manual content.
        /// </summary>
        /// <param name="loadAllContent">Which type of content to load.</param>
        /// 
        private void LoadAlien(int index,string alienName,int alienType,int xDirection,int yDirection)
        {

            
                int temp = RandomGenerator.getNextRandomNumber(-50, -1);
                int offset = temp * 20;
                if (alienType == ALIEN_LEVEL4)
                {
                    offset = 20;
                }
                alienSprites[index] = new Alien(2, 2);
                alienSprites[index].Load(content, alienName);
                alienSprites[index].SpritePosition = new Vector2(index*80, offset );
                alienSprites[index].Speed = RandomGenerator.getNextRandomNumber(200, 400);
                alienSprites[index].Direction = new Vector2(xDirection, yDirection);
                alienSprites[index].SpriteSourceRectangle = new Rectangle(0, 0, ALIEN_WIDTH, ALIEN_HEIGHT);
                alienSprites[index].alienType = alienType;
                Alien.path[index] = true;
                alienSprites[index].currentPath = index;
                spriteManager.Add(alienSprites[index]);
          }
        
        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            
            const string ALIEN_1 = @"Textures\testSprite12";
            const string ALIEN_1_LEVEL2 = @"Textures\aliennew";
            const string ALIEN_2 = @"Textures\testSprite";
            const string ALIEN_LEVEL_4 = @"Textures\alienlevel4";
            string ALIEN=null;
            
            int ALIENTYPE = 0;
            if (gameLevel == 1 || gameLevel==2)
            {
                ALIEN = ALIEN_1;
                ALIENTYPE = ALIEN_LEVEL1;
            }
            else
            {
                ALIEN = ALIEN_1_LEVEL2;
              
                    ALIENTYPE = ALIEN_LEVEL2;
                
            }
            if (loadAllContent)
            {
                                                                             
           
                x = graphics.GraphicsDevice.Viewport.Height;
                y = graphics.GraphicsDevice.Viewport.Width;
                spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
                playerSprite.Load(content);
                spriteManager.Add(playerSprite);
                spriteFont = content.Load<SpriteFont>(@"Fonts\GameFont");
                spriteFont2 = content.Load<SpriteFont>(@"Fonts\GameFont1");
                gameMenu.Load(content);
                for (int i = 0; i < 10; i++)
                {
                    Alien.path[i] = false;
                }
                for (int i = 0; i < maxAlien; i++)
                 {
                    
                     LoadAlien(i,ALIEN,ALIENTYPE,0,1);
                     
                 }
                 if (gameLevel > 1)
                 {
                     LoadAlien(maxAlien + 1, ALIEN_LEVEL_4, ALIEN_LEVEL4, 1, 0);
                 }
                 if(gameLevel>3)

                 {
                     LoadAlien(maxAlien + 2, ALIEN_2,ALIEN_LEVEL3, 1, 1);
                     
                 }
                if(gameLevel>4)
                 for (int i = 0; i < 2; i++)
                 {
                     LoadAlien(maxAlien + 2 + i, ALIEN_1, ALIEN_LEVEL1, 0, 1);
                 }

            
                // TODO: Load any ResourceManagementMode.Automatic content
           }


            // TODO: Load any ResourceManagementMode.Manual content
        }


        /// <summary>
        /// Unload your graphics content.  If unloadAllContent is true, you should
        /// unload content from both ResourceManagementMode pools.  Otherwise, just
        /// unload ResourceManagementMode.Manual content.  Manual content will get
        /// Disposed by the GraphicsDevice during a Reset.
        /// </summary>
        /// <param name="unloadAllContent">Which type of content to unload.</param>
        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {
            if (unloadAllContent)
            {
                // TODO: Unload any ResourceManagementMode.Automatic content
                content.Unload();
            }

            // TODO: Unload any ResourceManagementMode.Manual content
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (gameLevelChange == true && pause ==false)
            {
                if (soundOnce == true)
                {
                    SoundPlayer.soundBank.PlayCue("SP0000");
                    soundOnce = false;
                }
                currentKeystate = Keyboard.GetState();
                if (currentKeystate.IsKeyDown(Keys.Enter))
                {
                    soundOnce = true;
                    gameLevelChange = false;
                    playerSprite.Visible = true;
                    playerSprite.life++;
                    spriteManager = new SpriteManager();
                    spriteManager.Add(gameBackground);
                    LoadGraphicsContent(true);
                    SoundPlayer.soundBank.PlayCue("menu_select3 2");
                }
            }
           
            
                if (gameMenu.select == SELECT_NOT)
                {
                    if (pause == true)
                        gameStatus = 1;
                    else if (showHighScore == true)
                        gameStatus = 2;
                    else
                        gameStatus = 0;
                    prevKeystate = gameMenu.Update(gameTime, currentKeystate, gameStatus);
                }
                else if (gameMenu.select == SELECT_PLAY)
                {

                    spriteManager = new SpriteManager();
                    playerSprite = new Player();
                    gameStarted = true;
                    maxAlien = 5;
                    Player.immune = false;
                    gameLevel = 1;

                    LoadGraphicsContent(true);
                    spriteManager.UpdateSprites(gameTime);
                   
                    playerScore = 0;
                    gameMenu.select = SELECT_RESUME;
                    pause = false;

                }
                else if (gameMenu.select == SELECT_EXIT)
                {
                    this.Exit();
                }
                else if (gameMenu.select == SELECT_RESUME)
                {
                    
                    
                    pause = false;
                }
                else if (gameMenu.select == SELECT_HIGHSCORE)
                {
                    showHighScore = true;
                    
                }
                else if (gameMenu.select == SELECT_HELP)
                {
                    showHelp = true;
                }

                currentKeystate = Keyboard.GetState();
                
                if (gameStarted == true )
                {
                    if (currentKeystate.IsKeyDown(Keys.Escape) && prevKeystate.IsKeyDown(Keys.Escape) == false)
                    {
                        if (playerSprite.life > 0)
                        {
                            gameMenu.select = SELECT_NOT;
                            gameMenu.downInc = -60;
                            pause = true;
                            SoundPlayer.soundBank.PlayCue("menu_select3 2");
                        }

                    }
                }
                if (showHighScore == true)
                {
                    if (currentKeystate.IsKeyDown(Keys.Enter) && prevKeystate.IsKeyDown(Keys.Enter) == false)
                    {
                        showHighScore = false;
                        gameMenu.select = SELECT_NOT;
                        SoundPlayer.soundBank.PlayCue("menu_select3 2");

                    }
                }
                if(showHelp== true)
                {
                    if (currentKeystate.IsKeyDown(Keys.Enter) && prevKeystate.IsKeyDown(Keys.Enter) == false)
                    {
                        showHelp = false;
                        gameMenu.select = SELECT_NOT;
                        SoundPlayer.soundBank.PlayCue("menu_select3 2");

                    }
                }
                prevKeystate = currentKeystate;
               
                
                if (pause == false && gameMenu.select > 0 && showHighScore == false&&showHelp==false)
                {

                    if (playerScore >= 500 && gameLevel == 1)
                    {
                        gameLevel = 2;
                        gameBackground.Load(content, @"Textures\back");
                        
                        maxAlien = 5;

                        playerSprite.Visible = false;
                       
                        gameLevelChange = true;

                    }
                    if (playerScore >= 1500 && gameLevel == 2)
                    {
                        gameLevel = 3;
                       
                        maxAlien = 4 ;
                        playerSprite.Visible = false;
                        gameBackground = new Sprite();
                        gameBackground.Load(content, @"Textures\back_level3");
                        
                        gameLevelChange = true;
                    }
                    if (playerScore >= 2000&& gameLevel == 3)
                      {
                          gameLevel = 4;

                          maxAlien = 4;
                          playerSprite.Visible = false;
                          gameBackground = new Sprite();
                          gameBackground.Load(content, @"Textures\back");

                          gameLevelChange = true;

                       }
                       if (playerScore >= 3000 && gameLevel == 4)
                       {
                           gameLevel = 5;

                           maxAlien = 4;
                           playerSprite.Visible = false;
                           gameBackground = new Sprite();
                           gameBackground.Load(content, @"Textures\back_level3");

                           gameLevelChange = true;

                       }
                   
                   
                    KeyboardState curState;
                    curState = Keyboard.GetState();
                    if (playerSprite.life <= 0)
                    {
                        if (curState.IsKeyDown(Keys.Enter))
                        {
                            Player.immune = false;
                            playerSprite.life = 3;
                            gameMenu.downInc = 0;
                            gameMenu.select = 0;
                            gameStarted = false;


                        }
                    }
                    
                    if (gameMenu.select > 0)
                    {
                        spriteManager.UpdateSprites(gameTime);
                        base.Update(gameTime);
                    }

                }
                         
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            int score;
            graphics.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
          
             if (gameStarted == true&&showHighScore==false&&showHelp==false)
            {

                spriteManager.DrawSprites(spriteBatch);
                spriteBatch.DrawString(spriteFont, "SCORE ", new Vector2(580, 20), Color.Red);
                spriteBatch.DrawString(spriteFont, Convert.ToString(playerScore), new Vector2(660, 20), Color.Red);
                spriteBatch.DrawString(spriteFont, "HIGH SCORE ", new Vector2(580, 0), Color.Red);
                spriteBatch.DrawString(spriteFont, Convert.ToString(playerHighScore), new Vector2(725, 0), Color.Red);
                if (playerSprite.life <= 0)
                {

                    spriteBatch.DrawString(spriteFont, "GAME OVER!!! ", new Vector2(250, 240), Color.Red,0,new Vector2(0,0),2,0,0);
                    spriteBatch.DrawString(spriteFont2, "Press Enter to continue.... ", new Vector2(250, 290), Color.Red, 0, new Vector2(0, 0), 1, 0, 0);
                   
                        string temp = null ;
                        FileStream stream1 = File.Open(FILE_NAME, FileMode.Open,FileAccess.ReadWrite);
                        BinaryReader r = new BinaryReader(stream1);

                        if (r.PeekChar() != -1)
                        {
                            temp = r.ReadString();
                            score = Convert.ToInt32(temp);
                        }
                        else
                            score = 0;
                                                                 
                        r.Close();
                        stream1.Close();
                        if (playerScore > score)
                            playerHighScore = playerScore;
                        else
                            playerHighScore = score;
                        FileStream stream = File.Open(FILE_NAME, FileMode.OpenOrCreate, FileAccess.Write);
                        BinaryWriter w = new BinaryWriter(stream);
                        
                        w.Write(Convert.ToString(playerHighScore));
                        w.Close();
                        stream.Close();                                                                

                }
            }
            if ((gameLevelChange == true && pause == false) && !(playerSprite.life <= 0))
            {

                spriteBatch.DrawString(spriteFont, "LEVEL   FINISHED", new Vector2(250, 240), Color.Green, 0, new Vector2(0, 0), 2, 0, 0);
                spriteBatch.DrawString(spriteFont, Convert.ToString(gameLevel-1), new Vector2(390, 240), Color.Red, 0, new Vector2(0, 0), 2, 0, 0);
                spriteBatch.DrawString(spriteFont2, "Press ENTER to Continue...", new Vector2(250, 290), Color.Green, 0, new Vector2(0, 0), 1, 0, 0);
            }
            if (showHighScore == true)
            {
                spriteBatch.DrawString(spriteFont2, "HIGH SCORE", new Vector2(250, 240), Color.Green, 0, new Vector2(0, 0), 2, 0, 0);
                spriteBatch.DrawString(spriteFont2, Convert.ToString(playerHighScore), new Vector2(550, 240), Color.Red ,0, new Vector2(0, 0), 2, 0,0);
                spriteBatch.DrawString(spriteFont2, "Press ENTER to Continue...", new Vector2(250, 290), Color.Green, 0, new Vector2(0, 0), 1, 0, 0);
            }
            if (showHelp == true)
            {
                spriteBatch.DrawString(spriteFont2, "HELP", new Vector2(20, 20), Color.White, 0, new Vector2(0, 0), 2, 0, 0);
                spriteBatch.DrawString(spriteFont2, "Move Left  : A or Left Arrow", new Vector2(180, 200), Color.White, 0, new Vector2(0, 0), 1.5f, 0, 0);
                spriteBatch.DrawString(spriteFont2, "Move Right : D or Right Arrow", new Vector2(180, 240), Color.White, 0, new Vector2(0, 0), 1.5f, 0, 0);
                spriteBatch.DrawString(spriteFont2, "Fire         : W or Space", new Vector2(180, 280), Color.White, 0, new Vector2(0, 0), 1.5f, 0, 0);
                spriteBatch.DrawString(spriteFont2, "Press ENTER to Continue...", new Vector2(180, 320), Color.Green, 0, new Vector2(0, 0), 1, 0, 0);
            }
            if((gameStarted==false||pause==true)&&showHighScore==false&&showHelp==false)
            gameMenu.Draw(spriteBatch);
        
            
            spriteBatch.End();
            // TODO: Add your drawing code here
            
            base.Draw(gameTime);
        }
    }
}
