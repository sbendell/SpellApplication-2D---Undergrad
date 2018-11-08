using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpellApp
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        PlayerManager playerManager;
        SpellManager spellManager;
        TextManager textManager;
        CollisionManager collisionManager;
        IconManager iconManager;
        LevelManager levelManager;

        enum GameStates { TitleScreen, Playing, Dead, Credits, Settings };
        GameStates gameState = GameStates.TitleScreen;

        Texture2D TitleScreenTexture;
        Texture2D pointerTexture;
        Texture2D pointerRightTexture;
        Texture2D creditsTexture;
        Texture2D deadTexture;
        Texture2D soundIcon;
        Texture2D musicIcon;

        Texture2D spellTexture;
        Texture2D enemyTexture;
        Texture2D playerTexture;
        Texture2D playingBackground;
        Texture2D iconTexture;
        Texture2D expBarOutline;
        Texture2D expBarInside;

        SpriteFont Font;

        Rectangle expBarDestRect = new Rectangle(222, 696, 304, 16);
        float expBarScale;

        Rectangle playRectangle= new Rectangle(290, 375, 190, 64);
        Rectangle settingsRectangle = new Rectangle(238, 438, 295, 64);
        Rectangle creditsRectangle = new Rectangle(254, 501, 261, 64);
        Rectangle exitRectangle = new Rectangle(308, 564, 154, 64);
        Rectangle soundIconRectangle = new Rectangle(30, 30, 128, 128);
        Rectangle musicIconRectangle = new Rectangle(30, 188, 128, 128);

        Color soundIconColor = Color.White;
        Color musicIconColor = Color.White;

        BaseSprite musicBar;
        BaseSprite musicTab;
        Texture2D musicBarTexture;
        Texture2D musicTabTexture;
        bool MusicTabMoving = false;

        MouseState mouseState;
        MouseState prevMouseState;
        Vector2 mousePosition;

        Vector2 leftPointerLocation;
        Vector2 rightPointerLocation;

        float deadTimer = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 768;
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
            this.IsMouseVisible = true;

            base.Initialize();
            musicBar = new BaseSprite(
                musicBarTexture,
                new Vector2(188, 242),
                new Rectangle(0, 0, 523, 20));

            musicTab = new BaseSprite(
                musicTabTexture,
                new Vector2((musicBar.location.X + musicBarTexture.Width - 16), (musicBar.location.Y - 22)),
                new Rectangle(0, 0, 32, 64));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Font = Content.Load<SpriteFont>(@"DamageFont");
            playerTexture = Content.Load<Texture2D>(@"drow_male1");
            enemyTexture = Content.Load<Texture2D>(@"EnemiesSpriteSheet");
            spellTexture = Content.Load<Texture2D>(@"SpellSpritesheet");
            playingBackground = Content.Load<Texture2D>(@"Outside");
            iconTexture = Content.Load<Texture2D>(@"IconSpriteSheet");
            expBarOutline = Content.Load<Texture2D>(@"EXPBarOutline");
            expBarInside = Content.Load<Texture2D>(@"EXPBarInside");
            TitleScreenTexture = Content.Load<Texture2D>(@"TitleScreenBackground");
            pointerTexture = Content.Load<Texture2D>(@"pointer");
            pointerRightTexture = Content.Load<Texture2D>(@"pointerRight");
            creditsTexture = Content.Load<Texture2D>(@"credits");
            deadTexture = Content.Load<Texture2D>(@"DeadScreen");
            soundIcon = Content.Load<Texture2D>(@"speaker");
            musicIcon = Content.Load<Texture2D>(@"double-quaver");
            musicBarTexture = Content.Load<Texture2D>(@"MusicBar");
            musicTabTexture = Content.Load<Texture2D>(@"MusicTab");

            spellManager = new SpellManager(
                spellTexture,
                8,
                new Rectangle(0,
                    0,
                    this.Window.ClientBounds.Width,
                    this.Window.ClientBounds.Height));

            playerManager = new PlayerManager(
                playerTexture,
                Font,
                new Rectangle(0, 0, 32, 48),
                16,
                new Rectangle(0,
                    0,
                    this.Window.ClientBounds.Width,
                    this.Window.ClientBounds.Height),
                spellManager);

            iconManager = new IconManager(
                Font,
                iconTexture,
                playerManager);

            levelManager = new LevelManager(
                1,
                enemyTexture,
                Font,
                new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height),
                playerManager,
                levelManager);

            textManager = new TextManager(
                Font,
                new Rectangle(0,
                    0,
                    this.Window.ClientBounds.Width,
                    this.Window.ClientBounds.Height),
                playerManager,
                levelManager.enemyManager);

            collisionManager = new CollisionManager(
                playerManager,
                levelManager.enemyManager,
                spellManager,
                textManager,
                levelManager);

            SoundManager.Initialize(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        
        void ResetGame()
        {
            playerManager.currentHealth = playerManager.maxHealth;
            levelManager.enemyManager.enemies.Clear();
            levelManager.enemyManager.bossHealth = levelManager.enemyManager.bossMaxHealth;
            playerManager.playerSprite.Location = new Vector2(0, 0);
            levelManager.enemyManager.bossSprite.Location = new Vector2(400, 400);
            textManager.damageText.Clear();
        }

        protected override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            mousePosition = new Vector2(mouseState.X, mouseState.Y);
            KeyboardState keyState = Keyboard.GetState();

            // TODO: Add your update logic here
            switch (gameState)
            {
                case GameStates.TitleScreen:
                    if (playRectangle.Contains(mousePosition))
                    {
                        leftPointerLocation = new Vector2(playRectangle.X - 40, playRectangle.Y + 20);
                        rightPointerLocation = new Vector2(playRectangle.X + playRectangle.Width, playRectangle.Y + 20);
                        if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                        {
                            ResetGame();
                            gameState = GameStates.Playing;
                        }
                    }
                    if (settingsRectangle.Contains(mousePosition))
                    {
                        leftPointerLocation = new Vector2(settingsRectangle.X - 40, settingsRectangle.Y + 20);
                        rightPointerLocation = new Vector2(settingsRectangle.X + settingsRectangle.Width, settingsRectangle.Y + 20);
                        if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                        {
                            gameState = GameStates.Settings;
                        }
                    }
                    if (creditsRectangle.Contains(mousePosition))
                    {
                        leftPointerLocation = new Vector2(creditsRectangle.X - 40, creditsRectangle.Y + 20);
                        rightPointerLocation = new Vector2(creditsRectangle.X + creditsRectangle.Width, creditsRectangle.Y + 20);
                        if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                        {
                            gameState = GameStates.Credits;
                        }
                    }
                    if (exitRectangle.Contains(mousePosition))
                    {
                        leftPointerLocation = new Vector2(exitRectangle.X - 40, exitRectangle.Y + 20);
                        rightPointerLocation = new Vector2(exitRectangle.X + exitRectangle.Width, exitRectangle.Y + 20);
                        if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                        {
                            this.Exit();
                        }
                    }
                    prevMouseState = mouseState;
                    break;

                case GameStates.Playing:
                    playerManager.Update(gameTime);
                    levelManager.enemyManager.Update(gameTime);
                    spellManager.Update(gameTime);
                    textManager.Update(gameTime);
                    collisionManager.Update(gameTime);
                    iconManager.Update(gameTime);

                    expBarScale = ((float)playerManager.playerExperience / (float)playerManager.experienceForNextLevel);
                    expBarDestRect.Width = (int)(304 * expBarScale);

                    if (playerManager.playerAlive == false)
                    {
                        gameState = GameStates.Dead;
                    }
                    break;
                case GameStates.Dead:
                    deadTimer += (float)gameTime.ElapsedGameTime.Milliseconds;

                    if (deadTimer > 3000f)
                    {
                        gameState = GameStates.TitleScreen;
                        deadTimer = 0;
                    }
                    break;
                case GameStates.Settings:
                    if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        if (soundIconRectangle.Contains(mousePosition))
                        {
                            SoundManager.SoundOn = !SoundManager.SoundOn;
                        }
                        if (musicIconRectangle.Contains(mousePosition))
                        {
                            SoundManager.MusicOn = !SoundManager.MusicOn;
                        }
                    }

                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (musicTab.CollisionRect.Contains(mousePosition))
                        {
                            MusicTabMoving = true;
                        }
                    }

                    if (prevMouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (MusicTabMoving)
                        {
                            musicTab.location.X = mousePosition.X - 16;
                        }
                    }
                    else
                    {
                        MusicTabMoving = false;
                    }
                    prevMouseState = mouseState;

                    if (musicTab.location.X < 172)
                    {
                        musicTab.location.X = 172;
                    }
                    if (musicTab.location.X > 695)
                    {
                        musicTab.location.X = 695;
                    }

                    SoundManager.volume = (float)((musicTab.location.X - 172) / (float)(695 - 172));

                    if (SoundManager.SoundOn)
                    {
                        soundIconColor = Color.White;
                    }
                    else
                    {
                        soundIconColor = Color.Red;
                    }
                    if (SoundManager.MusicOn)
                    {
                        musicIconColor = Color.White;
                    }
                    else
                    {
                        musicIconColor = Color.Red;
                    }

                    if (keyState.IsKeyDown(Keys.Escape))
                    {
                        gameState = GameStates.TitleScreen;
                    }
                    break;
                case GameStates.Credits:
                    if (keyState.IsKeyDown(Keys.Escape))
                    {
                        gameState = GameStates.TitleScreen;
                    }
                    break;
            }
            base.Update(gameTime);
            SoundManager.PlayMusic();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            switch(gameState)
            {
                case GameStates.TitleScreen:
                    spriteBatch.Draw(
                    TitleScreenTexture,
                    new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height),
                    Color.White);

                    if (playRectangle.Contains(mousePosition) ||
                        settingsRectangle.Contains(mousePosition) ||
                        creditsRectangle.Contains(mousePosition) ||
                        exitRectangle.Contains(mousePosition))
                    {
                        spriteBatch.Draw(
                            pointerTexture,
                            leftPointerLocation,
                            Color.White);

                        spriteBatch.Draw(
                            pointerRightTexture,
                            rightPointerLocation,
                            Color.White);
                    }
                    break;
                case GameStates.Credits:
                    spriteBatch.Draw(
                    creditsTexture,
                    new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height),
                    Color.White);
                    break;
                case GameStates.Settings:
                    spriteBatch.Draw(
                        soundIcon,
                        soundIconRectangle,
                        soundIconColor);

                    spriteBatch.Draw(
                        musicIcon,
                        musicIconRectangle,
                        musicIconColor);

                    musicBar.Draw(spriteBatch);
                    musicTab.Draw(spriteBatch);
                    break;
                case GameStates.Playing:
                    spriteBatch.Draw(
                    playingBackground,
                    Vector2.Zero,
                    Color.White);
                    playerManager.Draw(spriteBatch);
                    levelManager.enemyManager.Draw(spriteBatch);
                    spellManager.Draw(spriteBatch);
                    textManager.Draw(spriteBatch);
                    iconManager.Draw(spriteBatch);

                    spriteBatch.Draw(
                        expBarInside,
                        expBarDestRect,
                        Color.White);

                    spriteBatch.Draw(
                        expBarOutline,
                        new Rectangle(222, 696, 304, 16),
                        Color.White);
                    break;
                case GameStates.Dead:
                    spriteBatch.Draw(
                    deadTexture,
                    new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height),
                    Color.White);
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
