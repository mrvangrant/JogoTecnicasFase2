using JogoTecnicas.Graficos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace JogoTecnicas
{
    public class Game1 : Game
    {
        //outras classes
        private KeyboardInput _keyboardInput = new KeyboardInput();
        private Buildings _buildings;
        private Player _player;

        //sprites
        private string ASSET_NAME_SPRITESHEET = "TheDummyAnim-SpriteSheet";
        private string ASSET_NAME_BACKGROUND = "shaolin_background_a";
        private string ASSET_NAME_FLOOR = "shaolin_background_floor";
        private string ASSET_NAME_OBSTACLES = "Obstaculo";

        //obstaculos
        private List<Obstaculos> _obstacles = new List<Obstaculos>();
        private Texture2D _obstacleTexture;
        private Random _random = new Random();
        private float _obstacleSpawnTimer = 0f;


        //gameState
        private GameState _gameState = GameState.Running;
        private SpriteFont _font; // Para desenhar o texto



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


            _font = Content.Load<SpriteFont>("DefaultFont");

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _spriteSheetTextureRun = Content.Load<Texture2D>(ASSET_NAME_SPRITESHEET);
            _obstacleTexture = Content.Load<Texture2D>(ASSET_NAME_OBSTACLES);

            // Crie as animações de correr e saltar (
            var runAnimation = new SpriteAnimation(_spriteSheetTextureRun,320, _frameWidth, _frameHeight, _totalFrames, _timePerFrame);
            var jumpAnimation = new SpriteAnimation(_spriteSheetTextureRun,448, _frameWidth, _frameHeight, _totalFrames, _timePerFrame); 

            // Inicializa o Player
            _player = new Player(runAnimation, jumpAnimation, new Vector2(180, floorY - _frameHeight));

            _backgroundTexture = Content.Load<Texture2D>(ASSET_NAME_BACKGROUND);
            _floorTexture = Content.Load<Texture2D>(ASSET_NAME_FLOOR);

            // Inicializa a classe Buildings
            _buildings = new Buildings(_backgroundTexture, _floorTexture, _screenWidth, _screenHeight);
        }

        protected override void Update(GameTime gameTime)
        {
            if (_gameState == GameState.GameOver)
                return; // Não atualiza nada se o jogo acabou

            _keyboardInput.Update();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Atualiza o cenário (background e chão)
            _buildings.Update(gameTime);

            // Atualiza o player (animação correr/saltar)
            _player.Update(gameTime, _keyboardInput);

            // Obstáculos
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Spawna obstáculos a cada X segundos
            _obstacleSpawnTimer += deltaTime;
            if (_obstacleSpawnTimer > 2f) // a cada 2 segundos
            {
                float y = _buildings.FloorY - _obstacleTexture.Height;
                _obstacles.Add(new Obstaculos(_obstacleTexture, new Vector2(_screenWidth, y)));
                _obstacleSpawnTimer = 0f;
            }

            // Atualiza e remove obstáculos fora da tela
            for (int i = _obstacles.Count - 1; i >= 0; i--)
            {
                _obstacles[i].Update(200f, deltaTime); // 200f = mesma velocidade do chão
                if (_obstacles[i].Position.X + _obstacleTexture.Width < 0)
                    _obstacles.RemoveAt(i);
            }

            // Verifica colisão com obstáculos
            foreach (var obstacle in _obstacles)
            {
                if (_player.BoundingBox.Intersects(obstacle.BoundingBox))
                {
                    _gameState = GameState.GameOver;
                    break;
                }
            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            if (_gameState == GameState.GameOver)
            {
                string message = "Game Over";
                Vector2 size = _font.MeasureString(message);
                Vector2 position = new Vector2((_screenWidth - size.X) / 2, (_screenHeight - size.Y) / 2);
                _spriteBatch.DrawString(_font, message, position, Color.Red);
            }

            // Desenha cenário (background e chão)
            _buildings.Draw(_spriteBatch);

            foreach (var obstacle in _obstacles)
                obstacle.Draw(_spriteBatch);

            // Desenha personagem
            _player.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
