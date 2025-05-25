# TrbalhoPraticoTecnicas2

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



# Classes #

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
Nesta classe é onde a informação relevante ao estado e variaveis dos inimigos é gerida


Temos aqui definidos 
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

##EnemiesManage##

