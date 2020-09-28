using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationService.Repository
{
    public class CredentialsRepo: ICredentialsRepo
    {
        private Dictionary<string, string> ValidUsersDictionary = new Dictionary<string, string>()
        {
               {"Sujoy","Basak"},
               {"Supriya","Sinha"},
               {"Aman","Bharti"},
               {"Shubham","Debnath"}
        };
        public Dictionary<string,string> GetCredentials()
        {
            return ValidUsersDictionary;
        }
    }
}
