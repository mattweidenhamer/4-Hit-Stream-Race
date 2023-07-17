using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.Json;

using NativeWebSocket;

public class WebsocketManager : MonoBehaviour
{
    WebSocket webSocket;
    [SerializeField] int defaultPort = 1031;
    int? gameID = null;
    bool waitingForGameID = false;
    [SerializeField] float secondsToWaitForGameID = 10f;

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
            Reconnect();
        };

        webSocket.OnMessage += (bytes) =>
        {
            // getting the message as a string
            var message = System.Text.Encoding.UTF8.GetString(bytes);
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
        Dictionary<string, string> message = new Dictionary<string, string>();
        if (gameID != null){
            Debug.LogWarning("Already have a game ID, sending overwrite!");
            SendEndGame();
        }
        message.Add("type", "STARTGAME");
        message.Add("gameID", "0");

        SendWebSocketMessage(message);
        StartCoroutine(WaitForGameID());
    }
    private void ReceiveMessage(string message){
        Debug.Log("Received message!");
        Dictionary<string, string> parsedMessage = JsonSerializer.Deserialize<Dictionary<string, string>>(message);
        switch (parsedMessage["type"]) {
            case "SETUP":
                checkSetupMessage(parsedMessage);
                break;
            case "UPDATE":
                checkUpdateMessage(parsedMessage);
                break;
        }

    }
    private async void OnDestroy() {
        await webSocket.Close();
    }
    public void SendEndGame(){
        Dictionary<string, string> message = new Dictionary<string, string>();
        message.Add("type", "ENDGAME");
        message.Add("gameID", gameID.ToString());
        SendWebSocketMessage(message);
        gameID = null;
    }
    IEnumerator WaitForGameID(){
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
    private void Reconnect(){
        Debug.Log("Reconnecting...");
        OpenWebSocket(defaultPort, true);
    }
    private void checkSetupMessage(Dictionary<string, string> message){
        if(message["setup"] == "OK"){
            if(waitingForGameID){
                gameID = int.Parse(message["gameID"]);
            }
        }
        else {
            Debug.LogError("Setup failed!");
        }

    }
    private void checkUpdateMessage(Dictionary<string, string> message){
        
    }
}