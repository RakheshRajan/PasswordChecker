using PasswordStrengthChecker.BusinessLogic.Interface;
using PasswordStrengthChecker.BusinessLogic.Model;
using System;
using System.Text.RegularExpressions;

namespace PasswordStrengthChecker.BusinessLogic
{
    public class PasswordChecker : IPasswordChecker
    {
        public PasswordRank CheckStrength(string password)
        {
            int score = 0;

            if (password.Length < 1)
                return PasswordRank.Blank;
            if (password.Length < 4)
                return PasswordRank.VeryWeak;

            if (password.Length >= 8)
                score++;
            if (password.Length >= 12)
                score++;

            if (Regex.Match(password, @"/\d+/", RegexOptions.ECMAScript).Success)
                score++;

            if (Regex.Match(password, @"/[a-z]/", RegexOptions.ECMAScript).Success &&
              Regex.Match(password, @"/[A-Z]/", RegexOptions.ECMAScript).Success)
                score++;

            if (Regex.Match(password, @"/.[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]/", RegexOptions.ECMAScript).Success)
                score++;

            return (PasswordRank)score;
        }
    }
}
