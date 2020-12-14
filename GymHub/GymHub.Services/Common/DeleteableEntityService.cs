using GymHub.Data;
using GymHub.Data.Data;
using System;
using System.Threading.Tasks;

namespace GymHub.Services.Common
{
    public class DeleteableEntityService
    {
        protected readonly ApplicationDbContext context;

        public DeleteableEntityService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task DeleteEntityAsync<EntityType>(EntityType entity)
            where EntityType : IDeletableEntity
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            await this.context.SaveChangesAsync();
        }
    }
}
