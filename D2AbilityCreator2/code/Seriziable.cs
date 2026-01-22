using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D2AbilityCreator2
{
    public partial class Form1 : Form
    {

        [Serializable()]
        public class MyAddItemRequirementsNode
        {
        }

        [Serializable()]
        public class MyAddActionNode
        {
        }

        [Serializable()]
        public class MyAddStatesOrPropertiesNode
        {
            public string name { get; set; }
        }

        [Serializable()]
        public class MyAddAbilitySpecialNode
        {
        }

        [Serializable()]
        public class MyAddNodes
        {
            public string[] items { get; set; }
        }

        [Serializable()]
        public class MyAddCusttomNode
        {
        }

        [Serializable()]
        public class MyCheckbox
        {
            public string name { get; set; }
            public bool check { get; set; }
        }

        [Serializable()]
        public class MyString
        {
            public string name { get; set; }
            public string str { get; set; }
        }

        [Serializable()]
        public class MyStringSelect
        {
            public string name { get; set; }
            public string str { get; set; }
            public string[] selectlist { get; set; }
        }

        [Serializable()]
        public class MyCheckboxString
        {
            public string name { get; set; }
            public bool check { get; set; }
            public string str { get; set; }
        }

        [Serializable()]
        public class MyCheckboxStringString
        {
            public string name { get; set; }
            public bool check { get; set; }
            public string str1 { get; set; }
            public string str2 { get; set; }
        }

        [Serializable()]
        public class MyCheckboxStringSelect
        {
            public string name { get; set; }
            public bool check { get; set; }
            public string str { get; set; }
            public string[] selectlist { get; set; }
        }

        [Serializable()]
        public class MyCheckboxStringOpen
        {
            public string name { get; set; }
            public bool check { get; set; }
            public string str { get; set; }
        }

        [Serializable()]
        public class MyCheckboxStringStringSelect
        {
            public string name { get; set; }
            public bool check { get; set; }
            public string str1 { get; set; }
            public string str2 { get; set; }
            public string[] selectlist { get; set; }
        }

        [Serializable()]
        public class MyCheckboxStringSelectString
        {
            public string name { get; set; }
            public bool check { get; set; }
            public string str1 { get; set; }
            public string str2 { get; set; }
            public string[] selectlist { get; set; }
        }

        [Serializable()]
        public class MyCheckboxStringSelectStringSelect
        {
            public string name { get; set; }
            public bool check { get; set; }
            public string str1 { get; set; }
            public string str2 { get; set; }
            public string[] selectlist1 { get; set; }
            public string[] selectlist2 { get; set; }
        }
    }
}
