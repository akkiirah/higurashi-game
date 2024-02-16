using Higurashi_When_They_Cry_Hashiru.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Higurashi_When_They_Cry_Hashiru.System;

public class InputManager
{
    private Keiichi _keiichi;
    private KeyboardState _previousKeyboardState;

    public InputManager(Keiichi keiichi)
    {
        _keiichi = keiichi;
    }

    public void ProcessControls(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        
        bool isJumpKeyPressed = keyboardState.IsKeyDown(Keys.Up) || 
                                keyboardState.IsKeyDown(Keys.Space) || 
                                keyboardState.IsKeyDown(Keys.W);
        bool wasJumpKeyPressed = _previousKeyboardState.IsKeyDown(Keys.Up) || 
                                 _previousKeyboardState.IsKeyDown(Keys.Space) || 
                                 _previousKeyboardState.IsKeyDown(Keys.W);

        if (!wasJumpKeyPressed && isJumpKeyPressed)
        {
            if (_keiichi.State != PlayerState.Jumping)
                _keiichi.BeginJump();
        }
        else if (_keiichi.State == PlayerState.Jumping && !isJumpKeyPressed)
            _keiichi.CancelJump();
        else if (keyboardState.IsKeyDown(Keys.Down))
        {
            if (_keiichi.State == PlayerState.Jumping || _keiichi.State == PlayerState.Falling)
                _keiichi.Drop();

            else
                _keiichi.Duck();
        }
        else if (_keiichi.State == PlayerState.Ducking && !keyboardState.IsKeyDown(Keys.Down))
            _keiichi.GetUp();

        _previousKeyboardState = keyboardState;
    }
}