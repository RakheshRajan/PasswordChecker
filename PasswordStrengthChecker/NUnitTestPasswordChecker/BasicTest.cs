using NUnit.Framework;
using PasswordStrengthChecker.BusinessLogic;
using PasswordStrengthChecker.BusinessLogic.Interface;
using PasswordStrengthChecker.BusinessLogic.Model;
using System.Threading.Tasks;

namespace Tests
{
    public class Tests
    {
        IPasswordCheckerHandler objPasswordChecker;
        IPasswordBreachChecker objDataBreachChecker;
        PasswordRank pRank;
        int score = 0;

        private readonly string PWNEDApiURL = "https://haveibeenpwned.com/api/v3";
        private readonly string RangeApiURL = "https://api.pwnedpasswords.com";
        private readonly string UserAgent = "EcommerceTestPortal";
        private readonly string HIBPApiKey = "1f1e5caf16ac4a3c8572f301e38af583";

        [SetUp]
        public void Setup()
        {
            objPasswordChecker = new PasswordStringValidations();
            objDataBreachChecker = new DataBreachChecker(PWNEDApiURL,UserAgent, HIBPApiKey, RangeApiURL);
        }
        /// <summary>
        /// Check with a weak password
        /// </summary>
        [Test]
        public void Test1()
        {

            score = objPasswordChecker.CheckStrength("1");

            pRank = (PasswordRank)score;

            if (pRank == PasswordRank.VeryWeak)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }
        /// <summary>
        /// Check with a strong password
        /// </summary>
        [Test]
        public void Test2()
        {
            score = objPasswordChecker.CheckStrength("Password$3");
            pRank = (PasswordRank)score;
            if (pRank == PasswordRank.Strong)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }
        /// <summary>
        /// Check with a very strong password
        /// </summary>
        [Test]
        public void Test3()
        {
            score = objPasswordChecker.CheckStrength("Password12$3");
            pRank = (PasswordRank)score;
            if (pRank == PasswordRank.VeryStrong)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }
        /// <summary>
        /// Check if the password is in the data breach list. Check the count of times it is in the list.
        /// Test is in the list, hence if the count returned is more than 0 the test is success.
        /// </summary>
        [Test]
        public async Task Test4()
        {
            int count = await objDataBreachChecker.CheckIfPasswordPwned("test");
            if (count > 0)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }
        /// <summary>
        /// Testing data breach with a complex password.
        /// </summary>
        [Test]
        public async Task Test5()
        {
            int count = await objDataBreachChecker.CheckIfPasswordPwned("Password12$3Test");
            if (count == 0)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }
        /// <summary>
        /// Testing data breach with a medium strong password.
        /// </summary>
        [Test]
        public async Task Test6()
        {
            int count = await objDataBreachChecker.CheckIfPasswordPwned("password");
            if (count > 0)
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