using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;
using vueProject.Models;

namespace vueProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            SELECT DepartmentId, DepartmentName
                            FROM Department";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mySqlCon = new MySqlConnection(sqlDataSource))
            {
                mySqlCon.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlCon))
                {
                    myReader = mySqlCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Department dep)
        {
            string query = @"
                            INSERT INTO Department (DepartmentName)
                            VALUES (@DepartmentName)
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mySqlCon = new MySqlConnection(sqlDataSource))
            {
                mySqlCon.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlCon))
                {
                    mySqlCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
                    myReader = mySqlCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mySqlCon.Close();
                }
            }

            return new JsonResult("Added Sucessfully.");
        }

        [HttpPut]
        public JsonResult Put(Department dep)
        {
            string query = @"
                            UPDATE Department
                            SET DepartmentName = @DepartmentName
                            WHERE DepartmentId = @DepartmentId
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mySqlCon = new MySqlConnection(sqlDataSource))
            {
                mySqlCon.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlCon))
                {
                    mySqlCommand.Parameters.AddWithValue("@DepartmentId", dep.DepartmentId);
                    mySqlCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
                    myReader = mySqlCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mySqlCon.Close();
                }
            }

            return new JsonResult("Updated Sucessfully.");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                            DELETE FROM Department
                            WHERE DepartmentId = @DepartmentId
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mySqlCon = new MySqlConnection(sqlDataSource))
            {
                mySqlCon.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlCon))
                {
                    mySqlCommand.Parameters.AddWithValue("@DepartmentId", id);
                    myReader = mySqlCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mySqlCon.Close();
                }
            }

            return new JsonResult("Deleted Sucessfuly.");
        }
    }
}
