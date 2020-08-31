using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GC_Capstone4_ToDoListRevisited.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GC_Capstone4_ToDoListRevisited.Controllers
{
    public class ToDoListController : Controller
    {
        private readonly ToDoListDbContext _toDoListDb;

        public ToDoListController(ToDoListDbContext toDoListDb)
        {
            _toDoListDb = toDoListDb;
        }

        //CRUD

        //Read
        [Authorize]
        public async Task<IActionResult> ToDoList()
        {
            string activeUserName = User.Identity.Name; //finds the user name of the logged in user
            string activeUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value; //finds the user id of the logged in user

            var tasks = await _toDoListDb.ToDo.Where(x => x.UserId == activeUserId).ToListAsync(); //Fill list with all tasks from db that match the user id of the logged in user

            ViewBag.UserName = activeUserName; //sends user name to view

            return View(tasks);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ToDoSearch(string searchString)
        {
            string activeUserName = User.Identity.Name; //finds the user name of the logged in user
            string activeUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value; //finds the user id of the logged in user

            var tasks = await _toDoListDb.ToDo.Where(x => x.UserId == activeUserId).ToListAsync(); //Fill list with all tasks from db that match the user id of the logged in user
            tasks = await _toDoListDb.ToDo.Where(x => x.TaskDescription.Contains(searchString)).ToListAsync(); //Fill list with all tasks from db that match the user id of the logged in user

            ViewBag.UserName = activeUserName; //sends user name to view

            return RedirectToAction("ToDoList", tasks);

        }

        [Authorize]
        [HttpGet]
        public IActionResult AddTask()
        {
            return View();
        }

        //Create
        [Authorize]
        [HttpPost]
        public IActionResult AddTask(ToDo newToDo)
        {
            string activeUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value; //finds the user id of the logged in user

            if (ModelState.IsValid)
            {
                newToDo.UserId = activeUserId;
                _toDoListDb.ToDo.Add(newToDo);
                _toDoListDb.SaveChanges();
            }

            return RedirectToAction("ToDoList");
        }

        //Delete
        public IActionResult DeleteTask(int id)
        {
            var foundTask = _toDoListDb.ToDo.Find(id);
            if (foundTask != null)
            {
                _toDoListDb.ToDo.Remove(foundTask);
                _toDoListDb.SaveChanges();
            }

            return RedirectToAction("ToDoList");

        }

        public IActionResult UpdateCompletion(int id)
        {
            ToDo toDoToUpdate = _toDoListDb.ToDo.Find(id);

            toDoToUpdate.Complete = !toDoToUpdate.Complete;

            _toDoListDb.Entry(toDoToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _toDoListDb.Update(toDoToUpdate);
            _toDoListDb.SaveChanges();

            return RedirectToAction("ToDoList");

        }
    }
}
