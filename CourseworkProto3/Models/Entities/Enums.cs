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

    public enum MusicGenre
    {
        Rock = 1,
        Pop,
        Jazz,
        Classical,
        HipHop,
        Electronic,
        Blues,
        Reggae,
        Metal
    }

    public enum MovieGenre
    {
        Action = 1,
        Comedy,
        Drama,
        Horror,
        Thriller,
        Fantasy,
        SciFi,
        Romance,
        Documentary,
        Animation,
        Adventure,
        Mystery,
        Crime,
        Musical,
        Historical,
        Western
    }

    public enum BookGenre
    {
        Horror = 1,
        Adventure,
        Mystery,
        Fantasy,
        Fiction,
        Romance,
        Thriller,
        Biography,
        Historical,
        ScienceFiction
    }

    public enum GameGenre
    {
        Action = 1,
        Strategy,
        RPG,
        Simulation,
        Horror,
        Puzzle,
        Sports,
        Fighting,
        Racing,
        Platformer
    }


    public enum GamePlatform
    {
        PC = 1,
        PS4,
        PS5,
        Xbox,
        NintendoSwitch,
    }
}
