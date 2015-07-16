using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Query;
using VDS.RDF.Writing;

namespace ExportRdf
{
    /// <summary>
    /// Class to write a graph to a file in the same folder as the binary
    /// </summary>
    static class PersonGraphWriter
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly string _folder;
        private static readonly string _filename;
        public static string Filename { get { return _filename; } }

        private static readonly IRdfWriter writer = new NTriplesWriter();

        private const string EXT = ".nt";

        static PersonGraphWriter()
        {
            _folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _filename = "RdfExport_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + EXT;
            File.Create(_folder + "\\" + _filename);
        }

        /// <summary>
        /// Write a graph to a string and append it to _filename in folder _filder
        /// </summary>
        /// <param name="g">Graph to write</param>
        public static void Write(IGraph g)
        {
            var stringWriter = new System.IO.StringWriter();
            writer.Save(g, stringWriter);
            File.AppendAllText(_folder + "\\" + _filename, stringWriter.ToString());
        }
    }
}
