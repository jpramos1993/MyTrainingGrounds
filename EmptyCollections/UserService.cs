using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyCollections
{
    public record User(string FullName);

    public class UserService
    {
        public IEnumerable<User> GetUsers()
        {
            // Validation
            if(!validation)
            {
                return Enumerable.Empty<User>();
                return Array.Empty<User>();
                return ReadOnlyCollection<User>.Empty();
                return ReadOnlyDictionary<int, User>.Empty;
            }
            // Call to a Database

            return null;
        }
    }
}
