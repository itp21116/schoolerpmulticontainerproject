using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using SchoolERP.MVC.UI.Data;
using SchoolERP.MVC.UI.Models;
using System.Security.Policy;

namespace SchoolERP.MVC.UI.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            try
            {

                var url = "http://courses-services-clip/api/Course"; //GET THE NAME OF THE CONTAINER
                var restClient = new RestClient(url);
                var request = new RestRequest(url, RestSharp.Method.Get);
                RestResponse response = await restClient.ExecuteAsync(request);
                var output = response.Content;
                if (output != null)
                {
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Course>>(output);
                    if (data != null)
                    {
                        return View(data.ToList());
                    }
                }

            }
            catch (Exception)
            {

            }
            return Problem("oops!");
        }

        public async Task<IActionResult> Details(Guid? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var url = "http://courses-services-clip/api/Course/" + id.ToString(); //GET THE NAME OF THE CONTAINER
                var restClient = new RestClient(url);
                var request = new RestRequest(url, RestSharp.Method.Get);
                RestResponse response = await restClient.ExecuteAsync(request);
                var output = response.Content;
                if (output != null)
                {
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Course>(output);
                    if (data != null)
                    {
                        return View(data);
                    }
                }
            }
            catch (Exception)
            {

            }

            return Problem("oops!");

        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Course course)
        {

            try
            {
                var url = "http://courses-services-clip/api/Course"; //GET THE NAME OF THE CONTAINER
                var restClient = new RestClient(url);
                var request = new RestRequest(url, RestSharp.Method.Post);
                var body = JsonConvert.SerializeObject(course);
                request.AddBody(body, "application/json");

                RestResponse response = await restClient.ExecuteAsync(request);
                var output = response.Content;

                if (response.StatusCode.ToString().Equals("Created"))
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception)
            {

            }
            return Problem("oops!");
        }


        public async Task<IActionResult> Edit(Guid? id)
        {

            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }


            try
            {
                var url = "http://courses-services-clip/api/Course/" + id.ToString(); //GET THE NAME OF THE CONTAINER
                var restClient = new RestClient(url);
                var request = new RestRequest(url, RestSharp.Method.Get);
                RestResponse response = await restClient.ExecuteAsync(request);
                var output = response.Content;
                if ((int)response.StatusCode == 200)
                {
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Course>(output);
                    if (data != null)
                    {
                        return View(data);
                    }
                }
            }
            catch (Exception)
            {

            }
            return Problem("oops!");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {


                try
                {
                    var url = "http://courses-services-clip/api/Course/" + id.ToString(); //GET THE NAME OF THE CONTAINER
                    var restClient = new RestClient(url);
                    var request = new RestRequest(url, RestSharp.Method.Put);
                    var body = JsonConvert.SerializeObject(course);
                    request.AddBody(body, "application/json");

                    RestResponse response = await restClient.ExecuteAsync(request);
                    var output = response.Content;

                    if ((int)response.StatusCode == 200)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {

                }
            }
            return Problem("oops!");
        }

        public async Task<IActionResult> Delete(Guid? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var url = "http://courses-services-clip/api/Course/" + id.ToString(); //GET THE NAME OF THE CONTAINER
                var restClient = new RestClient(url);
                var request = new RestRequest(url, RestSharp.Method.Get);
                RestResponse response = await restClient.ExecuteAsync(request);
                var output = response.Content;
                if ((int)response.StatusCode == 200)
                {
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Course>(output);
                    if (data != null)
                    {
                        return View(data);
                    }
                }
            }
            catch (Exception)
            {

            }
            return Problem("oops!");
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {


            try
            {
                var url = "http://courses-services-clip/api/Course/" + id.ToString(); //GET THE NAME OF THE CONTAINER
                var restClient = new RestClient(url);
                var request = new RestRequest(url, RestSharp.Method.Delete);
                RestResponse response = await restClient.ExecuteAsync(request);
                var output = response.Content;
                if ((int)response.StatusCode == 200)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {

            }

            

            return Problem("oops!");
        }

    }
}
