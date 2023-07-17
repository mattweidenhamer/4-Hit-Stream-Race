using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RaceManager : MonoBehaviour
{
    [SerializeField] RaceStartSettings raceStartSettings;
    [SerializeField] RaceFinishSettings raceFinishSettings;
    [SerializeField] RacerObjInfo racerObjInfo;
    [SerializeField] GameObject raceUI;
    CameraRotator cameraRotator;
    TimerScript timer;
    List<GameObject> players;
    bool raceIsRunning = false;

    
    void Start(){
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
            StartCoroutine(RaceStarter());
        }

    }
    public void FinishRace(GameObject winnerPlayer){
        if(raceIsRunning){
            raceIsRunning = false;
            StartCoroutine(RaceFinisher(winnerPlayer));            
        }
    }

    void ReadyAllPlayers(){
        int count = 0;
        foreach (GameObject player in players) {
            player.GetComponent<RacerBehavior>().SetReady();
            count++;
        }
        print("Readied " + count + " players");
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

    IEnumerator RaceStarter() {
        ReadyAllPlayers();
        AudioSource.PlayClipAtPoint(raceStartSettings.StartingNoise, Camera.main.transform.position, raceStartSettings.StartingNoiseVolume);
        yield return new WaitForSeconds(raceStartSettings.StartOffset);
        StartAllPlayers();
        timer.StartTimer();
    }
    IEnumerator RaceFinisher(GameObject winnerPlayer) {
            print(winnerPlayer.GetComponent<RacerCosmetic>().racerName + " won the race!");
            cameraRotator.SetActiveCamera(winnerPlayer.GetComponent<RacerCosmetic>().GetCamera());
            AudioSource.PlayClipAtPoint(raceFinishSettings.GoalNoise, Camera.main.transform.position, raceFinishSettings.GoalNoiseVolume);
            timer.StopTimer();
            yield return new WaitForSeconds(raceFinishSettings.DelayUntilRaceEndAfterFinish);
            StopAllPlayers(winnerPlayer);
    }
    public void SpawnRaceFromSettings(List<RacerDataBaseScript> racerInfo){
        foreach (GameObject player in players){
            Destroy(player);
        }
        players.Clear();
        foreach (RacerDataBaseScript racer in racerInfo){
            GameObject newPlayer = Instantiate(racerObjInfo.RacerPrefab, (racerObjInfo.RacerSpawnBase + racerObjInfo.RacerSpawnOffset * players.Count), Quaternion.identity, racerObjInfo.RacerParent.transform);
            players.Add(newPlayer);
            newPlayer.GetComponent<RacerCosmetic>().racerName = racer.racerName;
            newPlayer.GetComponent<Animator>().runtimeAnimatorController = racer.animatorController;
        }
        Instantiate(raceUI);
        timer = GameObject.Find("Timer").GetComponent<TimerScript>();
    }
}
[Serializable]
struct RaceStartSettings {
    [SerializeField] AudioClip startingNoise;
    [SerializeField] float startingNoiseVolume;
    [SerializeField] float startOffset;
    public AudioClip StartingNoise => startingNoise;
    public float StartingNoiseVolume => startingNoiseVolume;
    public float StartOffset => startOffset;
};
[Serializable]
struct RaceFinishSettings {
    [SerializeField] AudioClip goalNoise;
    [SerializeField] float goalNoiseVolume;
    [SerializeField] float delayUntilRaceEndAfterFinish;
    public AudioClip GoalNoise => goalNoise;
    public float GoalNoiseVolume => goalNoiseVolume;
    public float DelayUntilRaceEndAfterFinish => delayUntilRaceEndAfterFinish;
};

[Serializable]
struct RacerObjInfo {
    [SerializeField] GameObject racerPrefab;
    [SerializeField] GameObject racerParent;
    [SerializeField] Vector2 racerSpawnBase;
    [SerializeField] Vector2 racerSpawnOffset;
    public GameObject RacerPrefab => racerPrefab;
    public GameObject RacerParent => racerParent;
    public Vector2 RacerSpawnBase => racerSpawnBase;
    public Vector2 RacerSpawnOffset => racerSpawnOffset;
}