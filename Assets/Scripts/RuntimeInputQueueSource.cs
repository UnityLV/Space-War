using System;
using System.Collections.Generic;
using UnityEngine;

public interface IInputQueueSource
{
    Queue<KeyboardInputData> KeyboardInputQueue { get; }
    Queue<MouseInputData> MouseInputQueue { get; }
}

public class RecordedInputQueueSource : IInputQueueSource
{
    public Queue<KeyboardInputData> KeyboardInputQueue { get; }
    public Queue<MouseInputData> MouseInputQueue { get; }

    public RecordedInputQueueSource()
    {
        KeyboardInputQueue = new Queue<KeyboardInputData>(InputRecorder.LoadData());
        MouseInputQueue = new Queue<MouseInputData>(InputRecorder.LoadMouseData());
        
    }
}

public class RuntimeInputQueueSource : ITickable, IInputQueueSource
{
    public Queue<KeyboardInputData> KeyboardInputQueue { get; } = new Queue<KeyboardInputData>();
    public Queue<MouseInputData> MouseInputQueue { get; } = new Queue<MouseInputData>();
    private Vector2 _previousInput;
     
    private Vector2 _previousMousePosition;
    private bool _previousMouseLeftButton;
    
    public void Tick()
    {
        Vector2 currentInput = InputReader.ReadInput();
        if (currentInput != _previousInput)
        {
            _previousInput = currentInput;
            SendInput(currentInput);
        }
        
        Vector2 currentMousePosition = InputReader.ReadMousePosition();
        bool currentMouseLeftButton = Input.GetMouseButton(0);
        
        if (currentMousePosition != _previousMousePosition || currentMouseLeftButton != _previousMouseLeftButton)
        {
            _previousMousePosition = currentMousePosition;
            _previousMouseLeftButton = currentMouseLeftButton;
            SendMouseInput(currentMousePosition, currentMouseLeftButton);
        }


    }

    private void SendMouseInput(Vector2 currentMousePosition, bool currentMouseLeftButton)
    {
        MouseInputData data = new MouseInputData
        {
            Time = TimeInGame.TimeFromStart(),
            X = currentMousePosition.x,
            Y = currentMousePosition.y,
            LeftButton = currentMouseLeftButton
        };
        MouseInputQueue.Enqueue(data);
    }

    private void SendInput(Vector2 currentInput)
    {
        KeyboardInputData data = new KeyboardInputData
        {
            Time = TimeInGame.TimeFromStart(),
            X = currentInput.x,
            Y = currentInput.y
        };
        KeyboardInputQueue.Enqueue(data);
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

        public static Vector2 ReadMousePosition()
        {
            return new Vector2(MathF.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,2),
                MathF.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y,2));
        }
    }
}