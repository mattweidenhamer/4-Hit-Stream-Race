using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    RaceManager raceManager;
    private void Start() {
        raceManager = GameObject.Find("Race Manager").GetComponent<RaceManager>();

    }
    void OnTriggerEnter2D(Collider2D collided) {
        if(collided.gameObject.tag == "racer") {
            raceManager.FinishRace(collided.gameObject);
            collided.gameObject.GetComponent<RacerCosmetic>();
        }
    }
}
