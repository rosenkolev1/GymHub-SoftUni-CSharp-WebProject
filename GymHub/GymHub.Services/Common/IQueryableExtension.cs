using GymHub.Common;
using GymHub.Data.Models;
using GymHub.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GymHub.Services.Common
{
    public static class IQueryableExtension
    {
        public static IQueryable<EntityType> IgnoreAllQueryFilters<EntityType>(this DbSet<EntityType> source, bool ignore = false) where EntityType : class
        {
            if (ignore == true) return source.IgnoreQueryFilters();
            return source.AsQueryable();
        }
    }
}
