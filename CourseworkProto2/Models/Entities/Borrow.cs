using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CourseworkProto1.Models.Entities;
public class Borrow
{
    [Key]
    public int BorrowId { get; set; }

    public int LenderId { get; set; } // Хто позичає
    public User Lender { get; set; }
    public int BorrowerId { get; set; } // Хто бере у позичку
    public User Borrower { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public DateTime BorrowStartDate { get; set; }
    public DateTime BorrowEndDate { get; set; }
}
