using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JogoTecnicas.Objetos
{
    public class Wall
    {
        private Vector2 _position;
        private int _width;
        private int _height;
        private float _speed;
        private Texture2D _texture; // Textura da parede
        private Texture2D _hitboxTexture; // Textura para desenhar a hitbox

        // Propriedade ajustável da hitbox
        public Rectangle BoundingBox
        {
            get
            {
                int offsetX = 0; // Reduz a hitbox horizontalmente (margem esquerda e direita)
                int offsetY = -20; // Reduz a hitbox verticalmente (margem superior e inferior)
                int reducedWidth = _width - 385; // Largura reduzida
                int reducedHeight = _height; // Altura reduzida

                return new Rectangle(
                    (int)_position.X + offsetX, // Aplica o deslocamento horizontal
                    (int)_position.Y + offsetY, // Aplica o deslocamento vertical
                    reducedWidth, // Define a largura reduzida
                    reducedHeight // Define a altura reduzida
                );
            }
        }

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        public Wall(GraphicsDevice graphicsDevice, ContentManager content, int screenHeight, float speed)
        {
            _width = 400; // Aumente a largura da parede
            _height = screenHeight; // Altura da parede igual à altura da tela
            _position = new Vector2(0, -38); // Sempre começa na posição X = 0
            _speed = speed; // Velocidade de movimento da parede

            // Carrega a textura da parede a partir do arquivo wall1long.png
            _texture = content.Load<Texture2D>("wall1long");

            //// Cria uma textura de 1x1 pixel para desenhar a hitbox
            //_hitboxTexture = new Texture2D(graphicsDevice, 1, 1);
            //_hitboxTexture.SetData(new[] { Color.Red }); // Define a cor da hitbox como vermelha
        }

        public void Update(GameTime gameTime)
        {
            // Move a parede para a direita
            _position.X += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Desenha a textura da parede com sua resolução original
            spriteBatch.Draw(_texture, _position, Color.White);

            //// Desenha a hitbox (borda do retângulo)
            //DrawRectangle(spriteBatch, BoundingBox, Color.Red);
        }


        //private void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        //{
        //    // Desenha as bordas do retângulo
        //    spriteBatch.Draw(_hitboxTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, 1), color); // Topo
        //    spriteBatch.Draw(_hitboxTexture, new Rectangle(rectangle.X, rectangle.Y, 1, rectangle.Height), color); // Esquerda
        //    spriteBatch.Draw(_hitboxTexture, new Rectangle(rectangle.X + rectangle.Width - 1, rectangle.Y, 1, rectangle.Height), color); // Direita
        //    spriteBatch.Draw(_hitboxTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - 1, rectangle.Width, 1), color); // Base
        //}

        public void Reset()
        {
            // Reseta a posição da parede para o início
            _position.X = 0;
            _speed = 20;
        }
    }
}
