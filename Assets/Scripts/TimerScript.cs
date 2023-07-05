using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    bool running = false;
    float time = 0;
    TMP_Text timerDisplay;

    private void Start() {
        timerDisplay = GetComponent<TMP_Text>();
    }
    public void StartTimer(){
        running = true;
    }
    public void StopTimer(){
        running = false;
    }
    private void Update() {
        if (running){
            time += Time.deltaTime;
        }
        timerDisplay.text = getTime();
        //Edit the text object this is connected to
    }
    private string getTime() {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 100) % 100);
        return minutes + ":" + seconds + ":" + milliseconds;
    }
}
