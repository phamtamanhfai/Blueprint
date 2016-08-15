using Blueprint.DbOperation;
using Blueprint.Models;
using Blueprint.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blueprint.Controllers
{
    public class HomeController : Controller
    {
        BaseDA db = new BaseDA("YourCSName");
        private string key = "fyF8k91g2q";
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult RandomTest()
        {
            var header = db.GetListTestHeader();
            int r = new Random().Next(0, header.Count);
            return RedirectToAction("DoTest", new { id = header.ElementAt(r).ID });
        }

        public ActionResult DoTest(int id) {

            ViewBag.ListQuestion = db.GetQuestionByHeaderId(id);
            if (((List<QuestionItem>)ViewBag.ListQuestion).Count == 0)
                return View("_NotFound");

            IDictionary<int, List<AnswerItem>> lstAns = new Dictionary<int, List<AnswerItem>>();
            foreach (var item in ViewBag.ListQuestion as List<QuestionItem>)
            {
                lstAns.Add(new KeyValuePair<int, List<AnswerItem>>(item.ID, db.GetAnswerByQuesId(item.ID)));
            }
            ViewBag.ListAnswer = lstAns;
            return View(db.GetTestHeaderById(id));
        }

        public JsonResult CheckResult(int testId)
        {
            string query = "EXEC Test_CheckResult @TestID";
            string result = db.Database.SqlQuery<string>(query,
                    new SqlParameter("TestID", testId)
                ).First();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult YourScore(int testId, int noCorrect)
        {
            ViewBag.NoCorrect = noCorrect;
            return View(db.GetTestHeaderById(testId));
        }

        public ActionResult About() {
            return View();
        }

        public ActionResult Enquire()
        {
            ViewBag.Courses = db.GetListCourses().Where(item => item.Status.Equals("A")).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult PostEnquire(FormCollection frm)
        {
            string name = frm["Name"];
            string CourseID = frm["CourseID"];
            string address = frm["Address"];
            string phone = frm["Phone"];
            string email = frm["Email"];
            DateTime birthDate = DateTime.Parse(frm["BirthDate"]);
            string purpose = frm["Purpose"];
            db.AddNewStudent(name, address, phone, email, birthDate, purpose, CourseID);
            TempData["Msg"] = "Bạn đã đăng ký thành công.";
            return RedirectToAction("Enquire","Home");
        }

        public PartialViewResult RenderCoursesMenu()
        {
            var model = db.GetListCourses().Where(item => item.Status.Equals("A")).ToList();
            return PartialView("pvMenu_Courses", model);
        }

        public PartialViewResult RenderCourses()
        {
            var model = db.GetListCourses().Where(item => item.Status.Equals("A")).ToList();
            return PartialView("pvCourse", model);
        }

        [CourseFilter]
        public ActionResult Course(int id = 0)
        {
            if (id == 0)
            {
                var model = db.GetListCourses().Where(item => item.Status.Equals("A")).ToList();
                return View(model);
            }
            else {
                var model = db.GetCourseById(id);
                return View("MyCourse", model);
            }
        }

        public ActionResult Feedback()
        {
            var model = db.GetListFeedback();
            return View(model);
        }

        public ActionResult FeedbackDetail(string id)
        {
            var model = db.GetFeedbackById(id);
            return View(model);
        }

        [Authorize]
        public ActionResult Homework()
        {
            var model = db.GetListHomework();
            return View(model);
        }

        [Authorize]
        public ActionResult Download(string file)
        {
            var fileLocation = Server.MapPath("/Upload/Homework/" + file);
            var filecontent = System.IO.File.ReadAllBytes(fileLocation);
            return File(filecontent, System.Net.Mime.MediaTypeNames.Application.Octet, file);
        }

        public ActionResult Whatever(string k) {

            string fileName = db.DecryptQueryString(k, key, "emm@nuel1984");
            string js_file = Directory.GetFiles(Server.MapPath("~/Scripts/Home/"), fileName.Split('\0').First()).First();
            var file = new FileInfo(js_file);
            using (var stream = new StreamReader(file.OpenRead()))
            {
                return JavaScript(stream.ReadToEnd());
            }
        }
    }
}