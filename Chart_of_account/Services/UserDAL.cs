using Chart_of_account.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace CRUD_ADO.NET.Services
{
    public class UserDAL
    {
        private readonly string _connectionString;
        public int AccountClassId { get; private set; }
        public string? AccountTitle { get; private set; }
        public string? AccountClassTitle { get; private set; }

        public UserDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool CreateAccount(User_Login login)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("Sp_InsertUser", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FullName", login.FullNamee);
                cmd.Parameters.AddWithValue("@UserName", login.UserName);
                cmd.Parameters.AddWithValue("@Email", login.Email);
                cmd.Parameters.AddWithValue("@Password", login.Password);
                cmd.Parameters.AddWithValue("@Photo", login.Photo);
                con.Open();
                int r = cmd.ExecuteNonQuery();

                if (r > 0)
                {
                    string imagePath = "https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-registration/draw1.webp";
                    string mailbody = @"
        <html>
            <head>
                <style>
                    body {
                    font-family: Arial, sans-serif;
                    background-color: #f2f2f2;
                    margin: 0;
                    padding: 0;
                    }

                    .container {
                    max-width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                    background-color: #ffffff;
                    border-radius: 10px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                    }

                    h1 {
                    color: #333333;
                    margin-bottom: 10px;
                    }

                    p {
                    font-size: 16px;
                    line-height: 1.5;
                    }

                    table {
                    border-collapse: collapse;
                    width: 100%;
                    border: 1px solid #ccc;
                    }

                    th, td {
                    padding: 10px;
                    border: 1px solid #ccc;
                    }

                    th {
                    background-color: #f2f2f2;
                    font-weight: bold;
                    }

                    /* Additional styles */
                    .note {
                    font-style: italic;
                    color: #888888;
                    }

                    .footer {
                    margin-top: 20px;
                    text-align: center;
                    font-size: 12px;
                    color: #555555;
                    }
                </style>
            </head>
        <body>
            <div class=""container"">
                <h1>Hello " + login.FullNamee + @"</h1>
                <p>Here are your credentials as a thank you for signing up. Please keep them safe for any upcoming contact.</p>
                <table>
                    <tr>
                        <td>UserName:</td>
                        <td>" + login.UserName + @"</td>
                    </tr>
                    <tr>
                        <td>Password:</td>
                        <td>" + login.Password + @"</td>
                    </tr>
                </table>
                 <img src=""" + imagePath + @""" alt=""Your Image"" style=""max-width: 100%; margin-top:20px"">
            </div>
            <p class=""footer"">© 2023 AirCod. All rights reserved.</p>
        </body>
    </html>";
                SendEmail(login, mailbody);
                return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool SignIn(User_Login login)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("Sp_login", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", login.UserName);
                    cmd.Parameters.AddWithValue("@Password", login.Password);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void SendEmail(User_Login user, string body)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("zainaircod@outlook.com");
            message.To.Add(user.Email);
            message.Subject = "We appreciate your interest, and you have registered.";
            message.Body = body;
            message.IsBodyHtml = true;
            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.Host = "smtp.outlook.com";
                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential("zainaircod@outlook.com", "Corona2020!");
                smtp.EnableSsl = true;
                smtp.Send(message);
            }
        }

        public bool GetPasswordByEmail(User_Login login)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_forgot", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@email", login.Email);
                con.Open();
                // Execute the stored procedure and get the password
                object result = cmd.ExecuteScalar();
                con.Close();
                if (result != null)
                {
                    string password = result.ToString();
                    login.Password = password;
                    string mailbody = "Hello your Password is " + login.Password;
                    ForgotPassword(login, mailbody);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void ForgotPassword(User_Login user, string body)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("zainaircod@outlook.com");
            message.To.Add(user.Email);
            message.Subject = "We appreciate your interest Here is your Password";
            message.Body = body;
            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.Host = "smtp.outlook.com";
                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential("zainaircod@outlook.com", "Titspiheu1");
                smtp.EnableSsl = true;
                smtp.Send(message);
            }
        }

        public bool CreatetheChart(Combine_Model model)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("Sp_InsertValues", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AccountClassId", model.AccountClassId);
            cmd.Parameters.AddWithValue("@AccountTitle", model.AccountTitle);
            con.Open();
            int r = cmd.ExecuteNonQuery();
            if (r > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Combine_Model> GetList()
        {
            List<Combine_Model> users = new List<Combine_Model>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("Sp_ChartAccount", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    users.Add(new Combine_Model
                    {
                        AccountClassId = Convert.ToInt32(dr["AccountClassId"]),
                        AccountTitle = dr["AccountTitle"].ToString(),
                        AccountClassTitle = dr["AccountClassTitle"].ToString(),
                    });
                }
                return users;
            }
        }

        public List<Combine_Model> ShowList()
        {
            List<Combine_Model> users = new List<Combine_Model>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("Sp_List", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    users.Add(new Combine_Model
                    {
                        AccountClassId = Convert.ToInt32(dr["AccountClassId"]),
                        AccountClassTitle = dr["AccountClassTitle"].ToString(),
                    });
                }
                return users;
            }
        }

        public bool CreatetheVoucher(CombineVoucherModel model)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("InsertVoucher", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@VoucherTypeId", model.VoucherTypeId);
            cmd.Parameters.AddWithValue("@TotalAmount", model.TotalAmount);
            cmd.Parameters.AddWithValue("@From", model.From);
            cmd.Parameters.AddWithValue("@To", model.To);
            cmd.Parameters.AddWithValue("@Description", model.Description);
            cmd.Parameters.AddWithValue("@CreatedOn", model.CreatedOn);

            con.Open();
            int r = cmd.ExecuteNonQuery();
            if (r > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<CombineVoucherModel> VoucherList()
        {
            List<CombineVoucherModel> users = new List<CombineVoucherModel>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("Sp_VoucherType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    users.Add(new CombineVoucherModel
                    {
                        VoucherTypeId = Convert.ToInt32(dr["VoucherTypeId"]),
                        VoucherTypeTitle = dr["VoucherTypeTitle"].ToString(),
                    });
                }
                return users;
            }
        }

        public List<CombineVoucherModel> AccountNameList()
        {
            List<CombineVoucherModel> users = new List<CombineVoucherModel>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("Sp_AccountTitle", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    users.Add(new CombineVoucherModel
                    {
                        AccountClassId = Convert.ToInt32(dr["AccountId"]),
                        AccountTitle = dr["AccountTitle"].ToString(),
                    });
                }
                return users;
            }
        }

        public List<CombineVoucherModel> GetListVoucher()
        {
            List<CombineVoucherModel> users = new List<CombineVoucherModel>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("Sp_ShowVoucher", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    users.Add(new CombineVoucherModel
                    {
                        VoucherId = Convert.ToInt32(dr["VoucherId"]),
                        VoucherTypeTitle = dr["VoucherTypeTitle"].ToString(),
                        TotalAmount = Convert.ToInt32(dr["TotalAmount"]),
                        Description = dr["Description"].ToString(),
                    });
                }
                return users;
            }
        }
    }
}