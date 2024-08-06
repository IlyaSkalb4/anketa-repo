using anketa_webapi_app.Services.Abstract;
using AnketaDatabaseLibrary.Data;
using AnketaDatabaseLibrary.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace anketa_webapi_app.Services.Implementation
{
	public class UserService : IEntityService<User, string>
	{
		private readonly AnketaDatabaseContext context;

		public UserService(AnketaDatabaseContext context)
		{
			this.context = context;
		}

		public void CreateEntityAsync(User entity)
		{
			throw new NotImplementedException();
		}

		public void DeleteEntityAsync(string id)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<User>> GetEntitiesAsync(int numberOfEntities)
		{
			IEnumerable<User> users = await context.Users.Take(numberOfEntities).ToListAsync();

			return users;
		}

		public User GetEntityAsync(string id)
		{
			throw new NotImplementedException();
		}

		public void UpdateEntityAsync(User entity)
		{
			throw new NotImplementedException();
		}
	}
}
