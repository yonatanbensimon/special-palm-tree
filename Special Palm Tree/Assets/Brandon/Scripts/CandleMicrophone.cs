using UnityEngine;

public class CandleMicrophone : MonoBehaviour
{
    private string _deviceName;
    [SerializeField] private int _sampleWindow = 128;
    private AudioClip _audioBuffer;

    private void Start()
    {
        StartMic();
    }

    private void Update()
    {
        CheckMicrophoneVolume();
    }

    private void StartMic()
    {
        if (_deviceName == null) _deviceName = Microphone.devices[0];
        _audioBuffer = Microphone.Start(_deviceName, true, 999, 44100);
    }

    private void StopMic()
    {
        Microphone.End(_deviceName);
    }

    private float CheckMicrophoneVolume()
    {
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
