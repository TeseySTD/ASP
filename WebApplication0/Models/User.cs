using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplication0.Models
{
    public enum AccessRights
    {
        Owner, 
        Administrator,
        Operator,
        Default
    }
    public class User
    {
        public int ID { get; set; }
        public string Login {get; set;}
        public string Password {get; set;}
        public AccessRights AccessRights {get; set;}
    }
}
