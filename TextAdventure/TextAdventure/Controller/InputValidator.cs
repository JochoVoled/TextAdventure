using System.Collections.Generic;
using System.Linq;
using TextAdventure.Extensions;

namespace TextAdventure.Controller
{
    public static class InputValidator
    {
        public static bool Validate(string command)
        {
            var wordList = command.Trim().ToLower().Split().ToList();
            switch (wordList[0])
            {
                case "get":
                    {
                        if (!HasObject(wordList)) return false;

                        var item = command.FindItemInRoom();
                        if (item != null) return true;

                        var getFailRoomEmpty = $"Could not get {command.Substring(command.IndexOf(' ')+1)}, as the room is empty.";
                        var getFailRoomNotEmpty = $"Could not get {wordList[1]}. Try getting any of {Player.Position.Inventory.ValuesToString()}instead.";
                        GameMaster.OutMessage = Player.Position.Inventory.Count > 0 ? getFailRoomNotEmpty : getFailRoomEmpty;
                        
                        return false;
                    }

                case "inspect":
                    {
                        if (!HasObject(wordList)) return false;

                        var item = command.FindItemInRoom();
                        if (item != null) return true;

                        item = command.FindItemInInventory();
                        if (item != null) return true;

                        var inspectFailBothEmpty =
                            $"Could not inspect item, as both your pockets and the room is empty";
                        var inspectFailRoomEmpty = $"Could not inspect item. The room is empty, but you find these in your pockets: {Player.Inventory.ValuesToString()}";
                        var inspectFailPocketEmpty = $"Could not inspect item. The room is empty, but you find these in your pockets: {Player.Position.Inventory.ValuesToString()}";
                        var inspectFailNeitherEmpty =
                            $"Could not inspect {wordList[1]}. You see these items in the room: {Player.Position.Inventory.ValuesToString()}\nYou also see these in your pockets {Player.Inventory.ValuesToString()}";
                        if (Player.Position.Inventory.Count != 0 && Player.Inventory.Count != 0)
                        {
                            GameMaster.OutMessage = inspectFailNeitherEmpty;
                        }
                        else if (Player.Position.Inventory.Count == 0)
                        {
                            GameMaster.OutMessage = inspectFailRoomEmpty;
                        }
                        else if (Player.Inventory.Count == 0)
                        {
                            GameMaster.OutMessage = inspectFailPocketEmpty;
                        }
                        else
                        {
                            GameMaster.OutMessage = inspectFailBothEmpty;
                        }
                        return false;
                    }

                case "use":
                    if (!HasObject(wordList)) return false;

                    if (command.Contains("Door") && command.Contains(" on "))
                    {
                        if (command.FindDoorInRoom() != null) return true;
                        GameMaster.OutMessage = $"Found no door with that name. Try using that item on {Player.Position.Children.ValuesToString()}instead";
                        return false;
                    }
                    if (!command.Contains("Door") && command.Contains(" on "))
                    {
                        if (command.FindItemsInInventory() != null) return true;
                        GameMaster.OutMessage = $"Could not use {wordList[1]}. Try using one of {Player.Inventory.ValuesToString()}instead.";
                        return false;
                    }
                    if (command.FindItemInInventory() == null)
                    {
                        GameMaster.OutMessage = $"Could not use {wordList[1]}, as you do not hold it. Try getting it, first.";
                        return false;
                    }
                    return true;

                case "drop":
                    if (!HasObject(wordList)) return false;

                    var dropItem = command.FindItemInInventory();
                    if (dropItem == null)
                    {
                        GameMaster.OutMessage = $"Could not drop {wordList[1]}. Try dropping one of {Player.Inventory.ValuesToString()}instead.";
                        return false;
                    }
                    return true;

                case "go":
                    if (!HasObject(wordList)) return false;
                    switch (wordList[1])
                    {
                        case "east":
                        case "north":
                        case "west":
                        case "south":
                            return true;
                        default:
                            GameMaster.OutMessage = wordList[1] + " is not a valid direction. Try North, South, West or East instead.";
                            return false;
                    }

                case "noclip":
                    GameMaster.NoClip = !GameMaster.NoClip;
                    const string noClipOnMsg = "You let your spirit leave your body for awhile (cheater)";
                    const string noClipOffMsg = "Your body, somehow, returned to where you were. Your brilliance is incomparible...";
                    GameMaster.OutMessage = GameMaster.NoClip ? noClipOnMsg : noClipOffMsg;
                    return false;
                case "exit":
                case "help":
                case "look":
                case "examine":
                case "inventory":
                case "pocket":
                    return true;
                default:
                    MessageError(wordList[0]);
                    return false;
            }
        }

        private static bool HasObject(IReadOnlyCollection<string> wordList)
        {
            if (wordList.Count != 1) return true;
            GameMaster.OutMessage = $"{wordList.First()} must be followed by an object";
            return false;
        }

        private static void MessageError(string word)
        {
            GameMaster.OutMessage = $"{word} is an unknown command. Type 'help' for known commands.";
        }
    }
}
