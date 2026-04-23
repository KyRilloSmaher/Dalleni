using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Domin.Interfaces.Repositories
{
    public interface IRoleManager<TRole> where TRole : class
    {
        Task<bool> RoleExistsAsync(string roleName);

        Task<(bool Succeeded, IEnumerable<string> Errors)> CreateAsync(TRole role);
    }

}
