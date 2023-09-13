using APIMVC.MyApis.DAL.Entitites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMVC.MyApis.DAL.Context
{
    public class MyContext:DbContext
    {
        public MyContext(DbContextOptions<MyContext> context): base(context)
        {

        }
        public DbSet<Shippers> Shippers { get; set; }
    }
}
