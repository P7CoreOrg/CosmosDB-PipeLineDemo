using Xunit.DependencyInjection;

namespace XUnitTest_ToDo_Store
{
    internal class DependencyClass : IDependency
    {
        private readonly ITestOutputHelperAccessor _testOutputHelperAccessor;

        public DependencyClass(ITestOutputHelperAccessor testOutputHelperAccessor)
        {
            _testOutputHelperAccessor = testOutputHelperAccessor;
        }
        public int Value => 1;
    }
}
