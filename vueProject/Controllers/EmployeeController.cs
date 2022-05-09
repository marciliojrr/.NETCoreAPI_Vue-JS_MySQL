using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Data;
using System.IO;
using vueProject.Models;

namespace vueProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            SELECT EmployeeId, EmployeeName, Department, DATE_FORMAT(DateOfJoining, '%d-%M-%Y') as DateOfJoining, PhotoFileName
                            FROM Employee
                            ";

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
        public JsonResult Post(Employee emp)
        {
            string query = @"
                            INSERT INTO Employee (EmployeeName, Department, DateOfJoining, PhotoFileName)
                            VALUES (@EmployeeName, @Department, @DateOfJoining, @PhotoFileName)
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mySqlCon = new MySqlConnection(sqlDataSource))
            {
                mySqlCon.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlCon))
                {
                    mySqlCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    mySqlCommand.Parameters.AddWithValue("@Department", emp.Department);
                    mySqlCommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                    mySqlCommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
                    myReader = mySqlCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mySqlCon.Close();
                }
            }

            return new JsonResult("Added Sucessfully.");
        }

        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            string query = @"
                            UPDATE Employee
                            SET EmployeeName = @EmployeeName,
                                Department = @Department,
                                DateOfJoining = @DateOfJoining,
                                PhotoFileName = @PhotoFileName
                            WHERE EmployeeId = @EmployeeId
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mySqlCon = new MySqlConnection(sqlDataSource))
            {
                mySqlCon.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlCon))
                {
                    mySqlCommand.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
                    mySqlCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    mySqlCommand.Parameters.AddWithValue("@Department", emp.Department);
                    mySqlCommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                    mySqlCommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
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
                            DELETE FROM Employee
                            WHERE EmployeeId = @EmployeeId
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mySqlCon = new MySqlConnection(sqlDataSource))
            {
                mySqlCon.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlCon))
                {
                    mySqlCommand.Parameters.AddWithValue("@EmployeeId", id);
                    myReader = mySqlCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mySqlCon.Close();
                }
            }

            return new JsonResult("Deleted Sucessfuly.");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {

                return new JsonResult("anounymous.png");
            }
        }
    }
}
