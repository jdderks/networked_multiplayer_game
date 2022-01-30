using UnityEngine;
using Unity.Networking.Transport;
using System.Collections.Generic;
using NaughtyAttributes;

public class ClientBehaviour : MonoBehaviour
{
    static Dictionary<GameEvent, GameEventHandler> gameEventDictionary = new Dictionary<GameEvent, GameEventHandler>()
    {
        ////Link game events to functions...
        //{ 
        //    //GameEvent.NUMBER_REPLY, HandleChatMessage 
        //}
    };

    public NetworkDriver m_Driver;
    public NetworkConnection m_Connection;
    public bool m_Done;

    // Start is called before the first frame update
    private void Start()
    {
        m_Driver = NetworkDriver.Create();
        m_Connection = default(NetworkConnection);

        //Loopback any Ipv4 comes down to a connection with 127.0.0.1 (localhost)
        var endpoint = NetworkEndPoint.LoopbackIpv4;

        //Listening on firewall port 9000
        endpoint.Port = 9000;

        //Send connection request
        m_Connection = m_Driver.Connect(endpoint);
    }

    private void OnDestroy()
    {
        m_Driver.Dispose();
    }

    // Update is called once per frame
    private void Update()
    {
        m_Driver.ScheduleUpdate().Complete();
        if (!m_Connection.IsCreated)
        {
            if (!m_Done)
            {
                Debug.Log("Something went wrong during connect.");
                return;
            }
        }

        DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = m_Connection.PopEvent(m_Driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                Debug.Log("I (The client) am now connected to the server");

                uint value = 1;
                DataStreamWriter writer;
                int result = m_Driver.BeginSend(m_Connection, out writer);
                if (result == 0)
                {
                    //Game Event value
                    writer.WriteUInt((uint)GameEvent.NUMBER);
                    writer.WriteUInt(value);
                    m_Driver.EndSend(writer);
                }

            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                //Read GameEvent type from stream
                GameEvent gameEventType = (GameEvent)stream.ReadUInt();

                if (gameEventDictionary.ContainsKey(gameEventType))
                {
                    gameEventDictionary[gameEventType].Invoke(stream, this, m_Connection);
                }
                else
                {
                    Debug.LogWarning("Unsupported event received");
                    //Unsupported event received!
                }
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("I (client) got disconnected from the server.");
                m_Connection = default(NetworkConnection);
            }
        }
    }

    //Event functions...
    //static void HandleChatMessage(ClientBehaviour client, MessageHeader header)
    //{
    //    uint value = stream.ReadUInt();
    //    Debug.Log("Got the value = " + value + " back from the server");

    //    ClientBehaviour client = sender as ClientBehaviour;

    //    //TODO: Remove when building more complex client...
    //    client.m_Done = true;
    //    connection.Disconnect(client.m_Driver);
    //    connection = default(NetworkConnection);
    //}
}
