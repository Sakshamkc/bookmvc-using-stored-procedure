using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmployeesCoreMvc.Models;

namespace EmployeesCoreMvc.Data
{
    public class EmployeesCoreMvcContext : DbContext
    {
        public EmployeesCoreMvcContext (DbContextOptions<EmployeesCoreMvcContext> options)
            : base(options)
        {

        }

        public DbSet<EmployeesCoreMvc.Models.BookViewModel> BookViewModel { get; set; }
    }
}
