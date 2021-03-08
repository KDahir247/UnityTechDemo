using MasterData;

namespace Tech.DB
{
    public interface IStream
    {
        DatabaseBuilder Builder { get; set; }
        public MemoryDatabase TryGetDatabase(FileDestination fileDestination);
        public string GetDatabaseFileName(FileDestination fileDestination);
    }
}