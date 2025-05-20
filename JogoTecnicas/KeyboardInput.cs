using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogoTecnicas
{
    public class KeyboardInput
    {
        private KeyboardState _currentState;
        private KeyboardState _previousState;

        public void Update()
        {
            _previousState = _currentState;
            _currentState = Keyboard.GetState();
        }

        public bool IsLeftPressed() => _currentState.IsKeyDown(Keys.Left);
        public bool IsRightPressed() => _currentState.IsKeyDown(Keys.Right);
        public bool IsUpPressed() => _currentState.IsKeyDown(Keys.Up);
        public bool IsDownPressed() => _currentState.IsKeyDown(Keys.Down);

        public bool IsLeftJustPressed() => _currentState.IsKeyDown(Keys.Left) && !_previousState.IsKeyDown(Keys.Left);
        public bool IsRightJustPressed() => _currentState.IsKeyDown(Keys.Right) && !_previousState.IsKeyDown(Keys.Right);
        public bool IsUpJustPressed() => _currentState.IsKeyDown(Keys.Up) && !_previousState.IsKeyDown(Keys.Up);
        public bool IsDownJustPressed() => _currentState.IsKeyDown(Keys.Down) && !_previousState.IsKeyDown(Keys.Down);

        public bool IsLeftJustReleased() => !_currentState.IsKeyDown(Keys.Left) && _previousState.IsKeyDown(Keys.Left);
        public bool IsRightJustReleased() => !_currentState.IsKeyDown(Keys.Right) && _previousState.IsKeyDown(Keys.Right);
        public bool IsUpJustReleased() => !_currentState.IsKeyDown(Keys.Up) && _previousState.IsKeyDown(Keys.Up);
        public bool IsDownJustReleased() => !_currentState.IsKeyDown(Keys.Down) && _previousState.IsKeyDown(Keys.Down);
    }
}