using System;
using System.IO;

namespace Mockingjay.Common.Storage
{
    public static class EndpointDatafile
    {
        public static string Directory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Mockingjay");
        public static string Filename => "Mockingjay.db";
        public static string FullPath => Path.Combine(Directory, Filename);
    }
}
