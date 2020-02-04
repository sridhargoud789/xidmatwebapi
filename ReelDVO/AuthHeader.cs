using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;

namespace ReelDvo
{
    public class AuthHeader : SoapHeader
    {
        public string Username;
        public string Password;
    }
}
