using System;
using System.IO;
using System.IO.Compression;
using UnityEngine;

namespace SortingGame.Core
{
    /// <summary>Where the save lives. Local file now; cloud save is [AÇIK] (GDD 15.4) and plugs in here.</summary>
    public interface ISaveStorage
    {
        string Read();
        void Write(string content);
        void Delete();
    }

    public class FileSaveStorage : ISaveStorage
    {
        readonly string _path;

        public FileSaveStorage(string fileName)
        {
            _path = Path.Combine(Application.persistentDataPath, fileName);
        }

        public string Read() => File.Exists(_path) ? File.ReadAllText(_path) : null;

        public void Write(string content)
        {
            // Write-then-replace so a crash mid-write never leaves a half file.
            var temp = _path + ".tmp";
            File.WriteAllText(temp, content);
            if (File.Exists(_path)) File.Delete(_path);
            File.Move(temp, _path);
        }

        public void Delete()
        {
            if (File.Exists(_path)) File.Delete(_path);
        }
    }

    /// <summary>JSON (de)serialisation and helpers. Pure apart from the storage it is given.</summary>
    public class SaveSystem
    {
        /// <summary>Tests switch this so they never touch the player's save.</summary>
        public static string FileName = "save.json";

        readonly ISaveStorage _storage;

        public SaveSystem(ISaveStorage storage) => _storage = storage;

        public static SaveSystem CreateDefault() => new(new FileSaveStorage(FileName));

        public SaveData Load()
        {
            try
            {
                var json = _storage.Read();
                if (string.IsNullOrEmpty(json)) return null;
                var data = JsonUtility.FromJson<SaveData>(json);
                if (data == null || data.Version > SaveData.CurrentVersion) return null;
                return data;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Save] Could not read save, starting fresh: {e.Message}");
                return null;
            }
        }

        public void Save(SaveData data)
        {
            data.Version = SaveData.CurrentVersion;
            data.SavedAtUtc = DateTime.UtcNow.ToString("o");
            _storage.Write(JsonUtility.ToJson(data));
        }

        public void Delete() => _storage.Delete();

        public static string Pack(byte[] bytes)
        {
            using var output = new MemoryStream();
            using (var gzip = new GZipStream(output, System.IO.Compression.CompressionLevel.Fastest))
                gzip.Write(bytes, 0, bytes.Length);
            return Convert.ToBase64String(output.ToArray());
        }

        public static byte[] Unpack(string packed)
        {
            using var input = new MemoryStream(Convert.FromBase64String(packed));
            using var gzip = new GZipStream(input, CompressionMode.Decompress);
            using var output = new MemoryStream();
            gzip.CopyTo(output);
            return output.ToArray();
        }
    }
}
