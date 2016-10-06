namespace TextAdventure.Model
{
    public class Item: Object
    {
        public string Description { get; private set; } = string.Empty;
        public bool IsMobile { get; private set; } = false;
        public int CombinePair { get; private set; } = 0;
        public int CombineId { get; set; } = 0;

        public void SetProperties(string name, string description, bool isMobile = false, int combinePair = 0, int combineId = 0)
        {
            Name = name;
            Description = description;
            IsMobile = isMobile;
            CombinePair = combinePair;
            CombineId = combineId;
        }
    }
}