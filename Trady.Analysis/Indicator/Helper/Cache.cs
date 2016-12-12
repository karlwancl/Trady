using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Core;

namespace Trady.Analysis.Indicator.Helper
{
    internal class Cache<TTick> where TTick: ITick
    {
        private List<TTick> _cache;
        private int _cacheSize;
        private readonly object _syncLock = new object();

        public Cache(int size = 128)
        {
            _cache = new List<TTick>();
            _cacheSize = size;
        }

        public int CacheSize
        {
            get
            {
                return _cacheSize;
            }
            set
            {
                if (_cache.Count > value)
                    for (int i = 0; i < _cache.Count - value; i++)
                        _cache.RemoveAt(0);
                _cacheSize = value;
            }
        }

        public TTick GetFromCacheOrDefault(DateTime dateTime)
            => _cache.FirstOrDefault(t => t.DateTime.Equals(dateTime));

        public void AddToCache(TTick tick)
        {
            //var items = _cache.Where(r => r.DateTime.Equals(tick.DateTime));
            //if (items != null)
            //    foreach (var item in items)
            //        _cache.Remove(item);

            if (_cache.Count > _cacheSize)
                for (int i = 0; i < _cache.Count - _cacheSize + 1; i++)
                    _cache.RemoveAt(0);

            _cache.Add(tick);
        }
    }
}
