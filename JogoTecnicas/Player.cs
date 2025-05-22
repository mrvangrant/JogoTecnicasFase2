using JogoTecnicas.Graficos;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace JogoTecnicas
{
    public class Player
    {
        private SpriteAnimation _runAnimation;
        private SpriteAnimation _jumpAnimation;
        private SpriteAnimation _slideAnimation; // Adicionado: Campo para a animação de deslizar
        private SpriteAnimation _currentAnimation;

        private Vector2 _position;
        private bool _isJumping = false;
        private bool _isSliding = false;
        private float _verticalVelocity = 0f; // Velocidade vertical do jogador


        public Player(SpriteAnimation runAnimation, SpriteAnimation jumpAnimation, SpriteAnimation slideAnimation, Vector2 startPosition)

        private const float JumpDelay = 0.27f; // Atraso antes de iniciar a animação de salto

        public Player(SpriteAnimation runAnimation, SpriteAnimation jumpAnimation, Vector2 startPosition)

        {
            _runAnimation = runAnimation;
            _jumpAnimation = jumpAnimation;
            _slideAnimation = slideAnimation; // Inicializa o campo _slideAnimation
            _currentAnimation = _runAnimation;
            _position = startPosition;
        }

        public void Update(GameTime gameTime, KeyboardInput input, Rectangle floorRect)
        {

            // Variáveis de física
            const float gravity = 0.6f; // Gravidade que puxa o jogador para baixo
            const float jumpForce = -12.5f; // Força inicial do salto
            const float groundLevel = 346; // Posição Y do chão

            const float gravity = 0.6f;
            const float jumpForce = -12.5f;


            // Aplica gravidade sempre
            _verticalVelocity += gravity;
            _position.Y += _verticalVelocity;


            // Inicia o salto apenas se o jogador estiver no chão e não estiver deslizando
            if (input.IsUpPressed() && !_isJumping && !_isSliding && isOnGround)
            {
                _isJumping = true;
                _verticalVelocity = jumpForce; // Aplica a força inicial do salto

                // Inicia a animação de salto
                _jumpAnimation.Loop = false;
                _jumpAnimation.Reset();
                _jumpAnimation.Play();
                _currentAnimation = _jumpAnimation;
            }

            // Atualiza o retângulo do player após o movimento
            Rectangle playerRect = this.BoundingBox;

            // Verifica colisão com o chão (apenas se estiver a cair)
            bool isOnGround = playerRect.Intersects(floorRect) && _verticalVelocity >= 0;

            // Se colidiu com o chão, ajusta a posição e reseta a velocidade
            if (isOnGround)
            {
                _position.Y = floorRect.Top - _runAnimation.FrameHeight;
                _verticalVelocity = 0f;


                if (_isJumping && !_jumpAnimation.IsPlaying)
                {
                    _isJumping = false;
                    _runAnimation.Loop = true;
                    _runAnimation.Play();
                    _currentAnimation = _runAnimation;
                }
            }


            // Inicia a animação de deslizar apenas se o jogador estiver no chão e não estiver pulando
            if (input.IsDownPressed() && !_isSliding && !_isJumping && isOnGround)
            {
                _isSliding = true;

                // Inicia a animação de deslizar
                _slideAnimation.Loop = false;
                _slideAnimation.Reset();
                _slideAnimation.Play();
                _currentAnimation = _slideAnimation;
            }

            // Finaliza a animação de deslizar
            if (_isSliding && !_slideAnimation.IsPlaying)
            {
                _isSliding = false;

                // Retorna à animação de corrida
                _runAnimation.Loop = true;
                _runAnimation.Play();
                _currentAnimation = _runAnimation;
            }
           

            // Atualiza a animação atual

            // Só permite iniciar o salto se está no chão
            if (input.IsUpPressed() && isOnGround && !_isJumping)
            {
                _isJumping = true;
                _verticalVelocity = jumpForce;

                _jumpAnimation.Loop = false;
                _jumpAnimation.Reset();
                _jumpAnimation.Play();
                _currentAnimation = _jumpAnimation;
            }


            _currentAnimation.Update(gameTime);
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            _currentAnimation.Draw(spriteBatch, _position);
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
                return new Rectangle((int)_position.X, (int)_position.Y, _runAnimation.FrameWidth, _runAnimation.FrameHeight);
            }
        }

    }
}

