using System;

namespace Lit.Os.Fsys
{
    public interface IFsys
    {
        string Concat(string path, string file);

        string[] GetFiles(string path, string searchPattern);

        bool FileExists(string fileName);

        string GetFilePath(string fileName);

        string GetFileName(string fileName);

        string GetFileExtension(string fileName, bool trimDot = false);

        string GetFileNameWithoutExtension(string fileName);

        string FileReadAllText(string fileName);

        void FileWriteAllText(string fileName, string text);

        object FileOpenToWrite(string fileName, bool append);

        void FileClose(object file);

        bool FileWrite(object writer, string text, bool sync);

        T JsonDecode<T>(string text);

        object JsonDecode(Type type, string text);

        string JsonEncode(object obj);

        void DirectoryCreate(string path);
    }
}
