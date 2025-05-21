using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Atualiza o offset do chão
            _floorOffset += _floorScrollSpeed * delta;
            if (_floorOffset >= _floorTexture.Width)
                _floorOffset -= _floorTexture.Width;

            // Atualiza o offset do background
            _backgroundOffset += _backgroundScrollSpeed * delta;
            if (_backgroundOffset >= _backgroundTexture.Width)
                _backgroundOffset -= _backgroundTexture.Width;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Desenha o background (duas vezes para efeito de loop)
            spriteBatch.Draw(_backgroundTexture, new Vector2(-_backgroundOffset, 0), Color.White);
            spriteBatch.Draw(_backgroundTexture, new Vector2(_backgroundTexture.Width - _backgroundOffset, 0), Color.White);

            // Desenha o chão (duas vezes para efeito de loop)
            spriteBatch.Draw(_floorTexture, new Vector2(-_floorOffset, _floorY), Color.White);
            spriteBatch.Draw(_floorTexture, new Vector2(_floorTexture.Width - _floorOffset, _floorY), Color.White);
        }

        public float FloorY => _floorY;
    }
}