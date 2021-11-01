using System;
using System.IO;
using LiteDB;

namespace shortestpath_webapi.Repository
{
    public class LiteDbRepository : IDataRepository
    {
        private readonly ILiteDatabase _database;
        private readonly ILiteCollection<LiteDbGraphNode> _graph;

        public LiteDbRepository(ILiteDatabase liteDatabase)
        {
            _database = liteDatabase;
            _graph = _database.GetCollection<LiteDbGraphNode>("GraphNodes");
        }

        ~LiteDbRepository()
        {
            _database.Commit();
            _database.Dispose();
        }

        public void SaveConnection(char key, int connection)
        {
            var node = _graph.FindOne(x => x.Id == key);
            if (node is null)
            {
                _graph.Insert(new LiteDbGraphNode(key) { Connection = connection });
                return;
            }
            node.Connection = connection;
            _graph.Update(node);
        }

        public int GetConnection(char key)
        {
            var node = _graph.FindOne(x => x.Id == key);
            return node?.Connection ?? 0;
        }
    }

    public class LiteDbGraphNode
    {
        public LiteDbGraphNode(char id)
        {
            Id = id;
        }

        public char Id { get; }
        public int Connection { get; set; }
    }
}