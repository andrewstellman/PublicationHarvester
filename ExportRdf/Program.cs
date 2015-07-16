using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.StellmanGreene.PubMed;
using VDS.RDF.Storage.Management;
using VDS.RDF.Storage;
using NLog;
using System.Reflection;

namespace ExportRdf
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static int Main(string[] args)
        {
            if (args.Length != 1 && args.Length != 3)
            {
                Console.WriteLine("ExportRdf " + Assembly.GetExecutingAssembly().GetName().Version);
                Console.WriteLine(@"Export Publication Harvester data to an RDF NTriples file
that can be imported into an RDF store such as Sesame

usage: ExportRdf odbc_data_source [sesame_server sesame_repositry]

parameters:
  odbc_data_source:  ODBC data source name
  sesame_server:     optional Sesame server URL (reqiures repository as well)
  sesame_repository: optional Sesame repository name
                     if Sesame server and repository are specified, previously 
                     imported people and publications will be skipped.

example usage - exporting stars and loading them into a Sesame repsotory:
+---
| C:\Temp>ExportRdf stars_for_stars http://localhost:8080/openrdf-sesame SforS
| 2015-07-15 17:19:35 Exporting RDF to RdfExport_2015-07-15_17-19-35.nt
| 2015-07-15 17:19:36 Exporting RDF data from ODBC data source stars_for_stars
| ...
| 2015-07-15 18:02:12 Finished writing RDF to RdfExport_2015-07-15_17-19-35.nt
| 
| C:\Temp>console.bat -s http://localhost:8080/openrdf-sesame SforS
| Connected to http://localhost:8080/openrdf-sesame
| Opened repository 'SforS'
| Sesame Console, an interactive shell to communicate with Sesame repositories.
| 
| Type 'help' for help.
| SforS> load RdfExport_2015-07-15_17-19-35.nt
| Loading data...
+---

basic instructions for instsalling the Sesame server:
+---
| 1. Install Java 8 or later
|    http://www.oracle.com/technetwork/java/javase/downloads/index.html
| 2. Set your environment variable JAVA_HOME to the installation folder
| 3. Add %JAVA_HOME%\bin to your PATH
| 4. Install Apache Tomcat 8 or later: http://tomcat.apache.org/ (make sure
|    you run the Windows service installer to install the service, and set
|    the administrator username and password)
| 5. Run Tomcat8w.exe and start the Tomcat service
| 6. Download the distribution zip for Sesame 8 or later: http://rdf4j.org/
| 7. Extract the zip file to a folder (eg. c:\openrdf-sesame-2.8.4)
| 8. Open the Tomcat manager (eg. http://localhost:8080/manager) and log in
|    with the administrator username and password
| 9. Use the Deploy section of the manager to deploy the two WAR files 
|    in the war/ folder under the Sesame installation folder
| 
| /openrdf-sesame and /openrdf-workbench should now show up as applications
| in the Tomcat manager. You can use the workbench to create a repository
| and execute SPARQL queries. The Sesame console (console.bat) should be
| in the bin/ folder in the Sesame installation.
+---
");
                return -1;
            }

            logger.Info("Exporting RDF to " + PersonGraphWriter.Filename);

            try
            {
                Database db = new Database(args[0]);

                RdfExporter rdfExporter;
                logger.Info("Exporting RDF data from ODBC data source " + args[0]);
                if (args.Length == 1)
                {
                    rdfExporter = new RdfExporter(db, 10);
                }
                else
                {
                    logger.Info(String.Format("Reading previously imported data from {0} repository {1}", args[1], args[2]));
                    SesameHttpProtocolConnector sesame = new SesameHttpProtocolConnector(args[1], args[2]);
                    rdfExporter = new RdfExporter(db, 10, sesame);
                }

                rdfExporter.ExportRdf();

                return 0;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred while generating the RDF");
                return -1;
            }
        }
    }
}
