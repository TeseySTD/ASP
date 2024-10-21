using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CourseworkProto1.Models.Entities
{

    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Login {get; set;}
        public string Password {get; set;}
        public AccessRights AccessRights {get; set;}
        public Gender Gender {get; set;}
    }

}
