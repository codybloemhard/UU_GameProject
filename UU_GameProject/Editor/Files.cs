using System;
using System.Collections.Generic;
using System.Text;
//<author:cody>
//Copy of:
//https://github.com/ocdy1001/CodeAnalyser/blob/master/CodeAnalyser/Files.cs
namespace UU_GameProject
{
    public static class Files
    {
        private static List<string> l;

        static Files()
        {
            l = new List<string>();
        }

        public static string FormatToDir(string path)
        {
            StringBuilder builder = new StringBuilder(path);
            builder.Replace("/", "\\");
            return builder.ToString();
        }

        //https://stackoverflow.com/questions/12332451/list-all-files-and-directories-in-a-directory-subdirectories
        public static string[] AllFiles(string dir)
        {
            string[] allfiles = System.IO.Directory.GetFiles(dir, "*.*", System.IO.SearchOption.AllDirectories);
            return allfiles;
        }

        public static bool HasExtension(string s, string extension)
        {
            string ex = "";
            bool read = true;
            for(int i = s.Length - 1; i >= 0; i--)
            {
                if (s[i] == '.') break;
                ex += s[i];
            }
            string ext = "";
            for(int i = ex.Length - 1; i >= 0; i--)
                ext += ex[i];
            if (ext == extension) return true;
            return false;
        }

        public static string[] AllFilesOfExtension(string dir, string ex)
        {
            string[] all = AllFiles(dir);
            l.Clear();
            foreach (string s in all) if (HasExtension(s, ex)) l.Add(s);
            return l.ToArray();
        }

        public static string[] ExcludeFromDir(string[] dirs, string[] files)
        {
            l.Clear();
            foreach (string s in files)
            {
                bool ok = true;
                foreach (string d in dirs)
                    if (s.Contains(d))
                    {
                        ok = false;
                        break;
                    }
                if (ok) l.Add(s);
            }
            return l.ToArray();
        }

        public static string RemoveStartDirString(string dir, string file)
        {
            StringBuilder builder = new StringBuilder(file);
            builder.Replace(dir, "");
            return builder.ToString();
        }

        public static string[] RemoveStartDirString(string dir, string[] files)
        {
            string[] newfiles = new string[files.Length];
            for (int j = 0; j < files.Length; j++)
                newfiles[j] = RemoveStartDirString(dir, files[j]);
            return newfiles;
        }

        public static void PrintFile(string startdir, string file)
        {
            Console.WriteLine(RemoveStartDirString(startdir, file));
        }

        public static void PrintFiles(string startdir, string[] files)
        {
            files = Files.RemoveStartDirString(startdir, files);
            foreach (string f in files) Console.WriteLine(f);
        }

        public static string ReadAll(string file)
        {
            return System.IO.File.ReadAllText(file);
        }
    }
}