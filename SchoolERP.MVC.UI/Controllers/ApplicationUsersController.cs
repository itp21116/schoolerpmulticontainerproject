using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using SchoolERP.MVC.UI.Data;
using SchoolERP.MVC.UI.Models;
using SchoolERP.MVC.UI.ViewModels;
using System.Security.Claims;
using System.Security.Policy;

namespace SchoolERP.MVC.UI.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ApplicationUsersController
        public async Task<IActionResult> Index()
        {
            try
            {
                var url = "http://students-services-clip/api/Student"; //to be changed 
                var restClient = new RestClient(url);
                var request = new RestRequest(url, RestSharp.Method.Get);
                RestResponse response = await restClient.ExecuteAsync(request);
                var output = response.Content;
                if (output != null)
                {
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<ApplicationUser>>(output);
                    if (data != null)
                    {
                        return View(data.ToList());
                    }

                }
            }
            catch (Exception)
            {

            }

            return Problem("Entity set 'ApplicationDbContext.Course'  is null.");
        }

        // GET: ApplicationUsersController/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var url = "http://students-services-clip/api/Student/" + id.ToString(); //to be changed 
                var restClient = new RestClient(url);
                var request = new RestRequest(url, RestSharp.Method.Get);
                RestResponse response = await restClient.ExecuteAsync(request);
                var output = response.Content;
                if (output != null)
                {
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ApplicationUser>(output);
                    if (data != null)
                    {
                        return View(data);
                    }

                }
            }
            catch (Exception)
            {

            }

            return Problem("Entity set 'ApplicationDbContext.Course'  is null.");
        }

        // GET: ApplicationUsersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplicationUsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateOfEnrollment,Average,IsAGraduate,FirstName,LastName,DateOfBirth,Email")] UpdatableStudentAdminVM student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var url = "http://students-services-clip/api/Student/"; //to be changed 
                    var restClient = new RestClient(url);
                    var request = new RestRequest(url, RestSharp.Method.Post);
                    var body = JsonConvert.SerializeObject(student);
                    request.AddBody(body, "application/json");

                    RestResponse response = await restClient.ExecuteAsync(request);

                    if (response.StatusCode.ToString().Equals("Created"))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {

                }
            }
            return Problem("ApplicationUser failed to be created.");
        }

        // GET: ApplicationUsersController/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            try
            {
                var url = "http://students-services-clip/api/Student/" + id.ToString();  
                var restClient = new RestClient(url);
                var request = new RestRequest(url, RestSharp.Method.Get);
                RestResponse response = await restClient.ExecuteAsync(request);
                var output = response.Content;
                if (output != null)
                {
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ApplicationUser>(output);
                    if (data != null)
                    {
                        return View(data);
                    }

                }
            }
            catch (Exception)
            {

            }

            return Problem("Entity set 'ApplicationDbContext.Course'  is null.");
        }

        // POST: ApplicationUsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,DateOfEnrollment,Average,IsAGraduate,FirstName,LastName,DateOfBirth,Email")] UpdatableStudentAdminVM student)
        {
            /*                if (!(id.Equals(student.Id)))
                            {
                                return NotFound();
                            }*/

            if (ModelState.IsValid)
            {
                try
                {
                    var url = "http://students-services-clip/api/Student/" + id.ToString(); //to be changed 
                    var restClient = new RestClient(url);
                    var request = new RestRequest(url, RestSharp.Method.Put);
                    var body = JsonConvert.SerializeObject(student);
                    request.AddBody(body, "application/json");

                    RestResponse response = await restClient.ExecuteAsync(request);

                    if ((int)response.StatusCode == 200)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                }
                catch (Exception)
                {

                }

            }
            return NotFound();
        }

        // GET: ApplicationUsersController/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {

            if (id == null)
            {
                return NotFound();
            }

                try
                {
                    var url = "http://students-services-clip/api/Student/" + id.ToString();
                    var restClient = new RestClient(url);
                    var request = new RestRequest(url, RestSharp.Method.Get);
                    RestResponse response = await restClient.ExecuteAsync(request);
                    var output = response.Content;
                    if (output != null)
                    {
                        var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ApplicationUser>(output);
                        if (data != null)
                        {
                            return View(data);
                        }

                    }
                }
                catch (Exception)
                {

                }

            

            return Problem("Entity set 'ApplicationDbContext.Course'  is null.");

        }

        // POST: ApplicationUsersController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var url = "http://students-services-clip/api/Student/" + id.ToString();
                var restClient = new RestClient(url);
                var request = new RestRequest(url, RestSharp.Method.Delete);
                RestResponse response = await restClient.ExecuteAsync(request);
                var output = response.Content;
                if (output != null)
                {
                    return RedirectToAction(nameof(Index));

                }
            }
            catch
            {
                return View();
            }

            return Problem("Entity set 'ApplicationDbContext.Course'  is null.");
        }


        public async Task<IActionResult> EnrollToCourses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var allCourses = new List<Course>();
            var enrolledCourses = new List<StudentCourseVM>();
            bool selected = false;


            //get all the available courses
            try
            {
                var url = "http://courses-services-clip/api/Course"; 
                var restClient = new RestClient(url);
                var request = new RestRequest(url, RestSharp.Method.Get);
                RestResponse response = await restClient.ExecuteAsync(request);
                var output = response.Content;

                if (output != null)
                {
                    var responseC = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Course>>(output);
                    if (responseC != null)
                    {
                        allCourses = responseC.ToList();
                    }
                }
            }
            catch (Exception)
            {
                return Problem("No courses found error"); 
            }

            //get enrolled courses
            try
            {
                var url = "http://students-services-clip/api/Student/Grades/" + userId.ToString(); //
                var restClient = new RestClient(url);
                var request = new RestRequest(url, RestSharp.Method.Get);
                RestResponse response = await restClient.ExecuteAsync(request);
                var output = response.Content;

                if (output != null)
                {
                    var responceEnroll = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<StudentCourseVM>>(output);
                    if (responceEnroll != null)
                    {
                        enrolledCourses = responceEnroll.ToList();
                    }
                }
            }
            catch (Exception)
            {

            }

            var selectedCourses = new List<SelectedCoursesVM>();

            foreach (var c in allCourses)
            {
                foreach (var e in enrolledCourses)
                {
                    if (c.Name.Equals(e.CourseName))
                    {
                        selected = true;
                        break;

                    }
                }
                var selectCourse = new SelectedCoursesVM();
                selectCourse.SelectedCourseCourseId = c.Id;
                selectCourse.CourseName = c.Name;
                selectCourse.IsSelected = selected;
                selectedCourses.Add(selectCourse);
                selected = false;
            }
            return selectedCourses != null ? View(selectedCourses) : Problem("Entity set 'ApplicationDbContext.Course'  is null.");

        }

        [Route("UpdateEnrollments")]
        [HttpPost, ActionName("UpdateEnrollments")]
        public async Task<ActionResult> UpdateEnrollments([FromForm] List<SelectedCoursesVM> selectedCoursesUpdated)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var url = "http://students-services-clip/api/Student/Grades/" + userId.ToString(); 
                var restClient = new RestClient(url);
                var request = new RestRequest(url, RestSharp.Method.Put);
                var body = JsonConvert.SerializeObject(selectedCoursesUpdated.Where(c => c.IsSelected == true));
                request.AddBody(body, "application/json");
                RestResponse response = await restClient.ExecuteAsync(request);
                var output = response.Content;

                if ((int)response.StatusCode == 200)
                {
                    return RedirectToAction(nameof(EnrollToCourses));
                }
            }
            catch (Exception)
            {

            }
            return Problem("something went bad!");
        
        }

        [HttpPost]
        public async Task<ActionResult> InsertGrades(List<StudentCourseVM> studentCourse)
        {
            if (studentCourse == null)
            {
                return NotFound();
            }


            try
            {
                var url = "http://students-services-clip/api/Student/Grade"; 
                var restClient = new RestClient(url);
                var request = new RestRequest(url, RestSharp.Method.Post);
                var body = JsonConvert.SerializeObject(studentCourse);
                request.AddBody(body, "application/json");
                RestResponse response = await restClient.ExecuteAsync(request);

                if ((int)response.StatusCode == 200)
                {
                    var student = await _context.Students.Where(s => s.Id.Equals(studentCourse[0].StudentId.ToString())).FirstOrDefaultAsync();
                    var sum = 0f;
                    foreach (var s in studentCourse)
                    {
                        sum += (float)s.Grade;
                    }
                    student.Average = sum / studentCourse.Count;
                    _context.Students.Update(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {

            }

            return Problem("oops!");
        }


        [Route("GetCourses")]
        [HttpGet, ActionName("GetCourses")]
        public async Task<IActionResult> GetCourses(Guid? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (id != null)
            {
                userId = id.ToString();
            }

            try
            {
                var url = "http://students-services-clip/api/Student/Grades/" + userId.ToString();
                var restClient = new RestClient(url);
                var request = new RestRequest(url, RestSharp.Method.Get);
                RestResponse response = await restClient.ExecuteAsync(request);

                if ((int)response.StatusCode == 200)
                {
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<StudentCourseVM>>(response.Content);

                    if (User.IsInRole("Admin"))
                    {
                        return View("GradeStudents", data.ToList());
                    }
                    else if (User.IsInRole("Student"))
                    {
                        return View("GetCourses", data.ToList());
                    }
                                        
                }
            }
            catch (Exception)
            {

            }
            

            return Problem("oops!");
        }
    }
}
