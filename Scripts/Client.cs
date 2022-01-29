using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;
using UnityEngine.UI;
using Unity.Networking.Transport.Utilities;
using UnityEngine.SceneManagement;

namespace ChatClientExample
{
    public class Client : MonoBehaviour
    {
        static Dictionary<NetworkMessageType, NetworkMessageHandler> networkMessageHandlers = new Dictionary<NetworkMessageType, NetworkMessageHandler> {
            { NetworkMessageType.HANDSHAKE_RESPONSE, HandleServerHandshakeResponse },
        };

        public NetworkDriver m_Driver;
        public NetworkPipeline m_Pipeline;
        public NetworkConnection m_Connection;
        public bool Done;

        public static string serverIP;
        public static string clientName = "";

        // Start is called before the first frame update
        void Start() {
            // Create connection to server IP
            m_Driver = NetworkDriver.Create(new ReliableUtility.Parameters { WindowSize = 32 });
            m_Pipeline = m_Driver.CreatePipeline(typeof(ReliableSequencedPipelineStage));

            m_Connection = default(NetworkConnection);

            var endpoint = NetworkEndPoint.Parse(serverIP, 9000, NetworkFamily.Ipv4);
            endpoint.Port = 1511;
            m_Connection = m_Driver.Connect(endpoint);
        }

        // No collections list this time...
        void OnApplicationQuit() {
            // Disconnecting on application exit currently (to keep it simple)
            if (m_Connection.IsCreated) {
                m_Connection.Disconnect(m_Driver);
                m_Connection = default(NetworkConnection);
            }
        }

        void OnDestroy() {
            m_Driver.Dispose();
        }

        void Update() {
            m_Driver.ScheduleUpdate().Complete();

            if (!m_Connection.IsCreated) {
                if (!Done)
                    Debug.Log("Something went wrong during connect");
                return;
            }

            DataStreamReader stream;
            NetworkEvent.Type cmd;
            while ((cmd = m_Connection.PopEvent(m_Driver, out stream)) != NetworkEvent.Type.Empty) {
                if (cmd == NetworkEvent.Type.Connect) {
                    Debug.Log("We are now connected to the server");

                    // TODO: Send handshake message
                    uint msgType = (uint)NetworkMessageType.HANDSHAKE;
                    FixedString128Bytes str = new FixedString128Bytes(clientName);

                    DataStreamWriter writer;
                    int result = m_Driver.BeginSend(m_Pipeline, m_Connection, out writer);

                    // non-0 is an error code
                    if (result == 0) {
                        writer.WriteUInt(msgType);
                        writer.WriteFixedString128(str);

                        m_Driver.EndSend(writer);
                    }
                    else {
                        Debug.LogError($"Could not wrote message to driver: {result}", this);
                    }
                }
                else if (cmd == NetworkEvent.Type.Data) {
                    Done = true;

                    // First UInt is always message type (this is our own first design choice)
                    NetworkMessageType msgType = (NetworkMessageType)stream.ReadUInt();

                    if (networkMessageHandlers.ContainsKey(msgType)) {
                        networkMessageHandlers[msgType].Invoke(this, m_Connection, stream);
                    }
                    else {
                        Debug.LogWarning($"Unsupported message type received: {msgType}", this);
                    }
                }
                else if (cmd == NetworkEvent.Type.Disconnect) {
                    Debug.Log("Client got disconnected from server");
                    m_Connection = default(NetworkConnection);
                }
            }
        }

        public InputField input;
        public void SendMessage() {

            DataStreamWriter writer;
            int result = m_Driver.BeginSend(m_Pipeline, m_Connection, out writer);

            if (result == 0) {
                writer.WriteUInt((uint)NetworkMessageType.CHAT_MESSAGE);
                writer.WriteFixedString512(input.text);
                m_Driver.EndSend(writer);
            }

            // Prevent spam!
            input.text = "";
        }

        public void ExitChat() {
            DataStreamWriter writer;
            int result = m_Driver.BeginSend(m_Pipeline, m_Connection, out writer);

            if (result == 0) {
                writer.WriteUInt((uint)NetworkMessageType.CHAT_QUIT);
                m_Driver.EndSend(writer);
            }

            SceneManager.LoadScene(0);
        }

        // Receive message function
        // TODO
        //      - Server response handshake (DONE)

        static void HandleServerHandshakeResponse(object handler, NetworkConnection connection, DataStreamReader stream) {
            FixedString128Bytes str = stream.ReadFixedString128();
            Debug.Log(str);
        }
    }
}