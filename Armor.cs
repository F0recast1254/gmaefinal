using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheHouse
{
    public class Armor : IItem, IArmor
    {
        private string _name;
        private float _weight;
        private float _volume;
        private IItem _decorator;

        public string Name { get { return _name; } }
        public string LongName { get { return Name + (_decorator == null ? "" : "with " + _decorator.LongName); } }
        public float Weight { get { return _weight + (_decorator == null ? 0 : _decorator.Weight); } }
        public float Volume { get { return _volume + (_decorator == null ? 0 : _decorator.Volume); } }
        public string Description { get { return LongName + ',' + Weight; } }
        public bool isContainer { get { return false; } }

        public int ArmorValue { get; set; }

        public Armor(string name, float weight, float volume) //armor constructor
        {
            _name = name;
            _volume = volume;
            _weight = weight;
            _decorator = null;
        }

        public void AddDecorator(IItem decorator) //decorator function
        {
            if (_decorator == null)
            {
                _decorator = decorator;
            }
            else
            {
                _decorator.AddDecorator(decorator);
            }
        }
    }
}

