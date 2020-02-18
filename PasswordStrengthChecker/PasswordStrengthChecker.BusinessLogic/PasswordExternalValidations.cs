using PasswordStrengthChecker.BusinessLogic.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordStrengthChecker.BusinessLogic
{
    public class PasswordExternalValidations : AbstractHandler
    {
        /// <summary>
        /// Blank implementation for future changes
        /// </summary>
        /// <param name="password"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public override int CheckStrength(string password, int score = 0)
        {
            return base.CheckStrength(password, score);
        }
    }
}
