using PasswordStrengthChecker.BusinessLogic.Interface;
using PasswordStrengthChecker.BusinessLogic.Model;

namespace PasswordStrengthChecker.BusinessLogic.Interface
{
    // The Handler interface declares a method for building the chain of
    // handlers. It also declares a method for executing a request.

    // The default chaining behavior can be implemented inside a base handler
    // class.
    public abstract class AbstractHandler : IPasswordCheckerHandler
    {
        private IPasswordCheckerHandler _nextHandler;

        public IPasswordCheckerHandler SetNext(IPasswordCheckerHandler handler)
        {
            this._nextHandler = handler;
            return handler;
        }
       

        public virtual int CheckStrength(string password, int score = 0)
        {
            if (this._nextHandler != null)
            {
                return this._nextHandler.CheckStrength(password, score);
            }
            else
            {
                return score;
            }
        }
    }
}
