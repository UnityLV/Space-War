using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class InputRecorder : ITickable, IDisposable
{
    private readonly IInputQueue _inputQueue;

    private static readonly string FilePath = Path.Combine(Application.dataPath, "inputEvents.dat");
    private List<KeyboardInputData> _dataList = new List<KeyboardInputData>();
    private List<MouseInputData> _mouseDataList = new List<MouseInputData>();
    private float _lastRecordTime = -1;

    public InputRecorder(IInputQueue inputQueue)
    {
        _inputQueue = inputQueue;
    }

    public void Tick()
    {
        foreach (var data in _inputQueue.KeyboardInputQueue)
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

        foreach (var data in _inputQueue.MouseInputQueue)
        {
            if (data.Time > _lastRecordTime)
            {
                _lastRecordTime = data.Time;
                _mouseDataList.Add(data);
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
                    if (_mouseDataList.Any() && data.Time > _mouseDataList[0].Time)
                    {
                        MouseInputData mouseData = _mouseDataList[0];
                        _mouseDataList.RemoveAt(0);
                        writer.WriteLine($"{mouseData.Time}:{mouseData.X}:{mouseData.Y}:{mouseData.LeftButton}");
                    }

                    writer.WriteLine($"{data.Time}:{data.X}:{data.Y}");
                }

                foreach (var data in _mouseDataList)
                {
                    writer.WriteLine($"{data.Time}:{data.X}:{data.Y}:{data.LeftButton}");
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

    public static List<MouseInputData> LoadMouseData()
    {
        List<MouseInputData> dataList = new List<MouseInputData>();

        if (File.Exists(FilePath))
        {
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(':');
                    if (parts.Length == 4)
                    {
                        float time = float.Parse(parts[0]);
                        float x = float.Parse(parts[1]);
                        float y = float.Parse(parts[2]);
                        bool leftButton = bool.Parse(parts[3]);
                        dataList.Add(new MouseInputData { Time = time, X = x, Y = y, LeftButton = leftButton });
                    }
                }
            }
        }

        return dataList;
    }
}