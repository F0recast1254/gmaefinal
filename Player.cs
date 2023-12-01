using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Numerics;

namespace TheHouse
{
    /*
     * Spring 2023
     */
    public class Player : ICombatant
    {

        public Stack<Room> _rooms = new Stack<Room>(); //generation of stack for Back()
        private Room _currentRoom = null;
        public Room CurrentRoom { get { return _currentRoom; } set { _currentRoom = value; } }
        private List<IItem> _inventory; //List containing inventory
        private ICombatant characterinfo; // stats list
        private IWeapon _equippedWeapon; // equipped weapon info
        private IArmor _equippedArmor; // equipped armor info
        public bool CanEquip()
        {
            return _hand == null || (_hand is IWeapon) || (_hand is IArmor);
        }
        public String name { get; set; }
        public int health { get; set; }
        public float maxweight { get; set; }
        public float currentweight { get; set; }
        public int attack { get; set; }

        public int defense { get; set; }
        public float maxvolume { get; set; }
        public float currentvolume { get; set; }

        public int maxHealth { get; set; }
        public void Levelup() //Levelup function, picks a random number and updates stats according to the number chosen
        {
            int randomnumber;
            Random rng = new Random();
            for (int i = 0; i < 3; i++)
            {
                randomnumber = rng.Next(1, 3);
                switch (randomnumber)
                {
                    case 1:
                        maxHealth += 10;
                        break;
                    case 2:
                        attack += 5;
                        break;
                    case 3:
                        defense += 5;
                        break;

                }
            }
            InfoMessage("You leveled up!\nPlayer stats:\nMax Health: " + maxHealth + "\nAttack: " + attack + "\nDefense: " + defense + "\n");

        }

        public void fight(String enemy) //Fight function, player swings at enemy, enemy automatically swings back. Player can leave the room any time
        {
            ICombatant target = CurrentRoom.Enemygrab(enemy);
            if (target != null)
            {
                int output = attack - target.defense;
                if (output > 0)
                {
                    target.health = target.health - output;
                }
                else
                {
                    output = 1;
                    target.health = target.health - output;
                }
                if (target.health < 0)
                {
                    InfoMessage(target.name + " has been defeated!");
                    CurrentRoom.Pickup(enemy);
                    Levelup();
                    return;
                }
                InfoMessage("You dealt " + output + " damage to " + target.name + "\nEnemy health remaining: " + target.health);
                int input = target.attack - defense;
                if (input > 0)
                {
                    health = health - input;
                }
                else
                {
                    input = 1;
                    health = health - input;
                }
                if (health < 0)
                {
                    WarningMessage("You were defeated by: " + target.name);
                    health = maxHealth;
                    Back();
                    InfoMessage("After being defeated, you woke up in the last room you were in, rested and ready to continue.");
                    return;
                }
                WarningMessage("You recieved " + input + " damage from " + target.name + "\nYour health remaining: " + health + "/" + maxHealth);

            }
            else
            {
                InfoMessage("There is no enemy");
            }
        }

        public void engage(String enemy) //Command to display enemy stats for player
        {
            ICombatant opponent = CurrentRoom.Enemygrab(enemy);
            if (opponent != null)
            {
                InfoMessage("Engaging enemy named " + opponent.name);
                InfoMessage("Enemy health remaining: " + opponent.health + "\nEnemy attack: " + opponent.attack + "\nEnemy defense: " + opponent.defense);
            }
            else
            {
                InfoMessage("Enemy not found.");
            }
        }



        private IItem _hand; // hand for player to pickup items

        public Player(Room room) // player constructor
        {
            _currentRoom = room;
            _hand = null;
            maxHealth = 10;
            health = 10;
            attack = 5;
            defense = 5;
            maxweight = 100f;
            currentweight = 0f;
            currentvolume = 0f;
            maxvolume = 50f;
            ItemContainer _inventory = new ItemContainer("inventory", 0f);

        }
        public void Back() // back function, uses stack to move to prior room
        {

            CurrentRoom = _rooms.Pop();
            Notification notification = new Notification("PlayerDidEnterRoom", this);
            NotificationCenter.Instance.PostNotification(notification);
            NormalMessage("\n" + this.CurrentRoom.Description());
        }

        private bool CanPickup()
        {
            return _hand == null;
        }
        public void Pickup(string itemName) // pickup function
        {
            if (!CanPickup())
            {
                WarningMessage("Your hands are full. Store or use the " + _hand.Name + " in your hand first.");
                return;
            }

            IItem item = CurrentRoom.Pickup(itemName);
            if (item != null)
            {
                _hand = item;
                InfoMessage(_hand.Name + " has been added to your hand.");
            }
            else
            {
                WarningMessage("There is no item with this name in the room.");
            }
        }

