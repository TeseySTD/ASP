using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication0.Data;

namespace WebApplication0.Components
{
    public class EnrollmentsListViewComponent : ViewComponent
    {
        private readonly ApplicationContext _context;
        public EnrollmentsListViewComponent(ApplicationContext context){
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(){
            if (_context == null)
                Console.WriteLine("Null in enrollments");
                
            return View("Enrollments", await _context.Enrollments
                                                    .Include(e => e.Course)
                                                    .Include(e => e.Student)
                                                    .ToListAsync());
        }
    }
}
