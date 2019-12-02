using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using second.Models;

namespace second.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TablesController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: table by hall id 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTables([FromRoute] string id)
        {

            var tables = _context.TablesList.Where(u=>u.HallID==id);

            if (tables == null)
            {
                return NotFound();
            }

            return Ok(tables);
        }

       
    }
}