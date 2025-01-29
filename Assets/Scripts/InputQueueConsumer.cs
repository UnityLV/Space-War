using UnityEngine;

public class InputQueueConsumer : IKeyboardInputSource, IMouseInputSource, ITickable
{
    private readonly IInputQueue _inputQueue;

    private KeyboardInputData _inputDataKeyboard;
    private MouseInputData _inputDataMouse;

    public InputQueueConsumer(IInputQueue inputQueue)
    {
        _inputQueue = inputQueue;
    }

    public void Tick()
    {
        Consume();
    }

    private void Consume()
    {
        while (_inputQueue.KeyboardInputQueue.Count > 0)
        {
            KeyboardInputData data = _inputQueue.KeyboardInputQueue.Peek();
            if (data.Time < TimeInGame.TimeFromStart())
            {
                _inputDataKeyboard = _inputQueue.KeyboardInputQueue.Dequeue();
            }
            else
            {
                break;
            }
        }

        while (_inputQueue.MouseInputQueue.Count > 0)
        {
            MouseInputData data = _inputQueue.MouseInputQueue.Peek();
            if (data.Time < TimeInGame.TimeFromStart())
            {
                _inputDataMouse = _inputQueue.MouseInputQueue.Dequeue();
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