using Xunit;
using Xunit.Abstractions;

namespace Fanzoo.Kernel.Testing
{
    public class PriorityCollectionOrderer : ITestCollectionOrderer
    {
        public IEnumerable<ITestCollection> OrderTestCollections(IEnumerable<ITestCollection> testCollections)
        {
            var sortedCollections = new SortedDictionary<int, List<ITestCollection>>();

            foreach (var testCollection in testCollections)
            {
                var priority = int.MaxValue;

                priority = testCollection.CollectionDefinition
                    .GetCustomAttributes(typeof(PriorityAttribute).AssemblyQualifiedName)
                        .FirstOrDefault()?
                            .GetNamedArgument<int>("Priority")
                            ?? int.MaxValue;

                GetOrCreate(sortedCollections, priority).Add(testCollection);
            }

            foreach (var list in sortedCollections.Keys.Select(priority => sortedCollections[priority]))
            {
                list.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.CollectionDefinition.Name, y.CollectionDefinition.Name));

                foreach (var testCase in list)
                {
                    yield return testCase;
                }
            }
        }

        private static TValue GetOrCreate<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key)
            where TValue : new()
        {

            if (dictionary.TryGetValue(key, out var result))
            {
                return result;
            }

            result = new TValue();

            dictionary[key] = result;

            return result;
        }
    }
}
