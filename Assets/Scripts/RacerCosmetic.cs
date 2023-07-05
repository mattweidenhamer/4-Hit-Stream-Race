using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RacerCosmetic : MonoBehaviour
{
    public string racerName;
    public CinemachineVirtualCamera myCamera;
    [SerializeField] GameObject cameraPrefab;
    // Idea: Create a new virtual camera on creation assigned to this player.
    
    // This function is called by the CammeraRotator on Start. It creates a camera assigned to this player.
    public CinemachineVirtualCamera Init(){
        //myCamera = Instantiate(cameraPrefab);
        myCamera.m_Follow = this.transform;
        myCamera.gameObject.name = racerName + "'s Camera";
        return myCamera;
        
    }
}
