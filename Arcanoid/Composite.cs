using System;
using System.Collections.Generic;

namespace Arcanoid
{
    class Composite : Component
    {
        private List<Component> Children = new List<Component>();

        public Composite(string name) : base(name)
        {

        }

        public void Add(Component Component)
        {
            Children.Add(Component);
        }

        public void Remove(Component Component)
        {
            Children.Remove(Component);
        }

        public override void Show()
        {
            foreach (var VARIABLE in Children)
            {
                Console.WriteLine(VARIABLE.ToString());
            }
        }
    }

    abstract class Component
    {
        protected string name;
        public Component(string name)
        {
            this.name = name;
        }
        public abstract void Show();

    }

    class Derive : Component
    {
        public override void Show()
        {

        }

        public Derive(string name) : base(name)
        {
        }
    }

    class SomeMain
    {
        public static void DO()
        {
            Composite composite = new Composite("ROOT");
            composite.Add(new Derive("Hello"));
            Component leaf = new Derive("awdaw");
            composite.Add(leaf);
            Composite sss = new Composite("leaf");
            sss.Add(composite);

        }
    }

}
