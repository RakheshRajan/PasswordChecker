using PasswordStrengthChecker.BusinessLogic.Interface;
using PasswordStrengthChecker.BusinessLogic.Model;
using System;
using System.Text.RegularExpressions;

namespace PasswordStrengthChecker.BusinessLogic
{
    public class PasswordChecker : IPasswordChecker
    {
        /// <summary>
        /// Check the strength of the password by a basic scoring logic.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public PasswordRank CheckStrength(string password)
        {
            int score = 0;

            if (password.Length < 1)
                return PasswordRank.Blank;
            if (password.Length <= 7)
                return PasswordRank.VeryWeak;

            //If password contains digits
            if (Regex.Match(password, @"\d+", RegexOptions.ECMAScript).Success)
                score++;

            //If password contains lower & upper characters
            if (Regex.Match(password, @"[a-z]", RegexOptions.ECMAScript).Success &&
              Regex.Match(password, @"[A-Z]", RegexOptions.ECMAScript).Success)
                score++;
            //If password contains special characters
            if (Regex.Match(password, @".[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript).Success)
                score++;

            //If the password length greater than 8
            if (password.Length >= 8)
                score++;

            //If the password length greater than 12
            if (password.Length >= 12)
                score++;
            //Password doesnot contain any complex combinations
            if(score==0)
                return PasswordRank.VeryWeak;

            return (PasswordRank)score;
        }
    }
}
