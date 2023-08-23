using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.Json;

using NativeWebSocket;
using System;

public class WebsocketManager : MonoBehaviour
{
    WebSocket webSocket;
    [SerializeField] int defaultPort = 1031;
    string gameID = "";
    bool waitingForGameID = false;
    [SerializeField] float secondsToWaitForGameID = 10f;
    public void OpenDefaultWebsocket(bool overwrite = false){
        OpenWebSocket(defaultPort, overwrite);
    }

    public async void OpenWebSocket(int port, bool overwrite = false){
        if(webSocket != null && !overwrite){
            Debug.LogError("Websocket already exists!");
            return;
        }
        webSocket = new WebSocket("ws://localhost:" + port.ToString());
        webSocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        webSocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        webSocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
            //Reconnect();
        };

        webSocket.OnMessage += (bytes) =>
        {
            Debug.Log("Received message!");
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            // getting the message as a string
            Debug.Log(message);
            ReceiveMessage(message);
        };
        await webSocket.Connect();
        
    }

    public async void SendWebSocketMessage(Dictionary<string, string> message){
        if(webSocket == null){
            Debug.LogError("Websocket was not created before message was sent! In the future, make sure it's created before sending a message.");
            OpenWebSocket(defaultPort);
        }
        string jsonMessage = JsonSerializer.Serialize(message);

        await webSocket.SendText(jsonMessage);
    }

    public void SendNewGameMessage(){
        if (gameID != ""){
            Debug.LogWarning("Already have a game ID, ending and overwriting!");
            SendEndGameMessage();
        }
        Dictionary<string, string> message = new Dictionary<string, string>() {
            { "type", "STARTGAME" },
            { "gameID", "0" }
        };

        SendWebSocketMessage(message);
        StartCoroutine(WaitForGameID());
    }
    public void SendEndGameMessage(){
        Dictionary<string, string> message = new Dictionary<string, string>() {
            { "type", "ENDGAME" },
            { "gameID", gameID.ToString() }

        };
        SendWebSocketMessage(message);
        gameID = null;
    }
    public void SendRacerMessage(string discordID){
        Dictionary<string, string> message = new Dictionary<string, string>() {
            { "type", "RACER" },
            { "gameID", gameID.ToString() },
            { "discordID", discordID }
        };
        SendWebSocketMessage(message);
    }
    private void ReceiveMessage(string message){
        Dictionary<string, string> parsedMessage = JsonSerializer.Deserialize<Dictionary<string, string>>(message);
    
        switch (parsedMessage["type"]) {
            case "SETUP":
                checkSetupMessage(parsedMessage);
                break;
            // Update messages should be the ones that affect the racers.
            case "UPDATE":
                checkUpdateMessage(parsedMessage);
                break;
        }
    }


    IEnumerator WaitForGameID(){
        Debug.Log("Waiting for game ID...");
        waitingForGameID = true;
        float timeWaited = 0f;
        while(gameID == null){
            timeWaited += 1f;
            if (timeWaited > secondsToWaitForGameID){
                Debug.LogError("Timed out waiting for game ID!");
                waitingForGameID = false;
                yield break;
            }
            yield return new WaitForSeconds(1f);
        }
        
        waitingForGameID = false;
        Debug.Log("Game ID received!");
    }

    private void checkSetupMessage(Dictionary<string, string> message){
        if(message["setup"] == "OK"){
            if(waitingForGameID){
                gameID = message["gameID"];
                Debug.Log("Game ID received!");
                Debug.Log("Game ID: " + gameID);
            }
        }
        else {
            Debug.LogError("Setup failed!");
        }

    }
    private void checkUpdateMessage(Dictionary<string, string> message){
        
    }
    private void Reconnect(){
        Debug.Log("Reconnecting...");
        OpenWebSocket(defaultPort, true);
    }
    void Update(){
        if(webSocket == null){
            return;
        }
        webSocket.DispatchMessageQueue();
    }
    private async void OnDestroy() {
        if(webSocket != null){
            await webSocket.Close();
        }
        
    }
}