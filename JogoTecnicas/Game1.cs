using JogoTecnicas.Graficos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Numerics;

// Adicione o alias para evitar ambiguidade
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace JogoTecnicas
{
    public class Game1 : Game
    {
        // Gerenciador de estado do jogo
        private GameManager _gameManager = new GameManager();

        //outras classes
        private KeyboardInput _keyboardInput = new KeyboardInput();
        private Buildings _buildings;
        private Player _player;
        private Obstacles _obstacles;
        private Texture2D _obstacleTexture;
        private SpriteFont _font;
        private bool _isGameOver = false;

        //sprites
        private const string ASSET_NAME_SPRITESHEET = "TheDummyAnim-SpriteSheet";
        private const string ASSET_NAME_BACKGROUND = "shaolin_background_a";
        private const string ASSET_NAME_FLOOR = "shaolin_background_floor";

        //tela
        private int _screenWidth = 740;
        private int _screenHeight = 470;

        //altura do chão
        private int floorY;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Textura do player e valores para os frames
        private Texture2D _spriteSheetTextureRun;
        private int _frameWidth = 64;
        private int _frameHeight = 64;
        private int _totalFrames = 8;
        private float _timePerFrame = 0.1f;

        // Textura de fundo e chão
        private Texture2D _backgroundTexture;
        private Texture2D _floorTexture;

        // Propriedades públicas para o GameManager
        public Player Player { get => _player; set => _player = value; }
        public Obstacles Obstacles { get => _obstacles; set => _obstacles = value; }
        public Buildings Buildings { get => _buildings; set => _buildings = value; }
        public Texture2D ObstacleTexture => _obstacleTexture;
        public Texture2D SpriteSheetTextureRun => _spriteSheetTextureRun;
        public Texture2D BackgroundTexture => _backgroundTexture;
        public Texture2D FloorTexture => _floorTexture;
        public int ScreenWidth => _screenWidth;
        public int ScreenHeight => _screenHeight;
        public int FloorY => floorY;
        public int FrameWidth => _frameWidth;
        public int FrameHeight => _frameHeight;
        public int TotalFrames => _totalFrames;
        public float TimePerFrame => _timePerFrame;
        public bool IsGameOver { get => _isGameOver; set => _isGameOver = value; }

        public Score _score;

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


            // Crie as animações de correr e saltar
            var runAnimation = new SpriteAnimation(_spriteSheetTextureRun, 320, _frameWidth, _frameHeight, _totalFrames, _timePerFrame);
            var jumpAnimation = new SpriteAnimation(_spriteSheetTextureRun, 448, _frameWidth, _frameHeight, _totalFrames, _timePerFrame);
            var slideAnimation = new SpriteAnimation(_spriteSheetTextureRun, 384, _frameWidth, _frameHeight, _totalFrames + 1, _timePerFrame);
            var idleAnimation = new SpriteAnimation(_spriteSheetTextureRun, 0, _frameWidth, _frameHeight, 4, _timePerFrame);
           


            // Inicializa o Player
            _player = new Player(runAnimation, jumpAnimation, slideAnimation, idleAnimation, new Vector2(180, floorY - _frameHeight));

            //
            _player.SetRunCollisionBox(new Rectangle(15, 20, 40, 45)); // Colisão para corrida
            _player.SetJumpCollisionBox(new Rectangle(15, 5, 35, 50)); // Colisão para salto
            _player.SetSlideCollisionBox(new Rectangle(10, 30, 45, 35)); // Colisão para deslizar
            _player.SetIdleCollisionBox(new Rectangle(20, 15, 20, 50)); // Colisão para Idle que está certa

            //carregar obstaculo
            _obstacleTexture = Content.Load<Texture2D>("Obstaculo");
            _obstacles = new Obstacles(_obstacleTexture);
            //carregar fonte
            _font = Content.Load<SpriteFont>("DefaultFont");
            _score = new Score(_font, new Vector2(20, 20));


            // Carrega as texturas de fundo e chão
            _backgroundTexture = Content.Load<Texture2D>(ASSET_NAME_BACKGROUND);
            _floorTexture = Content.Load<Texture2D>(ASSET_NAME_FLOOR);

            // Inicializa a classe Buildings
            _buildings = new Buildings(_backgroundTexture, _floorTexture, _screenWidth, _screenHeight);



        }

        protected override void Update(GameTime gameTime)
        {
            if (_isGameOver)
            {
                //para reiniciar o jogo
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    _gameManager.RestartGame(this);
                }
                return;
            }
            _score.Update(gameTime, _isGameOver);

            _keyboardInput.Update();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Atualiza o cenário (background e chão)
            _buildings.Update(gameTime, Player.isPlayerMovingRight);

            // Atualiza o player (animação correr/saltar)
            _player.Update(gameTime, _keyboardInput, _buildings.FloorRectangle);

            _obstacles.Update(gameTime, 5f, Player.isPlayerMovingRight);

            if (_obstacles.CheckCollision(_player.BoundingBox))
            {
                _isGameOver = true;
            }



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Desenha o cenário, o player e os obstáculos
            _buildings.Draw(_spriteBatch);
            _player.Draw(_spriteBatch);
            _obstacles.Draw(_spriteBatch);

            // Exibe mensagem de Game Over
            if (_isGameOver)
            {
                _spriteBatch.DrawString(_font, "Game Over", new Vector2(_screenWidth / 2 - 80, _screenHeight / 2 - 20), Color.Red, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
                _spriteBatch.DrawString(_font, "Pressione R para recomecar", new Vector2(_screenWidth / 2 - 120, _screenHeight / 2 + 30), Color.White);
            }

            _score.Draw(_spriteBatch);

            // Desenha os retângulos de colisão
            DrawCollisionBox(_spriteBatch, _player.BoundingBox, Color.Red); // Colisão do player
            foreach (var obstacle in _obstacles.GetBoundingBoxes()) // Supondo que Obstacles tenha um método para obter os retângulos
            {
                DrawCollisionBox(_spriteBatch, obstacle, Color.Yellow); // Colisão dos obstáculos
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        // Método auxiliar para desenhar retângulos de colisão
        private void DrawCollisionBox(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            // Cria uma textura de 1x1 pixel
            Texture2D texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new[] { color });

            // Desenha as bordas do retângulo
            spriteBatch.Draw(texture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, 1), color); // Topo
            spriteBatch.Draw(texture, new Rectangle(rectangle.X, rectangle.Y, 1, rectangle.Height), color); // Esquerda
            spriteBatch.Draw(texture, new Rectangle(rectangle.X + rectangle.Width - 1, rectangle.Y, 1, rectangle.Height), color); // Direita
            spriteBatch.Draw(texture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - 1, rectangle.Width, 1), color); // Base
        }
    }
}
