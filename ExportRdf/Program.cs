using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.StellmanGreene.PubMed;
using VDS.RDF.Storage.Management;
using VDS.RDF.Storage;

namespace ExportRdf
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("usage: ExportRdf odbc_database sesame_server sesame_repositry");
                Console.WriteLine();
                Console.WriteLine("   example: ExportRdf stars_for_stars http://localhost:8080/openrdf-sesame StarsForStars");
                return -1;
            }

            Database db = new Database(args[0]);
            SesameHttpProtocolConnector sesame = new SesameHttpProtocolConnector(args[1], args[2]);

            RdfExporter rdfExporter = new RdfExporter(db, 10, sesame);
            rdfExporter.ExportRdf();
            return 0;
        }
    }
}
