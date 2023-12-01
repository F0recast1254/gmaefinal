using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Numerics;

namespace TheHouse
{

    public class GameWorld
    {
        private static GameWorld _instance = null;

        public static GameWorld Instance //singleton
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameWorld();
                }
                return _instance;
            }
        }

        private Room _entrance;
        public Room Entrance { get { return _entrance; } }
        private Room _exit;


        private Dictionary<ITrigger, IGameEvent> _worldChanges;


        private GameWorld() 
        {
            _worldChanges = new Dictionary<ITrigger, IGameEvent>();
            _entrance = CreateWorld();
            NotificationCenter.Instance.AddObserver("PlayerWillEnterRoom", PlayerWillEnterRoom);
            NotificationCenter.Instance.AddObserver("PlayerDidEnterRoom", PlayerDidEnterRoom);

        }

        public void PlayerWillEnterRoom(Notification notification) //Observer/notification
        {
            Player player = (Player)notification.Object;
            if (player != null)
            {
                //player.WarningMessage("Player will leave " + player.CurrentRoom.Tag);
            }
        }

        public void PlayerDidEnterRoom(Notification notification) //Observer/notification
        {
            Player player = (Player)notification.Object;
            if (player != null)
            {
                if (player.CurrentRoom == _exit)
                    player.InfoMessage("You have completed the game!");

            }
            IGameEvent wc = null;
            _worldChanges.TryGetValue(player.CurrentRoom, out wc);

            if (wc != null)
            {
                wc.Execute(player);
            }
        }

        public Room CreateWorld() //World setup
        {
            Room foyer = new Room("in the foyer");
            Room kitchen = new Room("in the kitchen");
            Room livingroom = new Room("in a living area");
            Room diningroom = new Room("in a dining room");
            Room hallway = new Room("in the hallway");
            Room masterbedroom = new Room("in the master bedroom");
            Room bathroom = new Room("in the bathroom");
            Room closet = new Room("in the master bedroom closet");
            Room library = new Room("in a library");
            Room basement = new Room("in the basement");
            Room upstairs = new Room("in the upstairs hallway");
            Room guestbedroom = new Room("in what appears to be a guest bedroom");
            Room guestbathroom = new Room("in an upstairs bathroom");
            Room exit = new Room("out the front door and have now escaped, you hope you never have to return");


            Door door = Door.Connect(foyer, livingroom, "west", "east"); //Door setups


            door = Door.Connect(livingroom, hallway, "north", "south");


            door = Door.Connect(hallway, masterbedroom, "north", "south");


            door = Door.Connect(foyer, kitchen, "east", "west");


            door = Door.Connect(kitchen, diningroom, "north", "south");


            door = Door.Connect(masterbedroom, closet, "north", "south"); //placing enemy in room
            ICombatant enemy1 = new Enemy("spider", 15, 5, 3);
            masterbedroom.enemyDrop(enemy1);

            ICombatant enemy2 = new Enemy("flying book", 10, 6, 1);
            library.enemyDrop(enemy2);

            door = Door.Connect(masterbedroom, bathroom, "west", "east"); //using facade to lock door
            ILockable bathroomlock = LockableFacade.MakeLockable("Regular", "bathroomkey");
            door.Lockable = bathroomlock;
            door.Close();
            door.Lock();
            IItem key = door.Remove();
            kitchen.Drop(key);

            door = Door.Connect(diningroom, library, "north", "south");


            door = Door.Connect(library, basement, "north", "south");

            door = Door.Connect(foyer, exit, "south", "north");
            ILockable exitlock = LockableFacade.MakeLockable("Regular", "exitkey");
            door.Lockable = exitlock;
            door.Close();
            door.Lock();
            IItem exitkey = door.Remove();
            basement.Drop(exitkey);



            door = Door.Connect(hallway, diningroom, "east", "west");

            // inaccessible part of the world

            door = Door.Connect(foyer, upstairs, "north", "south");


            door = Door.Connect(upstairs, guestbedroom, "north", "south");


            door = Door.Connect(upstairs, guestbathroom, "west", "east");

            // generation of worldchange to unlock closet to basement door
            WorldChange wc = new WorldChange(closet, library, basement, "west", "east");
            _worldChanges[closet] = wc;

            IRoomDelegate basementtrap = new TrapRoom("basement"); //traproom
            basement.RoomDelegate = basementtrap;
            // tr.ContainingRoom = scct;

            IRoomDelegate guesttrap = new TrapRoom("guest");
            guestbedroom.RoomDelegate = guesttrap;
            //tr.ContainingRoom = parkingDeck;
            IRoomDelegate er = new EchoRoom(3);
            kitchen.RoomDelegate = er;

            IItem guestitem1 = new Item("iPad", 0.75f, 1f); // generation of ipad with cover decorator
            IItem decorator = new Item("cover", 0.25f, 1f);
            guestbedroom.Drop(guestitem1);
            guestitem1.AddDecorator(decorator);
            decorator = new Item("pen", 0.1f, 1f);
            guestitem1.AddDecorator(decorator);

            IWeapon knife = new Weapon("knife", 0.50f, 1f) //generation of weapon
            {
                AttackValue = 3
            };
            kitchen.Drop(knife);
            IArmor chefhat = new Armor("chefhat", 1.0f, 1f) //generation of armor
            {
                ArmorValue = 3
            };
            kitchen.Drop(chefhat);

            IWeapon gun = new Weapon("gun", 1.25f, 1f)
            {
                AttackValue = 6
            };
            masterbedroom.Drop(gun);

            IItem denimJacket = new Armor("denim jacket", 2.0f, 1f)
            {
                ArmorValue = 10
            };
            guestbedroom.Drop(denimJacket);

            IItem bathroomitem1 = new Item("toothbrush", 0.10f, 1f);
            bathroom.Drop(bathroomitem1);





            _exit = exit; //setup of wincon



            return foyer;
        }
    }


}