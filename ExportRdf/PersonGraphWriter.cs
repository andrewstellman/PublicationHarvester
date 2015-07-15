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
    class PersonGraphWriter
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly string folder;
        private readonly string filename;

        private readonly IRdfWriter writer = new NTriplesWriter();

        private const string EXT = ".nt";

        public PersonGraphWriter()
        {
            folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            filename = "RdfExport_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + EXT;
            logger.Info("Writing files to " + filename);
            File.Create(folder + "\\" + filename);

            /*
            outputFolder = workingFolder + "\\RdfExport_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            if (!Directory.Exists(outputFolder))
            {
                logger.Info("Creating output folder \"" + outputFolder + "\"");
                Directory.CreateDirectory(outputFolder);
            }
             */


        }


        public void Write(IGraph g)
        {
            /*
            SparqlResultSet results = g.ExecuteQuery(@"
PREFIX person: <http://www.stellman-greene.com/person#>
SELECT ?setnb { 
   ?person a person:Person .
   ?person person:setnb ?setnb .
}
ORDER BY ?setnb
"
                ) as SparqlResultSet;

            string startingSetnb = results[0]["setnb"].ToString();
            string endingSetnb = results[results.Count - 1]["setnb"].ToString();
            string filename = String.Format("{0} to {1} ({2} people).{3}", startingSetnb, endingSetnb, results.Count, EXT);

            logger.Info(String.Format("Writing file \"{0}\"", filename));
            */

            var stringWriter = new System.IO.StringWriter();
            writer.Save(g, stringWriter);
            File.AppendAllText(folder + "\\" + filename, stringWriter.ToString());
        }
    }
}
