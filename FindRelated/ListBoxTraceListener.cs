/*
 *                                FindRelated
 *              Copyright (c) 2003-2011 Stellman & Greene Consulting
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
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace Com.StellmanGreene.FindRelated
{
    class ListBoxTraceListener : TraceListener
    {
        private ListBox listBox = null;
        private ToolStripStatusLabel toolStripStatusLabel = null;
        public ListBoxTraceListener(ListBox listBox, ToolStripStatusLabel toolStripStatusLabel)
        {
            this.listBox = listBox;
            this.toolStripStatusLabel = toolStripStatusLabel;
        }

        delegate void WriteMethod(string message);

        public override void Write(string message)
        {
            listBox.Invoke(new WriteMethod(WriteOnGuiThread), message);
        }

        private void WriteOnGuiThread(string message)
        {
            if (listBox == null) return;
            if (listBox.Items.Count == 0)
                listBox.Items.Add(String.Empty);
            listBox.Items[listBox.Items.Count - 1] += message;
            listBox.SelectedIndex = listBox.Items.Count - 1;

            if (toolStripStatusLabel != null)
                toolStripStatusLabel.Text = Truncate(message);
        }

        public override void WriteLine(string message)
        {
            IEnumerable<string> lines = message.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in lines)
                listBox.Invoke(new WriteMethod(WriteLineOnGuiThread), line);
        }

        private void WriteLineOnGuiThread(string message)
        {
            if (listBox == null) return;
            Write(message);
            listBox.Items.Add(String.Empty);

            if (toolStripStatusLabel != null)
                toolStripStatusLabel.Text = Truncate(message);
        }

        public override void Flush()
        {
            Application.DoEvents();
            base.Flush();
        }

        private static string Truncate(string message)
        {
            if (message.Contains(" - "))
            {
                return (message.Substring(message.IndexOf(" - ") + 3));
            }
            else
                return message;
        }
    }
}
