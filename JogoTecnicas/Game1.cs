using JogoTecnicas.Graficos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JogoTecnicas
{
    public class Game1 : Game
    {

        private SpriteAnimation _runAnimation;



        private const string ASSET_NAME_SPRITESHEET_RUN = "shaolin_running_strip";
        private const string ASSET_NAME_BACKGROUND = "shaolin_background_a";
        private const string ASSET_NAME_FLOOR = "shaolin_background_floor";

        private int _screenWidth = 740;
        private int _screenHeight = 470;


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _spriteSheetTextureRun;
        private int _frameWidth = 45;    // largura de cada frame
        private int _frameHeight = 61;   // altura de cada frame
        private int _currentFrame = 0;
        private int _totalFrames = 5;
        private float _animationTimer = 0f;
        private float _timePerFrame = 0.1f; // 10 frames por segundo


        private Texture2D _backgroundTexture;
        private Texture2D _floorTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = _screenWidth;
            _graphics.PreferredBackBufferHeight = _screenHeight;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // TODO: use this.Content to load your game content here
            _spriteSheetTextureRun = Content.Load<Texture2D>(ASSET_NAME_SPRITESHEET_RUN);
            _runAnimation = new SpriteAnimation(_spriteSheetTextureRun, _frameWidth, _frameHeight, _totalFrames, _timePerFrame);
            _backgroundTexture = Content.Load<Texture2D>(ASSET_NAME_BACKGROUND);
            _floorTexture = Content.Load<Texture2D>(ASSET_NAME_FLOOR);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

           
            _runAnimation.Update(gameTime);

            base.Update(gameTime);
            
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            
            _spriteBatch.Draw(_backgroundTexture, Vector2.Zero, Color.White);
            _spriteBatch.Draw(_floorTexture, new Vector2(0, _screenHeight-60), Color.White);
            _runAnimation.Draw(_spriteBatch, new Vector2(60, _screenHeight - _frameHeight));


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
