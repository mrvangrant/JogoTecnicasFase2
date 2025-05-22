using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JogoTecnicas
{
    public class Score
    {
        private SpriteFont _font;
        private Vector2 _position;
        private int _score;
        private float _timer;

        public Score(SpriteFont font, Vector2 position)
        {
            _font = font;
            _position = position;
            _score = 0;
            _timer = 0f;
        }

        public void Update(GameTime gameTime, bool isGameOver)
        {
            if (!isGameOver)
            {
                _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_timer >= 0.1f)
                {
                    _score += 1;
                    _timer -= 0.1f;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, $"Score: {_score}", _position, Color.White);
        }

        public void Reset()
        {
            _score = 0;
            _timer = 0f;
        }
    }
}
