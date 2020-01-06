using System;
using System.IO;
using Newtonsoft.Json;
using Lit.Os.Fsys;

namespace Lit.Os.Win.Fsys
{
    public class Fsys : IFsys
    {
        public string Concat(string path, string file)
        {
            return path + (path[path.Length - 1] == '\\' ? "" : "\\") + file;
        }

        public string[] GetFiles(string path, string searchPattern)
        {
            return Directory.GetFiles(path, searchPattern);
        }

        public bool FileExists(string fileName)
        {
            return File.Exists(fileName);
        }

        public string GetFilePath(string fileName)
        {
            return Path.GetDirectoryName(fileName);
        }

        public string GetFileName(string fileName)
        {
            return Path.GetFileName(fileName);
        }

        public string GetFileExtension(string fileName, bool trimDot = false)
        {
            var ext = Path.GetExtension(fileName);
            if (trimDot && ext.Length > 0 && ext[0] == '.')
            {
                ext = ext.Substring(1);
            }

            return ext;
        }

        public string GetFileNameWithoutExtension(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName);
        }

        public string FileReadAllText(string fileName)
        {
            return File.ReadAllText(fileName);
        }

        public void FileWriteAllText(string fileName, string text)
        {
            File.WriteAllText(fileName, text);
        }

        public object FileOpenToWrite(string fileName, bool append)
        {
            return new StreamWriter(fileName, append);
        }

        public void FileClose(object file)
        {
            if (file is StreamWriter w)
            {
                w.Close();
                w.Dispose();
            }
        }

        public bool FileWrite(object writer, string text, bool sync)
        {
            if (writer is StreamWriter w)
            {
                w.Write(text);
                if (sync)
                {
                    w.Flush();
                }

                return true;
            }

            return false;
        }

        public void DirectoryCreate(string path)
        {
            Directory.CreateDirectory(path);
        }

        public T JsonDecode<T>(string text)
        {
            return JsonConvert.DeserializeObject<T>(text);
        }

        public object JsonDecode(Type type, string text)
        {
            return JsonConvert.DeserializeObject(text, type);
        }

        public string JsonEncode(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
