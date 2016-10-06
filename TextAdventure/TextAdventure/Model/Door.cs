namespace TextAdventure.Model
{
    public class Door: Object
    {
        public string GoDirection { get; private set; }
        public Room EndPoint { get; private set; } = new Room();
        public bool IsOpen { get; private set; }
        public Item Key { get; private set; } = new Item();

        
        public void SetProperties(string name, string direction, bool isOpen = true)
        {
            Name = name;
            GoDirection = direction;
            IsOpen = isOpen;
        }
        public void SetMembers(WorldMap map, string endPoint, string key=null)
        {
            EndPoint = map.Rooms.Find(x => x.Name.Equals(endPoint));
            if (key != null)
            {
                Key = map.Items.Find(x => x.Name.Equals(key));
            }
        }

        public void Unlock(Item key)
        {
            if (key == Key)
            {
                IsOpen = true;
            }
        }
    }
}