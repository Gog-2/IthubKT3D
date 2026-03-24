public struct itemsData
{
   public itemsData( int spriteid ,string name, string description)
   {
      _name = name;
      _description = description;
      imageId = spriteid;
   }
   private readonly int imageId;
   private readonly string _name;
   private readonly string _description;
   public string Name => _name; 
   public string Description => _description;
   public int SpriteId => imageId;
}
