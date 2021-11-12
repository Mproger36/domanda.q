using System;
using System.Linq;
using System.Threading.Tasks;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoviePortal.Context;

namespace App.Controllers
{
    public class QuestionController : Controller
    {
        private readonly QuestionsContext _db;
        private readonly ILogger<QuestionController> _logger;

        public QuestionController(QuestionsContext db, ILogger<QuestionController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> ShowAll()
        {
            var questions = await _db.Questions.ToListAsync();
            return View(questions);
        }

        [HttpGet("Question/Show/{id}")]
        public async Task<IActionResult> Show(Guid id)
        {
            var question = _db.Questions.Where(p => p.Id == id).FirstOrDefault();
            return View(question);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Question question)
        {
            await _db.Questions.AddAsync(
                    new Question() {
                    Id = Guid.NewGuid(),
                    Heading = question.Heading,
                    Text = question.Text
                    });
            await _db.SaveChangesAsync();

            return RedirectToAction("ShowAll", "Question");
        }
    }
}
