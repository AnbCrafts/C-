using System.Data.Entity;

namespace JQueryPractice.Models
{
    public class AssignmentDBEntities : DbContext
    {
        public AssignmentDBEntities() : base("name=AssignmentDBEntities")
        {
            Database.SetInitializer<AssignmentDBEntities>(null);
        }

        public virtual DbSet<Doctor> Doctors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
