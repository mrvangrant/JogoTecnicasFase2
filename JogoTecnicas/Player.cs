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

        private const float JumpDelay = 0.27f; // Atraso de 200ms antes de iniciar a animação de salto

        public Player(SpriteAnimation runAnimation, SpriteAnimation jumpAnimation, Vector2 startPosition)
        {
            _runAnimation = runAnimation;
            _jumpAnimation = jumpAnimation;
            _currentAnimation = _runAnimation;
            _position = startPosition;
        }

        public void Update(GameTime gameTime, KeyboardInput input)
        {
            // Variáveis de física
            const float gravity = 0.6f; // Gravidade que puxa o jogador para baixo
            const float jumpForce = -12.5f; // Força inicial do salto (ajustada para ser mais suave)
            const float groundLevel = 346; // Posição Y do chão

            bool isOnGround = _position.Y >= groundLevel;

            // Inicia o salto apenas se o jogador estiver no chão
            if (input.IsUpPressed() && !_isJumping && isOnGround)
            {
                _isJumping = true;
                _verticalVelocity = jumpForce; // Aplica a força inicial do salto

                // Inicia a animação de salto imediatamente
                _jumpAnimation.Loop = false;
                _jumpAnimation.Reset();
                _jumpAnimation.Play();
                _currentAnimation = _jumpAnimation;
            }

            // Aplica gravidade enquanto o jogador está no ar
            if (!isOnGround || _isJumping)
            {
                _verticalVelocity += gravity; // Aumenta a velocidade vertical devido à gravidade
                _position.Y += _verticalVelocity; // Atualiza a posição do jogador com base na velocidade
            }

            // Verifica se o jogador atingiu o chão
            if (_position.Y >= groundLevel)
            {
                _position.Y = groundLevel; // Mantém o jogador no chão
                _verticalVelocity = 0f; // Reseta a velocidade vertical

                // Só troca para a animação de corrida se a animação de salto tiver terminado
                if (_isJumping && !_jumpAnimation.IsPlaying)
                {
                    _isJumping = false; // Finaliza o estado de salto
                    _runAnimation.Loop = true;
                    _runAnimation.Play();
                    _currentAnimation = _runAnimation;
                }
            }

            // Atualiza a animação atual
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
    }
}
