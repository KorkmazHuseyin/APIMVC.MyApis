using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIMVC.MyApis.DAL.Entitites
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public string UserName { get; set; }

    }
}
