using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public class CameraRotator : MonoBehaviour
{
    public List<CinemachineVirtualCamera> cameras;
    int activeCameraIndex;
    public bool initted = false;
    // Start is called before the first frame update
    void Start()
    {
        initted = true;

        activeCameraIndex = 0;
        // Add cameras in this order: Whole Track Camera, Group Shot Camera, Player 1 Camera, Player 2 Camera, Player 3 Camera, Player 4 Camera
        // Note: Instead of having a cycling list, amybe have a button for each camera?
        List<CinemachineVirtualCamera> temp = GameObject.FindObjectsOfType<CinemachineVirtualCamera>().ToList();
        foreach (CinemachineVirtualCamera camera in temp){
            if (camera.gameObject.name == "Whole Track Camera"){
                cameras.Insert(0, camera);
                camera.Priority = 1;
            }
            else if (camera.gameObject.name == "Group Shot Camera"){
                if(cameras.Count >= 1){
                    cameras.Insert(1, camera);
                }
                else{
                    cameras.Add(camera);
                }
                camera.Priority = 0;
            }
            else {
                cameras.Add(camera);
                camera.Priority = 0;
            }
        }
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("racer")){
            cameras.Add(player.GetComponent<RacerCosmetic>().Init());
        }
    }
    public void nextCamera(){
        cameras[activeCameraIndex].Priority = 0;
        activeCameraIndex = (activeCameraIndex + 1) % cameras.Count;
        cameras[activeCameraIndex].Priority = 1;
    }
    public string GetActiveCameraName(){
        if(cameras[activeCameraIndex].gameObject.name == "Whole Track Camera"){
            return("Whole Track Camera");
        }
        else if(cameras[activeCameraIndex].gameObject.name == "Group Shot Camera"){
            return("Group Shot Camera");
        }
        else{
            return("" + cameras[activeCameraIndex].m_Follow.GetComponent<RacerCosmetic>().racerName + "'s Camera");
        }
    }
    public void SetActiveCamera(CinemachineVirtualCamera camera){
        cameras[activeCameraIndex].Priority = 0;
        camera.Priority = 1;
        if(cameras.Contains(camera)){
            activeCameraIndex = cameras.IndexOf(camera);
        }
        else{
            cameras.Add(camera);
            activeCameraIndex = cameras.Count - 1;
        }
    }
    public void AddCamera(CinemachineVirtualCamera camera){
        cameras.Add(camera);
    }
}
