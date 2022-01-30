using UnityEngine;
using UnityEngine.Assertions;

using Unity.Collections;
using Unity.Networking.Transport;
using System.Collections.Generic;
using NaughtyAttributes;
public enum GameEvent
{
    NUMBER = 0,
    NUMBER_REPLY = 1,
}

delegate void GameEventHandler(DataStreamReader stream, object sender, NetworkConnection connection);

public class ServerBehaviour : MonoBehaviour
{
    static Dictionary<GameEvent, GameEventHandler> gameEventDictionary = new Dictionary<GameEvent, GameEventHandler>()
    {
        //Link game events to functions...
        { GameEvent.NUMBER, NumberHandler }
    };


    public NetworkDriver m_Driver;
    private NativeList<NetworkConnection> m_Connections;



    private void Start()
    {
        m_Driver = NetworkDriver.Create();
        var endpoint = NetworkEndPoint.AnyIpv4;

        endpoint.Port = 9000;

        if (m_Driver.Bind(endpoint) != 0)
        {
            Debug.Log("Failed to bind to port " + endpoint.Port);
        }
        else
        {
            m_Driver.Listen();
        }
        m_Connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);
    }


    private void OnDestroy()
    {
        m_Driver.Dispose();
        m_Connections.Dispose();
    }

    private void Update()
    {
        //Required because com.unity.transport uses the Unity C# Job System internally
        m_Driver.ScheduleUpdate().Complete();

        //Cleanup any stale connections
        for (int i = 0; i < m_Connections.Length; i++)
        {
            if (!m_Connections[i].IsCreated)
            {
                m_Connections.RemoveAtSwapBack(i);
                --i;
            }
        }

        //Accept new connections
        NetworkConnection c;
        while ((c = m_Driver.Accept()) != default(NetworkConnection))
        {
            m_Connections.Add(c);
            Debug.Log("Accepted a connection");
        }

        DataStreamReader stream;
        for (int i = 0; i < m_Connections.Length; i++)
        {
            Assert.IsTrue(m_Connections[i].IsCreated);

            NetworkEvent.Type cmd;
            while ((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Data)
                {
                    //Check which game event we've received
                    GameEvent gameEventType = (GameEvent)stream.ReadUInt();

                    if (gameEventDictionary.ContainsKey(gameEventType))
                    {
                        gameEventDictionary[gameEventType].Invoke(stream, this, m_Connections[i]);
                    } 
                    else
                    {
                        Debug.LogWarning("Unsupported event received");
                        //Unsupported event received!
                    }

                    switch (gameEventType)
                    {
                        case GameEvent.NUMBER:
                            
                            break;
                        default:
                            Debug.LogWarning("Unsupported event received");
                            //Unsupported event received!
                            break;
                    }
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnected from server");
                    m_Connections[i] = default(NetworkConnection);
                }
            }
        }
    }

    //Event functions
    static void NumberHandler(DataStreamReader stream, object sender, NetworkConnection connection)
    {
        uint number = stream.ReadUInt();

        Debug.Log("Got " + number + " from the Client adding + 2 to it.");
        number += 2;

        ServerBehaviour server = sender as ServerBehaviour;

        DataStreamWriter writer;
        int result = server.m_Driver.BeginSend(NetworkPipeline.Null, connection, out writer);
        if (result == 0)
        {
            //Add GameEvent reply UINT
            writer.WriteUInt((uint)GameEvent.NUMBER_REPLY);
            writer.WriteUInt(number);
            server.m_Driver.EndSend(writer);
        }
    }
}