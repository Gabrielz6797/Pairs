using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public GUIStyle clockStyle;
    private float _timer;
    private float _minutes;
    private float _seconds;
    private const float virtualWidth = 800f;
    private const float virtualHeight = 600f;
    private bool _stopTimer;
    private Matrix4x4 _matrix;
    private Matrix4x4 _oldMatrix;

    // Start is called before the first frame update
    void Start()
    {
        _stopTimer = false;
        _matrix = Matrix4x4.TRS(
            Vector3.zero,
            Quaternion.identity,
            new Vector3((Screen.width / virtualWidth), (Screen.height / virtualHeight), 1f)
        );
        _oldMatrix = GUI.matrix;
    }

    void Update()
    {
        if (!_stopTimer)
        {
            _timer += Time.deltaTime;
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

    private void OnGUI()
    {
        GUI.matrix = _matrix;
        _minutes = Mathf.Floor(_timer / 60);
        _seconds = Mathf.RoundToInt(_timer % 60);
        GUI.Label(
            new Rect(Camera.main.rect.x + 20, 10, 120, 50),
            "" + _minutes.ToString("00") + ":" + _seconds.ToString("00"),
            clockStyle
        );
        GUI.matrix = _oldMatrix;
    }
}
