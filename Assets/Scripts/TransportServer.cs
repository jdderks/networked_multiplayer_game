using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;

namespace TransportExample
{
    public class TransportServer : MonoBehaviour
    {
        //public NetworkDriver m_Driver;
        ////private NativeList m_Connections;

        //void Start()
        //{
        //    m_Driver = NetworkDriver.Create();
        //    var endpoint = NetworkEndPoint.AnyIpv4;
        //    endpoint.Port = 1511;
        //    if (m_Driver.Bind(endpoint) != 0)
        //        Debug.Log("Failed to bind to port 1511");
        //    else
        //        m_Driver.Listen();

        //    m_Connections = new NativeList(16, Allocator.Persistent);
        //}

        //private void OnDestroy()
        //{
        //    m_Driver.Dispose();
        //    m_Connections.Dispose();
        //}

        //void Update()
        //{
        //    m_Driver.ScheduleUpdate().Complete();

        //    // Clean up connections
        //    for (int i = 0; i < m_Connections.Length; i++)
        //    {
        //        if (!m_Connections[i].IsCreated)
        //        {
        //            m_Connections.RemoveAtSwapBack(i);
        //            --i;
        //        }
        //    }

        //    // Accept new connections
        //    NetworkConnection c;
        //    while ((c = m_Driver.Accept()) != default(NetworkConnection))
        //    {
        //        m_Connections.Add(c);
        //        Debug.Log("Accepted a connection");
        //    }

        //    DataStreamReader stream;
        //    for (int i = 0; i < m_Connections.Length; i++)
        //    {
        //        if (!m_Connections[i].IsCreated)
        //            continue;

        //        NetworkEvent.Type cmd;
        //        while ((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) != NetworkEvent.Type.Empty)
        //        {
        //            if (cmd == NetworkEvent.Type.Data)
        //            {
        //                uint number = stream.ReadUInt();
        //                Debug.Log("Got " + number + " from the Client adding + 2 to it.");

        //                number += 2;

        //                DataStreamWriter writer;
        //                int result = m_Driver.BeginSend(NetworkPipeline.Null, m_Connections[i], out writer);

        //                if (result == 0)
        //                {
        //                    writer.WriteUInt(number);
        //                    m_Driver.EndSend(writer);
        //                }
        //            }
        //            else if (cmd == NetworkEvent.Type.Disconnect)
        //            {
        //                Debug.Log("Client disconnected from server");
        //                m_Connections[i] = default(NetworkConnection);
        //            }
        //        }
        //    }
        //}
    }
}