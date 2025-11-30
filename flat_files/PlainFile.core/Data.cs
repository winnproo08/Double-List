using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace PlainFile.core
{
    public class Data
    {
        private string _usersPath;
        private string _peoplePath;
        private string _logPath;


        public Data(string usersPath, string peoplePath, string logPath)
        {
            _usersPath = usersPath;
            _peoplePath = peoplePath;
            _logPath = logPath;

            EnsureFile(_usersPath);
            EnsureFile(_peoplePath);
            EnsureFile(_logPath);
        }

        private void EnsureFile(string path)
        {
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            if (!File.Exists(path))
            {
                using (File.Create(path)) { }
            }
        }

        public IEnumerable<User> LoadUsers()
        {
            foreach (var line in File.ReadAllLines(_usersPath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(',');
                if (parts.Length < 3) continue;

                bool active = false;
                bool.TryParse(parts[2], out active);

                yield return new User
                {
                    Username = parts[0],
                    Password = parts[1],
                    Active = active
                };
            }
        }


        public void SaveUsers(IEnumerable<User> users)
        {
            var lines = users.Select(u => $"{u.Username},{u.Password},{u.Active}");
            File.WriteAllLines(_usersPath, lines);
        }

        public void WriteLog(string user, string action)
        {
            var timestamp = DateTime.Now.ToString("s");
            File.AppendAllText(_logPath, $"[{timestamp}] - [{user}] - {action}{Environment.NewLine}");
        }
    }
}
