using PasswordStrengthChecker.BusinessLogic.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordStrengthChecker.BusinessLogic.Interface
{
    public interface IPasswordChecker
    {
        PasswordRank CheckStrength(string password);
    }
}
