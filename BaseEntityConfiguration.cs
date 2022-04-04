using Microsoft.EntityFrameworkCore;
using DBE.ENERGY.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DBE.ENERGY.Infrastructure.Data
{
    public static class BaseEntityConfiguration
    {
        static void Configure<TEntity, T>(ModelBuilder modelBuilder)
            where TEntity : BaseEntity<T>
        {
            modelBuilder.Entity<TEntity>(builder =>
            {

                // Id is nonclustered
                builder.HasKey(e => e.Id).ForSqlServerIsClustered(false);

                // Id value will be generated
                builder.Property(e => e.Id).ValueGeneratedOnAdd();
            });
        }

        static void ConfigureQueryFilter<TEntity, T>(ModelBuilder modelBuilder) where TEntity : BaseEntity<T>
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
        }

        public static ModelBuilder ApplyBaseEntityConfiguration(this ModelBuilder modelBuilder)
        {

            //var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            //        v => v.ToUniversalTime(),
            //        v => DateTime.SpecifyKind(v, DateTimeKind.Local));

            //var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
            //    v => v.HasValue ? v.Value.ToUniversalTime() : v,
            //    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Local) : v);

           
            var configureMethod = typeof(BaseEntityConfiguration).GetTypeInfo().DeclaredMethods
                .Single(m => m.Name == nameof(Configure));
            var queryFilterMethod = typeof(BaseEntityConfiguration).GetTypeInfo().DeclaredMethods
                .Single(m => m.Name == nameof(ConfigureQueryFilter));
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.IsBaseEntity(out var T))
                {
                    configureMethod.MakeGenericMethod(entityType.ClrType, T).Invoke(null, new[] { modelBuilder });
                    queryFilterMethod.MakeGenericMethod(entityType.ClrType, T).Invoke(null, new[] { modelBuilder });
                }

               
                //foreach (var property in entityType.GetProperties())
                //{
                //    if (property.ClrType == typeof(DateTime))
                //    {
                //        property.SetValueConverter(dateTimeConverter);
                //    }
                //    else if (property.ClrType == typeof(DateTime?))
                //    {
                //        property.SetValueConverter(nullableDateTimeConverter);
                //    }
                //}
            }
            return modelBuilder;
        }

        static bool IsBaseEntity(this Type type, out Type T)
        {
            for (var baseType = type.BaseType; baseType != null; baseType = baseType.BaseType)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(BaseEntity<>))
                {
                    T = baseType.GetGenericArguments()[0];
                    return true;
                }
            }
            T = null;
            return false;
        }
    }
}
