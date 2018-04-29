using System.Collections.Generic;

namespace Arcanoid.Patterns.Mediator
{
    class MyMediator : Mediator
    {
        public List<Colleague> Colleagues { get; set; }


        public MyMediator(string msg, Colleague Colleague)
        {

        }

        public override void Send(string msg, Colleague Colleague)
        {
            foreach (Colleague colleague in this.Colleagues)
            {
                if (Colleague != colleague)
                    colleague.Notify(msg);
            }
        }
    }
}