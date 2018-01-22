using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lealta_mobile
{
    internal class Token
    {
        internal DateTime AssinedAt;
        internal DateTime ExpiresAt;
        internal long ExpiresIn;
        internal string AccessToken;
    }
}
