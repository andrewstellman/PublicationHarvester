/*
 *                           Publication Harvester
 *              Copyright (c) 2003-2006 Stellman & Greene Consulting
 *      Developed for Joshua Zivin and Pierre Azoulay, Columbia University
 *            http://www.stellman-greene.com/PublicationHarvester
 *
 * This program is free software; you can redistribute it and/or modify it under
 * the terms of the GNU General Public License as published by the Free Software 
 * Foundation; either version 2 of the License, or (at your option) any later 
 * version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT 
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
 * FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with 
 * this program (GPL.txt); if not, write to the Free Software Foundation, Inc., 51 
 * Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
using Com.StellmanGreene.PubMed;

namespace Com.StellmanGreene.FindRelated
{
    public class RelatedFinder
    {
        const string ELINK_URL = "http://www.ncbi.nlm.nih.gov/entrez/eutils/elink.fcgi";
        const string ELINK_DB = "pubmed";
        const string ELINK_DBFROM = "pubmed";

        private NCBI ncbi = new NCBI("medline");

        //private string query = "11812492[uid] OR 11774222[uid] OR 11812492[uid] OR 11774222[uid] OR 11668631[uid] OR 15111095[uid] OR 10731564[uid] OR 15780005[uid] OR 19597542[uid] OR 19043737[uid] OR 18952001[uid] OR 17885136[uid] OR 19943957[uid] OR 16005284[uid] OR 19722191[uid] OR 15383292[uid] OR 10856373[uid] OR 17040125[uid] OR 18539347[uid] OR 10770808[uid] OR 17470297[uid] OR 15287587[uid] OR 11125122[uid] OR 10612825[uid] OR 20730111[uid] OR 12743802[uid] OR 15238684[uid] OR 15839745[uid] OR 10592273[uid] OR 18180957[uid] OR 15024419[uid] OR 15383308[uid] OR 19954456[uid] OR 18439691[uid] OR 12386340[uid] OR 16269725[uid] OR 15997407[uid] OR 19806204[uid] OR 14702162[uid] OR 11774221[uid] OR 16616613[uid] OR 10592272[uid] OR 17202370[uid] OR 19091018[uid] OR 11472559[uid] OR 15322925[uid] OR 15478601[uid] OR 11925998[uid] OR 18000556[uid] OR 15774024[uid] OR 19002498[uid] OR 18538871[uid] OR 17562224[uid] OR 15037105[uid] OR 14998511[uid] OR 10092480[uid] OR 19504759[uid] OR 12372145[uid] OR 7729881[uid] OR 18550617[uid] OR 12933853[uid] OR 16818783[uid] OR 11472553[uid] OR 17306254[uid] OR 16406333[uid] OR 11516587[uid] OR 11403387[uid] OR 20603211[uid] OR 9274032[uid] OR 12856318[uid] OR 12467974[uid] OR 20543958[uid] OR 14695526[uid] OR 19592535[uid] OR 11214099[uid] OR 14638788[uid] OR 16278157[uid] OR 18073380[uid] OR 20689574[uid] OR 12625936[uid] OR 11752345[uid] OR 15944077[uid] OR 11752242[uid] OR 14695451[uid] OR 12402526[uid] OR 14681474[uid] OR 16697384[uid] OR 16672453[uid] OR 10511685[uid] OR 10592200[uid] OR 15828434[uid] OR 11125071[uid] OR 12860672[uid] OR 15710433[uid] OR 16164550[uid] OR 11791238[uid] OR 15125639[uid] OR 15608284[uid] OR 15630619[uid] OR 19073702[uid] OR 11443570[uid] OR 21047535[uid] OR 19114486[uid] OR 12208043[uid] OR 20005876[uid] OR 11988510[uid] OR 18562339[uid] OR 16672057[uid] OR 11731507[uid] OR 15687015[uid] OR 10782070[uid] OR 18027007[uid] OR 16381944[uid] OR 11125038[uid] OR 17099226[uid] OR 17761848[uid] OR 12387845[uid] OR 18386064[uid] OR 17584494[uid] OR 16237012[uid] OR 19352421[uid] OR 19154594[uid] OR 18064491[uid] OR 17697334[uid] OR 17135206[uid] OR 15474306[uid] OR 18637161[uid] OR 19389774[uid] OR 15608233[uid] OR 18562031[uid] OR 18953038[uid] OR 16423288[uid] OR 16085497[uid] OR 15270538[uid] OR 12816546[uid] OR 16251775[uid] OR 17518759[uid] OR 12632152[uid] OR 21606368[uid] OR 18502862[uid] OR 16351742[uid] OR 19958475[uid] OR 10221636[uid] OR 16103603[uid] OR 12203989[uid] OR 15317790[uid] OR 11456466[uid] OR 12701381[uid] OR 17641731[uid] OR 9455480[uid] OR 11038309[uid] OR 10366827[uid] OR 11269648[uid] OR 12537121[uid] OR 10466136[uid] OR 12203988[uid] OR 17135198[uid] OR 15977173[uid] OR 12435493[uid] OR 18025705[uid] OR 11825250[uid] OR 16381840[uid] OR 18492133[uid] OR 11197770[uid] OR 11328780[uid] OR 9047337[uid] OR 14681353[uid] OR 18045790[uid] OR 15336912[uid] OR 17170002[uid] OR 12519941[uid] OR 12490454[uid] OR 16381973[uid] OR 16907992[uid] OR 15046636[uid] OR 10592169[uid] OR 11240843[uid] OR 16351753[uid] OR 21154707[uid] OR 20362581[uid] OR 16873516[uid] OR 12819149[uid] OR 9921679[uid] OR 18681951[uid] OR 17038195[uid] OR 15608248[uid] OR 11758285[uid] OR 15608286[uid] OR 15774022[uid] OR 12234534[uid] OR 11741630[uid] OR 15608257[uid] OR 15643605[uid] OR 15723693[uid] OR 16002116[uid] OR 18307806[uid] OR 11355885[uid] OR 20047494[uid] OR 10902212[uid] OR 21037260[uid] OR 19098027[uid] OR 18321385[uid] OR 10612821[uid] OR 14681351[uid] OR 15546336[uid] OR 17062145[uid] OR 12819150[uid] OR 14681478[uid] OR 16551372[uid] OR 17384426[uid] OR 15980532[uid] OR 21629728[uid] OR 19036787[uid] OR 18544553[uid] OR 18847484[uid] OR 17401150[uid] OR 11908756[uid] OR 10359795[uid] OR 11384164[uid] OR 19063745[uid] OR 15608226[uid] OR 18981050[uid] OR 19797660[uid] OR 18029361[uid] OR 12436197[uid] OR 11222582[uid] OR 16845091[uid] OR 12519977[uid] OR 17130148[uid] OR 17338820[uid] OR 19321736[uid] OR 21324604[uid] OR 20671203[uid] OR 17445272[uid] OR 20034492[uid] OR 18801163[uid] OR 11125059[uid] OR 12083398[uid] OR 17877839[uid] OR 18025704[uid] OR 17151077[uid] OR 17921498[uid] OR 19768586[uid] OR 19105187[uid] OR 14681471[uid] OR 16381974[uid] OR 15827081[uid] OR 11279516[uid] OR 17142236[uid] OR 12519964[uid] OR 10592263[uid] OR 21410491[uid] OR 18796476[uid] OR 17221864[uid] OR 10466135[uid] OR 21097891[uid] OR 15608212[uid] OR 19728865[uid] OR 16845079[uid] OR 17988782[uid] OR 17185755[uid] OR 15215374[uid] OR 17135185[uid] OR 17059604[uid] OR 17148475[uid] OR 17254505[uid] OR 11015564[uid] OR 20624716[uid] OR 10407783[uid] OR 20585501[uid] OR 9169870[uid] OR 11414208[uid] OR 9571806[uid] OR 20083406[uid] OR 20375450[uid] OR 21131495[uid] OR 10407677[uid] OR 9847220[uid] OR 18713719[uid] OR 11244060[uid] OR 20036185[uid] OR 20597434[uid] OR 8719164[uid] OR 9884329[uid] OR 19342283[uid] OR 21573076[uid] OR 10511680[uid] OR 21283610[uid] OR 21576469[uid] OR 21350051[uid] OR 11092731[uid] OR 10899154[uid] OR 10737802[uid] OR 10511682[uid] OR 10851186[uid] OR 9775388[uid] OR 11668619[uid] OR 19692404[uid] OR 20670087[uid] OR 18377816[uid] OR 21062823[uid] OR 10484179[uid] OR 9625791[uid] OR 11446511[uid] OR 10066467[uid] OR 17400791[uid] OR 11783003[uid] OR 20672376[uid] OR 10612820[uid] OR 10611059[uid] OR 10612824[uid] OR 21523552[uid] OR 10587943[uid] OR 10075567[uid] OR 20664631[uid] OR 21292630[uid] OR 21658361[uid] OR 19620973[uid] OR 11357826[uid] OR 9421619[uid] OR 10419978[uid] OR 9685316[uid] OR 18628874[uid] OR 20639550[uid] OR 19306393[uid] OR 21288496[uid] OR 21520333[uid] OR 10587942[uid] OR 16701248[uid] OR 11480780[uid] OR 20398331[uid] OR 11802378[uid] OR 21504866[uid] OR 10407668[uid] OR 21336565[uid] OR 21439036[uid] OR 10963611[uid] OR 16874317[uid] OR 19325849[uid] OR 19238236[uid] OR 18070518[uid] OR 9830540[uid] OR 20975904[uid] OR 11462837[uid] OR 10447503[uid] OR 21470650[uid] OR 10637631[uid] OR 18629076[uid] OR 20946650[uid] OR 20823861[uid] OR 21666073[uid] OR 19025664[uid] OR 21540879[uid] OR 21637800[uid]";
        private string query = "11812492[uid] OR 11774222[uid] OR 11812492[uid] OR 11774222[uid] OR 11668631[uid] OR 15111095[uid]";

        public void Go()
        {
            List<int> ids = new List<int>() { 11812492, 11774222, 11812492, 11774222, 11668631, 15111095 };
            string xml = GetRelatedXmlResults(ids);
            File.WriteAllText(@"c:\temp\abc_" + DateTime.Now.Ticks + ".txt", xml);
            IEnumerable<int> relatedIds = GetIdsFromXml(xml);

            //string results = ncbi.Search("11812492[uid] OR 11774222[uid]");
            StringBuilder searchQuery = new StringBuilder();
            foreach (int id in relatedIds)
            {
                searchQuery.AppendFormat("{0}{1}[uid]", searchQuery.Length == 0 ? String.Empty : " OR ", id);
            }
            NCBI.UsePostRequest = true;
            string searchResults = ncbi.Search(searchQuery.ToString());
            File.WriteAllText(@"c:\temp\abc_" + DateTime.Now.Ticks + ".txt", searchResults);

            Publications publications = new Publications(searchResults, new PublicationTypes(@"c:\temp\", "sample-pubtypes.csv"));
        }

        /// <summary>
        /// Use the NCBI Elink request to retrieve related IDs for one or more publication IDs
        /// </summary>
        /// <param name="ids">IDs to retrieve</param>
        /// <param name="mindate">Optional minimum date</param>
        /// <param name="maxdate">Optional maximum date</param>
        /// <returns>A string with XML results from elink.fcgi</returns>
        private static string GetRelatedXmlResults(IEnumerable<int> ids, string mindate = null, string maxdate = null)
        {
            if (ids == null)
                throw new ArgumentNullException("ids");

            StringBuilder query = new StringBuilder();
            query.AppendFormat("dbfrom={0}&db={1}&id=", ELINK_DBFROM, ELINK_DB);
            bool first = true;
            foreach (int id in ids)
            {
                if (!first)
                    query.Append(",");
                else
                    first = false;
                query.Append(id);
            }
            if (!string.IsNullOrEmpty(mindate))
                query.AppendFormat("&mindate={0}", mindate);
            if (!string.IsNullOrEmpty(maxdate))
                query.AppendFormat("&mindate={0}", maxdate);

            WebRequest request = WebRequest.Create(ELINK_URL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] byteArray = UTF8Encoding.UTF8.GetBytes(query.ToString());
            request.ContentLength = byteArray.Length;
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            using (WebResponse response = request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }

        /// <summary>
        /// Retrieve the IDs from the XML results from ELink
        /// </summary>
        /// <param name="xml">XML results from ELink</param>
        /// <returns>IDs extracted from the XML (or an empty list of none)</returns>
        private static IEnumerable<int> GetIdsFromXml(string xml)
        {
            List<int> ids = new List<int>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes("/eLinkResult/LinkSet/LinkSetDb/Link/Id");
            foreach (XmlNode node in xmlNodeList)
            {
                if (node.Name == "Id")
                {
                    int id;
                    if (int.TryParse(node.InnerText, out id))
                        ids.Add(id);
                }
            }
            return ids;
        }

    }
}
