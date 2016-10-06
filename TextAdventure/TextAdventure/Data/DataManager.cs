using TextAdventure.Model;

namespace TextAdventure.Data
{
    class DataManager
    {
        //private string WorldPath { get; } = Constants.WorldConfigXml;
        
        //var WorldXml = XDocument.Load(WorldPath);

        //public void SaveWorld()
        //{
        //    XDocument SaveFile = XDocument.Load(WorldPath);
        //}
        public static WorldMap LoadWorld()
        {
            //XDocument saveFile = XDocument.Load(WorldPath
            return TempData.Setup();
        }
    }
}
