using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatSample
{
    public sealed class UserManager
    {
        private Dictionary<string, string> dictionary;
        private static readonly Lazy<UserManager>
            lazy =
            new Lazy<UserManager>
                (() => new UserManager());

        public static UserManager Instance { get { return lazy.Value; } }
        
        public Dictionary<string, string> Dictionary
        {
            get
            {
                return dictionary;
            }
        }

        private UserManager()
        {
            dictionary = new Dictionary<string, string>();
        }
    }

}
