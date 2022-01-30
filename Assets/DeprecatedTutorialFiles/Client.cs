using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour
{
    int connectionID;
    int maxConnections = 10;
    int reliableChannelID;
    int hostID;
    int socketPort = 8888;
    byte error;

    void Start()
    {
        NetworkTransport.Init();
    }

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

                break;
            case NetworkEventType.DisconnectEvent:

                break;
            case NetworkEventType.DataEvent:
                break;
        }
    }

    public void Connect()
    {
        ConnectionConfig config = new ConnectionConfig();
        reliableChannelID = config.AddChannel(QosType.ReliableSequenced);
        HostTopology topology = new HostTopology(config, maxConnections);
        hostID = NetworkTransport.AddHost(topology, 0);
        Debug.Log("Socket open. Host ID is: " + hostID);
        connectionID = NetworkTransport.Connect(hostID, "127.0.0.1", socketPort, 0, out error);

    }

    public void Disconnect()
    {
        NetworkTransport.Disconnect(hostID, connectionID, out error);
    }
    public void sendMessage(string message)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(message);
        NetworkTransport.Send(hostID, connectionID, reliableChannelID, buffer, message.Length * sizeof(char), out error);
    }
}