        public void Store(string itemName) // function to store item in inventory
        {

            if (_equippedWeapon != null && _equippedWeapon.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
            {
                if (_equippedWeapon.Weight + currentweight <= maxweight)
                {
                    if (_equippedWeapon.Volume + currentvolume <= maxvolume)
                    {
                        InfoMessage($"{_equippedWeapon.Name} is now in your backpack.");
                        currentvolume += _equippedWeapon.Volume;
                        currentweight += _equippedWeapon.Weight;
                        _inventory.Add(_equippedWeapon);
                        _equippedWeapon = null;
                    }
                }
            }

            else if (_equippedArmor != null && _equippedArmor.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
            {
                if (_equippedArmor.Weight + currentweight <= maxweight)
                {
                    if (_equippedArmor.Volume + currentvolume <= maxvolume)
                    {
                        InfoMessage($"{_equippedArmor.Name} is now in your backpack.");
                        currentvolume += _equippedArmor.Volume;
                        currentweight += _equippedArmor.Weight;
                        _inventory.Add(_equippedArmor);
                        _equippedArmor = null;
                    }
                }
            }

            else if (_hand != null && _hand.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
            {
                if (_hand.Weight + currentweight <= maxweight)
                {
                    if (_hand.Volume + currentvolume <= maxvolume)
                    {
                        InfoMessage($"{_hand.Name} is now in your backpack.");
                        currentvolume += _hand.Volume;
                        currentweight += _hand.Weight;
                        _hand = null;
                    }
                }
            }
            else
            {
                WarningMessage("The item is either not on your person, or the item is too heavy or big to fit in your backpack.");
            }
        }



        public void DisplayInventory() // shows inventory
        {
            Console.WriteLine(" ");
            InfoMessage("Weight: " + currentweight);
            InfoMessage("Volume: " + currentvolume);

            InfoMessage("Weapon Slot:");
            if (_equippedWeapon != null)
            {
                InfoMessage(_equippedWeapon.Name);
            }
            else
            {
                InfoMessage("Empty");
            }

            InfoMessage("Armor Slot:");
            if (_equippedArmor != null)
            {
                InfoMessage(_equippedArmor.Name);
            }
            else
            {
                InfoMessage("Empty");
            }

            InfoMessage("\nBackpack Items:");
            if (_inventory != null)
            {
                foreach (IItem item in _inventory)
                {
                    InfoMessage(item.Name);
                }
            }
            else
            {
                InfoMessage("Your backpack is empty.");
            }
        }


        public void Retrieve(string itemName) // retrieves item from inventory
        {
            IItem itemInBackpack = _inventory.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));

            if (itemInBackpack == null)
            {
                WarningMessage(itemName + " is not in your backpack.");
                return;
            }

            if (!CanPickup())
            {
                WarningMessage("Your hands are full. Store or use the " + _hand.Name + " in your hand first.");
                return;
            }

            _hand = itemInBackpack;
            currentvolume -= itemInBackpack.Volume;
            currentweight -= itemInBackpack.Weight;
            _inventory.Remove(itemInBackpack);
            InfoMessage(_hand.Name + " is now in your hands.");
        }

        public void Equip(string itemName) // equips item from hand or backpack to armor or weapon slot
        {
            IItem itemInHand = _hand;

            if (itemInHand == null)
            {
                EquipFromBackpack(itemName);

            }

            else if (itemInHand is IArmor)
            {
                EquipArmor(itemInHand as IArmor);

            }
            else if (itemInHand is IWeapon)
            {
                EquipWeapon(itemInHand as IWeapon);

            }
            else
            {
                WarningMessage("This item cannot be equipped.");
            }
        }

        private void EquipWeapon(IWeapon weapon) // weapon equip function
        {
            if (_equippedWeapon == null)
            {
                _equippedWeapon = weapon;
                InfoMessage($"Equipped {weapon.Name} as a weapon.");
                attack += _equippedWeapon.AttackValue;
                _hand = null;
            }
            else
            {
                WarningMessage("You already have a weapon equipped. Unequip it or store it first.");
            }
        }

        private void EquipArmor(IArmor armor) // armor equip function
        {
            if (_equippedArmor == null)
            {
                _equippedArmor = armor;
                InfoMessage($"Equipped {armor.Name} as armor.");
                defense += _equippedArmor.ArmorValue;
                _hand = null;
            }
            else
            {
                WarningMessage("You already have armor equipped. Unequip it first.");
            }
        }

        private void EquipFromBackpack(string itemName) //equip from backpack function
        {
            IItem itemInBackpack = _inventory.FirstOrDefault(i =>
                i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase) &&
                (i is IWeapon || i is IArmor));

            if (itemInBackpack != null)
            {
                _inventory.Remove(itemInBackpack);

                if (itemInBackpack is IWeapon)
                {
                    currentweight -= itemInBackpack.Weight;
                    currentvolume -= itemInBackpack.Volume;
                    EquipWeapon(itemInBackpack as IWeapon);

                }
                else if (itemInBackpack is IArmor)
                {
                    currentweight -= itemInBackpack.Weight;
                    currentvolume -= itemInBackpack.Volume;
                    EquipArmor(itemInBackpack as IArmor);
                }
                else
                {
                    WarningMessage($"Cannot equip {itemInBackpack.Name}. The item is neither a weapon nor armor.");
                    _inventory.Add(itemInBackpack);
                }
            }
            else
            {
                WarningMessage($"No suitable item named {itemName} found in your backpack.");
            }
        }


