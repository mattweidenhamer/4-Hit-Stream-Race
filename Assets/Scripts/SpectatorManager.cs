using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpectatorManager : MonoBehaviour
{
    [SerializeField] GameObject spectatorPrefab;
    [SerializeField] int numberOfSpectators = 10;
    List<string> spectatorPaths;
    string spectatorFolder;
    List<Sprite> spectatorSprites;
    [SerializeField] Vector2 spectatorInitialPosition = new Vector2(0, 0);
    [SerializeField] Vector2 spectatorRightLimit = new Vector2(0, 0);
    [SerializeField] float spectatorIntervalMultiplier = 1f;
    [SerializeField] float spectatorIntervalY = 1f;
    [SerializeField] int pixelsPerUnit = 225;
    
    private void Start() {
        checkSpectatorFolder();
        loadSpectators();
        pullSpectatorPool();
    }


    private void checkSpectatorFolder(){
        // Check if the spectator folder exists under Documents/My Games/4 Hit Combo Race/, and create it if it doesn't exist.
        spectatorFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        spectatorFolder = System.IO.Path.Combine(spectatorFolder, "My Games", "Stream Racers", "Spectators");
        System.IO.Directory.CreateDirectory(spectatorFolder);
    }
    private void loadSpectators(){
        // Load all the spectators from the spectator folder.
        spectatorPaths = System.IO.Directory.GetFiles(spectatorFolder, "*.png").ToList<string>();
        print("Found " + spectatorPaths.Count + " spectator images.");
    }
    public void pullSpectatorPool(){
        if(spectatorPaths.Count == 0){
            print("No spectators found.");
            return;
        }
        List<string> spectatorsToUse = new List<string>();
        if(numberOfSpectators >= spectatorPaths.Count){
            spectatorsToUse = spectatorPaths;
        }
        else{
            List<string> tempSpectatorPaths = spectatorPaths;
            for (int i = 0; i < numberOfSpectators; i++) {
                int randomIndex = Random.Range(0, tempSpectatorPaths.Count);
                spectatorsToUse.Add(tempSpectatorPaths[randomIndex]);
                tempSpectatorPaths.RemoveAt(randomIndex);
            }
        }
        // Convert all the spectators to sprites.
        spectatorSprites = new List<Sprite>();
        foreach (string spectatorPath in spectatorsToUse) {
            Texture2D texture = new Texture2D(450, 450);
            byte[] fileData = System.IO.File.ReadAllBytes(spectatorPath);
            texture.LoadImage(fileData);
            // Note: May need to change the pixelsPerUnit to make it fit.
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, pixelsPerUnit);
            spectatorSprites.Add(sprite);
        }
    }
        public void generateSpectators(){
        if(spectatorSprites.Count == 0){
            print("No spectators found, pull a spectator pool first.");
            return;
        }
        int counter = 0;
        foreach(Sprite spectatorSprite in spectatorSprites){
            counter++;
            float spacerModifier = spectatorSprite.rect.width / pixelsPerUnit;
            float vectorX = spectatorInitialPosition.x;
            float vectorY = spectatorInitialPosition.y; 
            for(int i = 0; i < counter; i++){
                vectorX += spectatorIntervalMultiplier * spacerModifier;
                if(vectorX > spectatorRightLimit.x){
                    vectorX = spectatorInitialPosition.x + spectatorIntervalMultiplier * spacerModifier  / 2;
                    vectorY += spectatorIntervalY;
                }
            }
            GameObject spectator = Instantiate(spectatorPrefab, transform);
            spectator.transform.position = new Vector2(vectorX, vectorY);
            spectator.GetComponent<SpriteRenderer>().sprite = spectatorSprite;
        }
    }


}
