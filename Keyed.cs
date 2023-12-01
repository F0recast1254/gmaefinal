using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheHouse
{
    public class Keyed : IKeyed
    {
        private IItem originalKey;
        private IItem insertedKey;

        public bool HasKey { get { return insertedKey == originalKey; } }


        public Keyed(string keyname) // key constructor
        {
            originalKey = new Item(keyname, 0.1f, 1f);
            insertedKey = originalKey;
        }

        public IItem Insert(IItem key) //insert key function
        {
            IItem oldkey = insertedKey;
            insertedKey = key;
            return oldkey;
        }

        public IItem Remove() //remove key function
        {
            IItem oldkey = insertedKey;
            insertedKey = null;
            return oldkey;
        }





    }
}