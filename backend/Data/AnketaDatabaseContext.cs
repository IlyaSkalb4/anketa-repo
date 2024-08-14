using Microsoft.EntityFrameworkCore;
using backend.Data.Models;

namespace AnketaDatabaseLibrary.Data
{
	public class AnketaDatabaseContext : DbContext
	{
		public AnketaDatabaseContext(DbContextOptions<AnketaDatabaseContext> options) : base(options) { }

		public DbSet<Questionnaire> Questionnaires { get; set; }
		public DbSet<Question> Questions { get; set; }
		public DbSet<Answer> Answers { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<ApplicationUser>(entity =>
			{
				entity.ToTable("Users");

				entity.Property(u => u.Id).HasColumnName("Id"); 
				entity.Property(u => u.Email).HasColumnName("Email");
				entity.Property(u => u.LastLocation).HasColumnName("LastLocation");
				entity.Property(u => u.Name).HasColumnName("Name");
				entity.Property(u => u.Surname).HasColumnName("Surname");
				entity.Property(u => u.Sex).HasColumnName("Sex");
				entity.Property(u => u.Weight).HasColumnName("Weight");
				entity.Property(u => u.Height).HasColumnName("Height");
				entity.Property(u => u.Age).HasColumnName("Age");
				entity.Property(u => u.DateOfChange).HasColumnName("DateOfChange");

				entity.HasMany(u => u.Answers)
					  .WithMany(a => a.Users)
					  .UsingEntity(j => j.ToTable("UserAnswers"));

				entity.HasMany(u => u.Questionnaires)
					  .WithOne(q => q.User)
					  .HasForeignKey(q => q.UserId);
			});

			builder.Entity<Answer>(entity =>
			{
				entity.ToTable("Answers");

				entity.Property(a => a.Id).HasColumnName("Id");
				entity.Property(a => a.TextValue).HasColumnName("TextValue");
				entity.Property(a => a.FloatValue).HasColumnName("FloatValue");
				entity.Property(a => a.BooleanValue).HasColumnName("BooleanValue");

				entity.HasMany(a => a.Questions)
					  .WithMany(q => q.Answers)
					  .UsingEntity(j => j.ToTable("QuestionAnswers"));

				entity.HasMany(a => a.Users)
					  .WithMany(u => u.Answers)
					  .UsingEntity(j => j.ToTable("UserAnswers"));
			});

			builder.Entity<Question>(entity =>
			{
				entity.ToTable("Questions");

				entity.Property(q => q.Id).HasColumnName("Id");
				entity.Property(q => q.QuestionText).HasColumnName("QuestionText");

				entity.HasMany(q => q.Answers)
					  .WithMany(a => a.Questions)
					  .UsingEntity(j => j.ToTable("QuestionAnswers"));

				entity.HasMany(q => q.Questionnaires)
					  .WithMany(qnr => qnr.Questions)
					  .UsingEntity(j => j.ToTable("QuestionnaireQuestions"));
			});

			builder.Entity<Questionnaire>(entity =>
			{
				entity.ToTable("Questionnaires");

				entity.Property(qnr => qnr.Id).HasColumnName("Id");
				entity.Property(qnr => qnr.NumberOfPasses).HasColumnName("NumberOfPasses");
				entity.Property(qnr => qnr.Title).HasColumnName("Title");
				entity.Property(qnr => qnr.Description).HasColumnName("Description");
				entity.Property(qnr => qnr.UserId).HasColumnName("UserId");
				entity.Property(qnr => qnr.DateOfCreation).HasColumnName("DateOfCreation");

				entity.HasOne(qnr => qnr.User)
					  .WithMany(u => u.Questionnaires)
					  .HasForeignKey(qnr => qnr.UserId);

				entity.HasMany(qnr => qnr.Questions)
					  .WithMany(q => q.Questionnaires)
					  .UsingEntity(j => j.ToTable("QuestionnaireQuestions"));
			});
		}

	}
}
