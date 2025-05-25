
using JogoTecnicas.Graficos;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace JogoTecnicas.Inimigos
{



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

    public class Enemy
    {
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

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_state == EnemyState.Dead) return;

            if (_state == EnemyState.Dying)
                _deathAnimation.Draw(spriteBatch, Position);
            else
                Animation.Draw(spriteBatch, Position, _direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
        }


    }
}