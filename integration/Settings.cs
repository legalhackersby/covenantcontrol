using System;
using Xunit;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Diagnostics;

namespace integration
{
    public class Settings
    {
        public static int Port => 56248;
        public static string Host => "http://localhost:" + Settings.Port + "/";
    }
}
