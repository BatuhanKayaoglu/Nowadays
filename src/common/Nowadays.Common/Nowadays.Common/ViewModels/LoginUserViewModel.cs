using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Nowadays.Common.ViewModels
{
    public class LoginUserViewModel
    {
        public string Id { get; set; }
        public string NameSurname { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
