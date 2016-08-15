using Blueprint.DbOperation;
using Blueprint.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Blueprint.Controllers
{
    public class AdminController : Controller
    {

        BaseDA db = new BaseDA("YourCSName");
        private string key = "fyF8k91g2q";
        // GET: Admin
        public ActionResult Index()
        {
            return View("Login");
        }

        public ActionResult Login() {
            return View();
        }

        [HttpPost]
        public ActionResult CheckLogin(UserLoginRequest loginModel)
        {
            if (ModelState.IsValid) {
                if (db.CheckLogin(loginModel.Username, loginModel.Password))
                {
                    FormsAuthentication.SetAuthCookie(loginModel.Username, false);
                    var rdUrl = FormsAuthentication.GetRedirectUrl(loginModel.Username, false);
                    return Redirect(rdUrl);
                }
                else {
                    TempData["Msg"] = "Tên đăng nhập hoặc mật khẩu không đúng.";
                }
            }
            return View("Login");
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult ChangePass()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePass(ChangePassItem model)
        {
            db.ChangePassword(model.Username, model.NewPassword);
            FormsAuthentication.SignOut();
            TempData["Msg"] = "Mật khẩu cập nhật thành công.";
            return RedirectToAction("Login");
        }

        [Authorize]
        public ActionResult CheckPass(string username, string password)
        {
            if (ModelState.IsValid)
            {
                if (db.CheckLogin(username, password))
                    return Content("1");
            }
            return Content("0");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Test()
        {
            return View(db.GetListTestHeader());
        }

        [Authorize(Roles = "Admin")]
        public int Lock(int id, string entity) {
            return db.SetStatus(id,entity, "D");
        }

        [Authorize(Roles = "Admin")]
        public int Unlock(int id, string entity)
        {
            return db.SetStatus(id,entity, "A");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult NewTest() {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AddNewTest(TestHeader item)
        {
            int newId = 0;
            if (ModelState.IsValid) {
                newId = db.AddNewTest(item);
            }
            return RedirectToAction("EditQuestion", new { testId = newId});
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditQuestion(int testId = -1)
        {
            if (testId == -1)
                return View("_NotFound");

            ViewBag.ListQuestion = db.GetQuestionByHeaderId(testId);
            IDictionary<int, List<AnswerItem>> lstAns = new Dictionary<int, List<AnswerItem>>();
            foreach (var item in ViewBag.ListQuestion as List<QuestionItem>)
            {
                lstAns.Add(new KeyValuePair<int, List<AnswerItem>>(item.ID, db.GetAnswerByQuesId(item.ID)));
            }
            ViewBag.ListAnswer = lstAns;
            var model = db.GetTestHeaderById(testId);
            if(model == null)
                return View("_NotFound");

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public JsonResult ModifiedQuestion(int Id, int IdHeader, string Content, string Hint)
        {
            string query = "EXEC Question_Modify @Id, @IdHeader, @Content, @Hint";
            int result = db.Database.SqlQuery<int>(query,
                    new SqlParameter("Id", Id),
                    new SqlParameter("IdHeader", IdHeader),
                    new SqlParameter("Content", Content),
                    new SqlParameter("Hint", Hint)
                ).First();
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        public void ModifiedAnswer(int Id, int IdQuestion, string Content, bool IsCorrect)
        {
            string query = "EXEC Answer_Modify @Id, @IdQuestion, @Content, @IsCorrect";
            try
            {
                int result = db.Database.ExecuteSqlCommand(query,
                    new SqlParameter("Id", Id),
                    new SqlParameter("IdQuestion", IdQuestion),
                    new SqlParameter("Content", Content),
                    new SqlParameter("IsCorrect", IsCorrect ? "1" : "0")
                );
            }
            catch (Exception e)
            {
                int a = 1;
                
            }
        }

        [Authorize(Roles="Admin")]
        public ActionResult Courses() {
            return View(db.GetListCourses());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult NewCourses()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AddNewCourses(CoursesItem item)
        {
            HttpPostedFileBase file = Request.Files["image"];
            if (file.FileName != string.Empty)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                var fileName = DateTime.Now.ToString("yyyyMMddhhmmssffffff");
                var fileLocation = Server.MapPath("/Upload/Courses/" + fileName + fileExtension);
                file.SaveAs(fileLocation);
                item.Image = fileName + fileExtension;
            }
            if (ModelState.IsValid)
            {
                db.AddNewCourses(item);
            }
            return RedirectToAction("Courses");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditCourses(int id = -1)
        {
            if (id  == -1)
                return View("EditCourses", null);
            
            return View(db.GetCourseById(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EditCourses(CoursesItem item)
        {
            HttpPostedFileBase file = Request.Files["image"];
            if (file.FileName != string.Empty)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                var fileName = DateTime.Now.ToString("yyyyMMddhhmmssffffff");
                string path = Server.MapPath("/Upload/Courses/");
                if(item.Image != null)
                    System.IO.File.Delete(path + item.Image);
                file.SaveAs(path + fileName + fileExtension);
                item.Image = fileName + fileExtension;
            }

            db.UpdateCourse(item);
            return RedirectToAction("Courses");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Feedback()
        {
            ViewBag.Courses = db.GetListCourses().Where(item => item.Status.Equals("A")).ToList();
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetListFeedback(string name, string courseId, DateTime? fromDate, DateTime? toDate, string status)
        {

            List<FeedbackItem> model = new List<FeedbackItem>();
            if (fromDate != null && toDate != null)
            {
                string query = "EXEC [dbo].[Feedback_GetList] @Name, @CourseID, @StartDate, @EndDate, @Status";
                model = db.Database.SqlQuery<FeedbackItem>(query,
                    new SqlParameter("Name", name),
                    new SqlParameter("CourseID", courseId),
                    new SqlParameter("StartDate", fromDate),
                    new SqlParameter("EndDate", toDate),
                    new SqlParameter("Status", status)
                ).ToList();
            }
            
            return PartialView("pvListFeedback", model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult NewFeedback()
        {
            var dropdownOptions = db.GetListCourses().Where(item => item.Status.Equals("A"))
                .Select(d => new
                {
                    CourseID = d.ID,
                    Title = d.Title
                });

            ViewBag.DropDownCourses = new SelectList(dropdownOptions, "CourseID", "Title");
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AddNewFeedback(FeedbackItem item)
        {
            HttpPostedFileBase file = Request.Files["image"];
            if (file.FileName != string.Empty)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                var fileName = DateTime.Now.ToString("yyyyMMddhhmmssffffff");
                var fileLocation = Server.MapPath("/Upload/Feedback/" + fileName + fileExtension);
                file.SaveAs(fileLocation);
                item.Image = fileName + fileExtension;
            }
            if (ModelState.IsValid)
            {
                db.AddNewFeedback(item);
            }
            return RedirectToAction("Feedback");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult User()
        {
            
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetListUser(string name, string role, string status)
        {
            string query = "EXEC [dbo].[User_GetList] @Name, @Role, @Status";
            var model = db.Database.SqlQuery<UserInfoItem>(query,
                new SqlParameter("Name",name),
                new SqlParameter("Role", role),
                new SqlParameter("Status", status)
                ).Where(item => item.IsOwner == false).ToList();
            return PartialView("pvListUser", model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Client()
        {
            ViewBag.Courses = db.GetListCourses().Where(item => item.Status.Equals("A")).ToList();
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetListStudent(int id, string  name, string courseId, DateTime fromDate, DateTime toDate, string status) {

            var model = db.FindStudent(id, name, courseId, fromDate, toDate, status);
            return PartialView("pvListStudent",model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetStudent(int id)
        {
            var model = db.FindStudent(id, null, null, null, null, null);
            return Json(model.SingleOrDefault(), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        public int SetStudent(int id,string status)
        {
            string query = "UPDATE Students SET Status = @Status WHERE ID = @ID";
            int result = db.Database.ExecuteSqlCommand(query,
                new SqlParameter("Status", status),
                new SqlParameter("ID", id)
            );

            int userId = db.FindStudent(id, null, null, null, null, null).SingleOrDefault().UserID.Value;
            if (status.Equals("Q")) {
                db.SetStatus(userId, "User", "D");
            }
            else if (status.Equals("A"))
            {
                db.SetStatus(userId, "User", "A");
            }

            return result;
        }

        [Authorize(Roles = "Admin")]
        public int AddNewAccount(string userName, string role, long studentId)
        {
            string query = "EXEC [dbo].[User_AddNewAccount] @Username, @Role, @StudentId";
            int result = db.Database.ExecuteSqlCommand(query,
                new SqlParameter("Username", userName),
                new SqlParameter("Role", role),
                new SqlParameter("StudentId", studentId)
            );
           
            return result;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ModalNewUser()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public void ExportStudent(FormCollection frm)
        {
            string name = frm["Name"];
            string courId = frm["Course"];
            DateTime fromDate = DateTime.Parse(frm["FromDate"]);
            DateTime toDate = DateTime.Parse(frm["ToDate"]);
            string status = frm["Status"];

            var report = db.FindStudent(0, name, courId, fromDate, toDate, status);

            using (var excelPackage = new ExcelPackage()) {
                
                var sheet = excelPackage.Workbook.Worksheets.Add("Báo cáo");
                if (report.Count == 0 || report == null)
                {
                    sheet.Cells[1, 1].Value = "Không tìm thấy dữ liệu";
                }
                else 
                { 
                    //Write header
                    sheet.Cells[1, 1].Value = "STT";
                    sheet.Cells[1, 2].Value = "Tên học viên";
                    sheet.Cells[1, 3].Value = "Khóa học";
                    sheet.Cells[1, 4].Value = "Địa chỉ";
                    sheet.Cells[1, 5].Value = "SĐT";
                    sheet.Cells[1, 6].Value = "Email";
                    sheet.Cells[1, 7].Value = "Ngày sinh";
                    sheet.Cells[1, 8].Value = "Mục tiêu";
                    sheet.Cells[1, 9].Value = "Ngày đăng ký";
                    sheet.Cells[1, 10].Value = "Trạng thái";

                    sheet.Cells[1, 1, 1, 10].Style.Font.Bold = true;
                    sheet.Cells[1, 1, 1, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    sheet.Cells[1, 1, 1, 10].Style.Border.Bottom.Style = 
                        sheet.Cells[1, 1, 1, 10].Style.Border.Left.Style = 
                        sheet.Cells[1, 1, 1, 10].Style.Border.Right.Style = 
                        sheet.Cells[1, 1, 1, 10].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    //Write data
                    for (int i = 1; i <= report.Count; i++)
                    {
                        sheet.Cells[i + 1, 1].Value = i.ToString();
                        sheet.Cells[i + 1, 2].Value = report.ElementAt(i - 1).Name;
                        sheet.Cells[i + 1, 3].Value = report.ElementAt(i - 1).CourseName;
                        sheet.Cells[i + 1, 4].Value = report.ElementAt(i - 1).Address;
                        sheet.Cells[i + 1, 5].Value = report.ElementAt(i - 1).Phone;
                        sheet.Cells[i + 1, 6].Value = report.ElementAt(i - 1).Email;
                        sheet.Cells[i + 1, 7].Value = report.ElementAt(i - 1).BirthDate.ToString("dd-MM-yyyy");
                        sheet.Cells[i + 1, 8].Value = report.ElementAt(i - 1).Purpose;
                        sheet.Cells[i + 1, 9].Value = report.ElementAt(i - 1).DateCreated.ToString("dd-MM-yyyy");
                        switch (report.ElementAt(i - 1).Status) { 
                            case "P":
                                sheet.Cells[i + 1, 10].Value = "Chưa kích hoạt";
                                break;
                            case "A":
                                sheet.Cells[i + 1, 10].Value = "Đang hoạt động";
                                break;
                            case "Q":
                                sheet.Cells[i + 1, 10].Value = "Đã nghỉ";
                                break;
                        }
                    }

                    sheet.Cells[2, 1, report.Count() + 1, 10].Style.Border.Bottom.Style = 
                        sheet.Cells[2, 1, report.Count() + 1, 10].Style.Border.Left.Style = 
                        sheet.Cells[2, 1, report.Count() + 1, 10].Style.Border.Right.Style = 
                        sheet.Cells[2, 1, report.Count() + 1, 10].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    sheet.Cells[1, 1, report.Count() + 1, 10].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                    sheet.Cells.AutoFitColumns();
                }

                //Export excel
                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.Charset = "UTF-8";
                Response.AddHeader("content-disposition",
                          "attachment;filename=StudentReport.xlsx");
                Response.ContentType = "application/excel";
                Response.Flush();
                Response.End();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Homework()
        {
            List<HomeworkItem> model = db.GetListHomework();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult NewHomework()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AddNewHomework(HomeworkItem model)
        {
            List<string> delfile = Request.Params["delfile"].ToString().Split(new char[]{','}).ToList();
            delfile.RemoveAt(0);
            string saveFilename = "";
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                if (delfile.Count > 0 && i.ToString().Equals(delfile.First())) {
                    delfile.RemoveAt(0);
                    continue;
                }

                var fileExtension = Path.GetExtension(file.FileName);
                var fileName = DateTime.Now.ToString("yyyyMMddhhmmssffffff");
                var fileLocation = Server.MapPath("/Upload/Homework/" + fileName + fileExtension);
                file.SaveAs(fileLocation);
                saveFilename += "," + fileName + fileExtension;
            }

            model.Filename = saveFilename.Substring(1, saveFilename.Length - 1);
            db.AddNewHomework(model);

            return RedirectToAction("Homework");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteHomework(int id)
        {
            HomeworkItem item = db.GetHomework(id);
            db.DeleteHomework(id);
            TempData["Msg"] = "Xóa thành công";
            List<string> fileNames = item.Filename.Split(new char[] { ',' }).ToList();
            foreach (var name in fileNames)
            {
                var fileLocation = Server.MapPath("/Upload/Homework/" + name);
                System.IO.File.Delete(fileLocation);
            }

            return RedirectToAction("Homework");
        }

        public ActionResult Whatever(string k)
        {

            string fileName = db.DecryptQueryString(k, key, "emm@nuel1984");
            string js_file = Directory.GetFiles(Server.MapPath("~/Scripts/Admin/"), fileName.Split('\0').First()).First();
            var file = new FileInfo(js_file);
            using (var stream = new StreamReader(file.OpenRead()))
            {
                return JavaScript(stream.ReadToEnd());
            }
        }
    }
}