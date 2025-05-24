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

        public int CurrentScore => _score; // Propriedade para acessar o score atual

        public Score(SpriteFont font)
        {
            _font = font;
            _position = new Vector2(20, 20); // Posição fixa no canto superior esquerdo
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

        public void Draw(SpriteBatch spriteBatch, float scale = 1f)
        {
            spriteBatch.DrawString(_font, $"Score: {_score}", _position, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public void Reset()
        {
            _score = 0;
            _timer = 0f;
        }
    }
}
