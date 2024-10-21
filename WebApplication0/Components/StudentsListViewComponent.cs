using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApplication0.Data;

namespace WebApplication0.Components
{
    public class StudentsListViewComponent : ViewComponent
    {
        private readonly ApplicationContext _context;
        public StudentsListViewComponent(ApplicationContext context){
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(){
            if(_context == null)
                Console.WriteLine("Null in Students");
            return View("Students", await _context.Students.ToListAsync());
        }
        
    }
}
