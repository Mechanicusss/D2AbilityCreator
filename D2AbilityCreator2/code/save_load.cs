using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D2AbilityCreator2
{
    public partial class Form1 : Form
    {
        public void SaveNodeChanges()
        {
            if (selectednode != null)
            {
                TreeNode[] neednode = treeView1.Nodes.Find(selectednode, true);
                object[] data = (object[])neednode[0].Tag;
                object[] elementsdata = (object[])splitContainer1.Panel2.Tag;
                for (int i = 2; i < data.Length; i++)
                {
                    object[] elementdata = (object[])elementsdata[i - 2];
                    if (data[i].GetType() == typeof(MyCheckbox))
                    {
                        MyCheckbox thisdata = (MyCheckbox)data[i];
                        CheckBox mycheck = (CheckBox)elementdata[0];
                        thisdata.check = mycheck.Checked;
                        data[i] = thisdata;
                    }
                    if (data[i].GetType() == typeof(MyCheckboxString))
                    {
                        MyCheckboxString thisdata = (MyCheckboxString)data[i];
                        CheckBox mycheck = (CheckBox)elementdata[0];
                        TextBox mytextbox = (TextBox)elementdata[1];
                        thisdata.check = mycheck.Checked;
                        thisdata.str = mytextbox.Text;
                        data[i] = thisdata;
                    }
                    if (data[i].GetType() == typeof(MyCheckboxStringOpen))
                    {
                        MyCheckboxStringOpen thisdata = (MyCheckboxStringOpen)data[i];
                        CheckBox mycheck = (CheckBox)elementdata[0];
                        TextBox mytextbox = (TextBox)elementdata[1];
                        thisdata.check = mycheck.Checked;
                        thisdata.str = mytextbox.Text;
                        data[i] = thisdata;
                    }
                    if (data[i].GetType() == typeof(MyString))
                    {
                        MyString thisdata = (MyString)data[i];
                        TextBox mytextbox = (TextBox)elementdata[0];
                        thisdata.str = mytextbox.Text;
                        data[i] = thisdata;
                    }
                    if (data[i].GetType() == typeof(MyStringSelect))
                    {
                        MyStringSelect thisdata = (MyStringSelect)data[i];
                        TextBox mytextbox = (TextBox)elementdata[0];
                        thisdata.str = mytextbox.Text;
                        data[i] = thisdata;
                    }
                    if (data[i].GetType() == typeof(MyCheckboxStringSelect))
                    {
                        MyCheckboxStringSelect thisdata = (MyCheckboxStringSelect)data[i];
                        CheckBox mycheck = (CheckBox)elementdata[0];
                        TextBox mytextbox = (TextBox)elementdata[1];
                        thisdata.check = mycheck.Checked;
                        thisdata.str = mytextbox.Text;
                        data[i] = thisdata;
                    }
                    if (data[i].GetType() == typeof(MyCheckboxStringString))
                    {
                        //object[] ptag = (object[])neednode[0].Parent.Tag;
                        //Debug.WriteLine(data[0]);
                        //if ((string)data[0] == "AbilityValues")
                        //{
                        //    Debug.WriteLine(neednode[0].Nodes.Count);
                        //    //eee;
                        //    for (int x = 0; neednode[0].Nodes.Count > x; x++)
                        //    {
                        //        object[] ctag = (object[])neednode[0].Nodes[x].Tag;
                        //        Debug.WriteLine(ctag.Length);
                        //        Debug.WriteLine(ctag[0]);
                        //    }
                        //}
                        //else
                        //{
                        MyCheckboxStringString thisdata = (MyCheckboxStringString)data[i];
                        CheckBox mycheck = (CheckBox)elementdata[0];
                        TextBox mytextbox = (TextBox)elementdata[1];
                        TextBox mytextbox2 = (TextBox)elementdata[2];
                        thisdata.check = mycheck.Checked;
                        thisdata.str1 = mytextbox.Text;
                        thisdata.str2 = mytextbox2.Text;
                        data[i] = thisdata;
                        //}
                    }
                    if (data[i].GetType() == typeof(MyCheckboxStringStringSelect))
                    {
                        MyCheckboxStringStringSelect thisdata = (MyCheckboxStringStringSelect)data[i];
                        CheckBox mycheck = (CheckBox)elementdata[0];
                        TextBox mytextbox = (TextBox)elementdata[1];
                        TextBox mytextbox2 = (TextBox)elementdata[2];
                        thisdata.check = mycheck.Checked;
                        thisdata.str1 = mytextbox.Text;
                        thisdata.str2 = mytextbox2.Text;
                        data[i] = thisdata;
                    }
                    if (data[i].GetType() == typeof(MyCheckboxStringSelectString))
                    {
                        MyCheckboxStringSelectString thisdata = (MyCheckboxStringSelectString)data[i];
                        CheckBox mycheck = (CheckBox)elementdata[0];
                        TextBox mytextbox = (TextBox)elementdata[1];
                        TextBox mytextbox2 = (TextBox)elementdata[2];
                        thisdata.check = mycheck.Checked;
                        thisdata.str1 = mytextbox.Text;
                        thisdata.str2 = mytextbox2.Text;
                        data[i] = thisdata;
                    }
                    if (data[i].GetType() == typeof(MyCheckboxStringSelectStringSelect))
                    {
                        MyCheckboxStringSelectStringSelect thisdata = (MyCheckboxStringSelectStringSelect)data[i];
                        CheckBox mycheck = (CheckBox)elementdata[0];
                        TextBox mytextbox = (TextBox)elementdata[1];
                        TextBox mytextbox2 = (TextBox)elementdata[2];
                        thisdata.check = mycheck.Checked;
                        thisdata.str1 = mytextbox.Text;
                        thisdata.str2 = mytextbox2.Text;
                        data[i] = thisdata;
                    }
                }

                neednode[0].Tag = data;
                selectednode = null;
            }
        }

        public void LoadFileInfo(object sender, EventArgs e)
        {
            Button thisb = (Button)sender;
            TreeNode thisnode = (TreeNode)thisb.Tag;
            object[] data = (object[])thisnode.Tag;
            string line = "";
            string filetext = "";
            StreamReader file = new StreamReader((string)data[0]);
            while ((line = file.ReadLine()) != null)
            {
                if (line.IndexOf(@"//") != -1)
                {
                    filetext = filetext + line.Substring(0, line.IndexOf(@"//"));
                }
                else
                {
                    filetext = filetext + line;
                }
            }
            file.Close();
            //Debug.WriteLine(filetext);
            MyNodeData alldata = new MyNodeData();
            alldata.name = thisnode.Text.Substring(0, thisnode.Text.Length - 4);
            alldata.childs = new List<MyNodeData>();
            alldata.data = new Dictionary<string, string>();
            int level = 0;
            string locstr = "";
            bool writemode = false;
            string locstr2 = "";
            bool writed = false;
            for (int i = 0; filetext.Length > i; i++)
            {
                if (writemode == true)
                {
                    if (filetext[i] == '"')
                    {
                        writemode = false;
                        if (writed == false)
                        {
                            locstr2 = locstr;
                            writed = true;
                            locstr = "";
                        }
                        else
                        {
                            alldata.AddStringStringData(locstr2, locstr, level);
                            locstr2 = "";
                            locstr = "";
                            writed = false;
                        }
                    }
                    else
                    {
                        locstr = locstr + filetext[i];
                    }
                }
                else
                {
                    if (filetext[i] == '"')
                    {
                        if (writemode == false)
                        {
                            writemode = true;
                        }
                    }
                    else if (filetext[i] == '{')
                    {
                        alldata.CreateChield(locstr2, level);
                        writed = false;
                        level++;
                        locstr = "";
                        locstr2 = "";
                    }
                    else if (filetext[i] == '}')
                    {
                        level--;
                    }
                }
            }
            level = 0;
            string[] nodes = new string[1];
            nodes[0] = thisnode.Name;
            CreateNodes(alldata, nodes, level);
            //ClearPanels();
        }

        public void CreateNodes(MyNodeData nowselected, string[] nodes, int level)
        {
            if (nowselected.data.TryGetValue("BaseClass", out _))
            {
                string value = "";
                nowselected.data.TryGetValue("BaseClass", out value);
                if (value == "ability_datadriven")
                {
                    Array.Resize(ref nodes, nodes.Length + 1);
                    nodes[level + 1] = CreateAbility(nowselected.name, nowselected.data, nodes[level]);
                }
                else if (value == "item_datadriven")
                {
                    Array.Resize(ref nodes, nodes.Length + 1);
                    nodes[level + 1] = CreateItem(nowselected.name, nowselected.data, nodes[level]);
                }
                else
                {
                    Array.Resize(ref nodes, nodes.Length + 1);
                    nodes[level + 1] = AddAbilityDataByObject(nowselected, nodes[level]);
                }
            }
            else
            {
                Array.Resize(ref nodes, nodes.Length + 1);
                nodes[level + 1] = AddAbilityDataByObject(nowselected, nodes[level]);
            }
            for (int i = 0; nowselected.childs.Count > i; i++)
            {
                CreateNodes(nowselected.childs[i], nodes, level + 1);
            }
        }
    }
}
