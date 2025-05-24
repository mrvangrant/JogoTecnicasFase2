using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JogoTecnicas.Graficos;

namespace JogoTecnicas.Objetos
{
    public class Buildings
    {
        private Texture2D _backgroundTexture;
        private Texture2D _floorTexture;

        private float _backgroundScrollSpeed;
        private float _floorScrollSpeed;

        private float _floorOffset;
        private float _floorY;

        private Vector2[] backgroundPositions;

  

        private int _screenWidth;
        private int _screenHeight;

        public Buildings(Texture2D backgroundTexture, Texture2D floorTexture, int screenWidth, int screenHeight, float backgroundScrollSpeed = 20f, float floorScrollSpeed = 200f)
        {
            _backgroundTexture = backgroundTexture;
            _floorTexture = floorTexture;
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _backgroundScrollSpeed = backgroundScrollSpeed;
            _floorScrollSpeed = floorScrollSpeed;

            _floorOffset = 0f;
            _floorY = _screenHeight - 60;

            // Inicia 3 posições lado a lado
            backgroundPositions = new Vector2[3];
            for (int i = 0; i < backgroundPositions.Length; i++)
            {
                backgroundPositions[i] = new Vector2(i * _backgroundTexture.Width, 0);
            }
        }

        public void Update(GameTime gameTime, bool isPlayerMovingRight)
        {
            if (!isPlayerMovingRight)
                return;

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Atualiza posição do background (array de posições)
            float backgroundMovement = _backgroundScrollSpeed * delta;
            for (int i = 0; i < backgroundPositions.Length; i++)
            {
                backgroundPositions[i].X -= backgroundMovement;

                // Reposiciona os blocos que saíram da tela à direita
                if (backgroundPositions[i].X < -_backgroundTexture.Width)
                {
                    float maxX = backgroundPositions.Max(p => p.X);
                    backgroundPositions[i].X = maxX + _backgroundTexture.Width;
                }
            }

            // Atualiza offset do chão para scroll
            _floorOffset += _floorScrollSpeed * delta;
            if (_floorOffset >= _floorTexture.Width)
                _floorOffset -= _floorTexture.Width;
        }



        public void DrawBackground(SpriteBatch spriteBatch)
        {
            foreach (var pos in backgroundPositions)
            {
                spriteBatch.Draw(_backgroundTexture, pos, Color.White);
            }
        }

        public void DrawFloor(SpriteBatch spriteBatch)
        {
            // Desenha o chão 3 vezes para loop infinito
            spriteBatch.Draw(_floorTexture, new Vector2(-_floorOffset, _floorY), Color.White);
            spriteBatch.Draw(_floorTexture, new Vector2(_floorTexture.Width - _floorOffset, _floorY), Color.White);
            spriteBatch.Draw(_floorTexture, new Vector2(2 * _floorTexture.Width - _floorOffset, _floorY), Color.White);
        }


        //public void Draw(SpriteBatch spriteBatch)
        //{
        //     //Desenha o background
        //    foreach (var pos in backgroundPositions)
        //    {
        //        spriteBatch.Draw(_backgroundTexture, pos, Color.White);
        //    }

        //     //Desenha o chão
        //    spriteBatch.Draw(_floorTexture, new Vector2(-_floorOffset, _floorY), Color.White);
        //    spriteBatch.Draw(_floorTexture, new Vector2(_floorTexture.Width - _floorOffset, _floorY), Color.White);
        //    spriteBatch.Draw(_floorTexture, new Vector2(2 * _floorTexture.Width - _floorOffset, _floorY), Color.White);
        //}

        public float FloorY => _floorY;

        public Rectangle FloorRectangle => new Rectangle(0, (int)_floorY, _screenWidth, _floorTexture.Height);
    }
}