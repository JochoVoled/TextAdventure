using System.Collections.Generic;

namespace TextAdventure.Model
{
    public class WorldMap
    {
        public List<Room> Rooms { get; set; } = new List<Room>();
        public List<Item> Items { get; set; } = new List<Item>();
        public List<Door> Doors { get; set; } = new List<Door>();
        public Room VictoryLocation { get; set; } = new Room();

        public Room Find(string name)
        {
            return Rooms.Find(x => x.Name == name);
        }
    }
}
