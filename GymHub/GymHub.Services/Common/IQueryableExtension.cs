using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GymHub.Services.Common
{
    public static class IQueryableExtension
    {
        public static IQueryable<EntityType> IgnoreAllQueryFilter<EntityType>(this DbSet<EntityType> source, bool ignore = false) where EntityType : class
        {
            if (ignore == true) return source.IgnoreQueryFilters();
            return source.AsQueryable();
        } 
    }
}
