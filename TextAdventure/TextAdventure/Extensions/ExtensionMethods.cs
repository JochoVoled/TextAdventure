using System.Collections.Generic;
using System.Linq;
using TextAdventure.Controller;
using TextAdventure.Model;

namespace TextAdventure.Extensions
{
    public static class ExtensionMethods
    {
        public static string ValuesToString(this IEnumerable<Item> list)
        {
            return list.Aggregate("", (current, item) => current + $"{item.Name}, ");
        }
        public static string ValuesToString(this IEnumerable<Door> list)
        {
            return list.Aggregate("", (current, item) => current + $"{item.Name}, ");
        }
        public static List<Item> FindItemsInInventory(this string command)
        {
            var trimmedCommand = TrimCommand(command);
            var splitOnOn = SplitIfOn(trimmedCommand);
            
            return splitOnOn.Select(name => Player.Inventory.
                Find(x => x.Name.ToLower().Contains(name.ToLower()))).
                Where(item => item != null).
                ToList();
        }

        public static Item FindItemInInventory(this string command)
        {
            var trimmedCommand = TrimCommand(command);
            var splitOn = SplitIfOn(trimmedCommand);

            return Player.Inventory.Find(x => x.Name.ToLower().Contains(splitOn[0].ToLower()));
        }

        public static Item FindItemInRoom(this string command)
        {
            var trimmedCommand = TrimCommand(command);

            return Player.Position.Inventory.Find(x => x.Name.ToLower().Equals(trimmedCommand.ToLower()));
        }

        public static Door FindDoorInRoom(this string command)
        {
            var trimmedCommand = TrimCommand(command);
            var splitOnOn = SplitIfOn(trimmedCommand);

            return Player.Position.Children.Find(x => x.Name.ToLower().Equals(splitOnOn[1].ToLower()));
        }

        private static string TrimCommand(string command) => command.Remove(0, command.TrimStart().IndexOf(' ')).Trim();

        private static string[] SplitIfOn(string command) => command.Contains("on") ? command.Replace(" on ", "|").Split('|') : new[] { command , ""};
    }
}
