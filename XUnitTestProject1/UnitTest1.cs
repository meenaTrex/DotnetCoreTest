using dotnet.Controller;
using System;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            student home = new student();
            string result = home.GetEmployeeName(1);
            Assert.Equal("Jignesh", result);
        }
    }
}
