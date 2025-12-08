using UnityEngine;
using System.Collections.Generic;

namespace SmartRetailAR.Optimization
{
    public class MemoryManager : MonoBehaviour
    {
        private static MemoryManager _instance;
        public static MemoryManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("MemoryManager");
                    _instance = go.AddComponent<MemoryManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        [Header("Memory Settings")]
        public float memoryCheckInterval = 5f;
        public float memoryThresholdMB = 500f;
        public bool autoCleanup = true;

        [Header("Cache Settings")]
        public int maxCachedTextures = 50;
        public int maxCachedPrefabs = 20;

        private float _lastCheckTime;
        private Dictionary<string, Texture2D> _textureCache = new Dictionary<string, Texture2D>();
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        private Queue<string> _textureCacheOrder = new Queue<string>();
        private Queue<string> _prefabCacheOrder = new Queue<string>();

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        void Update()
        {
            if (Time.time - _lastCheckTime >= memoryCheckInterval)
            {
                CheckMemoryUsage();
                _lastCheckTime = Time.time;
            }
        }

        private void CheckMemoryUsage()
        {
            float memoryUsedMB = GetMemoryUsageMB();
            
            if (memoryUsedMB > memoryThresholdMB && autoCleanup)
            {
                Debug.LogWarning($"Memory usage high: {memoryUsedMB:F2} MB. Performing cleanup...");
                PerformCleanup();
            }
        }

        public float GetMemoryUsageMB()
        {
            return System.GC.GetTotalMemory(false) / (1024f * 1024f);
        }

        public void PerformCleanup()
        {
            // Clear old cache entries
            ClearOldCacheEntries();

            // Unload unused assets
            Resources.UnloadUnusedAssets();

            // Force garbage collection
            System.GC.Collect();

            Debug.Log($"Memory cleanup completed. Current usage: {GetMemoryUsageMB():F2} MB");
        }

        public void CacheTexture(string key, Texture2D texture)
        {
            if (_textureCache.ContainsKey(key))
                return;

            if (_textureCache.Count >= maxCachedTextures)
            {
                // Remove oldest
                string oldestKey = _textureCacheOrder.Dequeue();
                if (_textureCache.ContainsKey(oldestKey))
                {
                    Destroy(_textureCache[oldestKey]);
                    _textureCache.Remove(oldestKey);
                }
            }

            _textureCache[key] = texture;
            _textureCacheOrder.Enqueue(key);
        }

        public Texture2D GetCachedTexture(string key)
        {
            return _textureCache.ContainsKey(key) ? _textureCache[key] : null;
        }

        public void CachePrefab(string key, GameObject prefab)
        {
            if (_prefabCache.ContainsKey(key))
                return;

            if (_prefabCache.Count >= maxCachedPrefabs)
            {
                // Remove oldest
                string oldestKey = _prefabCacheOrder.Dequeue();
                if (_prefabCache.ContainsKey(oldestKey))
                {
                    Destroy(_prefabCache[oldestKey]);
                    _prefabCache.Remove(oldestKey);
                }
            }

            _prefabCache[key] = prefab;
            _prefabCacheOrder.Enqueue(key);
        }

        public GameObject GetCachedPrefab(string key)
        {
            return _prefabCache.ContainsKey(key) ? _prefabCache[key] : null;
        }

        private void ClearOldCacheEntries()
        {
            // Clear half of texture cache
            int texturesToRemove = _textureCache.Count / 2;
            for (int i = 0; i < texturesToRemove && _textureCacheOrder.Count > 0; i++)
            {
                string key = _textureCacheOrder.Dequeue();
                if (_textureCache.ContainsKey(key))
                {
                    Destroy(_textureCache[key]);
                    _textureCache.Remove(key);
                }
            }

            // Clear half of prefab cache
            int prefabsToRemove = _prefabCache.Count / 2;
            for (int i = 0; i < prefabsToRemove && _prefabCacheOrder.Count > 0; i++)
            {
                string key = _prefabCacheOrder.Dequeue();
                if (_prefabCache.ContainsKey(key))
                {
                    Destroy(_prefabCache[key]);
                    _prefabCache.Remove(key);
                }
            }
        }

        public void ClearAllCaches()
        {
            foreach (var texture in _textureCache.Values)
            {
                if (texture != null)
                    Destroy(texture);
            }
            _textureCache.Clear();
            _textureCacheOrder.Clear();

            foreach (var prefab in _prefabCache.Values)
            {
                if (prefab != null)
                    Destroy(prefab);
            }
            _prefabCache.Clear();
            _prefabCacheOrder.Clear();

            Debug.Log("All caches cleared");
        }

        public MemoryInfo GetMemoryInfo()
        {
            return new MemoryInfo
            {
                totalMemoryMB = GetMemoryUsageMB(),
                cachedTextures = _textureCache.Count,
                cachedPrefabs = _prefabCache.Count,
                systemMemoryMB = SystemInfo.systemMemorySize
            };
        }

        [System.Serializable]
        public class MemoryInfo
        {
            public float totalMemoryMB;
            public int cachedTextures;
            public int cachedPrefabs;
            public int systemMemoryMB;
        }
    }
}
