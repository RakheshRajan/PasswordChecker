using PasswordStrengthChecker.BusinessLogic.Model;
using PasswordStrengthChecker.BusinessLogic.Interface;
using System;
using System.Text.RegularExpressions;

namespace PasswordStrengthChecker.BusinessLogic
{
    public class PasswordStringValidations : AbstractHandler
    {
        /// <summary>
        /// Check the strength of the password by a basic scoring logic.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public override int CheckStrength(string password, int score = 0)
        {                                        
            //int score = 0;

            if (password.Length < 1)
                return 0;
            if (password.Length <= 7)
                return 1;

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

            return base.CheckStrength(password, score);
        }
    }
}
