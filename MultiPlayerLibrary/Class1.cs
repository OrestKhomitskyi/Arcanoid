using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayerLibrary
{
    [Serializable]
    public class Host
    {
        public TcpClient Creator { get; set; }
        public int RoomMaxAmount { get; set; }
        public string Name { get; set; }


        public override string ToString()
        {
            return Name;
        }
    }

    public enum MultiPlayerSignals { CREATE_HOST = 0x1234f, REMOVE_HOST = 0x4321f }
}
