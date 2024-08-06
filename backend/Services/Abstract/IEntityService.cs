namespace anketa_webapi_app.Services.Abstract
{
	public interface IEntityService<T, T2> where T : class where T2 : notnull
	{
		Task<IEnumerable<T>> GetEntitiesAsync(int numberOfEntities = 10);

		T GetEntityAsync(T2 id);

		void CreateEntityAsync(T entity);

		void UpdateEntityAsync(T entity);

		void DeleteEntityAsync(T2 id);
	}
}
