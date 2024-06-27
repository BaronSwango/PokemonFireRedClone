using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class InputManager
    {
        private KeyboardState currentKeyState, prevKeyState;
        private static InputManager instance;

        public static InputManager Instance
        {
            get
            {
                instance ??= new InputManager();

                return instance;
            }
        }

        public void Update()
        {
            prevKeyState = currentKeyState;
            if (!ScreenManager.Instance.IsTransitioning)
            {
                currentKeyState = Keyboard.GetState();
            }
        }


        public bool KeyReleased(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (currentKeyState.IsKeyUp(key) && prevKeyState.IsKeyDown(key))
                    return true;
            }
            return false;
        }

        public bool KeyPressed(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (currentKeyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key))
                    return true;
            }
            return false;
        }

        public bool KeyDown(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (currentKeyState.IsKeyDown(key))
                    return true;
            }
            return false;
        }

    }
}
