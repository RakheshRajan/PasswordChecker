using PasswordStrengthChecker.BusinessLogic.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordStrengthChecker.BusinessLogic.Interface
{
    public interface IPasswordCheckerHandler
    {
        IPasswordCheckerHandler SetNext(IPasswordCheckerHandler handler);
        int CheckStrength(string password, int score=0);
    }
}

