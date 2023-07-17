using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RacerDropdownHandler : MonoBehaviour
{
    public List<RacerDataBaseScript> availableRacers;
    List<TMP_Dropdown.OptionData> dropdownOptions;
    TMP_Dropdown[] dropdowns;
    public List<RacerDataBaseScript> selectedRacers;

    void Start(){
        dropdowns = GetComponentsInChildren<TMP_Dropdown>();
        selectedRacers = new List<RacerDataBaseScript>();
        dropdownOptions = new List<TMP_Dropdown.OptionData>();
        foreach(RacerDataBaseScript racer in availableRacers){
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(racer.racerName, racer.previewSprite);
            dropdownOptions.Add(option);
        }
        foreach (TMP_Dropdown dropdown in dropdowns){
            dropdown.AddOptions(dropdownOptions);
        }
    }
    public void SelectRacer(){
        selectedRacers.Clear();
        foreach (TMP_Dropdown dropdown in dropdowns){
            if (dropdown.value != 0){
                selectedRacers.Add(availableRacers[dropdown.value]);
            }
        }
    }

    public void OnStartClicked(){
        if (selectedRacers.Count > 0){
            StartRace();
        }
        else {
            //Indicate that there was a problem
            print("There were no racers");
        }
    }
    private void StartRace(){
            print("Starting!");
            GameObject.Find("Race Manager").GetComponent<RaceManager>().SpawnRaceFromSettings(selectedRacers);
            GameObject.FindGameObjectWithTag("SpectatorManager").GetComponent<SpectatorManager>().generateSpectators();
            Destroy(GameObject.FindGameObjectWithTag("UIRacerSelect"));
    }
    
}
