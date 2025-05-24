using JogoTecnicas.Graficos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using JogoTecnicas.Inimigos;


namespace JogoTecnicas.Inimigos
{
    public class EnemiesManage
    {
        private List<Enemy> _enemies = new();
        private Random _random = new();
        private float _spawnTimer;
        private float _spawnInterval = 2f;
        private float _playerPosition;

        private SpriteAnimation _staticAnim;
        private SpriteAnimation _patrolAnim;

        

        public EnemiesManage(SpriteAnimation staticAnim, SpriteAnimation patrolAnim,float PlayerPosition)
        {
            _staticAnim = staticAnim;
            _patrolAnim = patrolAnim;
            _playerPosition = PlayerPosition;
        }

        public void Update(GameTime gameTime, float worldSpeed, bool isPlayerMovingRight)
        {
            // O scroll do mundo só acontece se o player estiver a andar
            float scrollSpeed = isPlayerMovingRight ? worldSpeed : 0f;

            // Spawning de inimigos
            _spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_spawnTimer >= _spawnInterval)
            {
                _spawnTimer = 0;
                float y = 350; 
                float x = _playerPosition+100;

                // Sorteia o tipo de inimigo
                if (_random.Next(2) == 0)
                {
                    // Inimigo parado
                    var anim = CloneAnimation(_staticAnim);
                    _enemies.Add(new Enemy(anim, new Vector2(x, y), EnemyType.Static, 0f, 20, 15, 26, 22));
                }
                else
                {
                    // Inimigo corredor
                    var anim = CloneAnimation(_patrolAnim);
                    float patrolMinX = x - 60;
                    float patrolMaxX = x + 60;
                    _enemies.Add(new Enemy(anim, new Vector2(x, y), EnemyType.Runner, 4f, 10, 20, 44, 40));
                }
            }

            // Atualiza todos os inimigos SEMPRE (animação e patrulha)
            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                _enemies[i].Update(gameTime, scrollSpeed);

                // Remove se saiu do ecrã
                if (_enemies[i].Position.X < -_enemies[i].Animation.FrameWidth)
                    _enemies.RemoveAt(i);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var enemy in _enemies)
                enemy.Draw(spriteBatch);
        }

        public bool CheckCollision(Rectangle playerBounds)
        {
            foreach (var enemy in _enemies)
            {
                if (enemy.Bounds.Intersects(playerBounds))
                    return true;
            }
            return false;
        }

        public IEnumerable<Rectangle> GetBoundingBoxes()
        {
            foreach (var enemy in _enemies)
                yield return enemy.Bounds;
        }

        // Clona a animação para evitar bugs de estado compartilhado
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
    }
}
