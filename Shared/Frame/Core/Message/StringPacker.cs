using System;
using System.IO;

namespace Frame.Core.Message
{
    public class StringPacker : IMessagePacker
    {
        public byte[] SerializeTo(object obj)
        {
            throw new NotImplementedException();
        }

        public void SerializeTo(object obj, MemoryStream stream)
        {
            throw new NotImplementedException();
        }

        public object DeserializeFrom(Type type, byte[] bytes, int index, int count)
        {
            throw new NotImplementedException();
        }

        public object DeserializeFrom(object instance, byte[] bytes, int index, int count)
        {
            throw new NotImplementedException();
        }

        public object DeserializeFrom(Type type, MemoryStream stream)
        {
            throw new NotImplementedException();
        }

        public object DeserializeFrom(object instance, MemoryStream stream)
        {
            throw new NotImplementedException();
        }
    }
}