        public void Unequip(string itemName) //unequip function
        {
            if (_hand != null)
            {
                WarningMessage("Your hands are not empty. Unequip the item in your hand first.");
                return;
            }

            if (_equippedWeapon != null && _equippedWeapon.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
            {
                UnequipWeapon();
            }
            else if (_equippedArmor != null && _equippedArmor.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
            {
                UnequipArmor();
            }
            else
            {
                WarningMessage($"You are not currently equipped with {itemName}. Cannot unequip.");
            }
        }

        private void UnequipWeapon()
        {
            if (_equippedWeapon != null)
            {
                _hand = _equippedWeapon;
                InfoMessage($"Unequipped {_equippedWeapon.Name} and returned it to your hand.");
                attack -= _equippedWeapon.AttackValue;
                _equippedWeapon = null; // Reset the equipped weapon
            }
        }

        private void UnequipArmor()
        {
            if (_equippedArmor != null)
            {
                _hand = _equippedArmor;
                InfoMessage($"Unequipped {_equippedArmor.Name} and returned it to your hand.");
                defense -= _equippedArmor.ArmorValue;
                _equippedArmor = null; // Reset the equipped armor
            }
        }


        public void Insert(string exitname) //insert to door function
        {
            Door door = CurrentRoom.GetExit(exitname);
            if (door != null)
            {
                if (_hand != null)
                {
                    door.Insert(_hand);
                    InfoMessage("You inserted " + _hand.Name + " into the door " + exitname);
                }
                else
                {
                    WarningMessage("You aren't holding anything!");
                }
            }
            else
            {
                WarningMessage("There is no door on " + exitname);
            }
        }

        public void Walkto(string direction) //walk function
        {
            Door door = this.CurrentRoom.GetExit(direction);
            if (door != null)
            {
                if (door.IsOpened)
                {
                    Notification notification = new Notification("PlayerWillEnterRoom", this);
                    NotificationCenter.Instance.PostNotification(notification);
                    _rooms.Push(CurrentRoom);
                    CurrentRoom = door.RoomOnTheOtherSide(CurrentRoom);
                    notification = new Notification("PlayerDidEnterRoom", this);
                    NotificationCenter.Instance.PostNotification(notification);
                    NormalMessage("\n" + this.CurrentRoom.Description());
                }
                else
                {
                    WarningMessage("\nThe door on " + direction + " is closed.");
                }
            }
            else
            {
                WarningMessage("\n There is no door on " + direction);
            }

        }
        public void Open(string direction)// door opening function
        {
            Door door = this.CurrentRoom.GetExit(direction);
            if (door != null)
            {

                if (door.IsClosed)
                {
                    if (door.Open())
                    {
                        InfoMessage("The door on " + direction + " is now open.");
                    }
                    else
                    {
                        InfoMessage("The door on " + direction + " did not open.");
                    }
                }
                else
                {
                    InfoMessage("The door on " + direction + " is already open.");
                }
            }
            else
            {
                ErrorMessage("\nThere is no door on" + direction);
            }
        }
        public void OutputMessage(string message)
        {
            Console.WriteLine(message);
        }
        public void Unlock(string direction) //door unlock function
        {
            Door door = this.CurrentRoom.GetExit(direction);
            if (door != null)
            {
                if (door.IsLocked)
                {
                    if (door.Unlock())
                    {
                        InfoMessage("The door on " + direction + " is now unlocked (Remember to open!).");
                    }
                    else
                    {
                        InfoMessage("The door on " + direction + " did not unlock");
                    }
                }
            }
        }
        public void Shout(string word) // shout function
        {
            NormalMessage("<<<" + word + ">>>");
            Dictionary<string, object> userInfo = new Dictionary<string, object>();
            userInfo["word"] = word;
            Notification notification = new Notification("PlayerDidShoutAWord", this, userInfo);
            NotificationCenter.Instance.PostNotification(notification);
            InfoMessage("The player shouts.");
        }
        public void ColoredMessage(string message, ConsoleColor newColor)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = newColor;
            OutputMessage(message);
            Console.ForegroundColor = oldColor;
        }

        public void Inspect(string itemName) //item inspection function
        {
            IItem item = CurrentRoom.Pickup(itemName);
            if (item != null)
            {
                InfoMessage("Item description:  " + item.Description);
                CurrentRoom.Drop(item);
            }
            else
            {
                WarningMessage("There is no item named: " + itemName + " in the room.");
            }
        }
        public void NormalMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.White);
        }

        public void InfoMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.Blue);
        }

        public void WarningMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.DarkYellow);
        }

        public void ErrorMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.Red);
        }
    }

}