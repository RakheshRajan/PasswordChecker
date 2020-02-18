using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PasswordStrengthChecker.BusinessLogic;
using PasswordStrengthChecker.BusinessLogic.Interface;
using PasswordStrengthChecker.BusinessLogic.Model;

namespace PasswordStrengthChecker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordCheckerController : ControllerBase
    {
        public PasswordRank PwdRank { get; private set; }
        public IConfiguration IConfig { get; }
        PasswordStringValidations passwordChecker;

        /// <summary>
        /// Initialise configuration object - Dependency Injection
        /// </summary>
        /// <param name="iConfig"></param>
        public PasswordCheckerController(IConfiguration iConfig)
        {
            IConfig = iConfig;
            passwordChecker = new PasswordStringValidations();
            PasswordExternalValidations passwordExternalValidations = new PasswordExternalValidations();
            passwordChecker.SetNext(passwordExternalValidations);
        }
        /// <summary>
        /// Checks the strength of the password. Method returns an enum with values indicating the strength.
        /// </summary>
        /// <param name="password">password in string format</param>
        /// <returns>Enum</returns>
        // GET api/values/5
        [HttpGet("CheckStrength/{password}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<string> CheckStrength(string password)
        {
            try
            {
                int score = 0;
                passwordChecker = new PasswordStringValidations();
                score = passwordChecker.CheckStrength(password);
                PwdRank = (PasswordRank)score;
            }
            catch (Exception ex)
            {
                //Log exceptions here
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
            return PwdRank.ToString();
        }
        /// <summary>
        /// Checks if the provided password appears in any databreaches (asynchronously) and returns the number of times it is found.
        /// </summary>
        /// <param name="password">password in string format</param>
        /// <returns></returns>
        [HttpGet("CheckDataBreach/{password}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> CheckDataBreach(string password)
        {
            int DataBreachCount;
            try
            {
                IPasswordBreachChecker objDataBreachChecker = new DataBreachChecker(IConfig["PWNEDApiURL"].ToString(), IConfig["UserAgent"].ToString(), IConfig["HIBPApiKey"].ToString(), IConfig["RangeApiURL"].ToString());
                DataBreachCount = await objDataBreachChecker.CheckIfPasswordPwned(password);
            }
            catch (Exception ex)
            {
                //Log exceptions here
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
            return DataBreachCount;
        }
    }
}
