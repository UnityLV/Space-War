using UnityEngine;

public class KeyboardInputConsumer : IKeyboardInputSource , ITickable
{
    private readonly IInputQueueSource _inputQueueSource;
    private KeyboardInputData _inputData;
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
        while (_inputQueueSource.InputQueue.Count > 0)
        {
            KeyboardInputData data = _inputQueueSource.InputQueue.Peek();
            if (data.Time < Tools.StartGameTime())
            {
                _inputData = _inputQueueSource.InputQueue.Dequeue();
                Debug.Log(_inputData);
            }
            else
            {
                break;
            }
        }
    }

    public KeyboardInputData GetInput()
    {
        return _inputData;
    }
}