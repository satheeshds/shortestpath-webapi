using System.Collections.Generic;

namespace shortestpath_webapi.Services
{
    
    public class GraphSearchService :IGraphSearchService
    {
        private int[] _graph;

        public GraphSearchService()
        {
            _graph = new []
            {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0,0,0,0,0,0
            };
        }
        
        public string ShortestPath(char @from, char to)
        {
            Queue<int> bfsQueue = new Queue<int>();
            int visited = 0;

            int[] pred = new int[26];

            var fromIndex = Index(from);
            var toIndex = Index(to);

            for (int i = 0; i < 26; i++)
            {
                pred[i] = -1;
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
                    if (! IsVisited(visited, neighbor))
                    {
                        pred[neighbor] = current;

                        if (neighbor == toIndex)
                        {
                            bfsQueue.Clear();
                            break;
                        }
                        
                        bfsQueue.Enqueue(neighbor);
                    }
                }

                visited = SetCurrentVisited(visited, current);
            }
            
            if(pred[toIndex] == -1) return "No Connection";

            var crawl = toIndex;
            var result = string.Empty;
            while (crawl >= 0)
            {
                result = string.IsNullOrEmpty(result)? Value(crawl).ToString() : $"{Value(crawl)},{result}";
                crawl = pred[crawl];
            }

            return result;

        }

        private int SetCurrentVisited(int visited, int current)
        {
            var mask = 1 << current;
            return visited | mask;
        }

        private bool IsVisited(int visited, int index)
        {
            var mask = 1 << index;
            return (visited & mask) != 0;
        }

        private IList<int> GetNeighbors(int current)
        {
            
            var connections = _graph[current];

            return GetSetIndexes(connections);
        }

        private IList<int> GetSetIndexes(int num)
        {
            int index = 0;
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
            var fromIndex = Index(from);
            var toIndex = Index(to);

            var current = _graph[fromIndex];
            var mask = 1 << toIndex;
            _graph[fromIndex] = current | mask;
        }

        private int Index(char x)
        {
            return x - 'a';
        }

        private char Value(int index)
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