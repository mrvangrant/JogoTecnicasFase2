
using JogoTecnicas.Graficos;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace JogoTecnicas.Inimigos
{
    public enum EnemyType
    {
        Static,
        Runner
    }

    public class Enemy
    {
        public SpriteAnimation Animation;
        public Vector2 Position;
        public Rectangle Bounds => new((int)Position.X + _hitboxOffsetX, (int)Position.Y + _hitboxOffsetY, _hitboxWidth, _hitboxHeight);
        private EnemyType _type;
        private float _speed;
        private int _direction = 1; // 1: direita, -1: esquerda
        private int _hitboxWidth;
        private int _hitboxHeight;
        private int _hitboxOffsetX;
        private int _hitboxOffsetY;

        public Enemy(SpriteAnimation animation, Vector2 position, EnemyType type, float speed,int hitboxOffsetX, int hitboxOffsetY, int hitboxWidth, int hitboxHeight)
        {
            Animation = animation;
            Position = position;
            _type = type;
            _speed = speed;
            _hitboxOffsetX = hitboxOffsetX;
            _hitboxOffsetY = hitboxOffsetY;
            _hitboxWidth = hitboxWidth;
            _hitboxHeight = hitboxHeight;
        }

        public void Update(GameTime gameTime, float worldSpeed)
        {
            // Move o inimigo com o mundo
            Position.X -= worldSpeed;

            // Movimento de patrulha apenas para o tipo Patrol
            if (_type == EnemyType.Runner)
            {
                Position.X -=  _speed;
              
            }

            Animation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch, Position, _direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
        }
    }
}