
using JogoTecnicas.Graficos;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace JogoTecnicas
{
    public enum EnemyType
    {
        Static,
        Patrol
    }

    public class Enemy
    {
        public SpriteAnimation Animation;
        public Vector2 Position;
        public Rectangle Bounds => new((int)Position.X, (int)Position.Y, Animation.FrameWidth, Animation.FrameHeight);
        private EnemyType _type;
        private float _patrolMinX, _patrolMaxX;
        private float _speed;
        private int _direction = 1; // 1: direita, -1: esquerda

        public Enemy(SpriteAnimation animation, Vector2 position, EnemyType type, float patrolMinX = 0, float patrolMaxX = 0, float speed = 1.5f)
        {
            Animation = animation;
            Position = position;
            _type = type;
            _patrolMinX = patrolMinX;
            _patrolMaxX = patrolMaxX;
            _speed = speed;
        }

        public void Update(GameTime gameTime, float worldSpeed)
        {
            // Move o inimigo com o mundo
            Position.X -= worldSpeed;

            // Movimento de patrulha apenas para o tipo Patrol
            if (_type == EnemyType.Patrol)
            {
                Position.X += _direction * _speed;
                if (Position.X < _patrolMinX)
                {
                    Position.X = _patrolMinX;
                    _direction = 1;
                }
                else if (Position.X > _patrolMaxX)
                {
                    Position.X = _patrolMaxX;
                    _direction = -1;
                }
            }

            Animation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch, Position, _direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
        }
    }
}