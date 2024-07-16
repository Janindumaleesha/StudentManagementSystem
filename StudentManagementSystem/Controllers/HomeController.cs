using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;
using System.Diagnostics;

namespace StudentManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly StudentDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, StudentDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StudentRegister(Student student)
        {
            if (student.Password == student.ConfirmPassword)
            {
                _dbContext.Add(student);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return NotFound();
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> StudentLogin(string email, string password)
        {
            var studentLogin = await _dbContext.Students.FirstOrDefaultAsync(x => x.Email == email);

            if (studentLogin != null )
            {
                var studentPassword = await _dbContext.Students.FirstOrDefaultAsync(y => y.Password == password);

                if (studentPassword != null)
                {
                    return RedirectToAction(nameof(Privacy));
                }
                return NotFound();
            }
            return NotFound();
        }

        public async Task<IActionResult> Privacy()
        {
            return View(await _dbContext.Students.ToListAsync());
        }

        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = _dbContext.Students.FirstOrDefault(x => x.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,[Bind("FName,LName,Email,Password,ConfirmPassword")] Student student)
        {
            var studentId = await _dbContext.Students.FirstOrDefaultAsync(x => x.Id == id);

            if(studentId.Id != id)
            {
                return NotFound();
            }

            studentId.LName = student.LName;
            studentId.FName = student.FName;
            studentId.Email = student.Email;    
            studentId.Password = student.Password;  
            studentId.ConfirmPassword = student.ConfirmPassword;

            _dbContext.Students.Update(studentId);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Privacy));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                NotFound();
            }

            var student = await _dbContext.Students.FindAsync(id);

            if (student == null)
            {
                NotFound();
            }

             _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Privacy));
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _dbContext.Students.FirstOrDefaultAsync(x => x.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
