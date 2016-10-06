using System.Collections.Generic;
using System.Linq;

namespace TextAdventure.Model
{
    public class Room: Object
    {
        public List<Item> Inventory { get; set; } = new List<Item>();
        public List<Door> Children { get; } = new List<Door>();
        private string FirstEntryMessage { get; set; } = string.Empty;
        private string ReturnEntryMessage { get; set; } = string.Empty;
        public string LookMessage { get; private set; } = string.Empty;
        private bool IsFirstEntry { get; set; } = true;

        public void SetProperties(string name, string firstMessage, string returnMessage, string look)
        {
            Name = name;
            FirstEntryMessage = firstMessage;
            ReturnEntryMessage = returnMessage;
            LookMessage = look;
        }

        public void SetMembers(WorldMap map, Dictionary<string, string> doorDirections, List<string> inventory = null)
        {
            if (inventory != null)
            {
                foreach (var item in inventory)
                {
                    var addItem = (from i in map.Items where i.Name.ToLower() == item.ToLower() select i).First();
                    Inventory.Add(addItem);
                }
            }

            foreach (var item in doorDirections)
            {
                var addDoor = (from i in map.Doors where i.GoDirection.ToLower().Equals(item.Key.ToLower()) select i).Intersect(from i in map.Doors where i.Name.ToLower().Equals(item.Value.ToLower()) select i).First();
                Children.Add(addDoor);
            }

        }

        public string GetRoomMessage()
        {
            return IsFirstEntry ? FirstEntryMessage : ReturnEntryMessage;
        }

        public void FlagVisit()
        {
            IsFirstEntry = false;
        }
    }
}