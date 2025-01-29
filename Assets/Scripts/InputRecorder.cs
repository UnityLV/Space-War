using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InputRecorder : ITickable, IDisposable
{
    private readonly IInputQueueSource _inputQueueSource;
    private static readonly string FilePath = Path.Combine(Application.dataPath, "inputEvents.dat");
    private List<KeyboardInputData> _dataList = new List<KeyboardInputData>();
    private float _lastRecordTime = -1;

    public InputRecorder(IInputQueueSource inputQueueSource)
    {
        _inputQueueSource = inputQueueSource;
        
    }

    public void Tick()
    {
        foreach (var data in _inputQueueSource.InputQueue)
        {
            if (data.Time > _lastRecordTime)
            {
                _lastRecordTime = data.Time;
                _dataList.Add(data);
            }
            else
            {
                break;
            }
        }
    }

    public void Dispose()
    {
        if (_dataList.Count > 0)
        {
            using (StreamWriter writer = new StreamWriter(FilePath, true))
            {
                foreach (var data in _dataList)
                {
                    writer.WriteLine($"{data.Time}:{data.X}:{data.Y}");
                }
            }
        }
    }
    
    public static void ClearData()
    {
        if (File.Exists(FilePath))
        {
            File.Delete(FilePath);
        }
    }

    public static List<KeyboardInputData> LoadData()
    {
        List<KeyboardInputData> dataList = new List<KeyboardInputData>();

        if (File.Exists(FilePath))
        {
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(':');
                    if (parts.Length == 3)
                    {
                        float time = float.Parse(parts[0]);
                        float x = float.Parse(parts[1]);
                        float y = float.Parse(parts[2]);
                        dataList.Add(new KeyboardInputData { Time = time, X = x, Y = y });
                    }
                }
            }
        }

        return dataList;
    }
}