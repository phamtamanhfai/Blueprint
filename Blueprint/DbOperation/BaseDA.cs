using Blueprint.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Blueprint.DbOperation
{
    public class BaseDA : DbContext
    {
        public BaseDA(string connectionString)
            : base(connectionString)
        {
        }

        public bool CheckLogin(string username, string password) {
            string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password AND Status = 'A'";
            int result = Database.SqlQuery<int>(query, 
                    new SqlParameter("Username", username),
                    new SqlParameter("Password", password)
                ).First();
            return result == 1;
        }

        public bool ChangePassword(string username, string newPassword)
        {
            string query = "UPDATE Users SET Password = @Password WHERE Username = @Username";
            var result = Database.ExecuteSqlCommand(query,
                    new SqlParameter("Username", username),
                    new SqlParameter("Password", newPassword)
                );
            return true;
        }

        public List<TestHeader> GetListTestHeader() {
            string query = "SELECT * FROM Test_Header WHERE Status = 'A'";
            var result = Database.SqlQuery<TestHeader>(query);

            return result.ToList();
        }

        public TestHeader GetTestHeaderById(int id)
        {
            string query = "EXEC Test_GetById @ID";
            var result = Database.SqlQuery<TestHeader>(query,
                    new SqlParameter("ID", id)
                );

            return result.FirstOrDefault();
        }

        public List<QuestionItem> GetQuestionByHeaderId(int id)
        {
            string subquery = "SELECT * FROM Questions WHERE IDHeader = @ID";
            string query = "SELECT CONVERT(INT, DENSE_RANK()OVER(ORDER BY a.ID)) AS STT, a.* FROM (" + subquery + ") a";
            
            var result = Database.SqlQuery<QuestionItem>(query,
                    new SqlParameter("ID", id)
                );

            return result.ToList();
        }

        public List<AnswerItem> GetAnswerByQuesId(int id)
        {
            string query = "SELECT * FROM Answers WHERE IDQuestion = @ID";
            var result = Database.SqlQuery<AnswerItem>(query,
                    new SqlParameter("ID", id)
                );

            return result.ToList();
        }

        public int SetStatus(int id, string entity, string status) {

            string query = "";
            switch (entity) { 
                case "Test":
                    if (GetQuestionByHeaderId(id).Count == 0) {
                        return -1;
                    }
                    query = "UPDATE Test_Header SET Status = @Status WHERE ID = @ID";
                    
                    break;
                case "Courses":
                    query = "UPDATE Courses SET Status = @Status WHERE ID = @ID";
                    break;
                case "User":
                    query = "UPDATE Users SET Status = @Status WHERE ID = @ID";
                    break;
                case "Feedback":
                    query = "UPDATE Feedbacks SET Status = @Status WHERE ID = @ID";
                    break;
            }


            int result = Database.ExecuteSqlCommand(query,
                        new SqlParameter("Status", status),
                        new SqlParameter("ID", id)
                    );

            return result;
        }


        public int AddNewTest(TestHeader item) {
            string query = "INSERT INTO Test_Header(Title, NoQuestions, Status) VALUES(@Title, @NoQues,'D')";
            int result = Database.ExecuteSqlCommand(query,
                    new SqlParameter("Title", item.Title),
                    new SqlParameter("NoQues", item.NoQuestions)
                );

            return Database.SqlQuery<int>("SELECT MAX(ID) FROM Test_Header").First();
        }

        public int AddNewCourses(CoursesItem item)
        {
            string query = "INSERT INTO Courses(Title,  Subject,  Duration,  Content,  Image)" + 
                                        "VALUES(@Title, @Subject, @Duration, @Content, @Image)";
            int result = Database.ExecuteSqlCommand(query,
                    new SqlParameter("Title", item.Title),
                    new SqlParameter("Subject", item.Subject),
                    new SqlParameter("Duration", item.Duration),
                    new SqlParameter("Content", item.Content),
                    new SqlParameter("Image", item.Image??(object)DBNull.Value)
                );

            return result;
        }

        public int AddNewStudent(string name, string address, string phone, string email, DateTime birthDate, string purpose, string courseId)
        {
            string query = "INSERT INTO Students(Name,  Address,  Phone,  Email,  BirthDate,  Purpose,  CourseID) " +
                                        "VALUES(@Name, @Address, @Phone, @Email, @BirthDate, @Purpose, @CourseID)";
            int result = Database.ExecuteSqlCommand(query,
                    new SqlParameter("Name", name),
                    new SqlParameter("Address", address),
                    new SqlParameter("Phone", phone),
                    new SqlParameter("Email", email),
                    new SqlParameter("BirthDate", birthDate),
                    new SqlParameter("Purpose", purpose),
                    new SqlParameter("CourseID", courseId)
                );

            return result;
        }

        public int AddNewFeedback(FeedbackItem item)
        {
            string query = "INSERT INTO Feedbacks (StudentName,  CourseID,  Career, Quote, Content, Image)" +
                                        "VALUES(@StudentName, @CourseID, @Career, @Quote, @Content, @Image)";
            int result = Database.ExecuteSqlCommand(query,
                    new SqlParameter("StudentName", item.StudentName),
                    new SqlParameter("CourseID", item.CourseID),
                    new SqlParameter("Career", item.Career),
                    new SqlParameter("Quote", item.Quote),
                    new SqlParameter("Content", item.Content),
                    new SqlParameter("Image", item.Image ?? (object)DBNull.Value)
                );

            return result;
        }

        public int AddNewHomework(HomeworkItem item)
        {
            string query = "INSERT INTO Homework (Title,  Filename)" +
                                        "VALUES(@Title, @Filename)";
            int result = Database.ExecuteSqlCommand(query,
                    new SqlParameter("Title", item.Title),
                    new SqlParameter("Filename", item.Filename)
                );

            return result;
        }

        public HomeworkItem GetHomework(int id)
        {
            string query = "SELECT * FROM Homework WHERE ID = @ID";
            var result = Database.SqlQuery<HomeworkItem>(query,
                    new SqlParameter("ID", id)).ToList().First();
            return result;
        }

        public int DeleteHomework(int id)
        {
            string query = "DELETE Homework WHERE ID = @ID";
            int result = Database.ExecuteSqlCommand(query,
                    new SqlParameter("ID", id)
                );

            return result;
        }

        public List<CoursesItem> GetListCourses()
        {
            string query = "SELECT * FROM Courses";
            var result = Database.SqlQuery<CoursesItem>(query);

            return result.ToList();
        }

        public List<HomeworkItem> GetListHomework()
        {
            string query = "SELECT * FROM Homework";
            var result = Database.SqlQuery<HomeworkItem>(query);

            return result.ToList();
        }

        public List<FeedbackItem> GetListFeedback()
        {
            string query = "SELECT F.ID, F.StudentName, F.CourseID, C.Title AS CourseName, F.Career, F.Quote, F.Content, " +  
                "F.Image, F.DateCreated, F.Status FROM Feedbacks F INNER JOIN Courses C ON C.ID = F.CourseID WHERE F.Status = 'A'";
            var result = Database.SqlQuery<FeedbackItem>(query);
            return result.ToList();
        }

        public FeedbackItem GetFeedbackById(string id)
        {
            string query = "SELECT * FROM Feedbacks WHERE ID = '" + id + "'";
            var result = Database.SqlQuery<FeedbackItem>(query);
            return result.ToList().First();
        }

        public CoursesItem GetCourseById(int id)
        {
            string query = "SELECT * FROM Courses WHERE ID = '" + id + "'";
            var result = Database.SqlQuery<CoursesItem>(query);

            return result.First();
        }

        public int UpdateCourse(CoursesItem item)
        {
            string query = "UPDATE Courses Set Title = @Title, Subject = @Subject, Duration = @Duration," +
                                               "Content = @Content, Image = @Image WHERE ID = @ID";
            int result = Database.ExecuteSqlCommand(query,
                new SqlParameter("ID", item.ID),    
                new SqlParameter("Title", item.Title),
                new SqlParameter("Subject", item.Subject),
                new SqlParameter("Duration", item.Duration),
                new SqlParameter("Content", item.Content),
                new SqlParameter("Image", item.Image??(object)DBNull.Value)
            );

            return result;
        }

        public UserInfoItem GetUserInfo(string username)
        {
            string query = "SELECT * FROM Users WHERE Username = '" + username + "'";
            var result = Database.SqlQuery<UserInfoItem>(query);

            return result.First();
        }

        public List<StudentItem> FindStudent(int id, string name, string courseId, DateTime? fromDate, DateTime? toDate, string status)
        {
            string query = "EXEC Student_GetList @ID, @Name, @CourseID, @FromDate, @ToDate, @Status";
            var result = Database.SqlQuery<StudentItem>(query,
                new SqlParameter("ID", id),
                new SqlParameter("Name", name == null ? (object)DBNull.Value : name),
                new SqlParameter("CourseID", courseId == null ? (object)DBNull.Value : courseId),
                new SqlParameter("FromDate", fromDate ?? (object)DBNull.Value),
                new SqlParameter("ToDate", toDate ?? (object)DBNull.Value),
                new SqlParameter("Status", status == null ? (object)DBNull.Value : status)
            ).ToList();
            return result;
        }

        public void Injection(string email)
        {
            string query = "SELECT * FROM Injection WHERE Email = @email";
            var result = Database.ExecuteSqlCommand(query, new SqlParameter("email", email));

        }

        public string EncryptQueryString(string inputText, string key, string salt)
        {
            byte[] plainText = Encoding.UTF8.GetBytes(inputText);

            using (RijndaelManaged rijndaelCipher = new RijndaelManaged())
            {
                PasswordDeriveBytes secretKey = new PasswordDeriveBytes(Encoding.ASCII.GetBytes(key), Encoding.ASCII.GetBytes(salt));
                using (ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor(secretKey.GetBytes(32), secretKey.GetBytes(16)))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainText, 0, plainText.Length);
                            cryptoStream.FlushFinalBlock();
                            string base64 = Convert.ToBase64String(memoryStream.ToArray());

                            // Generate a string that won't get screwed up when passed as a query string.
                            string urlEncoded = HttpUtility.UrlEncode(base64);
                            return urlEncoded;
                        }
                    }
                }
            }
        }

        public string DecryptQueryString(string inputText, string key, string salt)
        {
            byte[] encryptedData = Convert.FromBase64String(inputText);
            PasswordDeriveBytes secretKey = new PasswordDeriveBytes(Encoding.ASCII.GetBytes(key), Encoding.ASCII.GetBytes(salt));

            using (RijndaelManaged rijndaelCipher = new RijndaelManaged())
            {
                using (ICryptoTransform decryptor = rijndaelCipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16)))
                {
                    using (MemoryStream memoryStream = new MemoryStream(encryptedData))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            byte[] plainText = new byte[encryptedData.Length];
                            cryptoStream.Read(plainText, 0, plainText.Length);
                            string utf8 = Encoding.UTF8.GetString(plainText);
                            return utf8;
                        }
                    }
                }
            }
        }
    }
}