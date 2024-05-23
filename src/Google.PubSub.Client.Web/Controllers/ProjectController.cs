using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Google.PubSub.Client.Web.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Google.PubSub.Client.Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly PubSubConfig _pubSubConfig;

        public ProjectController(
            ILogger<ProjectController> logger,
            PubSubConfig pubSubConfig)
        {
            _logger = logger;
            _pubSubConfig = pubSubConfig;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }
        
        [HttpGet("projects")]
        public IActionResult List()
        {
            var projectList = GetProjects();
            var model = new ProjectListModel {Projects = projectList};

            return View(model);
        }

        [HttpGet("project/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("project/create")]
        public IActionResult Create(string projectName)
        {
            var existingProjects = GetProjects();
            if (!existingProjects.Contains(projectName, StringComparer.OrdinalIgnoreCase))
            {
                existingProjects.Add(projectName);
                HttpContext.Session.SetString("projects", JsonSerializer.Serialize(existingProjects));
            }
            
            return RedirectToAction("List");
        }

        private List<string> GetProjects()
        {
            var projectsJson = HttpContext.Session.GetString("projects") ?? "[]";
            var projects = JsonSerializer.Deserialize<string[]>(projectsJson);
            if (!projects.Any())
            {
                projects = _pubSubConfig.Projects.ToArray();
            }

            return projects.ToList();
        }
    }

    public class ProjectListModel
    {
        public IReadOnlyCollection<string> Projects { get; set; }
    }
}