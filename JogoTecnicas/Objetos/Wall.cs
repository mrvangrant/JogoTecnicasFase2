using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JogoTecnicas.Objetos
{
    public class Wall
    {
        private Vector2 _position;
        private int _width;
        private int _height;
        private float _speed;
        private Texture2D _texture; // Textura de 1x1 pixel para desenhar o retângulo

        public Rectangle BoundingBox => new Rectangle((int)_position.X, (int)_position.Y, _width, _height);

        // Propriedade pública para acessar e modificar a velocidade
        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        public Wall(GraphicsDevice graphicsDevice, int screenHeight, float speed)
        {
            _width = 50; // Largura inicial da parede
            _height = screenHeight; // Altura da parede igual à altura da tela
            _position = new Vector2(0, 0); // Sempre começa na posição X = 0
            _speed = speed; // Velocidade de movimento da parede

            // Cria uma textura de 1x1 pixel para desenhar o retângulo
            _texture = new Texture2D(graphicsDevice, 1, 1);
            _texture.SetData(new[] { Color.Black });
        }

        public void Update(GameTime gameTime)
        {
            // Move a parede para a direita
            _position.X += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, BoundingBox, Color.Orange);
        }

        public void Reset()
        {
            // Reseta a posição da parede para o início
            _position.X = 0;
            _speed = 20;
        }
    }
}
