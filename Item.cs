using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheHouse
{
    public class Item : IItem, IArmor, IWeapon
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

        public bool IsArmor { get; private set; }
        public bool IsWeapon { get; private set; }

        public int ArmorValue { get; set; }
        public int AttackValue { get; set; }

        public Item(string name, float weight, float volume, bool isArmor, bool isWeapon) : this(name, weight, volume) //Item constructor
        {
            _name = name;
            _weight = weight;
            _volume = Volume;
            IsArmor = isArmor;
            IsWeapon = isWeapon;
        }
        public Item() : this("Nameless")
        {

        }
        public Item(string name) : this(name, 1f, 1f)
        {

        }

        public Item(string name, float weight, float volume) //Item constructor
        {
            _name = name;
            _volume = volume;
            _weight = weight;
            _decorator = null;
        }

        public void AddDecorator(IItem decorator) //Add decorator function
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


    public class ItemContainer : IItemContainer //Itemcontainer class
    {
        private string _name;
        private float _weight;
        private float _volume;
        private IItem _decorator;
        public Dictionary<string, IItem> _items;

        public string Name { get { return _name; } }

        public string LongName { get { return Name + (_decorator == null ? "" : " with " + _decorator.LongName); } }
        public float Weight //obtains weight of all items in container combined
        {
            get
            {
                Dictionary<string, IItem>.ValueCollection values = _items.Values;
                float totalContainedWeight = 0;
                foreach (IItem item in values)
                {
                    totalContainedWeight += item.Weight;
                }
                return _weight + totalContainedWeight + (_decorator == null ? 0 : _decorator.Weight);
            }
        }

        public float Volume // obtains volume of all items in container combined
        {
            get
            {
                Dictionary<string, IItem>.ValueCollection values = _items.Values;
                float totalContainedVolume = 0;
                foreach (IItem item in values)
                {
                    totalContainedVolume += item.Volume;
                }
                return _weight + totalContainedVolume + (_decorator == null ? 0 : _decorator.Weight);
            }
        }

        public string Description
        {
            get
            {
                string itemNames = "Items: ";
                Dictionary<string, IItem>.KeyCollection keys = _items.Keys;
                foreach (string itemName in keys)
                {
                    itemNames += " " + itemName;
                };
                return itemNames + ' ' + Weight;
            }
        }

        public bool isContainer { get { return true; } }


        public ItemContainer() : this("Nameless")
        {

        }
        public ItemContainer(string name) : this(name, 1f)
        {

        }

        public ItemContainer(string name, float weight)
        {
            _name = name;

            _weight = weight;
            _decorator = null;
            _items = new Dictionary<string, IItem>();
        }

        public void AddDecorator(IItem decorator)
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

        public void Add(IItem item)
        {
            if (item != null)
                _items.Add(item.Name, item);
        }

        public IItem Remove(string itemName)
        {
            IItem itemToRemove = null;
            _items.Remove(itemName, out itemToRemove);
            return itemToRemove;


        }
    }
}

