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
      

        public void Update()
        {
            
            _currentState = Keyboard.GetState();
        }

        public bool IsLeftPressed() => _currentState.IsKeyDown(Keys.Left);
        public bool IsRightPressed() => _currentState.IsKeyDown(Keys.Right);
        public bool IsUpPressed() => _currentState.IsKeyDown(Keys.Up);
        public bool IsDownPressed() => _currentState.IsKeyDown(Keys.Down);

        
    }
}