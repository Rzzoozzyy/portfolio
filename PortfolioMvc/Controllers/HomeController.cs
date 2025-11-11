using Microsoft.AspNetCore.Mvc;
using PortfolioMvc.Models;
using System.Diagnostics;
using System.Net.Mail;
using MailKit.Net.Smtp;
using MimeKit;
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
                new ProjectModel { Title = "Visitor Management System", Description = "RBAC, QR code check-ins, audit logging.", Link = "#" },
                new ProjectModel { Title = "School Management System", Description = "Student & admin portal improving reporting accuracy.", Link = "#" }
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
        public async Task<IActionResult> Contact(ContactModel model, [FromServices] IConfiguration config)
        {
            if (!ModelState.IsValid) return View(model);

            var smtp = config.GetSection("Smtp");
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(model.Email));
            message.To.Add(MailboxAddress.Parse(smtp["User"]));
            message.Subject = $"Portfolio Contact: {model.Name}";
            message.Body = new TextPart("plain") { Text = model.Message };

            using var client = new SmtpClient();
            await client.ConnectAsync(smtp["Host"], int.Parse(smtp["Port"]), MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(smtp["User"], smtp["Pass"]);
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
