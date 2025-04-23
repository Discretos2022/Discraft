using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscCraft
{
    class MouseInput
    {
        private Main main;

        private static MouseState mouseState;
        private static MouseState oldmouseState;
        private static Rectangle MousePos;

        public static Point WindowPosition
        {
            get { return mouseState.Position; }
        }


        private static float X;
        private static float Y;

        public MouseInput(Main main)
        {
            this.main = main;
        }


        public static void Update()   ///Screen screen
        {
            oldmouseState = mouseState;
            mouseState = Mouse.GetState();

            //X = GetScreenPosition(screen).X / 4;
            //Y = GetScreenPosition(screen).Y / 4;

            MousePos = new Rectangle((int)X / 4, (int)Y, 2, 2);

        }


        //public static Vector2 GetScreenPosition() ///Screen screen
        //{
        //    Rectangle screenDestinationRectangle = screen.CalculateDestinationRectangle();

        //    Point windowPosition = WindowPosition;

        //    float sx = windowPosition.X - screenDestinationRectangle.X;
        //    float sy = windowPosition.Y - screenDestinationRectangle.Y;

        //    sx /= (float)screenDestinationRectangle.Width;
        //    sy /= (float)screenDestinationRectangle.Height;

        //    sx *= (float)screen.Width;
        //    sy *= (float)screen.Height;

        //    return new Vector2(sx, sy);
        //}


        //public static Rectangle GetRectangle(Screen screen)
        //{
        //    return new Rectangle((int)GetScreenPosition(screen).X, (int)GetScreenPosition(screen).Y, 2, 2);
        //}

        public static Vector2 GetPos()
        {
            return new Vector2(getMouseState().X, getMouseState().Y);
        }


        public static Vector2 GetOldPos()
        {
            return new Vector2(getOldMouseState().X, getOldMouseState().Y);
        }

        public static MouseState getMouseState()
        {
            return mouseState;
        }

        //public static Vector2 GetLevelPos(bool isFullScreen, Camera camera)
        //{
        //    if (isFullScreen)
        //        return new Vector2(X + camera.Position.X - (1920 / 4) / 2, Y + camera.Position.Y - (1080 / 4) / 2);

        //    return new Vector2(X + camera.Position.X - (1920 / 8), Y + camera.Position.Y - (1080 / 8));
        //}

        public static MouseState getOldMouseState()
        {
            return oldmouseState;
        }

    }


    class KeyInput
    {

        private static KeyboardState keyState;
        private static KeyboardState oldKeyState;

        public KeyInput()
        {

        }


        public static void Update()
        {
            oldKeyState = keyState;
            keyState = Keyboard.GetState();

        }

        public static KeyboardState getKeyState()
        {
            return keyState;
        }

        public static KeyboardState getOldKeyState()
        {
            return oldKeyState;
        }

    }


    public class GamePadInput
    {

        private static GamePadState padState;
        private static GamePadState oldPadState;

        public static void Update(PlayerIndex index)
        {
            oldPadState = padState;
            padState = GamePad.GetState(index);
        }


        public static GamePadState GetPadState()
        {
            return padState;
        }

        public static GamePadState GetOldPadState()
        {
            return oldPadState;
        }


    }


}
