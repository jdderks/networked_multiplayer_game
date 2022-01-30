using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ChatClientExample
{
    public class ChatMessage : MessageHeader
    {
        public override NetworkMessageType Type
        {
            get
            {
                return NetworkMessageType.CHAT_MESSAGE;
            }
        }

        public MessageType messageType;
        public string message;

        //public override void SerializeObject(ref DataStreamWriter writer)
        //{
        //    // very important to call this first
        //    base.SerializeObject(ref writer);

        //    writer.WriteUInt((uint)messageType);
        //    writer.WriteFixedString128(message);
        //}

        //public override void DeserializeObject(ref DataStreamReader reader)
        //{
        //    // very important to call this first
        //    base.DeserializeObject(ref reader);

        //    messageType = (MessageType)reader.ReadUInt();
        //    message = reader.ReadFixedString128().ToString();
        //}
    }
}