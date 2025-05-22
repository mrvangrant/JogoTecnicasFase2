using JogoTecnicas.Graficos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JogoTecnicas
{
    public class Game1 : Game
    {
        //outras classes
        private KeyboardInput _keyboardInput = new KeyboardInput();
        private Buildings _buildings;
        private Player _player;

        //sprites
        private const string ASSET_NAME_SPRITESHEET = "TheDummyAnim-SpriteSheet";
        private const string ASSET_NAME_BACKGROUND = "shaolin_background_a";
        private const string ASSET_NAME_FLOOR = "shaolin_background_floor";

        //tela
        private int _screenWidth = 740;
        private int _screenHeight = 470;

        private int floorY;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _spriteSheetTextureRun;
        private int _frameWidth = 64;    // largura de cada frame
        private int _frameHeight = 64;   // altura de cada frame
        private int _totalFrames = 8;
        private float _timePerFrame = 0.1f; // 10 frames por segundo
       // private int _index = 0+64*anim; // índice do sprite na sprite sheet

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
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = _screenWidth;
            _graphics.PreferredBackBufferHeight = _screenHeight;
            _graphics.ApplyChanges();

            floorY = _screenHeight - 60;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _spriteSheetTextureRun = Content.Load<Texture2D>(ASSET_NAME_SPRITESHEET);

            // Crie as animações de correr e saltar (
            var runAnimation = new SpriteAnimation(_spriteSheetTextureRun,320, _frameWidth, _frameHeight, _totalFrames, _timePerFrame);
            var jumpAnimation = new SpriteAnimation(_spriteSheetTextureRun,448, _frameWidth, _frameHeight, _totalFrames, _timePerFrame); 

            // Inicializa o Player
            _player = new Player(runAnimation, jumpAnimation, new Vector2(180, floorY - _frameHeight-100));

            _backgroundTexture = Content.Load<Texture2D>(ASSET_NAME_BACKGROUND);
            _floorTexture = Content.Load<Texture2D>(ASSET_NAME_FLOOR);

            // Inicializa a classe Buildings
            _buildings = new Buildings(_backgroundTexture, _floorTexture, _screenWidth, _screenHeight);
        }

        protected override void Update(GameTime gameTime)
        {
            _keyboardInput.Update();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Atualiza o cenário (background e chão)
            _buildings.Update(gameTime);

            // Atualiza o player (animação correr/saltar)
            _player.Update(gameTime, _keyboardInput, _buildings.FloorRectangle);

            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Desenha cenário (background e chão)
            _buildings.Draw(_spriteBatch);

            // Desenha personagem
            _player.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
