using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RaceManager : MonoBehaviour
{
    TimerScript timer;
    List<GameObject> players;
    bool raceIsRunning = false;
    AudioSource audioManager;
    [SerializeField] AudioClip startingNoise;
    [SerializeField] float startingNoiseVolume = 0.5f;
    
    [SerializeField] float startOffset = 0.5f;
    [SerializeField] AudioClip goalNoise;
    [SerializeField] float goalNoiseVolume = 0.5f;
    [SerializeField] float winningLapTime = 2f;
    CameraRotator cameraRotator;
    void Start(){
        timer = GameObject.FindGameObjectWithTag("timer").GetComponent<TimerScript>();
        players = new List<GameObject>();
        foreach(GameObject player in GameObject.FindGameObjectsWithTag("racer")){
            players.Add(player);
        }
        cameraRotator = GameObject.FindGameObjectWithTag("Camera Manager").GetComponent<CameraRotator>();
    }    
    public void StartRace()
    {
        if(!raceIsRunning){
            raceIsRunning = true;
            StartCoroutine(raceStarter());
        }

    }
    public void FinishRace(GameObject winnerPlayer){
        if(raceIsRunning){
            raceIsRunning = false;
            StartCoroutine(raceFinisher(winnerPlayer));

            
        }

        
    }
    public void AddPlayer(GameObject player){
        players.Add(player);
    }
    void StartAllPlayers() {
        foreach (GameObject player in players) {
            player.GetComponent<RacerBehavior>().StartRace();
        }
    }
    void StopAllPlayers(GameObject winner) {
        foreach (GameObject player in players) {
            if (player.GetInstanceID() == winner.GetInstanceID()){
                player.GetComponent<RacerBehavior>().FinishRace(true);
            }
            else{
                player.GetComponent<RacerBehavior>().FinishRace(false);
            }  
        }
    }
    IEnumerator raceStarter() {
        AudioSource.PlayClipAtPoint(startingNoise, Camera.main.transform.position, startingNoiseVolume);
        yield return new WaitForSeconds(startOffset);
        StartAllPlayers();
        timer.StartTimer();
    }
    IEnumerator raceFinisher(GameObject winnerPlayer) {
            print(winnerPlayer.GetComponent<RacerCosmetic>().racerName + " won the race!");
            cameraRotator.SetActiveCamera(winnerPlayer.GetComponent<RacerCosmetic>().myCamera);
            AudioSource.PlayClipAtPoint(goalNoise, Camera.main.transform.position, goalNoiseVolume);
            timer.StopTimer();
            yield return new WaitForSeconds(winningLapTime);
            StopAllPlayers(winnerPlayer);
    }
}
