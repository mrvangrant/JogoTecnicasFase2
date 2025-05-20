using JogoTecnicas.Graficos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JogoTecnicas
{
    public class Game1 : Game
    {
        //outras classes
        private SpriteAnimation _runAnimation;
        private KeyboardInput _keyboardInput = new KeyboardInput();

        //sprites
        private const string ASSET_NAME_SPRITESHEET_RUN = "shaolin_running_strip";
        private const string ASSET_NAME_BACKGROUND = "shaolin_background_a";
        private const string ASSET_NAME_FLOOR = "shaolin_background_floor";


        //tela
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
        private float _floorOffset = 0f;
        private float _floorScrollSpeed = 200f; // pixels per second (increased speed)
        private float _backgroundOffset = 0f;
        // Move the background to the left
     
        private float _backgroundScrollSpeed = 20f; // pixels per second (slow speed)
        private float _characterY; // Current Y position of the character
        private float _characterVelocityY = 0f; // Vertical velocity
        private bool _isJumping = false;
        private float _jumpVelocity = -350f; // Initial jump velocity (negative = up)
        private float _gravity = 900f; // Gravity acceleration (pixels/sec^2)

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
            _keyboardInput.Update();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Move the ground to the left
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _floorOffset += _floorScrollSpeed * delta;
            if (_floorOffset >= _floorTexture.Width)
                _floorOffset -= _floorTexture.Width;

            // Move the background to the left
            _backgroundOffset += _backgroundScrollSpeed * delta;
            if (_backgroundOffset >= _backgroundTexture.Width)
                _backgroundOffset -= _backgroundTexture.Width;

            _runAnimation.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Draw the scrolling background
            _spriteBatch.Draw(_backgroundTexture, new Vector2(-_backgroundOffset, 0), Color.White);
            _spriteBatch.Draw(_backgroundTexture, new Vector2(_backgroundTexture.Width - _backgroundOffset, 0), Color.White);

            // Draw the scrolling ground
            float floorY = _screenHeight - 60;
            _spriteBatch.Draw(_floorTexture, new Vector2(-_floorOffset, floorY), Color.White);
            _spriteBatch.Draw(_floorTexture, new Vector2(_floorTexture.Width - _floorOffset, floorY), Color.White);

            // Draw the character exactly on top of the ground, further to the right
            _runAnimation.Draw(_spriteBatch, new Vector2(180, floorY - _frameHeight));

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
