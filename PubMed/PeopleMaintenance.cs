/*
 *                           Publication Harvester
 *              Copyright © 2003-2019 Stellman & Greene Consulting
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
using System.Data;
using System.Collections;
using System.IO;

namespace Com.StellmanGreene.PubMed
{
    /// <summary>
    /// Add, update and remove people from the People table
    /// </summary>
    public static class PeopleMaintenance
    {
        /// <summary>
        /// Read a People file, add/update anyone in that file with new information,
        /// and clear any associations with publications for those people
        /// </summary>
        /// <param name="PeopleFile">People file to read</param>
        public static int AddUpdate(Database DB, string PeopleFile) {
            People people = new People(Path.GetDirectoryName(PeopleFile), Path.GetFileName(PeopleFile));
            foreach (Person person in people.PersonList)
            {
                person.WriteToDB(DB);
                ArrayList Parameters = new ArrayList();
                Parameters.Add(Database.Parameter(person.Setnb));
                DB.ExecuteNonQuery(
                    @"DELETE FROM PeoplePublications 
                           WHERE Setnb = ?", Parameters
                );
            }
            return people.PersonList.Count;
        }

        /// <summary>
        /// Read a People file, delete anyone in that file from the People table,
        /// and clear associations with publications for those people
        /// </summary>
        /// <param name="PeopleFile">People file to read</param>
        public static int Remove(Database DB, string PeopleFile)
        {
            int Count = 0;
            People people = new People(Path.GetDirectoryName(PeopleFile), Path.GetFileName(PeopleFile));
            foreach (Person person in people.PersonList)
            {
                ArrayList Parameters = new ArrayList();
                Parameters.Add(Database.Parameter(person.Setnb));
                Count += DB.GetIntValue(
                    @"SELECT Count(*) FROM People WHERE Setnb = ?", Parameters
                );
                Parameters = new ArrayList();
                Parameters.Add(Database.Parameter(person.Setnb));
                DB.ExecuteNonQuery(
                    @"DELETE FROM People WHERE Setnb = ?", Parameters
                );
                Parameters = new ArrayList();
                Parameters.Add(Database.Parameter(person.Setnb));
                DB.ExecuteNonQuery(
                    @"DELETE FROM PeoplePublications 
                           WHERE Setnb = ?", Parameters
                );
            }
            return Count;
        }
    }
}
