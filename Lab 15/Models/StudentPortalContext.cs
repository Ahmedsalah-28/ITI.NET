using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab15_StudentPortalWeb.Models
{


    public class StudentPortalContext : DbContext
    {

        public StudentPortalContext(DbContextOptions<StudentPortalContext> options)
           : base(options)
        {

        }

        public DbSet<Student> Students { get; set; } 
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .Property(s => s.FullName)
                .IsRequired()
                .HasMaxLength(100); // Fluent Api 

            modelBuilder.Entity<Course>() // course
                .HasOne(c => c.Instructor) // connected to one instructor
                .WithMany(i => i.Courses) // can teach many courses
                .HasForeignKey(c => c.InstructorId) // link on instructor id
                .OnDelete(DeleteBehavior.SetNull); 


        }

    }
}