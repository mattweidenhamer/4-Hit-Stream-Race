using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField] String MenuSceneName;
    [SerializeField] String RacersEditSceneName;
    public void PressStartGame(){
        Debug.Log("Starting game!");
        SceneManager.LoadScene(MenuSceneName);
    }
    public void PressEditRacers(){
        Debug.Log("Editing racers!");
        SceneManager.LoadScene(RacersEditSceneName);
    }
    public void PressExit(){
        Debug.Log("Exiting game!");
        Application.Quit();
    }
}
