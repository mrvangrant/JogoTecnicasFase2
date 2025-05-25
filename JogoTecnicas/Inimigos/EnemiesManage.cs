using JogoTecnicas.Graficos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;




namespace JogoTecnicas.Inimigos
{
    public class EnemiesManage
    {
        private List<Enemy> _enemies = new();
        private Random _random = new();
        private float _spawnTimer;
        private float _spawnInterval = 2f;


        private SpriteAnimation _staticAnim;
        private SpriteAnimation _patrolAnim;
        private SpriteAnimation _deathAnimStatic;
        private SpriteAnimation _deathAnimRunner;


        //metodo para saber que inimigos estão vivos
        public IEnumerable<Enemy> GetAliveEnemies()
        {
            foreach (var enemy in _enemies)
                if (!enemy.IsDead && enemy.State != EnemyState.Dying)
                    yield return enemy;
        }



        public EnemiesManage(SpriteAnimation staticAnim, SpriteAnimation patrolAnim,
                             SpriteAnimation deathAnimStatic, SpriteAnimation deathAnimRunner)
        {
            _staticAnim = staticAnim;
            _patrolAnim = patrolAnim;
            _deathAnimStatic = deathAnimStatic;
            _deathAnimRunner = deathAnimRunner;
            
        }

        public void Update(GameTime gameTime, float worldSpeed, bool isPlayerMovingRight, float playerX, int currentscore)
        {
            float scrollSpeed = isPlayerMovingRight ? worldSpeed : 0f;
            _spawnInterval = Math.Max(0.8f, 2f - currentscore * 0.01f);

            _spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_spawnTimer >= _spawnInterval)
            {
                _spawnTimer = 0;
                float y = 350;
                float x = playerX + 1000; //spawn de inimigos 1000 a direita do player

                if (_random.Next(2) == 0)
                {
                    var anim = CloneAnimation(_staticAnim);
                    var deathAnim = CloneAnimation(_deathAnimStatic);
                    deathAnim.Loop = false; // Animação de morte não deve ser repetida
                    _enemies.Add(new Enemy(anim, new Vector2(x, y), EnemyType.Static, 0f, 20, 15, 26, 22, deathAnim));
                }
                else
                {
                    var anim = CloneAnimation(_patrolAnim);
                    var deathAnim = CloneAnimation(_deathAnimRunner);
                    deathAnim.Loop = false; // Animação de morte não deve ser repetida
                    _enemies.Add(new Enemy(anim, new Vector2(x, y), EnemyType.Runner, 4f, 10, 20, 44, 40, deathAnim));
                }
            }

            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                _enemies[i].Update(gameTime, scrollSpeed);

                // Remove inimigos completamente mortos
                if (_enemies[i].IsDead)
                    _enemies.RemoveAt(i);
            }
        }
        //função que verifica se o jogador esta a saltar em cima do inimigo
        public bool IsJumpingOnTop(Rectangle player, Rectangle enemy)
        {
            // Verifica se a parte inferior do jogador está acima da parte superior do inimigo e descendo
            return player.Bottom >= enemy.Top - 5 &&
                   player.Bottom <= enemy.Top + 10 &&
                   player.Center.X >= enemy.Left &&
                   player.Center.X <= enemy.Right;
        }

        //remove o inimigo runner quando o player faz um slide nele
        public void RemoveHitEnemies(Rectangle playerBounds, bool isSliding, bool isJumping)
        {
            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                var enemy = _enemies[i];

                if (enemy.Bounds.Intersects(playerBounds))
                {
                    if (enemy.Type == EnemyType.Runner && isSliding)
                    {
                        enemy.Die();
                    }
                    else if (enemy.Type == EnemyType.Static && isJumping && IsJumpingOnTop(playerBounds, enemy.Bounds))
                    {
                        enemy.Die();
                    }
                }
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

        // Clona a animação para evitar bugs
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
        public void Reset()
        {
            _enemies.Clear();
            _spawnTimer = 0f;
            _spawnInterval = 2f;
        }
    }
}
