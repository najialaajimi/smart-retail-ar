using UnityEngine;
using System.IO;
using System;

namespace SmartRetailAR.Utils
{
    public class DataManager : MonoBehaviour
    {
        private static DataManager _instance;
        public static DataManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("DataManager");
                    _instance = go.AddComponent<DataManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        private string _persistentDataPath;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                _persistentDataPath = Application.persistentDataPath;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void SaveData<T>(string filename, T data)
        {
            try
            {
                string json = JsonUtility.ToJson(data, true);
                string filePath = Path.Combine(_persistentDataPath, filename);
                File.WriteAllText(filePath, json);
                Debug.Log($"Data saved to {filePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save data: {e.Message}");
            }
        }

        public T LoadData<T>(string filename) where T : new()
        {
            try
            {
                string filePath = Path.Combine(_persistentDataPath, filename);
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    T data = JsonUtility.FromJson<T>(json);
                    Debug.Log($"Data loaded from {filePath}");
                    return data;
                }
                else
                {
                    Debug.LogWarning($"File not found: {filePath}");
                    return new T();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load data: {e.Message}");
                return new T();
            }
        }

        public bool DataExists(string filename)
        {
            string filePath = Path.Combine(_persistentDataPath, filename);
            return File.Exists(filePath);
        }

        public void DeleteData(string filename)
        {
            try
            {
                string filePath = Path.Combine(_persistentDataPath, filename);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Debug.Log($"Data deleted: {filePath}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete data: {e.Message}");
            }
        }

        public string GetDataPath()
        {
            return _persistentDataPath;
        }
    }
}
