using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Models.Entities
{
    public enum Roles
    {
        Owner, 
        Administrator,
        Operator,
        Default
    }

    public enum Gender{
        Male = 1,
        Female
    }

    public enum Permission{
        Create=1,
        Read=2,
        Update=3,
        Delete=4
    }

    public enum ProductType{
        Book = 1,
        Disc
    }
    public enum DiscFormat
    {
        CD = 1,
        DVD = 2,
        BluRay = 3
    }

    public enum DiscType
    {
        Movie = 1,
        Music = 2,
        Game = 3
    }


    public enum OrderStatus
    {
        Pending = 1,     // Очікує виконання
        Ordered,     // Замовлено
        Returned,    // Виконано,
    }
}
