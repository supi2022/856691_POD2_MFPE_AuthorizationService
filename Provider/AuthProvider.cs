using AuthorizationService.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationService.Provider
{
    public class AuthProvider:IAuthProvider
    {
        private static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(AuthProvider));
        private readonly ICredentialsRepo objRepository;
        public AuthProvider(ICredentialsRepo _objRepository)
        {
            objRepository = _objRepository;
        }
        /// <summary>
        /// This method is responsible for generating token as per the userinfo given by the authenticate method.
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="_config"></param>
        /// <returns></returns>
        public string GenerateJSONWebToken(Authenticate userInfo,IConfiguration _config)
        {
            if (userInfo == null)
                return null;
            try
            {
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

                SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                JwtSecurityToken token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Issuer"],
                    null,
                    expires: DateTime.Now.AddMinutes(15),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch(Exception e)
            {
                _log4net.Error("Exception Occured " + e.Message +nameof(AuthProvider));
                return null;
            }
            
        }
        /// <summary>
        /// This method is used to authenticate user if the user credentials exist in the database and it will return the same.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>

        public Authenticate AuthenticateUser(Authenticate login)
        {           
            try
            {
                Authenticate userCredentials = null;                

                Dictionary<string,string> ValidUsersDictionary = objRepository.GetCredentials();

                if (ValidUsersDictionary == null)
                    return null;
                else
                {
                    if (ValidUsersDictionary.Any(u => u.Key == login.Name && u.Value == login.Password))
                    {
                        userCredentials = new Authenticate { Name = login.Name, Password = login.Password };
                    }
                }               

                return userCredentials;
            }
            catch(Exception e)
            {
                _log4net.Error("Exception Occured " + e.Message+ " from "+ nameof(AuthProvider));
                return null;
            }
            
        }
    }
}
