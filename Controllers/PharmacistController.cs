using Microsoft.AspNetCore.Mvc;
using HospitalManagementAPI.Data;
using HospitalManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PharmacistController : ControllerBase
    {
        private readonly HospitalDbContext _context;

        public PharmacistController(HospitalDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pharmacist>>> GetPharmacists() =>
            await _context.Pharmacists.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Pharmacist>> GetPharmacist(int id)
        {
            var pharmacist = await _context.Pharmacists.FindAsync(id);
            return pharmacist == null ? NotFound() : pharmacist;
        }

        [HttpPost]
        public async Task<ActionResult<Pharmacist>> PostPharmacist(Pharmacist pharmacist)
        {
            _context.Pharmacists.Add(pharmacist);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPharmacist), new { id = pharmacist.Id }, pharmacist);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPharmacist(int id, Pharmacist pharmacist)
        {
            if (id != pharmacist.Id) return BadRequest();
            _context.Entry(pharmacist).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePharmacist(int id)
        {
            var pharmacist = await _context.Pharmacists.FindAsync(id);
            if (pharmacist == null) return NotFound();
            _context.Pharmacists.Remove(pharmacist);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
