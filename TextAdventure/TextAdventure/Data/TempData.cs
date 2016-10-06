using System.Collections.Generic;
using TextAdventure.Model;

namespace TextAdventure.Data
{
    public static class TempData
    {
        private static readonly WorldMap LoadMap = new WorldMap();

        public static WorldMap Setup()
        {
            for (int i = 0; i < 3; i++)
            {
                LoadMap.Items.Add(new Item());
            }
            for (int i = 0; i < 4; i++)
            {
                LoadMap.Rooms.Add(new Room());
            }
            for (int i = 0; i < 6; i++)
            {
                LoadMap.Doors.Add(new Door());
            }
            SetProperties();
            SetMembers();
            return LoadMap;
        }

        private static void SetProperties()
        {
            LoadMap.Items[0].SetProperties("Room Key Handle", "This is half a key - maybe it would be a whole key if you used it on something?",true,1);
            LoadMap.Items[1].SetProperties("Room Key Shaft", "This is half a key - maybe it would be a whole key if you used it on something?", true,1);
            LoadMap.Items[2].SetProperties("Room Key", "This key opens a door. However, due to the morning rush, you tend to leave the keys in once used", true,0,1);

            LoadMap.Doors[0].SetProperties("Bedroom Door", "East", false);
            LoadMap.Doors[1].SetProperties("Bedroom Door", "West");
            LoadMap.Doors[2].SetProperties("Hallway Arch","North");
            LoadMap.Doors[3].SetProperties("Hallway Arch","South");
            LoadMap.Doors[4].SetProperties("Kitchen Door", "East", false);
            LoadMap.Doors[5].SetProperties("Kitchen Door", "West");

            LoadMap.Rooms[0].SetProperties("Your room", "The alarm clock beeps angrily as you wake. It's another morning, and the first order of business is to reach the kitchen for breakfast. You remember you locked the Bedroom Door.",
            "Your back in your room. You'll clean it, some other day. Let's just back out East for now.",
            "It's bright, and the walls are covered with posters. The Room Key hangs in a hook East, beside the Bedroom Door. Home sweet home!");
            LoadMap.Rooms[1].SetProperties("South Hallway",
            "You drag yourself out to the South Hallway. You praise your clever move to lock your door, and recall a similiar trap to the Kitchen Door as you find a Room Key Shaft on the floor. 'No thief shalt entereth MY Kitchen!', you think thriumphantly.",
            "Your back to the South Hallway.",
            "The hallway has Your Room to the West, the North Hallway is North.");
            LoadMap.Rooms[2].SetProperties("North Hallway",
            "You move north in the hallway. Your breakfast just past the Kitchen Door East, but it seems the cat has disassembled the key there. A firemap of your apartment adorns the wall, along with a Room Key Handle.",
            "The firemap stares at you, letting you know your back in the north end of the hallway.",
            "The hallway has the Kitchen Door to the East, the South Hallway is South.");
            LoadMap.Rooms[3].SetProperties("Kitchen",
            "You reach the kitchen after what feels like an adventure, and make your breakfast ready. Good morning!\nGame Won! Press Any key to exit game.",
            "You return to the kitchen, even though you was just here.",
            "You won!");
        }

        private static void SetMembers()
        {
            LoadMap.Doors[0].SetMembers(LoadMap, "South Hallway", "Room Key");
            LoadMap.Doors[1].SetMembers(LoadMap, "Your room");
            LoadMap.Doors[2].SetMembers(LoadMap, "North Hallway");
            LoadMap.Doors[3].SetMembers(LoadMap, "South Hallway");
            LoadMap.Doors[4].SetMembers(LoadMap, "Kitchen", "Room Key");
            LoadMap.Doors[5].SetMembers(LoadMap, "North Hallway");
            
            LoadMap.Rooms[0].SetMembers(LoadMap, new Dictionary<string, string> { { "East", "Bedroom Door" } }, new List<string> {"Room Key"});
            LoadMap.Rooms[1].SetMembers(LoadMap, new Dictionary<string, string> { { "West", "Bedroom Door" }, { "North", "Hallway Arch" } }, new List<string>{ "Room Key Shaft" });
            LoadMap.Rooms[2].SetMembers(LoadMap, new Dictionary<string, string> { { "East", "Kitchen Door" }, { "South", "Hallway Arch" } }, new List<string> { "Room Key Handle" });
            LoadMap.Rooms[3].SetMembers(LoadMap, new Dictionary<string, string> { { "West", "Kitchen Door" } });
        }
    }
}
