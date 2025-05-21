using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogoTecnicas.Graficos
{
    public class SpriteAnimation
    {
        private Texture2D _texture;
        private int _index;
        private int _frameWidth;
        private int _frameHeight;
        private int _totalFrames;
        private float _timePerFrame;
        private float _timer;
        private int _currentFrame;
        

        public bool Loop { get; set; } = true;
        public bool IsPlaying { get; private set; } = true;

        public SpriteAnimation(Texture2D texture,int index, int frameWidth, int frameHeight, int totalFrames, float timePerFrame)
        {
            _texture = texture;
            _index = index;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _totalFrames = totalFrames;
            _timePerFrame = timePerFrame;
            _timer = 0f;
            _currentFrame = 0;
        }

        public void Play()
        {
            IsPlaying = true;
        }

        public void Stop()
        {
            IsPlaying = false;
        }

        public void Reset()
        {
            _currentFrame = 0;
            _timer = 0f;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsPlaying) return;

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer >= _timePerFrame)
            {
                _timer -= _timePerFrame;
                _currentFrame++;
                if (_currentFrame >= _totalFrames)
                {
                    if (Loop)
                        _currentFrame = 0;
                    else
                    {
                        _currentFrame = _totalFrames - 1;
                        Stop();
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Rectangle sourceRect = new Rectangle(_currentFrame * _frameWidth, _index, _frameWidth, _frameHeight);
            spriteBatch.Draw(_texture, position, sourceRect, Color.White);
        }
        public int FrameWidth => _frameWidth;
        public int FrameHeight => _frameHeight;
    }

   
    }