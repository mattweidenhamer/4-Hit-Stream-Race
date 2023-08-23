using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NativeWebSocket;
public class WebsocketTest : MonoBehaviour
{
    [SerializeField] WebsocketManager websocketManager;
    public void TestCreateWebsocket(){
        websocketManager.OpenWebSocket(1031);
    }
    public void TestSendNewGame(){
        websocketManager.SendNewGameMessage();
    }
    public void TestSendEndGame(){
        websocketManager.SendEndGameMessage();
    }
}
