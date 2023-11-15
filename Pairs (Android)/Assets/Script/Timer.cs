using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI timer;

    [Header("Timer Settings")]
    public float _timer;
    private bool _stopTimer;

    // Start is called before the first frame update
    void Start()
    {
        _stopTimer = false;
    }

    void Update()
    {
        if (!_stopTimer)
        {
            _timer += Time.deltaTime;
            timer.text = Mathf.Floor(_timer / 60).ToString("00") + ":" + Mathf.FloorToInt(_timer % 60).ToString("00");
        }
    }

    public float getCurrentTime()
    {
        return _timer;
    }

    public void stopTimer()
    {
        _stopTimer = true;
    }

    public void resumeTimer()
    {
        _stopTimer = false;
    }
}
