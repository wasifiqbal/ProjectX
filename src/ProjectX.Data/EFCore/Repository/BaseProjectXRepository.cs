using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjectX.Core.Commons.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ProjectX.Data.EFCore
{
	public class BaseProjectXRepository<TEntity, TPrimaryKey> : IProjectXRepository<TEntity, TPrimaryKey> where TEntity : class where TPrimaryKey : struct
	{
		private readonly ProjectXDbContext _context;
		private readonly DbSet<TEntity> _table;

		public BaseProjectXRepository(ProjectXDbContext context)
		{
			_context = context;
			_table = _context.Set<TEntity>();
		}

		public IQueryable<TEntity> GetAll()
		{
			return _table.AsQueryable();
		}

		public async Task<TEntity> GetByPrimaryKeyAsync(TPrimaryKey input)
		{
			return await _table.FindAsync(input);
		}

		public async Task<TEntity> InsertAsync(TEntity entity)
		{
			Validator.ValidateObject(entity, new ValidationContext(entity));

			await _table.AddAsync(entity);
			await SaveChangesAsync();
			return entity;
		}

		public async Task<TEntity> UpdateAsync(TEntity entity)
		{
			Validator.ValidateObject(entity, new ValidationContext(entity));

			_table.Attach(entity);
			_context.Entry(entity).State = EntityState.Modified;
			await SaveChangesAsync();
			return entity;
		}
		public async Task<int> DeleteAsync(TPrimaryKey input)
		{
			TEntity existing = _table.Find(input);
			_table.Remove(existing);
			return await SaveChangesAsync();
		}

		protected async Task<int> SaveChangesAsync()
		{
			var changedEntities = ApplyTrackingConcepts();
			var result = await _context.SaveChangesAsync();
			return result;
		}

		private Dictionary<string, string> ApplyTrackingConcepts()
		{
			var changes = new Dictionary<string, string>();
			foreach (var entry in _context.ChangeTracker.Entries().ToList())
			{
				ApplyTrackingConcepts(entry, changes);
			}
			return changes;
		}

		private void ApplyTrackingConcepts(EntityEntry entry, Dictionary<string, string> changes)
		{
			switch (entry.State)
			{
				case EntityState.Added: CreationConcept(entry, changes); break;
				case EntityState.Modified: ModificationConcept(entry, changes); break;
				case EntityState.Deleted: DeletionConcept(entry, changes); break;
			}
		}
		private void CreationConcept(EntityEntry entry, Dictionary<string, string> changes)
		{
			if (entry.Entity is not ICreationTime)
				return;

			var modified = entry.Entity as ICreationTime;
			modified.CreationTime = DateTime.UtcNow;
		}
		private void ModificationConcept(EntityEntry entry, Dictionary<string, string> changes)
		{
			if (entry.Entity is not IModificationTime)
				return;

			var modified = entry.Entity as IModificationTime;
			modified.ModificationTime = DateTime.UtcNow;
			RecordTrackingChanges(entry, changes);
		}
		private void DeletionConcept(EntityEntry entry, Dictionary<string, string> changes)
		{
			if (entry.Entity is not IDeletion)
				return;

			var modified = entry.Entity as IDeletion;
			modified.IsDeleted = true;
			modified.DeletionTime = DateTime.UtcNow;
			RecordTrackingChanges(entry, changes);
		}
		private void RecordTrackingChanges(EntityEntry entry, Dictionary<string, string> changes)
		{
			foreach (var props in entry.CurrentValues.Properties)
			{
				if (entry.OriginalValues[props] is null && entry.CurrentValues[props] is not null)
					changes.Add(props.Name, $"{entry.CurrentValues[props]}");
				else if (entry.CurrentValues[props]?.Equals(entry.OriginalValues[props]) == false)
					changes.Add(props.Name, $"{entry.OriginalValues[props]}=>{entry.CurrentValues[props]}");
			}
		}
	}

}
