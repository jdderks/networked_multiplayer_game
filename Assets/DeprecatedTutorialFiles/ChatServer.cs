using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ChatServer : MonoBehaviour
{
    int connectionID;
    int maxConnections = 10;
    int reliableChannelID;
    int hostID;
    int socketPort = 8888;
    byte error;

    public GameObject playerObject;
    public Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();
        reliableChannelID = config.AddChannel(QosType.ReliableSequenced);
        HostTopology topology = new HostTopology(config, maxConnections);
        hostID = NetworkTransport.AddHost(topology, socketPort, null);
        Debug.Log("Socket open. Host ID is: " + hostID);
    }

    // Update is called once per frame
    void Update()
    {
        int recHostID;
        int recConnectionID;
        int recChannelID;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recHostID, out recConnectionID, out recChannelID, recBuffer, bufferSize, out dataSize, out error);


        switch (recNetworkEvent)
        {
            case NetworkEventType.ConnectEvent:
                GameObject temp = Instantiate(playerObject, transform.position,transform.rotation);
                players.Add(recConnectionID, temp);
                break;
            case NetworkEventType.DataEvent:
                string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                Debug.Log("Receiving: " + msg);
                string[] splitData = msg.Split('|');

                switch (splitData[0])
                {
                    case "MOVE":
                        Move(splitData[1],splitData[2],players[recConnectionID]);
                    break;

                }

                break;
            case NetworkEventType.DisconnectEvent:
                break;
        }
    }

    private void Move(string x, string y, GameObject obj)
    {
        float xMov = float.Parse(x);
        float yMov = float.Parse(y);

        obj.transform.Translate(xMov, 0, yMov);
    }
}
