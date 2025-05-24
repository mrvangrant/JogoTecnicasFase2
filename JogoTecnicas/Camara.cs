using Microsoft.Xna.Framework;

namespace JogoTecnicas
{
    public class Camera
    {
        private Vector2 _position;
        private int _screenWidth;
        private int _screenHeight;

        public Camera(int screenWidth, int screenHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _position = Vector2.Zero;
        }

        public void Update(Vector2 playerPosition)
        {
            // A câmera só se move para a direita
            if (playerPosition.X > _position.X + _screenWidth / 2)
            {
                _position.X = playerPosition.X - _screenWidth / 2;
            }

            // Impede que a câmera vá para posições negativas
            if (_position.X < 0) _position.X = 0;
        }

        public Matrix GetViewMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-_position, 0));
        }
    }

}