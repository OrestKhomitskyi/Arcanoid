using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Arcanoid
{
    [Serializable]
    public class Prototype
    {
        public int Data { get; set; }
    }

    public static class ExtensionMethodsPrototype
    {
        public static T DeepCopy<T>(this T self)
        {
            if (!typeof(T).IsSerializable)
                throw new SerializationException("Class should be serializable");
            if (ReferenceEquals(self, null))
                return default(T);
            //Deep copying using BinaryFormatter
            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, self);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
