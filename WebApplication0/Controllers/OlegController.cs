using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication0.Data;
using WebApplication0.Models;

namespace WebApplication0.Controllers
{
    public class OlegController : Controller
    {
        private readonly ApplicationContext _context;

        public OlegController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> Details(int? studentID)
        {

            if (studentID == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == studentID);

            if (student == null)
            {
                return NotFound();
            }
            
            return View(student);
        }
    
        public async Task<IActionResult> Delete(int? studentID, int? enrollmentID){
            if (studentID == null){
                await _context.Enrollments
                            .Where(e => e.EnrollmentID == enrollmentID)
                            .ExecuteDeleteAsync();
            }
            else{
                await _context.Students
                                .Where(s => s.ID == studentID)
                                .ExecuteDeleteAsync();
            }
            return View("Index");
        }
    }
}
