using NUnit.Framework;
using shortestpath_webapi.Services;

namespace shortestpath_webapi_test
{
    [TestFixture]
    public class GraphSearchServiceTest
    {

        private IGraphSearchService _graphSearchService;

        [SetUp]
        public void Setup()
        {
            _graphSearchService = new GraphSearchService();
        }

        [Test]
        public void ABC_TEST()
        {
            _graphSearchService.AddConnection('a','b');
            _graphSearchService.AddConnection('b','c');

            var actual = _graphSearchService.ShortestPath('a', 'c');
            
            Assert.That(actual, Is.EqualTo("a,b,c"));
        }
        [Test]
        public void ABCD_TEST()
        {
            _graphSearchService.AddConnection('a','b');
            _graphSearchService.AddConnection('b','c');
            _graphSearchService.AddConnection('a','c');
            _graphSearchService.AddConnection('b','d');

            var actual = _graphSearchService.ShortestPath('a', 'd');
            
            Assert.That(actual, Is.EqualTo("a,b,d"));
        }
        
        [Test]
        public void NoConnection()
        {
            _graphSearchService.AddConnection('a','b');
            _graphSearchService.AddConnection('b','c');

            var actual = _graphSearchService.ShortestPath('a', 's');
            
            Assert.That(actual, Is.EqualTo("No Connection"));
        }
        

    }
}