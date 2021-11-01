using LiteDB;
using NUnit.Framework;
using shortestpath_webapi.Repository;
using Moq;

namespace shortestpath_webapi_test
{
    [TestFixture]
    public class LiteDBRepositoryTests
    {
        private IDataRepository _liteDbRepo;
        private Mock<ILiteDatabase> _liteDatabaseMock;
        private Mock<ILiteCollection<LiteDbGraphNode>> _liteCollectionMock;

        [SetUp]
        public void Setup()
        {
            _liteDatabaseMock = new Mock<ILiteDatabase>();
            _liteCollectionMock = new Mock<ILiteCollection<LiteDbGraphNode>>();
            _liteDatabaseMock.Setup(a => a.GetCollection<LiteDbGraphNode>(It.IsAny<string>(), It.IsAny<BsonAutoId>()))
                .Returns(_liteCollectionMock.Object);
            
            
            _liteDbRepo = new LiteDbRepository(_liteDatabaseMock.Object);
        }

        [Theory]
        [TestCase('a', 8)]
        [TestCase('z', 1)]
        public void AfterSavingConnection_ShouldReturnUponRetrieval(char node, int connections)
        {
            _liteCollectionMock.Setup(x => x.FindOne(x => x.Id == node))
                .Returns(new LiteDbGraphNode(node){Connection = connections});
            _liteCollectionMock.Setup(x => x.Update(It.IsAny<LiteDbGraphNode>()));
            
            _liteDbRepo.SaveConnection(node,connections);

            var actual = _liteDbRepo.GetConnection(node);
            
            Assert.That(actual, Is.EqualTo(connections));
        } 
    }
}