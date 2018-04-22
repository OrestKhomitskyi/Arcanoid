using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcanoid
{
    interface IOriginator
    {
        void SetMemento(object Memento);
        object GetMemento();
    }
}
