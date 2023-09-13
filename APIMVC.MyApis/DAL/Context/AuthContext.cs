using APIMVC.MyApis.DAL.Entitites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMVC.MyApis.DAL.Context
{
    public class AuthContext:DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> opt) : base(opt)
        {

        }
        public DbSet<User> User { get; set; }


    }
}
