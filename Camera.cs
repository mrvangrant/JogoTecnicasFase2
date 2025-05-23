using Microsoft.Xna.Framework;

namespace JogoTecnicas
{
    public class Camera
    {
        private Vector2 _position;
        private readonly int _screenWidth;
        private readonly int _screenHeight;

        public Camera(int screenWidth, int screenHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _position = Vector2.Zero;
        }

        public Matrix GetViewMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-_position, 0));
        }

        public void Update(Vector2 playerPosition)
        {
            // Centraliza a câmera na posição do jogador
            _position.X = playerPosition.X - _screenWidth / 2;
            _position.Y = playerPosition.Y - _screenHeight / 2;

            // Impede que a câmera vá para posições negativas
            if (_position.X < 0) _position.X = 0;
            if (_position.Y < 0) _position.Y = 0;
        }
    }
}
