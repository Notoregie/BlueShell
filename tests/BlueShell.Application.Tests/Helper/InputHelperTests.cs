using BlueShell.Application.Helper;

namespace BlueShell.Application.Tests.Helper
{
    public class InputHelperTests
    {
        [Fact]
        public void TestAutoReadLine()
        {
            var commands = new List<string> { "system", "clear", "ipconfig" };
            string expectedValue = "system";
            string input = "s";

            string autoCompletedValue = InputHelper.GetNextAutoCompletedValue(input, commands);

            Assert.Equal(expectedValue, autoCompletedValue);
        }
    }
}
