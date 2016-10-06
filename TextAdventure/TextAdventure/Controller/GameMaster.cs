using System;
using System.Linq;
using TextAdventure.Data;
using TextAdventure.Extensions;
using TextAdventure.Model;

namespace TextAdventure.Controller
{
    public static class GameMaster
    {
        public static string OutMessage { private get; set; } = Player.Position.GetRoomMessage();
        private static bool IsRunning { get; set; } = true;
        public static bool NoClip { get; set; }
        public static WorldMap Map { get; private set; }

        public static void GameLoop()
        {
            SetupGame();
            SetupCharacter();
            while (IsRunning)
            {
                Console.Clear();
                DisplayOutMessage();
                if (Player.Position == Map.VictoryLocation)
                {
                    Console.ReadKey();
                    break;
                }
                var command = Console.ReadLine();
                if (InputValidator.Validate(command))
                {
                    Execute(command);
                }
            }
        }

        private static void Execute(string command)
        {
            var firstWord = command;
            if (command.Trim().Contains(' '))
            {
                firstWord = command.Remove(command.IndexOf(' '));
            }
            switch (firstWord.ToLower())
            {
                case "use":
                    if (command.ToLower().Contains("on"))
                    {
                        if (command.ToLower().Contains("door"))
                        {
                            var item = command.FindItemInInventory();
                            var onDoor = command.FindDoorInRoom();
                            if (onDoor != null && item != null)
                            {
                                if (!onDoor.IsOpen)
                                {
                                    Player.Use(item, onDoor);
                                    Player.Inventory.Remove(item);
                                }
                                else
                                {
                                    OutMessage = $"That won't be necessary, as the door is already open.";
                                }
                                
                            }else if (onDoor == null)
                            {
                                var openDoorFailRoomHasNoDoors = $"It seems this room doesn't have any doors. Oh No, you're trapped!";
                                var openDoorFailNoDoorThatWay = Player.Position.Children.Aggregate("No door has that name. Try any of these:", (current, door) => current + $"\n{door.Name}, ");;
                                OutMessage = Player.Position.Children.Count > 0 ? openDoorFailNoDoorThatWay : openDoorFailRoomHasNoDoors;
                                
                            }
                        }
                        else
                        {
                            var items = command.FindItemsInInventory();
                            if (items.All(x => x != null))
                            {
                                Player.Use(items.First(), items.Last());
                            }
                            else
                            {
                                OutMessage = $"You can't use {items.First()} and {items.Last()} together, it seems.";
                            }
                        }
                    }
                    else
                    {
                        var item = command.FindItemInInventory();
                        Player.Use(item);
                    }
                    break;
                case "get":
                    var getItem = command.FindItemInRoom();
                    Player.Get(getItem);
                    break;
                case "inspect":
                    //var roomItem = command.FindItemInRoom();
                    //var pocketsItem = command.FindItemInInventory();
                    //var inspectItem = new Item();
                    //inspectItem = roomItem ?? pocketsItem;
                    Player.Inspect(command.FindItemInRoom() ?? command.FindItemInInventory());
                    break;
                case "drop":
                    var dropItem = command.FindItemInInventory();
                    Player.Drop(dropItem);
                    break;
                case "go":
                    Player.Go(command.Substring(2).TrimStart());
                    break;
                case "look":
                case "examine":
                    Player.Look();
                    break;
                case "exit":
                    EndGame();
                    break;
                case "help":
                    OutMessage = "Write on the format [command] [things].\nType Go [direction] to move between rooms.\nGet [item] in the room to pick them up, or Inspect [item] to learn more about it.\nDrop [item] in your inventory to leave it, or Use [item] to use them.\nItems can be merged with others, using Use [item] on [item].\nUnlock doors by Use [item] on [door].\nLastly, exit the game by typing Exit.";
                    break;
                case "pocket":
                case "inventory":
                    string checkFailInventoryEmpty = "You look into your pocket, and the empty pocket looks back into you.";
                    string checkFailInventoryHasItems = Player.Inventory.Aggregate("You look into your pocket, and find:\n", (current, item) => current + $"{item.Name}\n");
                    OutMessage = Player.Inventory.Count > 0 ? checkFailInventoryHasItems : checkFailInventoryEmpty;
                    break;
                default:
                    OutMessage = "Unaccepted command. Type 'help' for a list of accepted commands.";
                    break;
            }
        }

        private static void DisplayOutMessage()
        {
            Console.WriteLine(OutMessage);
            OutMessage = "";
        }

        private static void EndGame()
        {
            IsRunning = false;
        }

        private static void SetupGame()
        {
            Map = DataManager.LoadWorld();
            if (Map.Rooms != null)
            {
                Player.Position = Map.Find("Your room");
                Map.VictoryLocation = Map.Find("Kitchen");
            }
            OutMessage = Player.Position.GetRoomMessage();
        }

        private static void SetupCharacter()
        {
            Console.WriteLine("Welcome to this game, please write your character name");
            Player.Name = Console.ReadLine();
        }
    }
}
