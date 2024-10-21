using System;
using System.Collections.Generic;
using System.Text;

namespace CourseworkProto1.Models.Entities
{
    public enum AccessRights
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

    public enum MusicGenre
    {
        Rock = 1,
        Pop = 2,
        Jazz = 3,
        Classical = 4
    }

    public enum GamePlatform
    {
        PC = 1,
        PS5 = 2,
        Xbox = 3,
        NintendoSwitch = 4
    }


}
