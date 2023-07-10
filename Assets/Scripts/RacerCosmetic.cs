using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RacerCosmetic : MonoBehaviour
{
    public string racerName;
    CinemachineVirtualCamera myCamera;
    [SerializeField] GameObject cameraPrefab;
    // This function is called by the CammeraRotator on Start. It creates a camera assigned to this player.
    public CinemachineVirtualCamera Init(){
        myCamera = Instantiate(cameraPrefab).GetComponent<CinemachineVirtualCamera>();
        myCamera.Priority = 0;
        myCamera.m_Follow = this.transform;
        myCamera.gameObject.name = racerName + "'s Camera";
        return myCamera;
        
    }
    public CinemachineVirtualCamera GetCamera(){
        return myCamera;
    }
}
