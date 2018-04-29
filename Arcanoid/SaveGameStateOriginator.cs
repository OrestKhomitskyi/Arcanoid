using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Arcanoid
{
    class SaveGameStateOriginator : IOriginator
    {
        public object GetMemento()
        {
            //Load
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream("save.dat", FileMode.Open))
            {
                object obj = bf.Deserialize(fs);
                return obj;
            }
        }

        public void SetMemento(object Memento)
        {
            //Save
            BinaryFormatter xs = new BinaryFormatter();

            using (var stream = File.Create("save.dat"))
            {
                xs.Serialize(stream, Memento);
            }

            //using (var stream = File.OpenRead("save.dat"))
            //{
            //    object obj= xs.Deserialize(stream);
            //}

            //using (TextWriter tw = new StreamWriter("save.xml"))
            //{
            //    xs.Serialize(tw, Memento);
            //}

            //using (TextReader tr =new StreamReader("save.xml"))
            //{
            //    GameSystemDataState obj= xs.Deserialize(tr) as GameSystemDataState;
            //}


            //using (FileStream fs = new FileStream("save", FileMode.Create))
            //{


            //    bf.Serialize(fs, Memento);
            //}
        }
    }
}
