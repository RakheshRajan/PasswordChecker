using NUnit.Framework;
using PasswordStrengthChecker.BusinessLogic;
using PasswordStrengthChecker.BusinessLogic.Interface;
using PasswordStrengthChecker.BusinessLogic.Model;

namespace Tests
{
    public class Tests
    {
        IPasswordChecker objPasswordChecker;
        IPasswordBreachChecker objDataBreachChecker;
        PasswordRank pRank;

        private readonly string PWNEDApiURL = "https://haveibeenpwned.com/api/v3";
        private readonly string RangeApiURL = "https://api.pwnedpasswords.com";
        private readonly string UserAgent = "EcommerceTestPortal";
        private readonly string HIBPApiKey = "1f1e5caf16ac4a3c8572f301e38af583";

        [SetUp]
        public void Setup()
        {
            objPasswordChecker = new PasswordChecker();
            objDataBreachChecker = new DataBreachChecker(PWNEDApiURL,UserAgent, HIBPApiKey, RangeApiURL);
        }

        [Test]
        public void Test1()
        {
            pRank = objPasswordChecker.CheckStrength("1");
            if (pRank == PasswordRank.VeryWeak)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}