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
            // Queue for breadth first search
            var bfsQueue = new Queue<int>();
            
            // Set all nodes are visited false (each bit in the integer represents whether the corresponding indexed node is visited or not)
            // By setting to zero, all nodes will be visited false. 
            var visited = 0;

            // Store predecessors in the the shortest path, so that we can recreate the path.
            var predecessors = new int[26];

            //Starting node index in 32 bit integer representation
            var fromIndex = Index(from);
            
            //Target node index in 32 bit integer representation
            var toIndex = Index(to);

            //Setting -1 for all nodes predecessors, we will have total 26 nodes, as 26 letters.
            for (var i = 0; i < 26; i++)
            {
                predecessors[i] = -1;
            }
            
            //Starting from initial node
            bfsQueue.Enqueue(fromIndex);

            //While all reachable node from start. 
            while (bfsQueue.Count > 0)
            {
                var current = bfsQueue.Dequeue();

                // Reached target node
                if (current == toIndex)
                    break;

                // Get all the connected nodes for the current node
                var neighbors = GetNeighbors(current);
                
                // Process each connected node
                foreach (var neighbor in neighbors)
                {
                    // Don't process already visited node
                    if (IsVisited(visited, neighbor)) continue;
                    
                    // Set the connected nodes predecessor as current
                    predecessors[neighbor] = current;

                    // Reached target
                    if (neighbor == toIndex)
                    {
                        bfsQueue.Clear();
                        break;
                    }
                        
                    // Add connected node to the BFS queue.
                    bfsQueue.Enqueue(neighbor);
                }

                // set the current node as visited.
                visited = SetCurrentVisited(visited, current);
            }
            
            // if the target nodes predecessor is -1, then starting node could not reach target
            if(predecessors[toIndex] == -1) return "No Connection";

            // Track back from target node to -1 in predecessor array, to construct path
            var crawl = toIndex;
            var result = string.Empty;
            while (crawl >= 0)
            {
                result = string.IsNullOrEmpty(result)? Value(crawl).ToString() : $"{Value(crawl)},{result}";
                crawl = predecessors[crawl];
            }

            return result;

        }

        /// <summary>
        /// Set the <paramref name="index"/> bit on <paramref name="visited"/> integer.
        /// </summary>
        /// <param name="visited">bits representing visited nodes</param>
        /// <param name="index">index of the bit which needs to be set</param>
        /// <returns>bits representing visited nodes after setting the indexed position.</returns>
        private static int SetCurrentVisited(int visited, int index)
        {
            var mask = 1 << index;
            return visited | mask;
        }

        /// <summary>
        /// Finds whether the bit at index is set on the given visited representation.
        /// </summary>
        /// <param name="visited">bits representing visited nodes</param>
        /// <param name="index">index of the bit which needs to be set</param>
        /// <returns>True, if the bit at index is set, otherwise false</returns>
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

        /// <summary>
        /// Add connection <paramref name="from"/> node to <paramref name="to"/> node in, adjacency matrix representation.
        /// </summary>
        /// <param name="from">The node which connection starts on directed graph.</param>
        /// <param name="to">The node which connection ends on directed graph. </param>
        public void AddConnection(char @from, char to)
        {
            // The index of target char in 26 char set
            var toIndex = Index(to);

            // Current connections of connection starting node.
            var current = _repository.GetConnection(from);
            
            // the mask which has 1 set on target char index.
            var mask = 1 << toIndex;
            
            // Modify the current connection to include the target char.
            current |= mask;
            
            // Save the modified connection
            _repository.SaveConnection(from, current);
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