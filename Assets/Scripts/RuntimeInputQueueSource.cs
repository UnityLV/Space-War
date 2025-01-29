using System.Collections.Generic;
using UnityEngine;

public interface IInputQueueSource
{
    Queue<KeyboardInputData> InputQueue { get; }
}

public class RecordedInputQueueSource : IInputQueueSource
{
    public Queue<KeyboardInputData> InputQueue { get; }
    
    public RecordedInputQueueSource()
    {
        InputQueue = new Queue<KeyboardInputData>(InputRecorder.LoadData());
    }
}

public class RuntimeInputQueueSource : ITickable, IInputQueueSource
{
    public Queue<KeyboardInputData> InputQueue { get; } = new Queue<KeyboardInputData>();
    private Vector2 _previousInput;
    
    public void Tick()
    {
        Vector2 currentInput = InputReader.ReadInput();
        if (currentInput != _previousInput)
        {
            _previousInput = currentInput;
            SendInput(currentInput);
        }
    }

    private void SendInput(Vector2 currentInput)
    {
        KeyboardInputData data = new KeyboardInputData
        {
            Time = Tools.StartGameTime(),
            X = currentInput.x,
            Y = currentInput.y
        };
        InputQueue.Enqueue(data);
    }

    private class InputReader
    {
        public static Vector2 ReadInput()
        {
            int horizontal = Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) ? 0 :
                Input.GetKey(KeyCode.A) ? -1 :
                Input.GetKey(KeyCode.D) ? 1 : 0;
            int vertical = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) ? 0 :
                Input.GetKey(KeyCode.W) ? 1 :
                Input.GetKey(KeyCode.S) ? -1 : 0;
            return new Vector2(horizontal, vertical);
        }

        public static Vector3 ReadMousePosition()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}