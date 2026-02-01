using System.Collections.Generic;
using UnityEngine;

public class CandleMicrophone : MonoBehaviour
{
    private string _deviceName;
    [SerializeField] private int _sampleWindow = 128;
    private AudioClip _audioBuffer;
    private float _micLoudness = 0;
    private Queue<float> _lastTenFrames = new();
    [SerializeField] float _threshold = 0.75f;

    public delegate void BlowEventAction(float loudness);
    public static event BlowEventAction OnBlow;

    private void Start()
    {
        StartMic();
    }

    private void OnDestroy()
    {
        StopMic();
    }

    private void FixedUpdate()
    {
        _micLoudness = CheckMicrophoneVolume();
        float average = 0;
        _lastTenFrames.Enqueue(average);
        while (_lastTenFrames.Count > 10) _lastTenFrames.Dequeue();
        foreach (var frame in _lastTenFrames)
        {
            average += frame;
        }
        
        average /= _lastTenFrames.Count;

        if (average > _threshold)
        {
            OnBlow?.Invoke(_micLoudness);
        }
    }

    private void StartMic()
    {
        _deviceName = Microphone.devices[0];
        _audioBuffer = Microphone.Start(_deviceName, true, 999, 44100);
    }

    private void StopMic()
    {
        Microphone.End(_deviceName);
    }

    private void RestartMic()
    {
        StopMic();
        StartMic();
    }

    private float CheckMicrophoneVolume()
    {
        if (_audioBuffer == null) return 0f;

        if (_deviceName != Microphone.devices[0])
        {
            RestartMic();
            return 0f;
        }

        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(null) - (_sampleWindow + 1);
        if (micPosition < 0)
        {
            return 0;
        }
        _audioBuffer.GetData(waveData, micPosition);
        for (int i = 0; i < _sampleWindow; ++i)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax;
    }


}
