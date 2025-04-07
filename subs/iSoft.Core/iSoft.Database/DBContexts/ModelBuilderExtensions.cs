using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Database.DBContexts
{
  public static class ModelBuilderExtensions
  {
    public static void ConfigureDateTimeProperties(this ModelBuilder modelBuilder, string columnType)
    {
      foreach (var entityType in modelBuilder.Model.GetEntityTypes())
      {
        foreach (var property in entityType.ClrType.GetProperties())
        {
          if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
          {
            modelBuilder.Entity(entityType.Name).Property(property.Name).HasColumnType(columnType);
          }
        }
      }
    }
  }
}
