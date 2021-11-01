using System.Collections.Generic;
using shortestpath_webapi.Repository;

namespace shortestpath_webapi.Services
{
    
    public class GraphSearchService :IGraphSearchService
    {
        private readonly IDataRepository _repository;

        public GraphSearchService(IDataRepository repository)
        {
            _repository = repository;
        }
        
        public string ShortestPath(char @from, char to)
        {
            var bfsQueue = new Queue<int>();
            var visited = 0;

            var predecessors = new int[26];

            var fromIndex = Index(from);
            var toIndex = Index(to);

            for (var i = 0; i < 26; i++)
            {
                predecessors[i] = -1;
            }
            
            bfsQueue.Enqueue(fromIndex);

            while (bfsQueue.Count > 0)
            {
                var current = bfsQueue.Dequeue();

                if (current == toIndex)
                    break;

                var neighbors = GetNeighbors(current);
                foreach (var neighbor in neighbors)
                {
                    if (IsVisited(visited, neighbor)) continue;
                    predecessors[neighbor] = current;

                    if (neighbor == toIndex)
                    {
                        bfsQueue.Clear();
                        break;
                    }
                        
                    bfsQueue.Enqueue(neighbor);
                }

                visited = SetCurrentVisited(visited, current);
            }
            
            if(predecessors[toIndex] == -1) return "No Connection";

            var crawl = toIndex;
            var result = string.Empty;
            while (crawl >= 0)
            {
                result = string.IsNullOrEmpty(result)? Value(crawl).ToString() : $"{Value(crawl)},{result}";
                crawl = predecessors[crawl];
            }

            return result;

        }

        private static int SetCurrentVisited(int visited, int current)
        {
            var mask = 1 << current;
            return visited | mask;
        }

        private static bool IsVisited(int visited, int index)
        {
            var mask = 1 << index;
            return (visited & mask) != 0;
        }

        private IEnumerable<int> GetNeighbors(int current)
        {
            var connections = _repository.GetConnection(Value(current));

            return GetSetIndexes(connections);
        }

        private static IEnumerable<int> GetSetIndexes(int num)
        {
            var index = 0;
            var result = new List<int>();

            while (num != 0)
            {
                if((num & 1) == 1)
                    result.Add(index);

                num >>= 1;
                index++;
            }

            return result; 
        }

        public void AddConnection(char @from, char to)
        {
            var toIndex = Index(to);

            var current = _repository.GetConnection(from);
            var mask = 1 << toIndex;
            
            _repository.SaveConnection(from, current | mask);
        }

        private static int Index(char x)
        {
            return x - 'a';
        }

        private static char Value(int index)
        {
            return (char)('a' + index);
        }
    }

    public interface IGraphSearchService
    {
        public string ShortestPath(char from, char to);
        public void AddConnection(char from, char to);
    }
}