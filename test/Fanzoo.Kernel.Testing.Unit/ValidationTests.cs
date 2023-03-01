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

        [Fact]
        public void Test_Can_Cast_Bool_To_Check()
        {
            Check check = true;

            Assert.True(check.Result);

            //cannot operate on Check that's been created from bool cast
            Assert.Throws<InvalidOperationException>(() => check.IsNull<string?>(null));
        }
    }
}
