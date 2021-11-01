namespace shortestpath_webapi.Repository
{
    public interface IDataRepository
    {
        public int[] GetAllConnections();
        public void SaveConnection(char key, int connection);

        public int GetConnection(char key);
    }
}