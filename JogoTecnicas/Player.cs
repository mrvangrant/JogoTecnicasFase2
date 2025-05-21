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

        public Player(SpriteAnimation runAnimation, SpriteAnimation jumpAnimation, Vector2 startPosition)
        {
            _runAnimation = runAnimation;
            _jumpAnimation = jumpAnimation;
            _currentAnimation = _runAnimation;
            _position = startPosition;
        }

        public void Update(GameTime gameTime, KeyboardInput input)
        {
            // Troca de animação conforme a seta para cima
            if (input.IsUpPressed())
            {
                if (!_isJumping)
                {
                    _isJumping = true;
                    _jumpAnimation.Reset();
                    _currentAnimation = _jumpAnimation;
                }
            }
            else
            {
                if (_isJumping)
                {
                    _isJumping = false;
                    _runAnimation.Reset();
                    _currentAnimation = _runAnimation;
                }
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