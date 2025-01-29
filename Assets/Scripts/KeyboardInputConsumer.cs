using UnityEngine;

public class KeyboardInputConsumer : IKeyboardInputSource, IMouseInputSource, ITickable
{
    private readonly IInputQueueSource _inputQueueSource;

    private KeyboardInputData _inputDataKeyboard;
    private MouseInputData _inputDataMouse;

    public KeyboardInputConsumer(IInputQueueSource inputQueueSource)
    {
        _inputQueueSource = inputQueueSource;
    }

    public void Tick()
    {
        Consume();
    }

    private void Consume()
    {
        while (_inputQueueSource.KeyboardInputQueue.Count > 0)
        {
            KeyboardInputData data = _inputQueueSource.KeyboardInputQueue.Peek();
            if (data.Time < TimeInGame.TimeFromStart())
            {
                _inputDataKeyboard = _inputQueueSource.KeyboardInputQueue.Dequeue();
            }
            else
            {
                break;
            }
        }

        while (_inputQueueSource.MouseInputQueue.Count > 0)
        {
            MouseInputData data = _inputQueueSource.MouseInputQueue.Peek();
            if (data.Time < TimeInGame.TimeFromStart())
            {
                _inputDataMouse = _inputQueueSource.MouseInputQueue.Dequeue();
            }
            else
            {
                break;
            }
        }
    }

    KeyboardInputData IKeyboardInputSource.GetInput()
    {
        return _inputDataKeyboard;
    }

    MouseInputData IMouseInputSource.GetInput()
    {
        return _inputDataMouse;
    }
}