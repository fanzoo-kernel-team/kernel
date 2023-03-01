using Xunit;

namespace Fanzoo.Kernel.Tests
{
    public class ValidationTests
    {
        [Fact]
        public void Test_Can_Cast_Check_To_Bool()
        {
            var result = Check.For.NotNull<string?>(null);

            Assert.False(result);
        }
    }
}
