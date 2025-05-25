using JogoTecnicas.Gestão;
using JogoTecnicas.Graficos;
using JogoTecnicas.Inimigos;
using JogoTecnicas.Objetos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Numerics;

// Adicione o alias para evitar ambiguidade
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace JogoTecnicas
{
    public class Game1 : Game
    {

        //morte
        private bool _isDying = false;
        private double _deathAnimationTimer = 0;
        private double _deathAnimationDuration = 1.0; // tempo da animação


        // Gerenciador de estado do jogo
        private GameManager _gameManager = new GameManager();

        //outras classes
        private KeyboardInput _keyboardInput = new KeyboardInput();
        private Buildings _buildings;
        private Player _player;
        private EnemiesManage _enemies;
        private Texture2D _inimigovoa;
        private Texture2D _inimigochao;
        private SpriteFont _font;
        private bool _isGameOver = false;

        //sprites
        private const string ASSET_NAME_SPRITESHEET = "AnimacoesV2";
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
        private int _totalFrames = 7;
        private float _timePerFrame = 0.1f;

        // Textura de fundo e chão
        private Texture2D _backgroundTexture;
        private Texture2D _floorTexture;

        // Propriedades públicas para o GameManager
        public Player Player { get => _player; set => _player = value; }
        public EnemiesManage Enemies { get => _enemies; set => _enemies = value; }
        public Buildings Buildings { get => _buildings; set => _buildings = value; }
        public Texture2D Voador => _inimigovoa;

        public Texture2D Chao => _inimigochao;
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
        public Wall Wall { get; set; }
        public Camera _camera { get; private set; }


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

            // Inicializa a câmera
            _camera = new Camera(_screenWidth, _screenHeight);

            base.Initialize();
        }


        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Wall = new Wall(GraphicsDevice, ScreenHeight, 20f); 


            _spriteSheetTextureRun = Content.Load<Texture2D>(ASSET_NAME_SPRITESHEET);

            //Sons e musica
            Sound.LoadContent(Content);
            Sound.PlayBackgroundMusic();

            // Crie as animações de correr e saltar
            var runAnimation = new SpriteAnimation(_spriteSheetTextureRun, 193, _frameWidth, _frameHeight, _totalFrames, _timePerFrame);
            var jumpAnimation = new SpriteAnimation(_spriteSheetTextureRun, 129, _frameWidth, _frameHeight, _totalFrames, _timePerFrame);
            var slideAnimation = new SpriteAnimation(_spriteSheetTextureRun, 257, _frameWidth, _frameHeight, _totalFrames, _timePerFrame);
            var idleAnimation = new SpriteAnimation(_spriteSheetTextureRun, 65, _frameWidth, _frameHeight, 4, _timePerFrame);
            var deathAnimation = new SpriteAnimation(_spriteSheetTextureRun, 0, _frameWidth, _frameHeight, _totalFrames, _timePerFrame);


            //carrega textura inimigos
            _inimigovoa = Content.Load<Texture2D>("Enemy2");
            _inimigochao = Content.Load<Texture2D>("Site2Ground");

            // Cria as animações dos inimigos
            var voador = new SpriteAnimation(_inimigovoa, 65, 64, 64, 6, 0.1f);
            var caveira = new SpriteAnimation(_inimigochao, 65, 64, 64, 8, 0.2f);
            var caveiramorre = new SpriteAnimation(_inimigochao, 0, 64, 64, 8, 0.2f);
            caveiramorre.Loop = false; 
            var voadormorre = new SpriteAnimation(_inimigovoa, 0, 64, 64, 6, 0.1f);
            voadormorre.Loop = false; 
            // Inicializa o Player
            _player = new Player(runAnimation, jumpAnimation, slideAnimation, idleAnimation,deathAnimation, new Vector2(180, floorY - _frameHeight));

            //inicializa os inimigos
            _enemies = new EnemiesManage(voador, caveira,voadormorre,caveiramorre);




            //caixas de colisão
            _player.SetRunCollisionBox(new Rectangle(15, 20, 40, 45)); // Colisão para corrida
            _player.SetJumpCollisionBox(new Rectangle(15, 5, 35, 50)); // Colisão para salto
            _player.SetSlideCollisionBox(new Rectangle(10, 50, 45, 15)); // Colisão para deslizar
            _player.SetIdleCollisionBox(new Rectangle(20, 15, 20, 50)); // Colisão para Idle que está certa

            

            
            
            //carregar fonte
            _font = Content.Load<SpriteFont>("DefaultFont");
            _score = new Score(_font);



            // Carrega as texturas de fundo e chão
            _backgroundTexture = Content.Load<Texture2D>(ASSET_NAME_BACKGROUND);
            _floorTexture = Content.Load<Texture2D>(ASSET_NAME_FLOOR);

            // Inicializa a classe Buildings
            _buildings = new Buildings(_backgroundTexture, _floorTexture, _screenWidth, _screenHeight);
        }

        protected override void Update(GameTime gameTime)
        {

            if (_isDying)
            {
                _deathAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;

                _player.Update(gameTime, _keyboardInput, _buildings.FloorRectangle); 

                if (_deathAnimationTimer >= _deathAnimationDuration)
                {
                    _isGameOver = true;
                    _isDying = false;
                }

                return; // Sai do update para não atualizar mais nada
            }



            if (_isGameOver)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    _gameManager.RestartGame(this);
                }
                return;
            }

            // Define a velocidade máxima e base da parede
            const float maxWallSpeed = 500f; // Velocidade máxima da parede
            const float baseWallSpeed = 20f; // Velocidade inicial da parede
            const float playerBaseSpeed = 200f; // Velocidade base do jogador
            const float distanceThreshold = 200f; // Distância X para aumentar a velocidade

            // Calcula a distância entre a parede e o jogador
            float distanceToPlayer = Player.Position.X-15 - Wall.BoundingBox.Right;

            // Ajusta a velocidade da parede com base na distância
            if (distanceToPlayer > distanceThreshold)
            {
                // Aumenta a velocidade da parede gradualmente
                Wall.Speed = MathHelper.Clamp(Wall.Speed + 15f * (float)gameTime.ElapsedGameTime.TotalSeconds, baseWallSpeed, maxWallSpeed);
            }
            else
            {
                // Calcula a nova velocidade da parede com base no score
                float newWallSpeed = MathHelper.Clamp(baseWallSpeed + _score.CurrentScore / 10f, baseWallSpeed, maxWallSpeed);

                // Garante que a velocidade da parede seja sempre menor que a do jogador
                if (newWallSpeed >= playerBaseSpeed * 0.8f)
                {
                    newWallSpeed = playerBaseSpeed * 0.7f; // Mantém uma margem de segurança
                }

                Wall.Speed = newWallSpeed;
            }

            // Atualiza a parede
            Wall.Update(gameTime);

            // Verifica colisão entre o jogador e a parede
            if (!_isDying && Wall.BoundingBox.Intersects(Player.BoundingBox))
            {
                _player.Die();
                Sound.PlayDeath();
                _isDying = true;
                _deathAnimationTimer = 0;
            }

            _score.Update(gameTime, _isGameOver);
            _keyboardInput.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Atualiza o cenário
            _buildings.Update(gameTime, Player.isPlayerMovingRight);

            // Atualiza o player
            _player.Update(gameTime, _keyboardInput, _buildings.FloorRectangle);

            // Atualiza os inimigos
            _enemies.Update(gameTime, 5f, Player.isPlayerMovingRight, Player.Position.X,_score.CurrentScore);
            

            // Atualiza a câmera com a posição do jogador
            _camera.Update(_player.Position);


            bool playerHit = false;

            foreach (var enemy in _enemies.GetAliveEnemies())
            {
                if (_player.BoundingBox.Intersects(enemy.Bounds))
                {
                    if (enemy.Type == EnemyType.Runner && _player.IsSliding)
                    {
                        enemy.Die();
                        continue;
                    }
                    else if (enemy.Type == EnemyType.Static && _player.IsJumping && _enemies.IsJumpingOnTop(_player.BoundingBox, enemy.Bounds))
                    {
                        enemy.Die();
                        continue;
                    }
                    else
                    {
                        playerHit = true;
                        break;
                    }
                }
            }

            if (!_isDying && playerHit)
            {
                _player.Die();
                Sound.PlayDeath();
                _isDying = true;
                _deathAnimationTimer = 0;
            }


            base.Update(gameTime);
        }




        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _buildings.DrawBackground(_spriteBatch);
            _buildings.DrawFloor(_spriteBatch);
            _spriteBatch.End();

            

            // Obtém a matriz de visualização da câmera
            Matrix viewMatrix = _camera.GetViewMatrix();

            // Desenha o cenário, o player e os obstáculos com a câmera
            _spriteBatch.Begin(transformMatrix: viewMatrix);

            
            Wall.Draw(_spriteBatch);
            _player.Draw(_spriteBatch);
            _enemies.Draw(_spriteBatch);

            // Desenha os retângulos de colisão
            DrawCollisionBox(_spriteBatch, _player.BoundingBox, Color.Red);
            foreach (var enemies in _enemies.GetBoundingBoxes())
            {
                DrawCollisionBox(_spriteBatch, enemies, Color.Yellow);
            }

            _spriteBatch.End();

            // Desenha o Score sem a transformação da câmera
            _spriteBatch.Begin(); // Sem `transformMatrix`
            _score.Draw(_spriteBatch, 1.25f);
           


            // Exibe mensagem de Game Over
            if (_isGameOver)
            {
                string gameOverText = "Game Over";
                string restartText = "Pressione R para recomecar";

                // Calcula as posições para centralizar os textos
                Vector2 gameOverSize = _font.MeasureString(gameOverText);
                Vector2 restartSize = _font.MeasureString(restartText);

                // Centraliza os textos na tela
                Vector2 gameOverPosition = new Vector2((_screenWidth - gameOverSize.X) / 2, (_screenHeight - gameOverSize.Y) / 2 - 30);
                Vector2 restartPosition = new Vector2((_screenWidth - restartSize.X) / 2, (_screenHeight - restartSize.Y) / 2 + 20);

                // Desenha os textos
                _spriteBatch.DrawString(_font, gameOverText, gameOverPosition, Color.Red, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
                _spriteBatch.DrawString(_font, restartText, restartPosition, Color.White);
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
        public void ResetCamera()
        {
            _camera = new Camera(ScreenWidth, ScreenHeight);
            _camera.Update(Player.Position);
        }
        

    }
}
