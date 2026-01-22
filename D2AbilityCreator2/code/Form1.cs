using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace D2AbilityCreator2
{
    public partial class Form1 : Form
    {

        //[0] == name
        //[1] == type
        //types:
        //
        int nodenum = 0;
        string selectednode = null;
        string blankspath = @"blanks";

        public Form1()
        {
            InitializeComponent();

            UpdateBlankList();
        }

        public void UpdateBlankList()
        {
            if (!string.IsNullOrWhiteSpace(blankspath))
            {

                //Debug.WriteLine(fbd.SelectedPath);
                string[] files;
                try
                {
                    files = Directory.GetFiles(blankspath);
                }
                catch (Exception error)
                {
                    Debug.WriteLine(error);
                    return;
                }
                listBox1.Items.Clear();
                for (int i = 0; files.Length > i; i++)
                {
                    if (files[i].Substring(files[i].Length - 4) == ".txt")
                    {
                        string filename = RemovePath(files[i]);
                        listBox1.Items.Add(filename.Substring(0, filename.Length - 4));
                    }
                }
            }
        }
        public void ClearPanels()
        {
            SaveNodeChanges();
            splitContainer1.Panel1.Tag = null;
            splitContainer1.Panel2.Tag = null;
            splitContainer1.Panel1.Controls.Clear();
            splitContainer1.Panel2.Controls.Clear();
            object[] menutag = (object[])menuStrip2.Tag;
            if (menutag != null)
                for (int i = 0; menutag.Length > i; i++)
                {
                    ToolStripItem item = (ToolStripItem)menutag[i];
                    item.Dispose();
                }
            menuStrip2.Tag = null;
            button1.Enabled = false;
        }

        public void AddString(object sender, EventArgs e)
        {
            Button thisb = (Button)sender;
            object[] objlist = (object[])thisb.Tag;
            ListBox listbox = (ListBox)objlist[0];
            TextBox textbox = (TextBox)objlist[1];
            if (listbox.SelectedItem != null)
            {
                if (textbox.Text == "")
                {
                    textbox.Text = listbox.SelectedItem.ToString();
                }
                else
                {
                    textbox.Text = textbox.Text + " | " + listbox.SelectedItem.ToString();
                }
            }
        }

        public void OpenLuaFile(object sender, EventArgs e)
        {
            Button thisb = (Button)sender;
            object[] objlist = (object[])thisb.Tag;
            TextBox textbox = (TextBox)objlist[0];
            TreeNode[] neednodes = treeView1.Nodes.Find(textbox.Text.Replace(@"/", @"\"), true);
            if (neednodes.Length > 0)
            {
                object[] nodetag = (object[])neednodes[0].Tag;
                string path = (string)nodetag[0];
                Process.Start(path.Replace(@"/", @"\"));
            }
            else
            {
                MessageBox.Show("Lua file not found!", "Error");
            }
        }

        public void SelectModeOn(object sender, EventArgs e)
        {
            menuStrip1.Visible = false;
            menuStrip2.Visible = false;
            //treeView1.Visible = false;
            splitContainer2.Visible = false;
            Button thisb = (Button)sender;
            object[] objlist = (object[])thisb.Tag;
            TextBox oldtextbox = (TextBox)objlist[0];
            TextBox newtextbox = new TextBox();
            newtextbox.Text = oldtextbox.Text;
            newtextbox.Location = new Point(20, 20);
            newtextbox.Size = new Size(700, 24);
            Controls.Add(newtextbox);
            newtextbox.BringToFront();
            Label newlabel = new Label();
            newlabel.Text = "Search:";
            newlabel.Location = new Point(20, 54);
            newlabel.Size = new Size(60, 24);
            Controls.Add(newlabel);
            newlabel.BringToFront();
            ListBox newlistbox = new ListBox();
            string[] strlist = (string[])objlist[1];
            newlistbox.Items.AddRange(strlist);
            newlistbox.Tag = strlist;
            newlistbox.Location = new Point(20, 85);
            newlistbox.Size = new Size(500, 400);
            Controls.Add(newlistbox);
            newlistbox.BringToFront();
            TextBox newtextbox2 = new TextBox();
            newtextbox2.Location = new Point(70, 50);
            newtextbox2.Size = new Size(320, 24);
            newtextbox2.TextChanged += FindChanged;
            newtextbox2.Tag = newlistbox;
            Controls.Add(newtextbox2);
            newtextbox2.BringToFront();
            Button newbutton = new Button();
            //newbutton.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            newbutton.Location = new Point(530, 93);
            newbutton.Size = new Size(80, 23);
            newbutton.Text = "Add";
            newbutton.Tag = new object[] { newlistbox, newtextbox };
            newbutton.Click += AddString;
            Controls.Add(newbutton);
            newbutton.BringToFront();
            Button newbutton2 = new Button();
            //newbutton2.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            newbutton2.Location = new Point(530, 123);
            newbutton2.Size = new Size(80, 23);
            newbutton2.Text = "Back";
            newbutton2.Tag = new object[] { oldtextbox, newtextbox, newlistbox, newbutton, newtextbox2, newlabel };
            newbutton2.Click += SelectModeOff;
            Controls.Add(newbutton2);
            newbutton2.BringToFront();
        }

        public void FindChanged(object sender, EventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            ListBox newlistbox = (ListBox)textbox.Tag;
            string[] items = (string[])newlistbox.Tag;
            newlistbox.Items.Clear();
            if (textbox.Text != "")
            {
                string[] items1 = new string[0];
                string[] items2 = new string[0];
                for (int i = 0; items.Length > i; i++)
                {
                    if (items[i].ToLower().IndexOf(textbox.Text.ToLower()) != -1)
                    {
                        Array.Resize(ref items1, items1.Length + 1);
                        items1[items1.Length - 1] = items[i];
                    }
                    else
                    {
                        Array.Resize(ref items2, items2.Length + 1);
                        items2[items2.Length - 1] = items[i];
                    }
                }
                newlistbox.Items.AddRange(items1);
                newlistbox.Items.AddRange(items2);
            }
            else
            {
                newlistbox.Items.AddRange(items);
            }
        }

        public void SelectModeOff(object sender, EventArgs e)
        {
            Button thisb = (Button)sender;
            object[] objlist = (object[])thisb.Tag;
            TextBox oldtextbox = (TextBox)objlist[0];
            TextBox newtextbox = (TextBox)objlist[1];
            ListBox newlistbox = (ListBox)objlist[2];
            Button newbutton = (Button)objlist[3];
            TextBox newtextbox2 = (TextBox)objlist[4];
            Label newlabel = (Label)objlist[5];
            oldtextbox.Text = newtextbox.Text;
            thisb.Dispose();
            newtextbox.Dispose();
            newlistbox.Dispose();
            newbutton.Dispose();
            newtextbox2.Dispose();
            newlabel.Dispose();
            menuStrip1.Visible = true;
            menuStrip2.Visible = true;
            //treeView1.Visible = true;
            splitContainer2.Visible = true;
        }
        public string AddAbilityDataByObject(MyNodeData data, string nodename)
        {
            TreeNode newnode = new TreeNode();
            object[] nodetag = null;
            if (nodename != null)
            {
                TreeNode[] neednode = treeView1.Nodes.Find(nodename, true);
                newnode = neednode[0].Nodes.Add(data.name);
                nodetag = (object[])neednode[0].Tag;
                if (!neednode[0].IsExpanded)
                    neednode[0].Toggle();
            }
            else
            {
                newnode = treeView1.Nodes.Add(data.name);
            }
            //TreeNode[] neednode = treeView1.Nodes.Find(nodename, true);
            //TreeNode newnode = neednode[0].Nodes.Add(data.name);
            newnode.Name = "node" + nodenum;
            object[] tagdata = new object[] {
                data.name,
                "abilitydata"
            };
            nodenum++;
            if (nodetag != null && nodetag[0] != null && (string)nodetag[0] == "Modifiers")
            {
                string[] ThisEventList = new string[2 + ModifierEventList.Length];
                new string[] { "Properties", "States" }.CopyTo(ThisEventList, 0);
                ModifierEventList.CopyTo(ThisEventList, 2);
                tagdata = new object[] {
                    data.name,
                    "modifier",
                    new MyCheckbox(){ name = "Passive", check = MyMiniF2(data.data,"Passive") },
                    new MyCheckbox(){ name = "IsBuff", check = MyMiniF2(data.data,"IsBuff") },
                    new MyCheckbox(){ name = "IsDebuff", check = MyMiniF2(data.data,"IsDebuff") },
                    new MyCheckbox(){ name = "IsHidden", check = MyMiniF2(data.data,"IsHidden") },
                    new MyCheckbox(){ name = "IsPurgable", check = MyMiniF2(data.data,"IsPurgable") },
                    new MyCheckboxStringSelect(){ name = "OverrideAnimation", check = data.data.TryGetValue("OverrideAnimation", out _), str = MyMiniF(data.data,"OverrideAnimation") ?? "", selectlist = AbilityCastAnimationList },
                    new MyCheckboxString(){ name = "Duration", check = data.data.TryGetValue("Duration", out _), str = MyMiniF(data.data,"Duration") ?? "" },
                    new MyCheckboxStringSelect(){ name = "Attributes", check = data.data.TryGetValue("Attributes", out _), str = MyMiniF(data.data,"Attributes") ?? "", selectlist = AttributeList },
                    new MyCheckboxString(){ name = "TextureName", check = data.data.TryGetValue("TextureName", out _), str = MyMiniF(data.data,"TextureName") ?? "" },
                    new MyCheckboxString(){ name = "EffectName", check = data.data.TryGetValue("EffectName", out _), str = MyMiniF(data.data,"EffectName") ?? "" },
                    new MyCheckboxStringSelect(){ name = "EffectAttachType", check = data.data.TryGetValue("EffectAttachType", out _), str = MyMiniF(data.data,"EffectAttachType") ?? "", selectlist = EffectAttachTypeList },
                    new MyCheckboxString(){ name = "ModelName", check = data.data.TryGetValue("ModelName", out _), str = MyMiniF(data.data,"ModelName") ?? "" },
                    new MyCheckboxString(){ name = "Aura", check = data.data.TryGetValue("Aura", out _), str = MyMiniF(data.data,"Aura") ?? "" },
                    new MyCheckboxString(){ name = "Aura_Radius", check = data.data.TryGetValue("Aura_Radius", out _), str = MyMiniF(data.data,"Aura_Radius") ?? "" },
                    new MyCheckboxStringSelect(){ name = "Aura_Teams", check = data.data.TryGetValue("Aura_Teams", out _), str = MyMiniF(data.data,"Aura_Teams") ?? "", selectlist = TeamsList },
                    new MyCheckboxStringSelect(){ name = "Aura_Types", check = data.data.TryGetValue("Aura_Types", out _), str = MyMiniF(data.data,"Aura_Types") ?? "", selectlist = UnitTargetTypeSelectedList },
                    new MyCheckboxStringSelect(){ name = "Aura_Flags", check = data.data.TryGetValue("Aura_Flags", out _), str = MyMiniF(data.data,"Aura_Flags") ?? "", selectlist = FlagList },
                    new MyCheckboxString(){ name = "Aura_ApplyToCaster", check = data.data.TryGetValue("Aura_ApplyToCaster", out _), str = MyMiniF(data.data,"Aura_ApplyToCaster") ?? "1" },
                    new MyCheckboxString(){ name = "ThinkInterval", check = data.data.TryGetValue("ThinkInterval", out _), str = MyMiniF(data.data,"ThinkInterval") ?? "" },
                };
                data.data = DeleteItems(data.data, new string[] {
                    "Passive",
                    "IsBuff",
                    "IsDebuff",
                    "IsHidden",
                    "IsPurgable",
                    "OverrideAnimation",
                    "Duration",
                    "Attributes",
                    "TextureName",
                    "EffectName",
                    "EffectAttachType",
                    "ModelName",
                    "Aura",
                    "Aura_Radius",
                    "Aura_Teams",
                    "Aura_Types",
                    "Aura_Flags",
                    "Aura_ApplyToCaster",
                    "ThinkInterval"
                });
                Array.Resize(ref tagdata, tagdata.Length + data.data.Count + 1);
                if (data.data.Count > 0)
                {
                    for (int i = 0; data.data.Count > i; i++)
                    {
                        tagdata[tagdata.Length + i - (data.data.Count + 1)] = new MyCheckboxString() { name = data.data.ElementAt(i).Key, check = true, str = data.data.ElementAt(i).Value };
                    }
                }
                tagdata[tagdata.Length - 1] = new MyAddNodes() { items = ThisEventList };
            }
            else if (data.name == "Modifiers")
            {
                tagdata = new object[] {
                    data.name,
                    "abilitydata",
                    new MyAddCusttomNode(){ },
                };
            }
            else
            {
                bool nextno = false;
                for (int i = 0; data.data.Count > i; i++)
                {
                    Array.Resize(ref tagdata, tagdata.Length + 1);
                    if (nextno == true)
                    {
                        tagdata[tagdata.Length - 1] = new MyCheckboxStringString() { check = true, name = "", str1 = data.data.ElementAt(i).Key, str2 = data.data.ElementAt(i).Value };
                    }
                    else if (data.name == "Properties")
                    {
                        tagdata[tagdata.Length - 1] = new MyCheckboxStringSelectString() { check = true, name = "", str1 = data.data.ElementAt(i).Key, str2 = data.data.ElementAt(i).Value, selectlist = Properties };
                    }
                    else if (data.name == "States")
                    {
                        tagdata[tagdata.Length - 1] = new MyCheckboxStringSelectStringSelect() { check = true, name = "", str1 = data.data.ElementAt(i).Key, str2 = data.data.ElementAt(i).Value, selectlist1 = States, selectlist2 = StatesValues };
                    }
                    else
                    {
                        if (data.data.ElementAt(i).Key == "Action")
                        {
                            tagdata[tagdata.Length - 1] = new MyAddActionNode();
                        }
                        else if (data.data.ElementAt(i).Key == "AbilityName" || data.data.ElementAt(i).Key == "ModifierName" || data.data.ElementAt(i).Key == "EffectName" || data.data.ElementAt(i).Key == "ControlPoints" || data.data.ElementAt(i).Key == "TargetPoint" || data.data.ElementAt(i).Key == "EffectRadius" || data.data.ElementAt(i).Key == "EffectDurationScale" || data.data.ElementAt(i).Key == "EffectLifeDurationScale" || data.data.ElementAt(i).Key == "EffectColorA" || data.data.ElementAt(i).Key == "EffectColorB" || data.data.ElementAt(i).Key == "EffectAlphaScale" || data.data.ElementAt(i).Key == "CleavePercent" || data.data.ElementAt(i).Key == "CleaveRadius" || data.data.ElementAt(i).Key == "ModifierName" || data.data.ElementAt(i).Key == "MinDamage" || data.data.ElementAt(i).Key == "MaxDamage" || data.data.ElementAt(i).Key == "Damage" || data.data.ElementAt(i).Key == "CurrentHealthPercentBasedDamage" || data.data.ElementAt(i).Key == "MaxHealthPercentBasedDamage" || data.data.ElementAt(i).Key == "HealAmount" || data.data.ElementAt(i).Key == "Duration" || data.data.ElementAt(i).Key == "Distance" || data.data.ElementAt(i).Key == "Height" || data.data.ElementAt(i).Key == "IsFixedDistance" || data.data.ElementAt(i).Key == "ShouldStun" || data.data.ElementAt(i).Key == "LifestealPercent" || data.data.ElementAt(i).Key == "MoveSpeed" || data.data.ElementAt(i).Key == "StartRadius" || data.data.ElementAt(i).Key == "EndRadius" || data.data.ElementAt(i).Key == "FixedDistance" || data.data.ElementAt(i).Key == "StartPosition" || data.data.ElementAt(i).Key == "HasFrontalCone" || data.data.ElementAt(i).Key == "ProvidesVision" || data.data.ElementAt(i).Key == "VisionRadius" || data.data.ElementAt(i).Key == "Chance" || data.data.ElementAt(i).Key == "PseudoRandom" || data.data.ElementAt(i).Key == "Function" || data.data.ElementAt(i).Key == "UnitName" || data.data.ElementAt(i).Key == "UnitCount" || data.data.ElementAt(i).Key == "UnitLimit" || data.data.ElementAt(i).Key == "SpawnRadius" || data.data.ElementAt(i).Key == "GrantsGold" || data.data.ElementAt(i).Key == "GrantsXP" || data.data.ElementAt(i).Key == "Dodgeable" || data.data.ElementAt(i).Key == "SourceAttachment" || data.data.ElementAt(i).Key == "Radius")
                        {
                            tagdata[tagdata.Length - 1] = new MyCheckboxString() { check = true, name = data.data.ElementAt(i).Key, str = data.data.ElementAt(i).Value };
                        }
                        else if (data.data.ElementAt(i).Key == "TargetTeams" || data.data.ElementAt(i).Key == "Teams")
                        {
                            tagdata[tagdata.Length - 1] = new MyCheckboxStringSelect() { check = true, name = data.data.ElementAt(i).Key, str = data.data.ElementAt(i).Value, selectlist = TeamsList };
                        }
                        else if (data.data.ElementAt(i).Key == "TargetTypes" || data.data.ElementAt(i).Key == "Types" || data.data.ElementAt(i).Key == "ExcludeTypes")
                        {
                            tagdata[tagdata.Length - 1] = new MyCheckboxStringSelect() { check = true, name = data.data.ElementAt(i).Key, str = data.data.ElementAt(i).Value, selectlist = AbilityTypeSelectedList };
                        }
                        else if (data.data.ElementAt(i).Key == "TargetFlags" || data.data.ElementAt(i).Key == "Flags" || data.data.ElementAt(i).Key == "ExcludeFlags" || data.data.ElementAt(i).Key == "Flag")
                        {
                            tagdata[tagdata.Length - 1] = new MyCheckboxStringSelect() { check = true, name = data.data.ElementAt(i).Key, str = data.data.ElementAt(i).Value, selectlist = FlagList };
                        }
                        else if (data.data.ElementAt(i).Key == "EffectAttachType")
                        {
                            tagdata[tagdata.Length - 1] = new MyCheckboxStringSelect() { check = true, name = data.data.ElementAt(i).Key, str = data.data.ElementAt(i).Value, selectlist = EffectAttachTypeList };
                        }
                        else if (data.data.ElementAt(i).Key == "Target" || data.data.ElementAt(i).Key == "Center")
                        {
                            tagdata[tagdata.Length - 1] = new MyCheckboxStringSelect() { check = true, name = data.data.ElementAt(i).Key, str = data.data.ElementAt(i).Value, selectlist = TargetList };
                        }
                        else if (data.data.ElementAt(i).Key == "ScriptFile")
                        {
                            tagdata[tagdata.Length - 1] = new MyCheckboxStringOpen() { check = true, name = data.data.ElementAt(i).Key, str = data.data.ElementAt(i).Value };
                        }
                        else if (data.data.ElementAt(i).Key == "Type")
                        {
                            if (data.name == "Target")
                            {
                                tagdata[tagdata.Length - 1] = new MyCheckboxStringSelect() { check = true, name = data.data.ElementAt(i).Key, str = data.data.ElementAt(i).Value, selectlist = UnitTargetTypeSelectedList };
                            }
                            else
                            {
                                tagdata[tagdata.Length - 1] = new MyCheckboxStringSelect() { check = true, name = data.data.ElementAt(i).Key, str = data.data.ElementAt(i).Value, selectlist = DamageTypeSelectedList };
                            }
                        }
                        else if (data.data.ElementAt(i).Key == "var_type")
                        {
                            tagdata[tagdata.Length - 1] = new MyCheckboxStringSelect() { check = true, name = data.data.ElementAt(i).Key, str = data.data.ElementAt(i).Value, selectlist = VarTypes };
                            nextno = true;
                        }
                        else
                        {
                            tagdata[tagdata.Length - 1] = new MyCheckboxStringString() { check = true, name = "", str1 = data.data.ElementAt(i).Key, str2 = data.data.ElementAt(i).Value };
                        }
                    }
                }
            }
            //post
            if (data.name == "States" || data.name == "Properties")
            {
                Array.Resize(ref tagdata, tagdata.Length + 1);
                tagdata[tagdata.Length - 1] = new MyAddStatesOrPropertiesNode() { name = data.name };
            }
            if (data.name == "ItemRequirements")
            {
                Array.Resize(ref tagdata, tagdata.Length + 1);
                tagdata[tagdata.Length - 1] = new MyAddItemRequirementsNode();
            }
            if (data.name == "AbilityValues")
            {
                Array.Resize(ref tagdata, tagdata.Length + 1);
                //tagdata[tagdata.Length - 2] = new MyCheckboxStringString() { check = true, name = "", str1 = data.data.ElementAt(i).Key, str2 = data.data.ElementAt(i).Value };
                //for (int x = 0; e.Node.Nodes.Count > x; x++)
                //{
                //    object[] asdata = (object[])e.Node.Nodes[x].Tag;
                //    //Debug.WriteLine(asdata.Length);
                //    //Debug.WriteLine(asdata[3]);

                //    MyCheckboxStringString thisdata = (MyCheckboxStringString)asdata[3];
                //    CheckBox mycheck = new CheckBox();
                //    mycheck.Location = new Point(10, (30 * (i + num)) + 6);
                //    mycheck.AutoSize = true;
                //    mycheck.Checked = thisdata.check;
                //    mycheck.Parent = splitContainer1.Panel2;
                //    TextBox mytextbox = new TextBox();
                //    mytextbox.Text = thisdata.str1;
                //    mytextbox.Location = new Point(30, (30 * (i + num)) + 3);
                //    mytextbox.Size = new Size(400, 23);
                //    mytextbox.Parent = splitContainer1.Panel2;
                //    TextBox mytextbox2 = new TextBox();
                //    mytextbox2.Text = thisdata.str2;
                //    mytextbox2.Location = new Point(450, (30 * (i + num)) + 3);
                //    mytextbox2.Size = new Size(400, 23);
                //    mytextbox2.Parent = splitContainer1.Panel2;

                //    //Array.Resize(ref newdata, newdata.Length + 1);
                //    //newdata[i - 2] = new object[] { mycheck, mytextbox, mytextbox2 };

                //    Array.Resize(ref newdata, newdata.Length + 1);
                //    newdata[newdata.Length - 1] = new object[] { mycheck, mytextbox, mytextbox2 };
                //}
                tagdata[tagdata.Length - 1] = new MyAddAbilitySpecialNode();
            }
            if (data.name == "Action")
            {
                Array.Resize(ref tagdata, tagdata.Length + 1);
                tagdata[tagdata.Length - 1] = new MyAddActionNode();
            }
            if (Array.IndexOf(EventList, data.name) >= 0 || Array.IndexOf(ModifierEventList, data.name) >= 0)
            {
                Array.Resize(ref tagdata, tagdata.Length + 1);
                tagdata[tagdata.Length - 1] = new MyAddActionNode();
            }
            //Debug.WriteLine("Count in " + data.name + " = " + data.data.Count + ", writed = " + tagdata.Length);
            newnode.Tag = tagdata;
            return newnode.Name;
        }

        public string MyMiniF(Dictionary<string, string> data, string needname)
        {
            string result = "";
            if (data.TryGetValue(needname, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public bool MyMiniF2(Dictionary<string, string> data, string needname)
        {
            string result = "";
            if (data.TryGetValue(needname, out result))
                if (result == "1")
                    return true;
            return false;
        }

        public void AddLine(object sender, EventArgs e)
        {
            Debug.WriteLine("AddLine");
            ClearPanels();
            TreeNode selnode = treeView1.SelectedNode;
            object[] nodetag = (object[])selnode.Tag;
            Array.Resize(ref nodetag, nodetag.Length + 1);
            nodetag[nodetag.Length - 1] = new MyCheckboxStringString() { check = false, name = "", str1 = "", str2 = "" };
            selnode.Tag = nodetag;
            treeView1.SelectedNode = null;
            treeView1.SelectedNode = selnode;
        }

        public void AddModifier(object sender, EventArgs e)
        {
            Button thisb = (Button)sender;
            TextBox textbox = (TextBox)thisb.Tag;
            //ToolStripMenuItem thisb = (ToolStripMenuItem)sender;
            //ToolStripTextBox textbox = (ToolStripTextBox)thisb.Tag;
            if (textbox.Text != "")
            {
                MyNodeData newnode = new MyNodeData();
                newnode.name = textbox.Text;
                newnode.data = new Dictionary<string, string>();
                AddAbilityDataByObject(newnode, treeView1.SelectedNode.Name);
                //TreeNode[] neednode = treeView1.Nodes.Find(AddAbilityDataByObject(newnode, treeView1.SelectedNode.Name), true);
                //treeView1.SelectedNode = neednode[0];
            }
        }

        public void AddAbilitySpecial(object sender, EventArgs e)
        {
            ClearPanels();
            TreeNode selnode = treeView1.SelectedNode;
            object[] nodetag = (object[])selnode.Tag;
            int addNodeIndex = -1;
            for (int i = 0; i < nodetag.Length; i++)
            {
                if (nodetag[i].GetType() == typeof(MyAddAbilitySpecialNode))
                {
                    addNodeIndex = i;
                    break;
                }
            }

            if (addNodeIndex != -1)
            {
                Array.Resize(ref nodetag, nodetag.Length + 1);
                for (int i = nodetag.Length - 1; i > addNodeIndex; i--)
                {
                    nodetag[i] = nodetag[i - 1];
                }
                nodetag[addNodeIndex] = new MyCheckboxStringString()
                {
                    check = true,
                    name = "",
                    str1 = "",  
                    str2 = ""           
                };

                selnode.Tag = nodetag;
                treeView1.SelectedNode = null;
                treeView1.SelectedNode = selnode;
            }
        }

        public void AddItemRequirements(object sender, EventArgs e)
        {
            Debug.WriteLine("AddItemRequirements");
            ClearPanels();
            TreeNode selnode = treeView1.SelectedNode;
            object[] nodetag = (object[])selnode.Tag;
            Array.Resize(ref nodetag, nodetag.Length + 1);
            nodetag[nodetag.Length - 1] = nodetag[nodetag.Length - 2];
            string col = "";
            if (nodetag.Length - 3 < 10)
            {
                col = "0" + (nodetag.Length - 3);
            }
            else
            {
                col = (nodetag.Length - 3).ToString();
            }
            nodetag[nodetag.Length - 2] = new MyCheckboxStringString() { check = false, name = "", str1 = col, str2 = "" };
            selnode.Tag = nodetag;
            treeView1.SelectedNode = null;
            treeView1.SelectedNode = selnode;
        }

        public void AddStateOrPropertie(object sender, EventArgs e)
        {
            Debug.WriteLine("AddStateOrPropertie");
            Button thisb = (Button)sender;
            string sorp = (string)thisb.Tag;
            ClearPanels();
            TreeNode selnode = treeView1.SelectedNode;
            object[] nodetag = (object[])selnode.Tag;
            Array.Resize(ref nodetag, nodetag.Length + 1);
            nodetag[nodetag.Length - 1] = nodetag[nodetag.Length - 2];
            if (sorp == "States") nodetag[nodetag.Length - 2] = new MyCheckboxStringSelectStringSelect() { check = false, name = "", str1 = "", str2 = "", selectlist1 = States, selectlist2 = StatesValues };
            else nodetag[nodetag.Length - 2] = new MyCheckboxStringSelectString() { check = false, name = "", str1 = "", str2 = "", selectlist = Properties };
            selnode.Tag = nodetag;
            treeView1.SelectedNode = null;
            treeView1.SelectedNode = selnode;
        }

        public void OnAddActionDoubleClick(object sender, EventArgs e)
        {
            ListBox thislist = (ListBox)sender;
            //Debug.WriteLine(thislist.SelectedItem.ToString());
            //ToolStripMenuItem thisb = (ToolStripMenuItem)sender;
            // ToolStripComboBox combbox = (ToolStripComboBox)thisb.Tag;
            if (thislist.SelectedItem.ToString() != "")
            {
                AddMyActionNode(thislist.SelectedItem.ToString());
            }
        }

        public void OnAddActionClick(object sender, EventArgs e)
        {
            Button thisb = (Button)sender;
            ComboBox combbox = (ComboBox)thisb.Tag;
            //ToolStripMenuItem thisb = (ToolStripMenuItem)sender;
            // ToolStripComboBox combbox = (ToolStripComboBox)thisb.Tag;
            if (combbox.Text != "")
            {
                AddMyActionNode(combbox.Text);
            }
        }

        public void AddMyActionNode(string text)
        {
            int num = Array.IndexOf(ActionList, text);
            MyNodeData newnode = new MyNodeData();
            newnode.name = text;
            newnode.data = new Dictionary<string, string>();
            if (text == "ActOnTargets")
            {
                string newnodename = AddAbilityDataByObject(newnode, treeView1.SelectedNode.Name);
                MyNodeData newnode2 = new MyNodeData();
                newnode2.name = "Target";
                newnode2.data = new Dictionary<string, string>();
                newnode2.data["Center"] = "";
                newnode2.data["Radius"] = "";
                newnode2.data["Teams"] = "";
                newnode2.data["Flags"] = "";
                newnode2.data["ExcludeFlags"] = "";
                newnode2.data["Type"] = "";
                newnode2.data["ExcludeTypes"] = "";
                newnode2.data["MaxTargets"] = "";
                newnode2.data["Random"] = "";
                MyNodeData newnode3 = new MyNodeData();
                newnode3.name = "Action";
                newnode3.data = new Dictionary<string, string>();
                AddAbilityDataByObject(newnode2, newnodename);
                AddAbilityDataByObject(newnode3, newnodename);
            }
            else if (text == "DelayedAction")
            {
                newnode.data["Delay"] = "";
                MyNodeData newnode2 = new MyNodeData();
                newnode2.name = "Action";
                newnode2.data = new Dictionary<string, string>();
                AddAbilityDataByObject(newnode2, AddAbilityDataByObject(newnode, treeView1.SelectedNode.Name));
            }
            else if (text == "Random")
            {
                newnode.data["Chance"] = "";
                newnode.data["PseudoRandom"] = "";
                string newnodename = AddAbilityDataByObject(newnode, treeView1.SelectedNode.Name);
                MyNodeData newnode2 = new MyNodeData();
                newnode2.name = "OnSuccess";
                newnode2.data = new Dictionary<string, string>();
                newnode2.data["Action"] = "";
                MyNodeData newnode3 = new MyNodeData();
                newnode3.name = "OnFailure";
                newnode3.data = new Dictionary<string, string>();
                newnode3.data["Action"] = "";
                AddAbilityDataByObject(newnode2, newnodename);
                AddAbilityDataByObject(newnode3, newnodename);
            }
            else if (text == "IsCasterAlive")
            {
                string newnodename = AddAbilityDataByObject(newnode, treeView1.SelectedNode.Name);
                MyNodeData newnode2 = new MyNodeData();
                newnode2.name = "OnSuccess";
                newnode2.data = new Dictionary<string, string>();
                newnode2.data["Action"] = "";
                MyNodeData newnode3 = new MyNodeData();
                newnode3.name = "OnFailure";
                newnode3.data = new Dictionary<string, string>();
                newnode3.data["Action"] = "";
                AddAbilityDataByObject(newnode2, newnodename);
                AddAbilityDataByObject(newnode3, newnodename);
            }
            else
            {
                string[] actprop = (string[])ActionPropertiesList[num];
                for (int i = 0; actprop.Length > i; i++)
                {
                    newnode.data[actprop[i]] = "";
                }
                AddAbilityDataByObject(newnode, treeView1.SelectedNode.Name);
            }
        }

        public void OnAddDoubleClick(object sender, EventArgs e)
        {
            ListBox thislist = (ListBox)sender;
            //Debug.WriteLine(thislist.SelectedItem.ToString());
            //ToolStripMenuItem thisb = (ToolStripMenuItem)sender;
            // ToolStripComboBox combbox = (ToolStripComboBox)thisb.Tag;
            if (thislist.SelectedItem.ToString() != "")
            {
                AddMyNode(thislist.SelectedItem.ToString());
            }
        }

        public void OnAddClick(object sender, EventArgs e)
        {
            Button thisb = (Button)sender;
            ComboBox combbox = (ComboBox)thisb.Tag;
            //ToolStripMenuItem thisb = (ToolStripMenuItem)sender;
            // ToolStripComboBox combbox = (ToolStripComboBox)thisb.Tag;
            if (combbox.Text != "")
            {
                AddMyNode(combbox.Text);
            }
        }

        public void AddMyNode(string text)
        {
            MyNodeData newnode = new MyNodeData();
            newnode.name = text;
            newnode.data = new Dictionary<string, string>();
            AddAbilityDataByObject(newnode, treeView1.SelectedNode.Name);
            //TreeNode[] neednode = treeView1.Nodes.Find(AddAbilityDataByObject(newnode,treeView1.SelectedNode.Name), true);
            //treeView1.SelectedNode = neednode[0];
        }

        public void CreateItemButtonClick(object sender, EventArgs e)
        {
            TextBox namebox = (TextBox)splitContainer1.Panel2.Tag;
            if (namebox.Text != "")
            {
                if (namebox.Text.IndexOf("item_") == 0)
                {
                    TreeNode[] neednode = treeView1.Nodes.Find(CreateItem(namebox.Text, new Dictionary<string, string>(), null), true);
                    treeView1.SelectedNode = neednode[0];
                }
                else
                {
                    //MessageBox.Show("Invalid name!\r\nAt the beginning of the item name should be 'item_'", "Error");
                    DialogResult dialogResult = MessageBox.Show("Invalid name!\r\nAt the beginning of the item name should be 'item_'\r\nAdd?", "Error", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        TreeNode[] neednode = treeView1.Nodes.Find(CreateItem("item_" + namebox.Text, new Dictionary<string, string>(), null), true);
                        treeView1.SelectedNode = neednode[0];
                    }
                }
            }
            else
            {
                MessageBox.Show("Invalid name!", "Error");
            }
        }

        public void CreateText(object sender, EventArgs e)
        {
            Debug.WriteLine("CreateText");
            ClearPanels();
            treeView1.SelectedNode = null;
            ToolStripMenuItem thisb = (ToolStripMenuItem)sender;
            TreeNode node = (TreeNode)thisb.Tag;
            string text = GetTextByNode(node, 0);
            RichTextBox newrtb = new RichTextBox();
            newrtb.WordWrap = false;
            newrtb.Parent = splitContainer1.Panel2;
            newrtb.Dock = DockStyle.Fill;
            newrtb.Text = text;

            button1.Tag = newrtb;
            button1.Enabled = true;

            //object[] menutag = new object[0] { };

            //ToolStripItem newitem = menuStrip2.Items.Add("Add To Blanks");
            //newitem.Margin = new Padding(0, 0, splitContainer3.Panel2.Size.Width, 0);
            //newitem.Alignment = ToolStripItemAlignment.Right;
            //newitem.Click += AddToBlanks;
            ////Array.Resize(ref menutag, menutag.Length + 1);
            ////menutag[menutag.Length - 1] = newitem;

            //ToolStripItem newitem2 = new ToolStripTextBox();
            //newitem2.Name = "toolStripTextBox1";
            ////ToolStripItem newitem2 = menuStrip2.Items.Add("New Blank Name");
            ////newitem2.Margin = new Padding(0, 0, splitContainer3.Panel2.Size.Width, 0);
            //newitem2.Alignment = ToolStripItemAlignment.Right;
            //int id = menuStrip2.Items.Add(newitem2);
            ////Array.Resize(ref menutag, menutag.Length + 1);
            ////menutag[menutag.Length - 1] = newitem2;
            ////newitem2.Dispose();
            //newitem.Tag = new object[2] { newitem2, text };
            //menuStrip2.Tag = new object[2] { newitem, newitem2 };
        }

        public void AddToBlanks(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            RichTextBox textBox = (RichTextBox)button.Tag;
            if (textBox1.Text == "")
            {
                MessageBox.Show("Invalid name!", "Error");
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(blankspath + @"/" + textBox1.Text + ".txt", false))
                {
                    string text = textBox.Text;
                    sw.WriteLine(text);
                }
                UpdateBlankList();
            }
        }

        public string GetTextByNode(TreeNode node, int level)
        {
            object[] data = (object[])node.Tag;
            string miniotstup = "";
            string otstup = "\t";
            for (int i = 0; level > i; i++)
            {
                otstup = otstup + "\t";
                miniotstup = miniotstup + "\t";
            }
            string readytext = miniotstup + '"' + (string)data[0] + '"' + "\r\n" + miniotstup + "{";
            if (level == 0)
            {
                readytext = readytext + "\r\n" + otstup + "" +"// github.com/Mechanicusss/";
            }
            if ((string)data[1] == "ability")
                readytext = readytext + "\r\n" + otstup + '"' + "BaseClass" + '"' + "\t\t" + '"' + "ability_datadriven" + '"';
            else if ((string)data[1] == "item")
                readytext = readytext + "\r\n" + otstup + '"' + "BaseClass" + '"' + "\t\t" + '"' + "item_datadriven" + '"';
            for (int i = 2; data.Length > i; i++)
            {
                if (data[i].GetType() == typeof(MyCheckbox))
                {
                    MyCheckbox thisdata = (MyCheckbox)data[i];
                    string value = "0";
                    if (thisdata.check)
                        value = "1";
                    readytext = readytext + "\r\n" + otstup + '"' + thisdata.name + '"' + "\t\t" + '"' + value + '"';
                }
                if (data[i].GetType() == typeof(MyCheckboxString))
                {
                    MyCheckboxString thisdata = (MyCheckboxString)data[i];
                    if (thisdata.check)
                    {
                        readytext = readytext + "\r\n" + otstup + '"' + thisdata.name + '"' + "\t\t" + '"' + thisdata.str + '"';
                    }
                }
                if (data[i].GetType() == typeof(MyCheckboxStringOpen))
                {
                    MyCheckboxStringOpen thisdata = (MyCheckboxStringOpen)data[i];
                    if (thisdata.check)
                    {
                        readytext = readytext + "\r\n" + otstup + '"' + thisdata.name + '"' + "\t\t" + '"' + thisdata.str + '"';
                    }
                }
                if (data[i].GetType() == typeof(MyString))
                {
                    MyString thisdata = (MyString)data[i];
                    readytext = readytext + "\r\n" + otstup + '"' + thisdata.name + '"' + "\t\t" + '"' + thisdata.str + '"';
                }
                if (data[i].GetType() == typeof(MyStringSelect))
                {
                    MyStringSelect thisdata = (MyStringSelect)data[i];
                    readytext = readytext + "\r\n" + otstup + '"' + thisdata.name + '"' + "\t\t" + '"' + thisdata.str + '"';
                }
                if (data[i].GetType() == typeof(MyCheckboxStringSelect))
                {
                    MyCheckboxStringSelect thisdata = (MyCheckboxStringSelect)data[i];
                    if (thisdata.check)
                    {
                        readytext = readytext + "\r\n" + otstup + '"' + thisdata.name + '"' + "\t\t" + '"' + thisdata.str + '"';
                    }
                }
                if (data[i].GetType() == typeof(MyCheckboxStringString))
                {
                    MyCheckboxStringString thisdata = (MyCheckboxStringString)data[i];
                    if (thisdata.check)
                    {
                        readytext = readytext + "\r\n" + otstup + '"' + thisdata.str1 + '"' + "\t\t" + '"' + thisdata.str2 + '"';
                    }
                }
                if (data[i].GetType() == typeof(MyCheckboxStringStringSelect))
                {
                    MyCheckboxStringStringSelect thisdata = (MyCheckboxStringStringSelect)data[i];
                    if (thisdata.check)
                    {
                        readytext = readytext + "\r\n" + otstup + '"' + thisdata.str1 + '"' + "\t\t" + '"' + thisdata.str2 + '"';
                    }
                }
                if (data[i].GetType() == typeof(MyCheckboxStringSelectString))
                {
                    MyCheckboxStringSelectString thisdata = (MyCheckboxStringSelectString)data[i];
                    if (thisdata.check)
                    {
                        readytext = readytext + "\r\n" + otstup + '"' + thisdata.str1 + '"' + "\t\t" + '"' + thisdata.str2 + '"';
                    }
                }
                if (data[i].GetType() == typeof(MyCheckboxStringSelectStringSelect))
                {
                    MyCheckboxStringSelectStringSelect thisdata = (MyCheckboxStringSelectStringSelect)data[i];
                    if (thisdata.check)
                    {
                        readytext = readytext + "\r\n" + otstup + '"' + thisdata.str1 + '"' + "\t\t" + '"' + thisdata.str2 + '"';
                    }
                }
            }
            for (int i = 0; node.Nodes.Count > i; i++)
            {
                readytext = readytext + "\r\n" + GetTextByNode(node.Nodes[i], level + 1);
            }
            readytext = readytext + "\r\n" + miniotstup + "}";
            return readytext;
        }

        private void fileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("fileToolStripMenuItem1_Click");
            ClearPanels();
            treeView1.SelectedNode = null;
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Open File";
            //fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "Txt, lua files (*.txt;*.lua)|*.txt;*.lua";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                LoadFiles(null, new string[] { fdlg.FileName });
            }
        }

        private void fToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("fToolStripMenuItem_Click");
            ClearPanels();
            treeView1.SelectedNode = null;
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    //Debug.WriteLine(fbd.SelectedPath);
                    string[] files = Directory.GetFiles(fbd.SelectedPath);
                    string[] folders = Directory.GetDirectories(fbd.SelectedPath);
                    TreeNode newnode = treeView1.Nodes.Add(RemovePath(fbd.SelectedPath));
                    newnode.Name = "node" + nodenum;
                    nodenum++;
                    newnode.Tag = new object[]
                    {
                        fbd.SelectedPath,
                        "folder"
                    };
                    //Debug.WriteLine(newnode.FullPath);
                    //Debug.WriteLine(newnode.Index);
                    LoadFiles(newnode.Name, files);
                    LoadFolders(newnode.Name, folders);
                    //MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
                }
            }
            //open folder
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("createToolStripMenuItem_Click");
            ClearPanels();
            treeView1.SelectedNode = null;
            //treeView1.Visible = false;
            //splitContainer1.Visible = false;
            //ListBox newlistbox = new ListBox();
            Label namelbl = new Label();
            namelbl.Text = "Ability/Item name:";
            namelbl.Location = new Point(10, 6);
            namelbl.AutoSize = true;
            namelbl.Parent = splitContainer1.Panel1;
            TextBox newtextbox = new TextBox();
            newtextbox.Location = new Point(10, 3);
            newtextbox.Size = new Size(220, 20);
            newtextbox.Parent = splitContainer1.Panel2;
            splitContainer1.Panel2.Tag = newtextbox;
            Button newbutton1 = new Button();
            newbutton1.BackColor = Color.LightGray;
            newbutton1.Location = new Point(240, 3);
            newbutton1.Size = new Size(100, 24);
            newbutton1.Text = "Create ability";
            newbutton1.Click += CreateAbilityButtonClick;
            newbutton1.Parent = splitContainer1.Panel2;
            Button newbutton2 = new Button();
            newbutton2.BackColor = Color.LightGray;
            newbutton2.Location = new Point(350, 3);
            newbutton2.Size = new Size(100, 24);
            newbutton2.Text = "Create item";
            newbutton2.Click += CreateItemButtonClick;
            newbutton2.Parent = splitContainer1.Panel2;
            //splitContainer1.Panel1.
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Debug.WriteLine("treeView1_AfterSelect");
            ClearPanels();
            selectednode = null;
            listBox2.Items.Clear();
            listBox2.DoubleClick -= OnAddDoubleClick;
            listBox2.DoubleClick -= OnAddActionDoubleClick;
            //Debug.WriteLine(sender);
            //Debug.WriteLine(e.Node.Tag);
            object[] data = (object[])e.Node.Tag;
            TextBox namelbl = new TextBox();
            namelbl.ReadOnly = true;
            namelbl.Dock = DockStyle.Fill;
            namelbl.BorderStyle = BorderStyle.None;
            namelbl.Text = (string)data[0];
            namelbl.Location = new Point(10, 3);
            namelbl.Font = new Font(namelbl.Font.FontFamily, 15);
            //namelbl.AutoSize = true;
            namelbl.Parent = splitContainer1.Panel1;
            Button newbutton = new Button();
            newbutton.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            newbutton.Location = new Point(splitContainer1.Size.Width - 110, 2);
            newbutton.Size = new Size(80, 23);
            newbutton.Text = "Delete";
            newbutton.Tag = e.Node;
            newbutton.Click += DeleteNode;
            newbutton.Parent = splitContainer1.Panel1;
            newbutton.BringToFront();
            if ((string)data[1] == "file.lua")
            {
                Button newbutton2 = new Button();
                newbutton2.BackColor = Color.LightGray;
                newbutton2.Location = new Point(10, 3);
                newbutton2.Size = new Size(100, 23);
                newbutton2.Text = "Open file";
                newbutton2.Tag = (string)data[0];
                newbutton2.Click += OpenFile;
                newbutton2.Parent = splitContainer1.Panel2;
            }
            else if ((string)data[1] == "file.txt")
            {
                Button newbutton1 = new Button();
                newbutton1.BackColor = Color.LightGray;
                newbutton1.Location = new Point(10, 3);
                newbutton1.Size = new Size(100, 23);
                newbutton1.Text = "Open file";
                newbutton1.Tag = (string)data[0];
                newbutton1.Click += OpenFile;
                newbutton1.Parent = splitContainer1.Panel2;
                Button newbutton2 = new Button();
                newbutton2.BackColor = Color.LightGray;
                newbutton2.Location = new Point(120, 3);
                newbutton2.Size = new Size(100, 23);
                newbutton2.Text = "Load file";
                newbutton2.Tag = e.Node;
                newbutton2.Click += LoadFileInfo;
                newbutton2.Parent = splitContainer1.Panel2;
            }
            else if ((string)data[1] == "ability" || (string)data[1] == "item" || (string)data[1] == "abilitydata" || (string)data[1] == "modifier")
            {
                selectednode = e.Node.Name;
                object[] newdata = new object[0];
                object[] menutag = new object[0] { };
                //if((string)data[1] == "ability" || (string)data[1] == "item" || (string)data[1] == "abilitydata")
                //{
                ToolStripItem newitem2 = menuStrip2.Items.Add("Create");
                newitem2.Margin = new Padding(0, 0, splitContainer3.Panel2.Size.Width - 94, 0);
                newitem2.Alignment = ToolStripItemAlignment.Right;
                newitem2.Tag = e.Node;
                newitem2.Click += CreateText;
                Array.Resize(ref menutag, menutag.Length + 1);
                menutag[menutag.Length - 1] = newitem2;
                //}
                ToolStripItem newitem = menuStrip2.Items.Add("Add Line");
                newitem.Alignment = ToolStripItemAlignment.Right;
                newitem.Click += AddLine;
                Array.Resize(ref menutag, menutag.Length + 1);
                menutag[menutag.Length - 1] = newitem;
                //Debug.WriteLine("Length = " + data.Length);
                for (int i = 2; i < data.Length; i++)
                {
                    //Debug.WriteLine("Load " + i);
                    if (data[i].GetType() == typeof(MyCheckbox))
                    {
                        MyCheckbox thisdata = (MyCheckbox)data[i];
                        CheckBox mycheck = new CheckBox();
                        mycheck.Location = new Point(250, (30 * (i - 2)) + 6);
                        mycheck.AutoSize = true;
                        mycheck.Checked = thisdata.check;
                        mycheck.Parent = splitContainer1.Panel2;
                        Label mynamelbl = new Label();
                        mynamelbl.Text = thisdata.name;
                        mynamelbl.Location = new Point(30, (30 * (i - 2)) + 3);
                        mynamelbl.AutoSize = true;
                        mynamelbl.Parent = splitContainer1.Panel2;

                        Array.Resize(ref newdata, newdata.Length + 1);
                        newdata[i - 2] = new object[] { mycheck };
                    }
                    if (data[i].GetType() == typeof(MyCheckboxString))
                    {
                        MyCheckboxString thisdata = (MyCheckboxString)data[i];
                        CheckBox mycheck = new CheckBox();
                        mycheck.Location = new Point(10, (30 * (i - 2)) + 6);
                        mycheck.AutoSize = true;
                        mycheck.Checked = thisdata.check;
                        mycheck.Parent = splitContainer1.Panel2;
                        Label mynamelbl = new Label();
                        mynamelbl.Text = thisdata.name;
                        mynamelbl.Location = new Point(30, (30 * (i - 2)) + 3);
                        mynamelbl.AutoSize = true;
                        mynamelbl.Parent = splitContainer1.Panel2;
                        TextBox mytextbox = new TextBox();
                        mytextbox.Text = thisdata.str;
                        mytextbox.Location = new Point(250, (30 * (i - 2)) + 3);
                        mytextbox.Size = new Size(400, 23);
                        mytextbox.Parent = splitContainer1.Panel2;

                        Array.Resize(ref newdata, newdata.Length + 1);
                        newdata[i - 2] = new object[] { mycheck, mytextbox };
                    }
                    if (data[i].GetType() == typeof(MyString))
                    {
                        MyString thisdata = (MyString)data[i];
                        Label mynamelbl = new Label();
                        mynamelbl.Text = thisdata.name;
                        mynamelbl.Location = new Point(30, (30 * (i - 2)) + 3);
                        mynamelbl.AutoSize = true;
                        mynamelbl.Parent = splitContainer1.Panel2;
                        TextBox mytextbox = new TextBox();
                        mytextbox.Text = thisdata.str;
                        mytextbox.Location = new Point(250, (30 * (i - 2)) + 3);
                        mytextbox.Size = new Size(400, 23);
                        mytextbox.Parent = splitContainer1.Panel2;

                        Array.Resize(ref newdata, newdata.Length + 1);
                        newdata[i - 2] = new object[] { mytextbox };
                    }
                    if (data[i].GetType() == typeof(MyStringSelect))
                    {
                        MyStringSelect thisdata = (MyStringSelect)data[i];
                        Label mynamelbl = new Label();
                        mynamelbl.Text = thisdata.name;
                        mynamelbl.Location = new Point(30, (30 * (i - 2)) + 3);
                        mynamelbl.AutoSize = true;
                        mynamelbl.Parent = splitContainer1.Panel2;
                        TextBox mytextbox = new TextBox();
                        mytextbox.Text = thisdata.str;
                        mytextbox.Location = new Point(250, (30 * (i - 2)) + 3);
                        mytextbox.Size = new Size(400, 23);
                        mytextbox.Parent = splitContainer1.Panel2;
                        Button newbutton2 = new Button();
                        newbutton2.BackColor = Color.LightGray;
                        newbutton2.Location = new Point(660, (30 * (i - 2)) + 3);
                        newbutton2.Size = new Size(100, 23);
                        newbutton2.Text = "Select";
                        newbutton2.Tag = new object[] { mytextbox, thisdata.selectlist };
                        newbutton2.Click += SelectModeOn;
                        newbutton2.Parent = splitContainer1.Panel2;

                        Array.Resize(ref newdata, newdata.Length + 1);
                        newdata[i - 2] = new object[] { mytextbox };
                    }
                    if (data[i].GetType() == typeof(MyCheckboxStringSelect))
                    {
                        MyCheckboxStringSelect thisdata = (MyCheckboxStringSelect)data[i];
                        CheckBox mycheck = new CheckBox();
                        mycheck.Location = new Point(10, (30 * (i - 2)) + 6);
                        mycheck.AutoSize = true;
                        mycheck.Checked = thisdata.check;
                        mycheck.Parent = splitContainer1.Panel2;
                        Label mynamelbl = new Label();
                        mynamelbl.Text = thisdata.name;
                        mynamelbl.Location = new Point(30, (30 * (i - 2)) + 3);
                        mynamelbl.AutoSize = true;
                        mynamelbl.Parent = splitContainer1.Panel2;
                        TextBox mytextbox = new TextBox();
                        mytextbox.Text = thisdata.str;
                        mytextbox.Location = new Point(250, (30 * (i - 2)) + 3);
                        mytextbox.Size = new Size(400, 23);
                        mytextbox.Parent = splitContainer1.Panel2;
                        Button newbutton2 = new Button();
                        newbutton2.BackColor = Color.LightGray;
                        newbutton2.Location = new Point(660, (30 * (i - 2)) + 3);
                        newbutton2.Size = new Size(100, 23);
                        newbutton2.Text = "Select";
                        newbutton2.Tag = new object[] { mytextbox, thisdata.selectlist };
                        newbutton2.Click += SelectModeOn;
                        newbutton2.Parent = splitContainer1.Panel2;

                        Array.Resize(ref newdata, newdata.Length + 1);
                        newdata[i - 2] = new object[] { mycheck, mytextbox };
                    }
                    if (data[i].GetType() == typeof(MyCheckboxStringString))
                    {
                        MyCheckboxStringString thisdata = (MyCheckboxStringString)data[i];
                        CheckBox mycheck = new CheckBox();
                        mycheck.Location = new Point(10, (30 * (i - 2)) + 6);
                        mycheck.AutoSize = true;
                        mycheck.Checked = thisdata.check;
                        mycheck.Parent = splitContainer1.Panel2;
                        TextBox mytextbox = new TextBox();
                        mytextbox.Text = thisdata.str1;
                        mytextbox.Location = new Point(30, (30 * (i - 2)) + 3);
                        mytextbox.Size = new Size(400, 23);
                        mytextbox.Parent = splitContainer1.Panel2;
                        TextBox mytextbox2 = new TextBox();
                        mytextbox2.Text = thisdata.str2;
                        mytextbox2.Location = new Point(450, (30 * (i - 2)) + 3);
                        mytextbox2.Size = new Size(400, 23);
                        mytextbox2.Parent = splitContainer1.Panel2;

                        Array.Resize(ref newdata, newdata.Length + 1);
                        newdata[i - 2] = new object[] { mycheck, mytextbox, mytextbox2 };
                    }
                    if (data[i].GetType() == typeof(MyCheckboxStringOpen))
                    {
                        MyCheckboxStringOpen thisdata = (MyCheckboxStringOpen)data[i];
                        CheckBox mycheck = new CheckBox();
                        mycheck.Location = new Point(10, (30 * (i - 2)) + 6);
                        mycheck.AutoSize = true;
                        mycheck.Checked = thisdata.check;
                        mycheck.Parent = splitContainer1.Panel2;
                        Label mynamelbl = new Label();
                        mynamelbl.Text = thisdata.name;
                        mynamelbl.Location = new Point(30, (30 * (i - 2)) + 3);
                        mynamelbl.AutoSize = true;
                        mynamelbl.Parent = splitContainer1.Panel2;
                        TextBox mytextbox = new TextBox();
                        mytextbox.Text = thisdata.str;
                        mytextbox.Location = new Point(250, (30 * (i - 2)) + 3);
                        mytextbox.Size = new Size(400, 23);
                        mytextbox.Parent = splitContainer1.Panel2;
                        Button newbutton2 = new Button();
                        newbutton2.BackColor = Color.LightGray;
                        newbutton2.Location = new Point(660, (30 * (i - 2)) + 3);
                        newbutton2.Size = new Size(100, 23);
                        newbutton2.Text = "Open";
                        newbutton2.Tag = new object[] { mytextbox };
                        newbutton2.Click += OpenLuaFile;
                        newbutton2.Parent = splitContainer1.Panel2;

                        Array.Resize(ref newdata, newdata.Length + 1);
                        newdata[i - 2] = new object[] { mycheck, mytextbox };
                    }

                    if (data[i].GetType() == typeof(MyCheckboxStringStringSelect))
                    {
                        MyCheckboxStringStringSelect thisdata = (MyCheckboxStringStringSelect)data[i];
                        CheckBox mycheck = new CheckBox();
                        mycheck.Location = new Point(10, (30 * (i - 2)) + 6);
                        mycheck.AutoSize = true;
                        mycheck.Checked = thisdata.check;
                        mycheck.Parent = splitContainer1.Panel2;
                        TextBox mytextbox = new TextBox();
                        mytextbox.Text = thisdata.str1;
                        mytextbox.Location = new Point(30, (30 * (i - 2)) + 3);
                        mytextbox.Size = new Size(400, 23);
                        mytextbox.Parent = splitContainer1.Panel2;
                        TextBox mytextbox2 = new TextBox();
                        mytextbox2.Text = thisdata.str2;
                        mytextbox2.Location = new Point(450, (30 * (i - 2)) + 3);
                        mytextbox2.Size = new Size(400, 23);
                        mytextbox2.Parent = splitContainer1.Panel2;
                        Button newbutton2 = new Button();
                        newbutton2.BackColor = Color.LightGray;
                        newbutton2.Location = new Point(860, (30 * (i - 2)) + 3);
                        newbutton2.Size = new Size(100, 23);
                        newbutton2.Text = "Select";
                        newbutton2.Tag = new object[] { mytextbox, thisdata.selectlist };
                        newbutton2.Click += SelectModeOn;
                        newbutton2.Parent = splitContainer1.Panel2;

                        Array.Resize(ref newdata, newdata.Length + 1);
                        newdata[i - 2] = new object[] { mycheck, mytextbox, mytextbox2 };
                    }

                    if (data[i].GetType() == typeof(MyCheckboxStringSelectString))
                    {
                        MyCheckboxStringSelectString thisdata = (MyCheckboxStringSelectString)data[i];
                        CheckBox mycheck = new CheckBox();
                        mycheck.Location = new Point(10, (30 * (i - 2)) + 6);
                        mycheck.AutoSize = true;
                        mycheck.Checked = thisdata.check;
                        mycheck.Parent = splitContainer1.Panel2;
                        TextBox mytextbox = new TextBox();
                        mytextbox.Text = thisdata.str1;
                        mytextbox.Location = new Point(30, (30 * (i - 2)) + 3);
                        mytextbox.Size = new Size(400, 23);
                        mytextbox.Parent = splitContainer1.Panel2;
                        TextBox mytextbox2 = new TextBox();
                        mytextbox2.Text = thisdata.str2;
                        mytextbox2.Location = new Point(560, (30 * (i - 2)) + 3);
                        mytextbox2.Size = new Size(400, 23);
                        mytextbox2.Parent = splitContainer1.Panel2;
                        Button newbutton2 = new Button();
                        newbutton2.BackColor = Color.LightGray;
                        newbutton2.Location = new Point(450, (30 * (i - 2)) + 3);
                        newbutton2.Size = new Size(100, 23);
                        newbutton2.Text = "Select";
                        newbutton2.Tag = new object[] { mytextbox, thisdata.selectlist };
                        newbutton2.Click += SelectModeOn;
                        newbutton2.Parent = splitContainer1.Panel2;

                        Array.Resize(ref newdata, newdata.Length + 1);
                        newdata[i - 2] = new object[] { mycheck, mytextbox, mytextbox2 };
                    }

                    if (data[i].GetType() == typeof(MyCheckboxStringSelectStringSelect))
                    {
                        MyCheckboxStringSelectStringSelect thisdata = (MyCheckboxStringSelectStringSelect)data[i];
                        CheckBox mycheck = new CheckBox();
                        mycheck.Location = new Point(10, (30 * (i - 2)) + 6);
                        mycheck.AutoSize = true;
                        mycheck.Checked = thisdata.check;
                        mycheck.Parent = splitContainer1.Panel2;
                        TextBox mytextbox = new TextBox();
                        mytextbox.Text = thisdata.str1;
                        mytextbox.Location = new Point(30, (30 * (i - 2)) + 3);
                        mytextbox.Size = new Size(400, 23);
                        mytextbox.Parent = splitContainer1.Panel2;
                        TextBox mytextbox2 = new TextBox();
                        mytextbox2.Text = thisdata.str2;
                        mytextbox2.Location = new Point(560, (30 * (i - 2)) + 3);
                        mytextbox2.Size = new Size(400, 23);
                        mytextbox2.Parent = splitContainer1.Panel2;
                        Button newbutton1 = new Button();
                        newbutton1.BackColor = Color.LightGray;
                        newbutton1.Location = new Point(450, (30 * (i - 2)) + 3);
                        newbutton1.Size = new Size(100, 23);
                        newbutton1.Text = "Select";
                        newbutton1.Tag = new object[] { mytextbox, thisdata.selectlist1 };
                        newbutton1.Click += SelectModeOn;
                        newbutton1.Parent = splitContainer1.Panel2;
                        Button newbutton2 = new Button();
                        newbutton2.BackColor = Color.LightGray;
                        newbutton2.Location = new Point(980, (30 * (i - 2)) + 3);
                        newbutton2.Size = new Size(100, 23);
                        newbutton2.Text = "Select";
                        newbutton2.Tag = new object[] { mytextbox2, thisdata.selectlist2 };
                        newbutton2.Click += SelectModeOn;
                        newbutton2.Parent = splitContainer1.Panel2;

                        Array.Resize(ref newdata, newdata.Length + 1);
                        newdata[i - 2] = new object[] { mycheck, mytextbox, mytextbox2 };
                    }
                    if (data[i].GetType() == typeof(MyAddNodes))
                    {
                        MyAddNodes locc = (MyAddNodes)data[i];
                        string[] items = locc.items;
                        ComboBox newcombbox = new ComboBox();
                        newcombbox.Location = new Point(30, (30 * (i - 2)) + 3);
                        newcombbox.Size = new Size(200, 23);
                        newcombbox.Parent = splitContainer1.Panel2;
                        newcombbox.Items.AddRange(items);
                        Button newbutton1 = new Button();
                        newbutton1.BackColor = Color.LightGray;
                        newbutton1.Location = new Point(250, (30 * (i - 2)) + 3);
                        newbutton1.Size = new Size(100, 23);
                        newbutton1.Text = "Add";
                        newbutton1.Tag = newcombbox;
                        newbutton1.Click += OnAddClick;
                        newbutton1.Parent = splitContainer1.Panel2;
                        //ToolStripItem newitem2 = menuStrip2.Items.Add("Add");
                        //newitem2.Alignment = ToolStripItemAlignment.Right;
                        //newitem2.Click += AddMyNode;
                        //ToolStripComboBox newitm = new ToolStripComboBox();
                        //newitm.Alignment = ToolStripItemAlignment.Right;
                        //newitm.Size = new Size(150,20);
                        //newitm.Items.AddRange(items);
                        //menuStrip2.Items.Add(newitm);
                        //newitem2.Tag = newitm;

                        //Array.Resize(ref menutag, menutag.Length + 2);
                        //menutag[menutag.Length - 2] = newitem2;
                        //menutag[menutag.Length - 1] = newitm;

                        listBox2.Items.AddRange(items);
                        listBox2.DoubleClick += OnAddDoubleClick;

                        Array.Resize(ref newdata, newdata.Length + 1);
                        newdata[i - 2] = new object[] { };
                    }

                    if (data[i].GetType() == typeof(MyAddCusttomNode))
                    {
                        Label mynamelbl = new Label();
                        mynamelbl.Text = "Add modifier:";
                        mynamelbl.Location = new Point(30, (30 * (i - 2)) + 3);
                        mynamelbl.AutoSize = true;
                        mynamelbl.Parent = splitContainer1.Panel2;
                        TextBox mytextbox = new TextBox();
                        mytextbox.Location = new Point(180, (30 * (i - 2)) + 3);
                        mytextbox.Size = new Size(200, 23);
                        mytextbox.Parent = splitContainer1.Panel2;
                        Button newbutton1 = new Button();
                        newbutton1.BackColor = Color.LightGray;
                        newbutton1.Location = new Point(400, (30 * (i - 2)) + 3);
                        newbutton1.Size = new Size(100, 23);
                        newbutton1.Text = "Add";
                        newbutton1.Tag = mytextbox;
                        newbutton1.Click += AddModifier;
                        newbutton1.Parent = splitContainer1.Panel2;
                        //ToolStripItem newitem2 = menuStrip2.Items.Add("Add Modifier");
                        //newitem2.Alignment = ToolStripItemAlignment.Right;
                        //newitem2.Click += AddModifier;
                        //ToolStripTextBox newitm = new ToolStripTextBox();
                        //newitm.Alignment = ToolStripItemAlignment.Right;
                        //menuStrip2.Items.Add(newitm);
                        //newitem2.Tag = newitm;

                        //Array.Resize(ref menutag, menutag.Length + 2);
                        //menutag[menutag.Length - 2] = newitem2;
                        //menutag[menutag.Length - 1] = newitm;

                        Array.Resize(ref newdata, newdata.Length + 1);
                        newdata[i - 2] = new object[] { };
                    }

                    if (data[i].GetType() == typeof(MyAddAbilitySpecialNode))
                    {
                        //Debug.WriteLine(e.Node.Nodes.Count);
                        //e.Node.Nodes
                        Button newbutton1 = new Button();
                        newbutton1.BackColor = Color.LightGray;
                        newbutton1.Location = new Point(30, (30 * (i - 2)) + 3);
                        newbutton1.Size = new Size(200, 23);
                        newbutton1.Text = "Add AbilityValues";
                        newbutton1.Click += AddAbilitySpecial;
                        newbutton1.Parent = splitContainer1.Panel2;

                        Array.Resize(ref newdata, newdata.Length + 1);
                        newdata[i - 2] = new object[] { };
                    }

                    if (data[i].GetType() == typeof(MyAddStatesOrPropertiesNode))
                    {
                        MyAddStatesOrPropertiesNode locc = (MyAddStatesOrPropertiesNode)data[i];
                        Button newbutton1 = new Button();
                        newbutton1.BackColor = Color.LightGray;
                        newbutton1.Location = new Point(30, (30 * (i - 2)) + 3);
                        newbutton1.Size = new Size(200, 23);
                        if (locc.name == "States")
                        {
                            newbutton1.Text = "Add State";
                        }
                        else
                        {
                            newbutton1.Text = "Add Propertie";
                        }
                        newbutton1.Tag = locc.name;
                        newbutton1.Click += AddStateOrPropertie;
                        newbutton1.Parent = splitContainer1.Panel2;

                        Array.Resize(ref newdata, newdata.Length + 1);
                        newdata[i - 2] = new object[] { };
                    }

                    if (data[i].GetType() == typeof(MyAddActionNode))
                    {
                        ComboBox newcombbox = new ComboBox();
                        newcombbox.Location = new Point(30, (30 * (i - 2)) + 3);
                        newcombbox.Size = new Size(200, 23);
                        newcombbox.Parent = splitContainer1.Panel2;
                        newcombbox.Items.AddRange(ActionList);
                        Button newbutton1 = new Button();
                        newbutton1.BackColor = Color.LightGray;
                        newbutton1.Location = new Point(250, (30 * (i - 2)) + 3);
                        newbutton1.Size = new Size(100, 23);
                        newbutton1.Text = "Add";
                        newbutton1.Tag = newcombbox;
                        newbutton1.Click += OnAddActionClick;
                        newbutton1.Parent = splitContainer1.Panel2;

                        listBox2.Items.AddRange(ActionList);
                        listBox2.DoubleClick += OnAddActionDoubleClick;

                        Array.Resize(ref newdata, newdata.Length + 1);
                        newdata[i - 2] = new object[] { };
                    }

                    if (data[i].GetType() == typeof(MyAddItemRequirementsNode))
                    {
                        Button newbutton1 = new Button();
                        newbutton1.BackColor = Color.LightGray;
                        newbutton1.Location = new Point(30, (30 * (i - 2)) + 3);
                        newbutton1.Size = new Size(200, 23);
                        newbutton1.Text = "Add ItemRequirements";
                        newbutton1.Click += AddItemRequirements;
                        newbutton1.Parent = splitContainer1.Panel2;

                        Array.Resize(ref newdata, newdata.Length + 1);
                        newdata[i - 2] = new object[] { };
                    }
                }
                splitContainer1.Panel2.Tag = newdata;
                menuStrip2.Tag = menutag;
            }
        }

        private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("collapseAllToolStripMenuItem_Click");
            ClearPanels();
            treeView1.SelectedNode = null;
            treeView1.CollapseAll();
        }

        private void expandAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("expandAllToolStripMenuItem_Click");
            ClearPanels();
            treeView1.SelectedNode = null;
            treeView1.ExpandAll();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Version: 2.1.0\r\nCreator: Mechanicus", "About");
        }

        private void saveTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("saveTreeToolStripMenuItem_Click");
            ClearPanels();
            treeView1.SelectedNode = null;
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "tree files (*.dact)|*.dact";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(myStream, treeView1.Nodes.Cast<TreeNode>().ToList());
                    myStream.Close();
                }
            }

            //using (var path_dialog = new FolderBrowserDialog())
            //{
            //    if (path_dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //    {
            //        using (var stream = new FileStream(path_dialog.SelectedPath, FileMode.Create))
            //        {
            //            BinaryFormatter bf = new BinaryFormatter();
            //            bf.Serialize(stream, treeView1.Nodes.Cast<TreeNode>().ToList());
            //        }
            //    };
            //}
        }

        private void loadTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Open Tree";
            //fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "Tree files (*.dact)|*.dact";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                using (var stream = new FileStream(fdlg.FileName, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    object obj = bf.Deserialize(stream);

                    TreeNode[] nodeList = (obj as IEnumerable<TreeNode>).ToArray();
                    treeView1.Nodes.AddRange(nodeList);
                }
            }
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void splitContainer3_Panel2_SizeChanged(object sender, EventArgs e)
        {
            //Debug.WriteLine("splitContainer3_Panel2_SizeChanged");
            //ClearPanels();
            //TreeNode selnode = treeView1.SelectedNode;
            //treeView1.SelectedNode = null;
            //treeView1.SelectedNode = selnode;
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            //if (treeView1.SelectedNode == null) return;
            string line = "";
            string filetext = "";
            StreamReader file;
            try
            {
                file = new StreamReader(blankspath + @"\" + listBox1.SelectedItem + ".txt");
            }
            catch (Exception error)
            {
                //Debug.WriteLine(error);
                return;
            }
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
            alldata.childs = new List<MyNodeData>();
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
            string[] nodes = new string[1];
            if (treeView1.SelectedNode != null) nodes[0] = treeView1.SelectedNode.Name;
            else nodes[0] = null;
            CreateNodes(alldata.childs[0], nodes, 0);
        }

        private void selectPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    blankspath = fbd.SelectedPath;

                    //Debug.WriteLine(fbd.SelectedPath);
                    string[] files = Directory.GetFiles(fbd.SelectedPath);
                    listBox1.Items.Clear();
                    for (int i = 0; files.Length > i; i++)
                    {
                        if (files[i].Substring(files[i].Length - 4) == ".txt")
                        {
                            string filename = RemovePath(files[i]);
                            listBox1.Items.Add(filename.Substring(0, filename.Length - 4));
                        }
                    }
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                File.Delete(blankspath + @"\" + listBox1.SelectedItem + ".txt");
                UpdateBlankList();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}