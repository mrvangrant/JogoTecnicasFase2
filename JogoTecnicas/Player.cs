using JogoTecnicas.Graficos;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace JogoTecnicas
{
    public class Player
    {
        private SpriteAnimation _runAnimation;
        private SpriteAnimation _jumpAnimation;
        private SpriteAnimation _currentAnimation;

        private Vector2 _position;
        private bool _isJumping = false;
        private float _jumpDelayTimer = 0f; // Timer para o atraso do salto
        private float _verticalVelocity = 0f; // Velocidade vertical do jogador

        private const float JumpDelay = 0.27f; // Atraso antes de iniciar a animação de salto

        public Player(SpriteAnimation runAnimation, SpriteAnimation jumpAnimation, Vector2 startPosition)
        {
            _runAnimation = runAnimation;
            _jumpAnimation = jumpAnimation;
            _currentAnimation = _runAnimation;
            _position = startPosition;
        }

        public void Update(GameTime gameTime, KeyboardInput input, Rectangle floorRect)
        {
            const float gravity = 0.6f;
            const float jumpForce = -12.5f;

            // Aplica gravidade sempre
            _verticalVelocity += gravity;
            _position.Y += _verticalVelocity;

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
