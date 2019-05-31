using System;
using System.Threading.Tasks;
using CosmosDB.ToDo.Store.Interfaces;
using Shouldly;
using Xunit;
using XUnitHelpers.TestCaseOrdering;


namespace XUnitTest_ToDo_Store
{
    [TestCaseOrderer("XUnitTest_ToDo_Store.TestCaseOrdering.PriorityOrderer", "XUnitTest_ToDo_Store")]
    public class MyAwesomeTests
    {
        private readonly IDependency _d;
        private ISimpleItemDbContext<Item> _simpleItemDbContext;
        private string _key { get; set; }
        private string _clientId { get; set; }
        public MyAwesomeTests(ISimpleItemDbContext<Item> simpleItemDbContext,
            IDependency d)
        {
            _d = d;
            _simpleItemDbContext = simpleItemDbContext;
            _key = "__the_key";
            _clientId = "__the_client";

        }
        string NewGuidS => Guid.NewGuid().ToString();
        [Fact]
        public void AssertThatWeDoStuff()
        {
            Assert.Equal(1, _d.Value);
        }
        
        [Fact, TestPriority(0)]
        public async Task success_persist()
        {
            _simpleItemDbContext.ShouldNotBeNull();
        }
       
    }

}
