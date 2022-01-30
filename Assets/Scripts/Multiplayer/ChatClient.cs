using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;
using Unity.Networking.Transport.Utilities;

namespace ChatClientExample
{
    public class ChatClient : MonoBehaviour
    {
        //static Dictionary<NetworkMessageType, NetworkMessageHandler> networkMessageHandlers = new Dictionary<NetworkMessageType, NetworkMessageHandler> {
        //    { NetworkMessageType.HANDSHAKE_RESPONSE, HandleClientResponse },
        //};

        public NetworkDriver m_Driver;
        public NetworkConnection m_Connection;
        bool m_Done;


        // Start is called before the first frame update
        void Start()
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
        void Update()
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
                    ////Read GameEvent type from stream
                    //GameEvent gameEventType = (GameEvent)stream.ReadUInt();

                    //if (networkMessageHandlers.ContainsKey(gameEventType))
                    //{
                    //    networkMessageHandlers[gameEventType].Invoke(stream, this, m_Connection);
                    //}
                    //else
                    //{
                    //    Debug.LogWarning("Unsupported event received");
                    //    //Unsupported event received!
                    //}
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("I (client) got disconnected from the server.");
                    m_Connection = default(NetworkConnection);
                }
            }
        }

        static void HandleClientResponse(Client client, MessageHeader header)
        {
            ChatMessage chatMsg = header as ChatMessage;

            Color c = ChatCanvas.chatColor;
            if (chatMsg.messageType == MessageType.JOIN) c = ChatCanvas.joinColor;
            if (chatMsg.messageType == MessageType.QUIT) c = ChatCanvas.leaveColor;

            //client.chatCanvas.NewMessage(chatMsg.message, c);
        }
    }
}