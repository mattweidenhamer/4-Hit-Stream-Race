using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraTrackerUpdater : MonoBehaviour
{
    int framesSinceLastUpdate = 0;
    [SerializeField] int updateEveryNthFrame = 10;
    CameraRotator cameraRotator;
    string activeCameraName;
    TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        cameraRotator = GetComponent<CameraRotator>();
        text = GetComponent<TMP_Text>();
        text.text = "Active Camera: Whole Track Camera";
    }

    // Update is called once per frame
    void fixedUpdate(){
        framesSinceLastUpdate++;
        if (framesSinceLastUpdate >= updateEveryNthFrame){
            framesSinceLastUpdate = 0;
            activeCameraName = cameraRotator.GetActiveCameraName();
            text.text = "Active Camera: " + activeCameraName;
        }
    }
}