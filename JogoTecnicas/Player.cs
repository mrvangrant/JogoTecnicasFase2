using JogoTecnicas.Graficos;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace JogoTecnicas
{
    public class Player
    {
        private SpriteAnimation _runAnimation;
        private SpriteAnimation _jumpAnimation;
        private SpriteAnimation _slideAnimation;
        private SpriteAnimation _idleAnimation;
        private SpriteAnimation _deathAnimation;
        private SpriteAnimation _currentAnimation;

        private Vector2 _position;
        private bool _isRunning = false;
        private bool _isJumping = false;
        private bool _isSliding = false;
        private bool _isIdle = true;
        private bool _isDead = false;
        private float _verticalVelocity = 0f;
        private float _horizontalVelocity = 0f;
        private bool _isFacingRight = true;
        private bool _isPlayerMovingRight = false;

        private Rectangle _runCollisionBox;
        private Rectangle _jumpCollisionBox;
        private Rectangle _slideCollisionBox;
        private Rectangle _idleCollisionBox;

        public bool IsIdle => _isIdle;
        public bool IsFacingRight => _isFacingRight;
        public bool isPlayerMovingRight => _isPlayerMovingRight;
        public bool IsSliding => _isSliding;
        public bool IsJumping => _isJumping;
        public bool IsDead => _isDead;

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
        public void Update(GameTime gameTime, KeyboardInput input, Rectangle floorRect)
        {
            const float gravity = 0.6f;
            const float jumpForce = -12.5f;
            const float slideForce = 3f;
            const float slideRes = 0.10f;


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

            // Física: gravidade e posição vertical (só se estiver vivo)
            _verticalVelocity += gravity;
            _position.Y += _verticalVelocity;

            Rectangle playerRect = this.BoundingBox;
            bool isOnGround = playerRect.Intersects(FixedYRectangle) && _verticalVelocity >= 0;



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
        public Rectangle FixedYRectangle
        {
            get
            {
                int rectWidth = 70; // Largura fixa do retângulo
                int rectHeight = 40; // Altura fixa do retângulo
                int rectX = (int)_position.X; // Mesmo X do player
                int rectY = 410; // Y constante (ajuste conforme necessário)

                return new Rectangle(rectX, rectY, rectWidth, rectHeight);
            }
        }




        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

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
        public Rectangle HorizontalTrackingBox
        {
            get
            {
                // Define o Y fixo e usa o X do player
                int fixedY = 300; // Valor fixo para o eixo Y
                return new Rectangle((int)Position.X, fixedY, BoundingBox.Width, BoundingBox.Height);
            }
        }


    }
}
