using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.Linq;
using System.Threading;

namespace xUnitMockApi.Extensions
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Used to reset the identity column auto-increment to its default value. This is to workaround the issue
        /// in the InMemoryDatabase implementation, where identity columns will not be reset to 1 for new databases.
        /// See: https://github.com/aspnet/EntityFrameworkCore/issues/6872#issuecomment-258025241
        /// </summary>
        /// <param name="context">Context to reset value generators for</param>
        public static void ResetValueGenerators(this DbContext context)
        {
            var cache = context.GetService<IValueGeneratorCache>();

            var keyProperties =
                context.Model.GetEntityTypes()
                .Select(e => e.FindPrimaryKey().Properties[0])
                .Where(p => p.ClrType == typeof(int) && p.ValueGenerated == ValueGenerated.OnAdd);

            foreach (var keyProperty in keyProperties)
            {
                var generator = cache.GetOrAdd(
                    keyProperty,
                    keyProperty.DeclaringEntityType,
                    (p, e) => new ResettableValueGenerator());

                var resettableValueGenerator = generator as ResettableValueGenerator;
                resettableValueGenerator?.Reset();
            }
        }
    }

    public class ResettableValueGenerator : ValueGenerator<int>
    {
        private int _current;

        public override bool GeneratesTemporaryValues => false;

        public override int Next(EntityEntry entry)
            => Interlocked.Increment(ref _current);

        public void Reset() => _current = 0;
    }
}
