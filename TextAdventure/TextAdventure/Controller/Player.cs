using System.Collections.Generic;
using System.Linq;
using TextAdventure.Model;

namespace TextAdventure.Controller
{
    public static class Player
    {
        public static string Name { get; set; } = "John Doe";
        public static Room Position { get; set; } = new Room();
        public static List<Item> Inventory { get; } = new List<Item>();

        public static void Go(string direction)
        {
            var door = Position.Children.Find(x => x.GoDirection.ToLower().Equals(direction.ToLower()));
            if (door != null)
            {
                if (door.IsOpen || GameMaster.NoClip)
                {
                    Position.FlagVisit();
                    Position = door.EndPoint;
                    GameMaster.OutMessage = Position.GetRoomMessage();
                }
                else
                {
                    GameMaster.OutMessage = "You try and open the door, but the door is locked.";
                }
            }
            else
            {
                GameMaster.OutMessage =
                    $"You move {direction}, and face off with a wall. Maybe you should try some other direction?";
            }
        }
        public static void Look()
        {
            GameMaster.OutMessage = Position.Inventory.Aggregate(Position.LookMessage, (current, item) => current + $"\nThere is a {item.Name} in this room");
        }
        public static void Get(Item item)
        {
            if (item.IsMobile)
            {
                Position.Inventory.Remove(item);
                Inventory.Add(item);
                GameMaster.OutMessage = $"You picked up {item.Name}.";
            }
            else
            {
                GameMaster.OutMessage = $"You consider moving {item.Name}, but conclude it's good where it is.";
            }
        }
        public static void Drop(Item item)
        {
            Position.Inventory.Add(item);
            Inventory.Remove(item);
            GameMaster.OutMessage = $"You dropped {item.Name}.";
        }

        public static void Use(Item item)
        {
            GameMaster.OutMessage = $"You used {item.Name}. You still have it, by the way!";
        }

        public static void Use(Item item1, Item item2)
        {
            if (item1.CombinePair == item2.CombinePair && item1.CombinePair != 0 && item2.CombinePair != 0 && item1 != item2)
            {
                var combinedItem = GameMaster.Map.Items.Find(x => x.CombineId == item1.CombinePair);
                Inventory.Add(combinedItem);
                Inventory.Remove(item1);
                Inventory.Remove(item2);
                GameMaster.OutMessage = $"You combined {item1.Name} and {item2.Name} to make a {combinedItem.Name}!";
            }else if (item1 == item2)
            {
                GameMaster.OutMessage = $"An item cannot be combined with itself.";
            }
            else
            {
                GameMaster.OutMessage = $"After contemplating the prospect of {item1.Name} and {item2.Name} together, you decide to postpone that train of thought.";
            }
        }

        public static void Use(Item key, Door door)
        {
            if (door.Key == key)
            {
                door.Unlock(key);
                GameMaster.OutMessage = $"You used {key.Name} to unlock {door.Name}.";
            }
            else
            {
                GameMaster.OutMessage = $"You try unlocking the {door.Name} with a {key.Name}, but it doesn't open. Stupid door...";
            }
        }
        public static void Inspect(Item item)
        {
            GameMaster.OutMessage = item.Description;
        }
    }
}
