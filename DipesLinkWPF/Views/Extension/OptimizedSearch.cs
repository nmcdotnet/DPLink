namespace DipesLink.Views.Extension
{
    public class OptimizedSearch : IDisposable
    {
        private readonly Dictionary<string, int> _lookup;

        public OptimizedSearch(List<string[]> dbList)
        {
            _lookup = new Dictionary<string, int>();
            for (int i = 0; i < dbList.Count; i++)
            {
                var key = string.Join(",", dbList[i].Take(dbList[i].Length - 1));
                if (!_lookup.ContainsKey(key))
                {
                    _lookup[key] = i;
                }
            }
        }

        public int FindIndexByData(string[] printedCode)
        {
            var key = string.Join(",", printedCode.Take(printedCode.Length - 1));
            return _lookup.TryGetValue(key, out var index) ? index : -1;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
