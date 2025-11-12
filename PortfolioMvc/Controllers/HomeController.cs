using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using PortfolioMvc.Models;
using System.Diagnostics;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace PortfolioMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Portfolio()
        {
            var projects = GetProjects();
            return View(projects);
        }

        public IActionResult Projects()
        {
            var projects = GetProjects();
            return View(projects);
        }

        // Helper method to avoid duplication
        private List<ProjectModel> GetProjects()
        {
            return new List<ProjectModel>
            {
                new ProjectModel { Title = "ERP System", Description = "Optimized ERP modules using ASP.NET Core & Azure SQL.", Link = "#" },
                new ProjectModel { Title = "Visitor Management System", Description = "RBAC, check-ins, audit logging.", Link = "#" },
                new ProjectModel { Title = "School Management System", Description = "Student & admin portal improving reporting accuracy, online exams.", Link = "#" }
            };
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Resume()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(model.Email));
            message.To.Add(MailboxAddress.Parse(Environment.GetEnvironmentVariable("SMTP_USER")));
            message.Subject = $"Portfolio Contact: {model.Name}";
            message.Body = new TextPart("plain") { Text = model.Message };

            using var client = new SmtpClient();
            await client.ConnectAsync(
                Environment.GetEnvironmentVariable("SMTP_HOST"),
                int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT")),
                bool.Parse(Environment.GetEnvironmentVariable("SMTP_ENABLESSL")) ? SecureSocketOptions.StartTls : SecureSocketOptions.None
            );
            await client.AuthenticateAsync(
                Environment.GetEnvironmentVariable("SMTP_USER"),
                Environment.GetEnvironmentVariable("SMTP_PASS")
            );
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            ViewBag.Message = "Message sent successfully!";
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
