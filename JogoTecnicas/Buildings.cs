using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace JogoTecnicas
{
    public class Buildings
    {
        private Texture2D _backgroundTexture;
        private Texture2D _floorTexture;

        private float _backgroundOffset;
        private float _floorOffset;

        private float _backgroundScrollSpeed;
        private float _floorScrollSpeed;

        private int _screenWidth;
        private int _screenHeight;
        private float _floorY;

        // Fatores de escala
        private float _scaleX;
        private float _scaleY;

        public Buildings(Texture2D backgroundTexture, Texture2D floorTexture, int screenWidth, int screenHeight, float backgroundScrollSpeed = 20f, float floorScrollSpeed = 200f)
        {
            _backgroundTexture = backgroundTexture;
            _floorTexture = floorTexture;
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _backgroundScrollSpeed = backgroundScrollSpeed;
            _floorScrollSpeed = floorScrollSpeed;
            _backgroundOffset = 0f;
            _floorOffset = 0f;
            _floorY = _screenHeight - 60; // Ajuste conforme necessário

            // Calcula os fatores de escala para background e chão
            _scaleX = (float)_screenWidth / _backgroundTexture.Width;
            _scaleY = (float)_screenHeight / _backgroundTexture.Height;
        }

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Atualiza o offset do chão
            _floorOffset += _floorScrollSpeed * delta;
            if (_floorOffset >= _floorTexture.Width * _scaleX)
                _floorOffset -= _floorTexture.Width * _scaleX;

            // Atualiza o offset do background
            _backgroundOffset += _backgroundScrollSpeed * delta;
            if (_backgroundOffset >= _backgroundTexture.Width * _scaleX)
                _backgroundOffset -= _backgroundTexture.Width * _scaleX;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Desenha o background (duas vezes para efeito de loop)
            spriteBatch.Draw(_backgroundTexture,new Vector2(-_backgroundOffset, 0), null, Color.White,0f, Vector2.Zero,new Vector2(_scaleX, _scaleY), SpriteEffects.None,0f);
            spriteBatch.Draw(_backgroundTexture,new Vector2(_backgroundTexture.Width * _scaleX - _backgroundOffset, 0),null, Color.White, 0f,Vector2.Zero, new Vector2(_scaleX, _scaleY),SpriteEffects.None, 0f);

            // Desenha o chão (duas vezes para efeito de loop)
            float floorScaleY = (_floorTexture.Height > 0) ? (_screenHeight - _floorY) / _floorTexture.Height : 1f;
            spriteBatch.Draw(
                _floorTexture,
                new Vector2(-_floorOffset, _floorY),
                null,
                Color.White,
                0f,
                Vector2.Zero,
                new Vector2(_scaleX, floorScaleY),
                SpriteEffects.None,
                0f
            );
            spriteBatch.Draw(
                _floorTexture,
                new Vector2(_floorTexture.Width * _scaleX - _floorOffset, _floorY),
                null,
                Color.White,
                0f,
                Vector2.Zero,
                new Vector2(_scaleX, floorScaleY),
                SpriteEffects.None,
                0f
            );
        }

        public float FloorY => _floorY;

        public Rectangle FloorRectangle
        {
            get
            {
                // Ajusta a altura do chão escalado
                int scaledFloorHeight = (int)(_floorTexture.Height * ((_screenHeight - _floorY) / _floorTexture.Height));
                return new Rectangle(0, (int)_floorY, _screenWidth, scaledFloorHeight);
            }
        }
    }
}
