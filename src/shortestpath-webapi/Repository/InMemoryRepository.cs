using System.Collections.Generic;
using System.Linq;

namespace shortestpath_webapi.Repository
{
    public class InMemoryRepository : IDataRepository
    {
        private readonly Dictionary<char, int> _graph;

        public InMemoryRepository()
        {
            _graph = new Dictionary<char, int>();
            for (var i = 'a'; i <= 'z'; i++)
            {
                // Initializing all connections with zero, No connections in the graph
                _graph.Add(i, 0);
            }
        }

        public int[] GetAllConnections()
        {
            return _graph.OrderBy(x => x.Key).Select(x => x.Value).ToArray();
        }

        public void SaveConnection(char key, int connection)
        {
            _graph[key] = connection;
        }

        public int GetConnection(char key)
        {
            return _graph[key];
        }
    }
}