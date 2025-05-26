# TrabalhoPraticoTecnicas2

Foi feito um Endless Runner em que o objetivo é sobreviver o máximo de tempo possível enquanto que se é perseguido por uma parede de espinhos e se desvia de obstáculos saltando e deslizando.

# Content #

Os assets utilizados foram retirados, com permissão, dos seguintes links:  

[base do personagem](https://benvictus.itch.io/test-dummy-platformer)  
[sound effects](https://brackeysgames.itch.io/brackeys-platformer-bundle)  
[sound effects](https://epesami.itch.io/small-explosion-audio-pack)  
[musica de background](https://oandco.itch.io/loopable-video-game-music)  

Os restantes assets forma criados para este jogo, por nôs.

# Gameplay #

O nosso jogo baseia-se num sidescroller em que o personagem tem de fugir de uma parede de espinhos que o persegue, enquanto se desvia de inimigos que aparecem com cada vez mais frequencia.
Para sobreviver o personagem utiliza as suas capacidades de correr, saltar e deslizar.
O ato de deslizar serve para desviar dos inimigos que voam, enquanto que o saltar serve para desviar dos inimigos que estao no chao.  
Também é possivel atacar inimigos voadores saltando na cabeça deles e inimigos 

# Classe auxiliares #
Para a criação do nosso jogo são utilizadas uma série de classe de pequeno tamnanho e complexidade com o objetivo de agilizar o processo.   
Sendo estas:
- Sound.cs, responsavel pelos sons do jogo;
- Score.cs, reponsavel por fazer o score do jogo visivel ao jogador;
- Collisions.cs, que tem o metodo qua é usado para verificar as colisóes entre dois retangulos;
- SpriteAnimation.cs, que contem os metodos relacionados a começar, parar, reiniciar e desenhar animações;
- GameState.cs, que contem os estados possiveis do jogo, sendo estes Running e GameOver;
- GameManager.cs, que guarda um backup das variaveis utilizadas pelo jogo para que quando seja iniciado o reset tudo volte ao valor inicial;

# Classes Principais #
## Game1 ##
A classe Game1 é o núcleo do jogo, sendo responsável por gerenciar as 4 partes mais importantes do jogo, a Inicialização (Initialize), o carregamento do conteúdo (LoadContent), a atualização lógica a cada frame (Update) e por fim o desenho na tela (Draw).
No início da classe são declaradas as variáveis dos principais elementos do jogo.
```
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
```
No construtor da classe Game1 é configurado o GraphicsDeviceManager e o diretório do conteúdo.
```
public Game1()
{
    _graphics = new GraphicsDeviceManager(this);
    Content.RootDirectory = "Content";
    IsMouseVisible = true;
}
```
No método Initialize são ajustadas as configurações gráficas e é inicializada a câmera.
```
protected override void Initialize()
{
    _graphics.IsFullScreen = false;
    _graphics.PreferredBackBufferWidth = _screenWidth;
    _graphics.PreferredBackBufferHeight = _screenHeight;
    _graphics.ApplyChanges();

    floorY = _screenHeight - 60;
    _camera = new Camera(_screenWidth, _screenHeight);

    base.Initialize();
}
```
No método LoadContent são carregadas todas as texturas, fontes, sons, objetos do jogador, inimigos e por fim o cenário.
```
 protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Wall = new Wall(GraphicsDevice, Content, ScreenHeight, 20f);



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
```
O método Update é chamado frame a frame para atualizar e verificar o estado do jogo, sendo dividido em diversas verificações e atualizações.
   Gerenciar o estado de morte e animação de morte:
   ```
if (_isDying)
{
    _deathAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
    _player.Update(gameTime, _keyboardInput, _buildings.FloorRectangle); 
    if (_deathAnimationTimer >= _deathAnimationDuration)
    {
        _isGameOver = true;
        _isDying = false;
    }
    return;
}
```
   Reiniciar quando acontece Game Over:
   ```
if (_isGameOver)
{
    if (Keyboard.GetState().IsKeyDown(Keys.R))
    {
        _gameManager.RestartGame(this);
    }
    return;
}
```
   Controlar a velocidade da parede e atualização da posição:
   ```
float distanceToPlayer = Player.Position.X-15 - Wall.BoundingBox.Right;

if (distanceToPlayer > distanceThreshold)
{
    Wall.Speed = MathHelper.Clamp(Wall.Speed + 15f * (float)gameTime.ElapsedGameTime.TotalSeconds, baseWallSpeed, maxWallSpeed);
}
else
{
    float newWallSpeed = MathHelper.Clamp(baseWallSpeed + _score.CurrentScore / 500f, baseWallSpeed, maxWallSpeed);
    if (newWallSpeed > playerBaseSpeed * 0.9f)
        newWallSpeed = playerBaseSpeed * 0.9f; 
    Wall.Speed = newWallSpeed;
}
```
   Atualizar entidades e verificar colisões:
   ```
Wall.Update(gameTime);
if (!_isDying && Wall.BoundingBox.Intersects(Player.BoundingBox))
{
    _player.Die();
    Sound.PlayDeath();
    _isDying = true;
    _deathAnimationTimer = 0;
}

_score.Update(gameTime, _isGameOver);
_keyboardInput.Update();

// Atualiza cenário, player, inimigos
_buildings.Update(gameTime, Player.isPlayerMovingRight);
_player.Update(gameTime, _keyboardInput, _buildings.FloorRectangle);
_enemies.Update(gameTime, 5f, Player.isPlayerMovingRight, Player.Position.X,_score.CurrentScore);
_camera.Update(_player.Position);

// Colisão player x inimigos
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
```
Por fim temos o método Draw, responsável por desenhar tudo na tela seguindo uma ordem.
```
GraphicsDevice.Clear(Color.CornflowerBlue);
_spriteBatch.Begin();
_buildings.DrawBackground(_spriteBatch);
_buildings.DrawFloor(_spriteBatch);
_spriteBatch.End();

// Matriz de visualização da câmera
Matrix viewMatrix = _camera.GetViewMatrix();

_spriteBatch.Begin(transformMatrix: viewMatrix);
Wall.Draw(_spriteBatch);
_player.Draw(_spriteBatch);
_enemies.Draw(_spriteBatch);
_spriteBatch.End();

// Desenha o Score (HUD)
_spriteBatch.Begin();
_score.Draw(_spriteBatch, 1.25f);

// Mensagem de Game Over centralizada
if (_isGameOver)
{
    string gameOverText = "Game Over";
    string restartText = "Pressione R para recomecar";
    Vector2 gameOverSize = _font.MeasureString(gameOverText);
    Vector2 restartSize = _font.MeasureString(restartText);
    Vector2 gameOverPosition = new Vector2((_screenWidth - gameOverSize.X) / 2, (_screenHeight - gameOverSize.Y) / 2 - 30);
    Vector2 restartPosition = new Vector2((_screenWidth - restartSize.X) / 2, (_screenHeight - restartSize.Y) / 2 + 20);
    _spriteBatch.DrawString(_font, gameOverText, gameOverPosition, Color.Red, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
    _spriteBatch.DrawString(_font, restartText, restartPosition, Color.White);
}
_spriteBatch.End();
}
```
Temos ainda o método ResetCamera que é responsável por fazer com que a camera volte aos seus parâmetros inicias quando é chamado.
```
public void ResetCamera()
{
    _camera = new Camera(ScreenWidth, ScreenHeight);
    _camera.Update(Player.Position);
}
```
------------------------------------------------------------------------------------------------
## Player ##

No inicio da classe, initializam-se as variaveis todas para as animacoes e os estados dos personagens, tal como as variaveis para as CollisionBoxes.

No construtor "Player" atribui-se as variaveis responsvaeis pelas animações e pelas colisões.

```
   public Player(SpriteAnimation runAnimation, SpriteAnimation jumpAnimation, SpriteAnimation slideAnimation, SpriteAnimation idleAnimation, SpriteAnimation deathAnimation, Vector2 startPosition)
        {
            _runAnimation = runAnimation;
            _jumpAnimation = jumpAnimation;
            _slideAnimation = slideAnimation;
            _idleAnimation = idleAnimation;
            _currentAnimation = _idleAnimation;
            _deathAnimation = deathAnimation;
            _position = startPosition;

            _runCollisionBox = new Rectangle(0, 0, _runAnimation.FrameWidth, _runAnimation.FrameHeight);
            _jumpCollisionBox = new Rectangle(0, 0, _jumpAnimation.FrameWidth, _jumpAnimation.FrameHeight);
            _slideCollisionBox = new Rectangle(0, 0, _slideAnimation.FrameWidth, _slideAnimation.FrameHeight);
            _idleCollisionBox = new Rectangle(0, 0, _idleAnimation.FrameWidth, _idleAnimation.FrameHeight);
            
        }
```

Na Função Die, verificamos se o player esta morto, se não estiver ele vai entrar no estado _isDead, vai sair de outros estados que esteja e vai fazer a animação de morte.

```
 public void Die()
        {
            if (!_isDead)
            {
                _isDead = true;
                _isJumping = false;
                _isSliding = false;
                _isRunning = false;
                _isIdle = false;

                SetCurrentAnimation(_deathAnimation, false);
            }
        }
```



No Update é onde são iniciados as variaveis de fisica e onde é verificado o estado do player para este reagir de acordo.

Inicializção das variaveis de fisica
```

const float gravity = 0.6f;
            const float jumpForce = -12.5f;
            const float slideForce = 3f;
            const float slideRes = 0.10f;
```

Verificar se o jogador morreu, altera a animação, perde a velocidade e prende o personagem para que não caia do mapa
```
// Verifica se o jogador está morto e atualiza a animação de morte e prende o jogador
            if (_isDead)
            {
                if (_currentAnimation.IsPlaying)
                {
                    _currentAnimation.Update(gameTime);
                }

                _position.Y = floorRect.Top - _runAnimation.FrameHeight;
                _verticalVelocity = 0f;

                return;
            }
```

Gravidade constantemente a afetar a velocidade vertical
```
// Física: gravidade e posição vertical (só se estiver vivo)
            _verticalVelocity += gravity;
            _position.Y += _verticalVelocity;

```

Verifica se o player esta a tocar no "chão", que é uma caixa que segue o player no eixo do X e mantém o seu Y
```
Rectangle playerRect = this.BoundingBox;
            bool isOnGround = playerRect.Intersects(FixedYRectangle) && _verticalVelocity >= 0;
```

Verifica se o player esta a fazer um slide, se sim, aumenta a velociade horizontal e oferece resistencia para ele parar de forma natural
```
//logica de slide
            if (_isSliding)
            {
                // Aplicar movimento
                _position.X += _horizontalVelocity;

                // Desacelerar
                if (_horizontalVelocity > 0)
                {
                    _horizontalVelocity -= slideRes;
                    if (_horizontalVelocity < 0) _horizontalVelocity = 0;
                }
                else if (_horizontalVelocity < 0)
                {
                    _horizontalVelocity += slideRes;
                    if (_horizontalVelocity > 0) _horizontalVelocity = 0;
                }

                // Parar slide quando velocidade chega a zero
                if (_horizontalVelocity == 0f)
                {
                    _isSliding = false;
                    SetCurrentAnimation(_runAnimation, true);
                }
            }
            else
            {
                _horizontalVelocity = 0f;
            }
```

Verifica se o player esta no chão, se sim, se estão a receber novos inputs altera o estado e a animação
```
if (isOnGround)
            {
                _position.Y = floorRect.Top - _runAnimation.FrameHeight;
                _verticalVelocity = 0f;

                if (_isJumping && !_jumpAnimation.IsPlaying)
                {
                    _isJumping = false;
                    SetCurrentAnimation(_runAnimation, true);
                }

                if (!_isRunning && !_isJumping && !_isSliding)
                {
                    _isPlayerMovingRight = false;
                    _isIdle = true;
                    SetCurrentAnimation(_idleAnimation, true);
                }
            }

            if (input.IsUpPressed() && !_isJumping && !_isSliding && isOnGround)
            {
                _isPlayerMovingRight = false;
                _isJumping = true;
                _isIdle = false;
                _verticalVelocity = jumpForce;
                Sound.PlayJump();

                SetCurrentAnimation(_jumpAnimation, false);
            }

            if (input.IsDownPressed() && !_isSliding && !_isJumping && isOnGround)
            {
                _isPlayerMovingRight = false;
                _isSliding = true;
                _isIdle = false;
                if (_isFacingRight)
                {
                    _horizontalVelocity = slideForce;
                }
                else
                {
                    _horizontalVelocity = -slideForce;
                }

                SetCurrentAnimation(_slideAnimation, false);
            }

            if (_isSliding && !_slideAnimation.IsPlaying)
            {
                _isSliding = false;
                SetCurrentAnimation(_runAnimation, true);
            }

            if (input.IsRightPressed())
            {
                _isPlayerMovingRight = true;
                _isFacingRight = true;
                _position.X += 0.5f;

                if (!_isJumping && !_isSliding)
                {
                    _isRunning = true;
                    _isIdle = false;
                    SetCurrentAnimation(_runAnimation, true);
                }
            }
            else if (input.IsLeftPressed())
            {
                _isPlayerMovingRight = false;
                _isFacingRight = false;
                _position.X -= 2f;

                if (!_isJumping && !_isSliding)
                {
                    _isRunning = true;
                    _isIdle = false;
                    SetCurrentAnimation(_runAnimation, true);
                }
            }
            else if (!_isJumping && !_isSliding)
            {
                _isRunning = false;
                _isIdle = true;
                SetCurrentAnimation(_idleAnimation, true);
            }

            _currentAnimation.Update(gameTime);
        }
```
Desenha o player com a animação necessaria atualmente, e vira o sprite se necessario
```
public void Draw(SpriteBatch spriteBatch)
        {
            // Desenha a animação do player
            SpriteEffects spriteEffect = _isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            _currentAnimation.Draw(spriteBatch, _position, spriteEffect);

            //// Desenha o retângulo com Y fixo
            //Texture2D rectangleTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            //rectangleTexture.SetData(new[] { Color.White });
            //spriteBatch.Draw(rectangleTexture, FixedYRectangle, Color.Red * 0.5f);
        }
```
Retangulo debaixo do player falado anteriormente
```
public Rectangle FixedYRectangle
        {
            get
            {
                int rectWidth = 70; // Largura fixa do retângulo
                int rectHeight = 40; // Altura fixa do retângulo
                int rectX = (int)_position.X; // Mesmo X do player
                int rectY = 410; // Y constante

                return new Rectangle(rectX, rectY, rectWidth, rectHeight);
            }
        }
```
Fazer a posição do player publica
```
public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }
```

Muda a Collisison box do player dependendo da animação atual e faz essas boxes publicas
```
public Rectangle BoundingBox
        {
            get
            {
                Rectangle collisionBox = _currentAnimation == _runAnimation ? _runCollisionBox :
                                         _currentAnimation == _jumpAnimation ? _jumpCollisionBox :
                                         _currentAnimation == _slideAnimation ? _slideCollisionBox :
                                         _currentAnimation == _deathAnimation ? _runCollisionBox :
                                         _idleCollisionBox;

                return new Rectangle((int)_position.X + collisionBox.X, (int)_position.Y + collisionBox.Y, collisionBox.Width, collisionBox.Height);
            }
        }

        public void SetRunCollisionBox(Rectangle collisionBox) => _runCollisionBox = collisionBox;
        public void SetJumpCollisionBox(Rectangle collisionBox) => _jumpCollisionBox = collisionBox;
        public void SetSlideCollisionBox(Rectangle collisionBox) => _slideCollisionBox = collisionBox;
        public void SetIdleCollisionBox(Rectangle collisionBox) => _idleCollisionBox = collisionBox;
```
Muda a animação atual para aquela que lhe for fornecida
```
private void SetCurrentAnimation(SpriteAnimation animation, bool loop)
        {
            if (_currentAnimation != animation)
            {
                animation.Loop = loop;
                animation.Reset();
                animation.Play();
                _currentAnimation = animation;
            }
        }

```
----------------------------------------------------------------------------------------------------------------------------
## Buildings ##
A classe Buildings é a classe responsável pelo Background e o chão.
No construtor atribuiram-se as variáveis responsáveis pela textura do background, pela textura do chão, pela definição do tamanho da tela e a velocidade com que o background e o chão se movem.
```
public Buildings(Texture2D backgroundTexture, Texture2D floorTexture, int screenWidth, int screenHeight, float backgroundScrollSpeed = 20f, float floorScrollSpeed = 200f)
        {
            _backgroundTexture = backgroundTexture;
            _floorTexture = floorTexture;
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _backgroundScrollSpeed = backgroundScrollSpeed;
            _floorScrollSpeed = floorScrollSpeed;

            _floorOffset = 0f;
            _floorY = _screenHeight - 60;

            // Inicia 3 posições lado a lado
            backgroundPositions = new Vector2[3];
            for (int i = 0; i < backgroundPositions.Length; i++)
            {
                backgroundPositions[i] = new Vector2(i * _backgroundTexture.Width, 0);
            }
        }
```
Na função Update efetuamos uma verificação "if (!isPlayerMovingRight)" para que o background e o chão se possam mover.
```
 public void Update(GameTime gameTime, bool isPlayerMovingRight)
        {
            if (!isPlayerMovingRight)
                return;

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Atualiza posição do background (array de posições)
            float backgroundMovement = _backgroundScrollSpeed * delta;
            for (int i = 0; i < backgroundPositions.Length; i++)
            {
                backgroundPositions[i].X -= backgroundMovement;

                // Reposiciona os blocos que saíram da tela à direita
                if (backgroundPositions[i].X < -_backgroundTexture.Width)
                {
                    float maxX = backgroundPositions.Max(p => p.X);
                    backgroundPositions[i].X = maxX + _backgroundTexture.Width;
                }
            }

            // Atualiza offset do chão para scroll
            _floorOffset += _floorScrollSpeed * delta;
            if (_floorOffset >= _floorTexture.Width)
                _floorOffset -= _floorTexture.Width;
        }
```
Por fim temos as funções "DrawBackground" e "DrawFloor", responsáveis por desenharem o Background e o chão, respetivamente, na tela.
```
 public void DrawBackground(SpriteBatch spriteBatch)
        {
            foreach (var pos in backgroundPositions)
            {
                spriteBatch.Draw(_backgroundTexture, pos, Color.White);
            }
        }

        public void DrawFloor(SpriteBatch spriteBatch)
        {
            // Desenha o chão 3 vezes para loop infinito
            spriteBatch.Draw(_floorTexture, new Vector2(-_floorOffset, _floorY), Color.White);
            spriteBatch.Draw(_floorTexture, new Vector2(_floorTexture.Width - _floorOffset, _floorY), Color.White);
            spriteBatch.Draw(_floorTexture, new Vector2(2 * _floorTexture.Width - _floorOffset, _floorY), Color.White);
        }
```

## Wall ##
Esta classe é a classe responsável, por um dos objetos mais essenciais, para a dinâmica do jogo, a parede.
Antes do construtor temos uma função responsável pela criação da collision_box da parede, que teve de ser ajustada.
```
 public Rectangle BoundingBox
        {
            get
            {
                int offsetX = 0; // Reduz a hitbox horizontalmente (margem esquerda e direita)
                int offsetY = -20; // Reduz a hitbox verticalmente (margem superior e inferior)
                int reducedWidth = _width - 385; // Largura reduzida
                int reducedHeight = _height; // Altura reduzida

                return new Rectangle(
                    (int)_position.X + offsetX, // Aplica o deslocamento horizontal
                    (int)_position.Y + offsetY, // Aplica o deslocamento vertical
                    reducedWidth, // Define a largura reduzida
                    reducedHeight // Define a altura reduzida
                );
            }
        }
```
Temos o construtor onde são atribuídas as variáveis responsáveis pelo tamanho, a posição inicial e a textura da parede.
```
public Wall(GraphicsDevice graphicsDevice, ContentManager content, int screenHeight, float speed)
        {
            _width = 400; // Aumente a largura da parede
            _height = screenHeight; // Altura da parede igual à altura da tela
            _position = new Vector2(0, -38); // Sempre começa na posição X = 0
            _speed = speed; // Velocidade de movimento da parede

            // Carrega a textura da parede a partir do arquivo wall1long.png
            _texture = content.Load<Texture2D>("wall1long");

            //// Cria uma textura de 1x1 pixel para desenhar a hitbox
            //_hitboxTexture = new Texture2D(graphicsDevice, 1, 1);
            //_hitboxTexture.SetData(new[] { Color.Red }); // Define a cor da hitbox como vermelha
        }
```
A função Update é a função responsável pelo movimento da parede em direção ao player.
```
public void Update(GameTime gameTime)
        {
            // Move a parede para a direita
            _position.X += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
```
Na função Draw é onde se desenha a parede, tendo como parâmetros a textura, a posição e a cor, que neste caso é Color.White, ou seje, sem cor.
```
public void Draw(SpriteBatch spriteBatch)
        {
            // Desenha a textura da parede com sua resolução original
            spriteBatch.Draw(_texture, _position, Color.White);

            //// Desenha a hitbox (borda do retângulo)
            //DrawRectangle(spriteBatch, BoundingBox, Color.Red);
        }
```
Por fim temos a função Reset, responsável por, em caso de reset, colocar a parede na posição inicial e retornar a velocidade inicial da parede.
```
 public void Reset()
        {
            // Reseta a posição da parede para o início
            _position.X = 0;
            _speed = 20;
        }
```
---------------------------------------------------------------------------------------------------------
## Camara ##
Esta classe é responsável pelo funcionamento da câmera que segue o player quando este se desloca para a direita.
Temos o construtor, responsável por definir a largura e a altura da tela e também a posição do player.
```
public Camera(int screenWidth, int screenHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _position = Vector2.Zero;
        }
```
Ainda temos a função Update responsável por verificar se a posição do player se altera para a direita para deslocar a câmera e também é responsável por impedir que a câmera se desloque para posições negativas.
```
public void Update(Vector2 playerPosition)
        {
            // A câmera só se move para a direita
            if (playerPosition.X > _position.X + _screenWidth / 2)
            {
                _position.X = playerPosition.X - _screenWidth / 2;
            }

            // Impede que a câmera vá para posições negativas
            if (_position.X < 0) _position.X = 0;
        }
```
Por fim temos a função GetViewMatrix responsável por retornar uma translação que dá o efeito visual de movimento da câmera.
```
public Matrix GetViewMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-_position, 0));
        }
```
## Enemy ##
Nesta classe é onde a informação relevante ao estado e variaveis dos inimigos é gerida.


Temos aqui definidos dois conjuntos de valores nomeados, um deles referente ao estado do inimigo e outro a referir ao seu tipo
```
public enum EnemyState
    {
        Alive,
        Dying,
        Dead
    }
    public enum EnemyType
    {
        Static,
        Runner
    }
```

As variaveis que são inicializadas para esta classe e o seu construtor
```
 public SpriteAnimation Animation;
        public Vector2 Position;
        public EnemyType Type => _type;
        public Rectangle Bounds => new((int)Position.X + _hitboxOffsetX, (int)Position.Y + _hitboxOffsetY, _hitboxWidth, _hitboxHeight);
        private EnemyType _type;
        private float _speed;
        private int _direction = 1; // 1: direita, -1: esquerda
        private int _hitboxWidth;
        private int _hitboxHeight;
        private int _hitboxOffsetX;
        private int _hitboxOffsetY;


        //para conrtrolar morte
        private EnemyState _state = EnemyState.Alive;
        private SpriteAnimation _deathAnimation;
        
        private bool _deathSoundPlayed = false;

        public bool IsDead => _state == EnemyState.Dead;
        public EnemyState State => _state;

        public Enemy(SpriteAnimation animation, Vector2 position, EnemyType type, float speed,
    int hitboxOffsetX, int hitboxOffsetY, int hitboxWidth, int hitboxHeight,
    SpriteAnimation deathAnim)
        {
            Animation = animation;
            Position = position;
            _type = type;
            _speed = speed;
            _hitboxOffsetX = hitboxOffsetX;
            _hitboxOffsetY = hitboxOffsetY;
            _hitboxWidth = hitboxWidth;
            _hitboxHeight = hitboxHeight;
            _deathAnimation = deathAnim;
            
        }
```
A função Die dos inimigos, que altera o seu estado, começa a sua animação e toca o som da sua morte
```
public void Die()
        {
            if (_state != EnemyState.Alive) return;

            _state = EnemyState.Dying;
            _deathAnimation.Reset();
            _deathAnimation.Play();

            if (!_deathSoundPlayed)
            {
                Sound.PlayExplosion();
                _deathSoundPlayed = true;
            }
        }
```
Verifica o estado do inimigo, faz update ao frame das suas animações e altera a posição do inimigo runner
```
public void Update(GameTime gameTime, float worldSpeed)
        {
            if (_state == EnemyState.Alive)
            {
                Position.X -= worldSpeed;
                if (_type == EnemyType.Runner)
                    Position.X -= _speed;
                Animation.Update(gameTime);
            }
            else if (_state == EnemyState.Dying)
            {
                _deathAnimation.Update(gameTime);
                if (!_deathAnimation.IsPlaying)
                    _state = EnemyState.Dead;
            }
        }
```
Desenha o inimigo, dependendo do estado dele muda a animação
```
public void Draw(SpriteBatch spriteBatch)
        {
            if (_state == EnemyState.Dead) return;

            if (_state == EnemyState.Dying)
                _deathAnimation.Draw(spriteBatch, Position);
            else
                Animation.Draw(spriteBatch, Position, _direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
        }
```
## EnemiesManage ##
Esta é a classe que gera a utilização dos inimigos no jogo como, afrequencia com os inimigos apareçem, remover os inimigos que já não são necessarios e as suas colisões

Inicializa as variaveis necessarias para esta classe
```
 private List<Enemy> _enemies = new();
        private Random _random = new();
        private float _spawnTimer;
        private float _spawnInterval = 2f;
        private SpriteAnimation _staticAnim;
        private SpriteAnimation _patrolAnim;
        private SpriteAnimation _deathAnimStatic;
        private SpriteAnimation _deathAnimRunner;
```
Verifica que inimigos estão mortos ou a morrer e returna apenas os vivos
```
//metodo para saber que inimigos estão vivos
        public IEnumerable<Enemy> GetAliveEnemies()
        {
            foreach (var enemy in _enemies)
                if (!enemy.IsDead && enemy.State != EnemyState.Dying)
                    yield return enemy;
        }
```
O construtor da classe
```
public EnemiesManage(SpriteAnimation staticAnim, SpriteAnimation patrolAnim,
                             SpriteAnimation deathAnimStatic, SpriteAnimation deathAnimRunner)
        {
            _staticAnim = staticAnim;
            _patrolAnim = patrolAnim;
            _deathAnimStatic = deathAnimStatic;
            _deathAnimRunner = deathAnimRunner;
            
        }
```
No metodo Update.
Verica-se que o jogador esta a mexer para a direita para os inimigos também se mexerem.  
Altera o intervalo entre o spawn de inimigos com o amumento do score.  
Liga o temporizador para o spawn de inimigos ao tempo de jogo.
```
float scrollSpeed = isPlayerMovingRight ? worldSpeed : 0f;
            _spawnInterval = Math.Max(0.8f, 2f - currentscore * 0.01f);

            _spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
```
Verifica se o temporizador do spawn esta maior que o intervalo, se sim reinica o temporizador, escolhe um inimigo aleatorio, clona a animação para não haver conflito e adiciona o inimigo.  
```
if (_spawnTimer >= _spawnInterval)
            {
                _spawnTimer = 0;
                float y = 350;
                float x = playerX + 1000; //spawn de inimigos 1000 a direita do player

                if (_random.Next(2) == 0)
                {
                    var anim = CloneAnimation(_staticAnim);
                    var deathAnim = CloneAnimation(_deathAnimStatic);
                    deathAnim.Loop = false; // Animação de morte não deve ser repetida
                    _enemies.Add(new Enemy(anim, new Vector2(x, y), EnemyType.Static, 0f, 20, 15, 26, 22, deathAnim));
                }
                else
                {
                    var anim = CloneAnimation(_patrolAnim);
                    var deathAnim = CloneAnimation(_deathAnimRunner);
                    deathAnim.Loop = false; // Animação de morte não deve ser repetida
                    _enemies.Add(new Enemy(anim, new Vector2(x, y), EnemyType.Runner, 4f, 10, 20, 44, 40, deathAnim));
                }
            }
```
Verifica a lista de inimigos se um deles esta morto é removido
```
            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                _enemies[i].Update(gameTime, scrollSpeed);

                // Remove inimigos completamente mortos
                if (_enemies[i].IsDead)
                    _enemies.RemoveAt(i);
            }
        }
```
Verifica se o jogador esta a saltar em cima de um inimigo
```
//função que verifica se o jogador esta a saltar em cima do inimigo
        public bool IsJumpingOnTop(Rectangle player, Rectangle enemy)
        {
            // Verifica se a parte inferior do jogador está acima da parte superior do inimigo e descendo
            return player.Bottom >= enemy.Top - 5 &&
                   player.Bottom <= enemy.Top + 10 &&
                   player.Center.X >= enemy.Left &&
                   player.Center.X <= enemy.Right;
        }
```
Verifica se o inimigo foi atingido enquanto que o player faz um slide
```
//remove o inimigo runner quando o player faz um slide nele
        public void RemoveHitEnemies(Rectangle playerBounds, bool isSliding, bool isJumping)
        {
            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                var enemy = _enemies[i];

                if (enemy.Bounds.Intersects(playerBounds))
                {
                    if (enemy.Type == EnemyType.Runner && isSliding)
                    {
                        enemy.Die();
                    }
                    else if (enemy.Type == EnemyType.Static && isJumping && IsJumpingOnTop(playerBounds, enemy.Bounds))
                    {
                        enemy.Die();
                    }
                }
            }

```
Desenhar os inimigos
```
public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var enemy in _enemies)
                enemy.Draw(spriteBatch);
        }
```
Verificar as colisões entre inimigos e player
```
public bool CheckCollision(Rectangle playerBounds)
        {
            foreach (var enemy in _enemies)
            {
                if (enemy.Bounds.Intersects(playerBounds))
                    return true;
            }
            return false;
        }
```
Percorre os inimigos e recolhe as boundingBoxes deles
```
public IEnumerable<Rectangle> GetBoundingBoxes()
        {
            foreach (var enemy in _enemies)
                yield return enemy.Bounds;
        }
```
Metodo para clonar as animações para que cada inimigo possa alterar a sua animação independente dos outros
```
// Clona a animação para evitar bugs
        private SpriteAnimation CloneAnimation(SpriteAnimation anim)
        {
            
            
            return new SpriteAnimation(
                anim.Texture,
                anim.Index,
                anim.FrameWidth,
                anim.FrameHeight,
                anim.TotalFrames,
                anim.TimePerFrame
            );
        }
```
O reset para retornar o jogo ao normal quando necessario
```
public void Reset()
        {
            _enemies.Clear();
            _spawnTimer = 0f;
            _spawnInterval = 2f;
        }
```
--------------------------------------------------------------------------------------
## Observações ##
Temos como maior observação a complicação que acabou por ficar a classe Game1 e gostariamos que ela tivesse ficado mais organizada.
Podiamos ter adicionado mais inimigos com padrões diferentes.
Podiamos ter adicionado mais mecânicas a nível da mobilidade do jogador, como por exemplo, um trampolim ou então um salto-duplo.
