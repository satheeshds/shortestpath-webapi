using NUnit.Framework;
using shortestpath_webapi.Repository;

namespace shortestpath_webapi_test
{
    [TestFixture]
    public class InMemoryRepositoryTest
    {
        private IDataRepository _inMemoryRepo;

        [SetUp]
        public void Setup()
        {
            _inMemoryRepo = new InMemoryRepository();
        }

        [Theory]
        [TestCase('a', 8)]
        [TestCase('z', 1)]
        public void AfterSavingConnection_ShouldReturnUponRetrieval(char node, int connections)
        {
            _inMemoryRepo.SaveConnection(node,connections);

            var actual = _inMemoryRepo.GetConnection(node);
            
            Assert.That(actual, Is.EqualTo(connections));
        }
    }
}