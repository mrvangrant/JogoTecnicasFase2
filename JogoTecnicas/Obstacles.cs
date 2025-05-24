using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace JogoTecnicas
{
    public class Obstacle
    {
        public Texture2D Texture;
        public Vector2 Position;
        public Rectangle Bounds => new((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

        public Obstacle(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }

        public void Update(float speed)
        {
            Position.X -= speed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }

    public class Obstacles
    {
        private readonly List<Obstacle> _obstacles = new();
        private readonly Texture2D _obstacleTexture;
        private readonly Random _random = new();
        private float _spawnTimer;
        private readonly float _spawnInterval = 2f; // segundos

        public Obstacles(Texture2D texture)
        {
            _obstacleTexture = texture;
        }

        private bool IsOverlapping(Rectangle newBounds)
        {
            foreach (var obs in _obstacles)
            {
                if (obs.Bounds.Intersects(newBounds))
                    return true;
            }
            return false;
        }

        public void Update(GameTime gameTime, float speed, bool isPlayerMovingRight)
        {
            _spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_spawnTimer >= _spawnInterval)
            {
                _spawnTimer = 0;
                float y = 350; // Ajuste conforme o chão do seu jogo
                var newPosition = new Vector2(1000, y);
                var newBounds = new Rectangle((int)newPosition.X, (int)newPosition.Y, _obstacleTexture.Width, _obstacleTexture.Height);

                if (!IsOverlapping(newBounds))
                {
                    _obstacles.Add(new Obstacle(_obstacleTexture, newPosition));
                }
                
            }

            if (isPlayerMovingRight)
            {
                for (int i = _obstacles.Count - 1; i >= 0; i--)
                {
                    _obstacles[i].Update(speed);
                    if (_obstacles[i].Position.X < -_obstacleTexture.Width)
                        _obstacles.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var obs in _obstacles)
                obs.Draw(spriteBatch);
        }

        public bool CheckCollision(Rectangle playerBounds)
        {
            foreach (var obs in _obstacles)
            {
                if (obs.Bounds.Intersects(playerBounds))
                    return true;
            }
            return false;
        }

        public IEnumerable<Rectangle> GetBoundingBoxes()
        {
            foreach (var obs in _obstacles)
                yield return obs.Bounds;
        }
    }
}
