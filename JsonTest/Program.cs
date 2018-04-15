using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace JsonTest
{
    [DataContract]
    public class Person
    {
        [DataMember]
        public string name { get; set; }

        [DataMember]
        public int age { get; set; }
        public void show()
        {
            Console.WriteLine("daw");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Person p = new Person { name = "Orest", age = 18 };
            MemoryStream ms = new MemoryStream();
            DataContractJsonSerializer contractJsonSerializer = new DataContractJsonSerializer(typeof(Person));
            contractJsonSerializer.WriteObject(ms, p);
            ms.Position = 0;

            StreamReader sr = new StreamReader(ms);
            Console.Write("JSON form of Person object: ");
            Console.WriteLine(sr.ReadToEnd());

        }
    }
}
