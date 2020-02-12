using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PasswordStrengthChecker.BusinessLogic.Interface
{
    public interface IPasswordBreachChecker
    {
        Task<int> CheckIfPasswordPwned(string password);
    }
}
