
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
        private List<Obstacle> _obstacles = new();
        private Texture2D _obstacleTexture;
        private Random _random = new();
        private float _spawnTimer;
        private float _spawnInterval = 2f; // segundos

        public Obstacles(Texture2D texture)
        {
            _obstacleTexture = texture;
        }

        public void Update(GameTime gameTime, float speed)
        {
            _spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_spawnTimer >= _spawnInterval)
            {
                _spawnTimer = 0;
                float y = 350; // Ajuste conforme o chão do seu jogo
                _obstacles.Add(new Obstacle(_obstacleTexture, new Vector2(800, y)));
            }

            for (int i = _obstacles.Count - 1; i >= 0; i--)
            {
                _obstacles[i].Update(speed);
                if (_obstacles[i].Position.X < -_obstacleTexture.Width)
                    _obstacles.RemoveAt(i);
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
    }
}