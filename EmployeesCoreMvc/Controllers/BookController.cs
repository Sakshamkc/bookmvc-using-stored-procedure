using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeesCoreMvc.Data;
using EmployeesCoreMvc.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeesCoreMvc.Controllers
{
    public class BookController : Controller
    {
        private readonly IConfiguration _configuration;

        public BookController(IConfiguration configuration)
        {
           this._configuration = configuration;
        }
        
        public IActionResult Index()
        {
            DataTable da = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sda = new SqlDataAdapter("BOOKVIEWALL", sqlConnection);
                sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                sda.Fill(da);
            }
            return View(da);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int ?id)
        {
            BookViewModel bookViewModel = new BookViewModel();
            if(id>0)
            {
                bookViewModel = FetchById(id);
            }
            return View(bookViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(int id, [Bind("BookID,Title,Author,Price")] BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlCommand sda = new SqlCommand("BookAddOrEdit", sqlConnection);
                    sda.CommandType = CommandType.StoredProcedure;
                    sda.Parameters.AddWithValue("BookID", bookViewModel.BookID);
                    sda.Parameters.AddWithValue("Author", bookViewModel.Author);
                    sda.Parameters.AddWithValue("Title", bookViewModel.Title);
                    sda.Parameters.AddWithValue("Price", bookViewModel.Price);
                    sda.ExecuteNonQuery();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bookViewModel);
           
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            BookViewModel bookViewModel = FetchById(id);
            return View(bookViewModel);
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlCommand sda = new SqlCommand("BookDeleteById", sqlConnection);
                sda.CommandType = CommandType.StoredProcedure;
                sda.Parameters.AddWithValue("BookID", id);
                sda.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));
        }
        [NonAction]

        public BookViewModel FetchById(int? id)
        {
            BookViewModel bookViewModel = new BookViewModel();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                DataTable da = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sdd = new SqlDataAdapter("BOOKVIEWBYID", sqlConnection);
                sdd.SelectCommand.CommandType = CommandType.StoredProcedure;
                sdd.SelectCommand.Parameters.AddWithValue("BOOKID", id);
                sdd.Fill(da);
                if(da.Rows.Count==1)
                {
                    bookViewModel.BookID = Convert.ToInt32(da.Rows[0]["BookID"].ToString());
                    bookViewModel.Author = da.Rows[0]["Author"].ToString();
                    bookViewModel.Title = da.Rows[0]["Title"].ToString();
                    bookViewModel.Price = Convert.ToInt32(da.Rows[0]["Price"].ToString());
                }
                return bookViewModel;
            }

        }
    }
}
