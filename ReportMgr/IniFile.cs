using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportMgr
{
    public class IniFile
    {
        public string path;
        public int listcount = 10;

        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);

        /// <summary>
        /// INIFile Constructor.
        /// </summary>
        /// <PARAM name="INIPath"></PARAM>
        public IniFile(string INIPath)
        {
            path = INIPath;
        }
        /// <summary>
        /// Write Data to the INI File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// Section name
        /// <PARAM name="Key"></PARAM>
        /// Key Name
        /// <PARAM name="Value"></PARAM>
        /// Value Name
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }

        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// <PARAM name="Key"></PARAM>
        /// <PARAM name="Path"></PARAM>
        /// <returns></returns>
        public string IniReadValue(string Section, string Key, string Default)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, Default, temp,
                                            255, this.path);
            return temp.ToString();

        }
        //public void IniReadList(string Section, string Key, ListBoxControl ListBox)
        //{
        //    string read;
        //    for (int i = 0; i < listcount; i++)
        //    {
        //        read = IniReadValue(Section, Key + i, "");
        //        if (read != "")
        //        {
        //            ListBox.Items.Add(read);
        //        }

        //    }
        //}

        //public void IniWriteList(string Section, string Key, ListBoxControl ListBox)
        //{
        //    for (int i = 0; i < listcount; i++)
        //    {
        //        IniWriteValue(Section, Key + i, "");
        //    }
        //    int t = 0;
        //    foreach (string it in ListBox.Items)
        //    {
        //        IniWriteValue(Section, Key + t, it);
        //        t++;
        //    }
        //}


    }
}
