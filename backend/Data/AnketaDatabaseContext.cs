
using AnketaDatabaseLibrary.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AnketaDatabaseLibrary.Data
{
	public class AnketaDatabaseContext : DbContext
	{
		public AnketaDatabaseContext(DbContextOptions<AnketaDatabaseContext> options) : base(options) { }

		public DbSet<User> Users { get; set; }

		public DbSet<Questionnaire> Questionnaires { get; set; }

		public DbSet<Question> Questions { get; set; }

		public DbSet<Answer> Answers { get; set; }
	} 
}
