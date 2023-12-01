using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TheHouse
{
    public interface ITrigger
    {
    }

    public interface IGameEvent
    {
        public void Execute(Player player);
    }
    public interface IRoomDelegate //delegate setup for trap and echo room
    {
        public Room ContainingRoom { get; set; }
        public void RoomDidSetExit(string exitName, Door door);
        public Door RoomDidGetExit(string exitName, Door door);

        public string RoomDidGetExits(string exits);
        public string RoomDidGetDescription(string description);
    }
    public interface ICloseable // setup for door closing
    {
        public bool IsClosed { get; }

        public bool IsOpened { get; }

        bool Close();
        bool Open();
        bool CanClose { get; }
        bool CanOpen { get; }

    }

    public interface IKeyed //key interface for doors and keyed
    {
        bool HasKey { get; }
        IItem Insert(IItem key);
        IItem Remove();
    }


    public interface ILockable : IKeyed
    {
        bool IsLocked { get; }
        bool IsUnlocked { get; }
        bool Lock();
        bool Unlock();
        bool CanLock { get; }
        bool CanUnlock { get; }
        bool CanClose { get; }
        bool CanOpen { get; }
        IKeyed Keyed { set; get; }
    }

    public interface IArmor : IItem //extending armor from item
    {
        int ArmorValue { get; }
    }

    public interface IWeapon : IItem //extending weapon from item
    {
        int AttackValue { get; }
    }


    public interface IItem // item and decorator setup
    {
        string Name { get; }

        string LongName { get; }
        float Weight { get; }
        float Volume { get; }
        string Description { get; }

        bool isContainer { get; }

        void AddDecorator(IItem decorator);

    }

    public interface IItemContainer : IItem // extending itemcontainer from item
    {
        void Add(IItem item);


    }


    public interface ICombatant //making stats for player and enemies
    {
        String name { get; }
        int health { get; set; }

        int attack { get; }
        int defense { get; }
        int maxHealth { get; }
        //Dictionary<string, IItem> Equipment { get; set;}

    }

}