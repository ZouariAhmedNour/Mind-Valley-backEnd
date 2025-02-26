using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MindValley.Models;

namespace MindValley.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoodLogsController : ControllerBase
    {
        private readonly MindValleyContext _context;

        public MoodLogsController(MindValleyContext context)
        {
            _context = context;
        }

        // GET: api/MoodLogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MoodLog>>> GetMoodLogs()
        {
            return await _context.MoodLogs.ToListAsync();
        }

        // GET: api/MoodLogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MoodLog>> GetMoodLog(int id)
        {
            var moodLog = await _context.MoodLogs.FindAsync(id);

            if (moodLog == null)
            {
                return NotFound();
            }

            return moodLog;
        }

        // PUT: api/MoodLogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMoodLog(int id, MoodLog moodLog)
        {
            if (id != moodLog.MoodLogId)
            {
                return BadRequest();
            }

            _context.Entry(moodLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MoodLogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MoodLogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MoodLog>> PostMoodLog([FromBody] MoodLogDto moodLogDto)
        {
            // Find the user by userId
            var user = await _context.Users.FindAsync(moodLogDto.UserId);
            if (user == null)
            {
                return BadRequest(new { message = "User not found" });
            }

            // Create a new MoodLog instance
            var moodLog = new MoodLog
            {
                UserId = moodLogDto.UserId,
                Mood = moodLogDto.Mood,
                Notes = moodLogDto.Notes,
                LogDate = moodLogDto.LogDate ?? DateTime.UtcNow,
                User = user
            };

            _context.MoodLogs.Add(moodLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMoodLog", new { id = moodLog.MoodLogId }, moodLog);
        }


        // DELETE: api/MoodLogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMoodLog(int id)
        {
            var moodLog = await _context.MoodLogs.FindAsync(id);
            if (moodLog == null)
            {
                return NotFound();
            }

            _context.MoodLogs.Remove(moodLog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MoodLogExists(int id)
        {
            return _context.MoodLogs.Any(e => e.MoodLogId == id);
        }
    }
}
