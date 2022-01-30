using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;
using Unity.Networking.Transport.Utilities;

namespace ChatClientExample
{
    public delegate void NetworkMessageHandler(object handler, NetworkConnection con, DataStreamReader stream);

    public enum NetworkMessageType
    {
        HANDSHAKE,
        HANDSHAKE_RESPONSE,
        CHAT_MESSAGE,
        CHAT_QUIT
    }

    public enum MessageType
    {
        MESSAGE,
        JOIN,
        QUIT
    }

    public class ChatServer : MonoBehaviour
    {
        static Dictionary<NetworkMessageType, NetworkMessageHandler> networkMessageHandlers = new Dictionary<NetworkMessageType, NetworkMessageHandler> {
            { NetworkMessageType.HANDSHAKE, HandleClientHandshake },
            { NetworkMessageType.CHAT_MESSAGE, HandleClientMessage },
            { NetworkMessageType.CHAT_QUIT, HandleClientExit },
        };

        public NetworkDriver m_Driver;
        public NetworkPipeline m_Pipeline;
        private NativeList<NetworkConnection> m_Connections;

        private Dictionary<NetworkConnection, string> nameList = new Dictionary<NetworkConnection, string>();

        public ChatCanvas chat;

        void Start()
        {
            // Create Driver
            m_Driver = NetworkDriver.Create(new ReliableUtility.Parameters { WindowSize = 32 });
            m_Pipeline = m_Driver.CreatePipeline(typeof(ReliableSequencedPipelineStage));

            // Open listener on server port
            NetworkEndPoint endpoint = NetworkEndPoint.AnyIpv4;
            endpoint.Port = 1511;
            if (m_Driver.Bind(endpoint) != 0)
                Debug.Log("Failed to bind to port 1511");
            else
                m_Driver.Listen();

            m_Connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);
        }

        // Write this immediately after creating the above Start calls, so you don't forget
        //  Or else you well get lingering thread sockets, and will have trouble starting new ones!
        void OnDestroy()
        {
            m_Driver.Dispose();
            m_Connections.Dispose();
        }

        void Update()
        {
            // This is a jobified system, so we need to tell it to handle all its outstanding tasks first
            m_Driver.ScheduleUpdate().Complete();

            // Clean up connections, remove stale ones
            for (int i = 0; i < m_Connections.Length; i++)
            {
                if (!m_Connections[i].IsCreated)
                {

                    if (nameList.ContainsKey(m_Connections[i]))
                    {
                        chat.NewMessage($"{ nameList[m_Connections[i]]} has disconnected.", ChatCanvas.leaveColor);
                        nameList.Remove(m_Connections[i]);
                    }

                    m_Connections.RemoveAtSwapBack(i);
                    // This little trick means we can alter the contents of the list without breaking/skipping instances
                    --i;
                }
            }

            // Accept new connections
            NetworkConnection c;
            while ((c = m_Driver.Accept()) != default(NetworkConnection))
            {
                m_Connections.Add(c);
                Debug.Log("Accepted a connection");
            }

            DataStreamReader stream;
            for (int i = 0; i < m_Connections.Length; i++)
            {
                if (!m_Connections[i].IsCreated)
                    continue;

                // Loop through available events
                NetworkEvent.Type cmd;
                while ((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) != NetworkEvent.Type.Empty)
                {
                    if (cmd == NetworkEvent.Type.Data)
                    {
                        // First UInt is always message type (this is our own first design choice)
                        NetworkMessageType msgType = (NetworkMessageType)stream.ReadUInt();

                        if (networkMessageHandlers.ContainsKey(msgType))
                        {
                            try
                            {
                                networkMessageHandlers[msgType].Invoke(this, m_Connections[i], stream);
                            }
                            catch
                            {
                                Debug.LogError("Badly formatted message received...");
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"Unsupported message type received: {msgType}", this);
                        }
                    }
                }
            }
        }

        // Static handler functions
        //  - Client handshake                  (DONE)
        //  - Client chat message               (DONE)
        //  - Client chat exit                  (DONE)

        static void HandleClientHandshake(object handler, NetworkConnection connection, DataStreamReader stream)
        {
            // Pop name
            FixedString128Bytes str = stream.ReadFixedString128();

            ChatServer serv = handler as ChatServer;

            // Add to list
            serv.nameList.Add(connection, str.ToString());
            serv.chat.NewMessage($"{str.ToString()} has joined the chat.", ChatCanvas.joinColor);

            // Send message back
            DataStreamWriter writer;
            int result = serv.m_Driver.BeginSend(NetworkPipeline.Null, connection, out writer);

            // non-0 is an error code
            if (result == 0)
            {
                writer.WriteUInt((uint)NetworkMessageType.HANDSHAKE_RESPONSE);
                writer.WriteFixedString128(new FixedString128Bytes($"Welcome {str.ToString()}!"));

                serv.m_Driver.EndSend(writer);
            }
            else
            {
                Debug.LogError($"Could not write message to driver: {result}", serv);
            }
        }

        static void HandleClientMessage(object handler, NetworkConnection connection, DataStreamReader stream)
        {
            // Pop message
            FixedString128Bytes str = stream.ReadFixedString128();

            ChatServer serv = handler as ChatServer;

            if (serv.nameList.ContainsKey(connection))
            {
                serv.chat.NewMessage($"{serv.nameList[connection]}: {str.ToString()}", ChatCanvas.chatColor);
            }
            else
            {
                Debug.LogError($"Received message from unlisted connection: {str}");
            }
        }

        static void HandleClientExit(object handler, NetworkConnection connection, DataStreamReader stream)
        {
            ChatServer serv = handler as ChatServer;

            if (serv.nameList.ContainsKey(connection))
            {
                serv.chat.NewMessage($"{serv.nameList[connection]} has left the chat.", ChatCanvas.leaveColor);
                connection.Disconnect(serv.m_Driver);
            }
            else
            {
                Debug.LogError("Received exit from unlisted connection");
            }
        }
    }
}