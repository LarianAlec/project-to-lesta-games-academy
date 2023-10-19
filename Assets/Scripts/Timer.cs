using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _timerText;
    float _elapsedTime = 0f;
    int _minutes = 0;
    int _seconds = 0;  
    int _milliSeconds = 0;

    bool _enabled = false;


    private void Update()
    {
        if (_enabled)
        {
            _elapsedTime += Time.deltaTime;
            _minutes = Mathf.FloorToInt(_elapsedTime / 60);
            _seconds = Mathf.FloorToInt(_elapsedTime % 60);
            _milliSeconds = (int)(_elapsedTime * 100f) % 100;
            _timerText.text = string.Format("{0:00}:{1:00}:{2:00}", _minutes, _seconds, _milliSeconds);
        }
    }

    public void StartTimer()
    {
        _enabled = true;
    }

    public void StopTimer()
    {
        _enabled = false;
    }

    public string GetFinishTime()
    {
        return string.Format("{0:00}:{1:00}:{2:00}", _minutes, _seconds, _milliSeconds);
    }
}
