using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D2AbilityCreator2
{
    public partial class Form1 : Form
    {
        public string RemovePath(string str)
        {
            return str.Substring(str.LastIndexOf(@"\") + 1);
        }

        public void OpenFile(object sender, EventArgs e)
        {
            Button thisb = (Button)sender;
            Process.Start((string)thisb.Tag);
        }

        public void DeleteNode(object sender, EventArgs e)
        {
            Debug.WriteLine("DeleteNode");
            ClearPanels();
            Button thisb = (Button)sender;
            treeView1.Nodes.Remove((TreeNode)thisb.Tag);
        }

        public void LoadFiles(string nodekey, string[] files)
        {
            for (int i = 0; files.Length > i; i++)
            {
                if (files[i].Substring(files[i].Length - 4) == ".txt")
                {
                    TreeNode newnode = new TreeNode();
                    if (nodekey != null)
                    {
                        TreeNode[] neednodes = treeView1.Nodes.Find(nodekey, true);
                        newnode = neednodes[0].Nodes.Add(RemovePath(files[i]));
                    }
                    else
                    {
                        newnode = treeView1.Nodes.Add(RemovePath(files[i]));
                    }
                    newnode.Name = "node" + nodenum;
                    nodenum++;
                    newnode.Tag = new object[]
                    {
                        files[i],
                        "file.txt"
                    };
                }
                else if (files[i].Substring(files[i].Length - 4) == ".lua")
                {
                    TreeNode newnode = new TreeNode();
                    if (nodekey != null)
                    {
                        TreeNode[] neednodes = treeView1.Nodes.Find(nodekey, true);
                        newnode = neednodes[0].Nodes.Add(RemovePath(files[i]));
                    }
                    else
                    {
                        newnode = treeView1.Nodes.Add(RemovePath(files[i]));
                    }
                    if (files[i].IndexOf("vscripts") != -1)
                    {
                        newnode.Name = files[i].Substring(files[i].IndexOf("vscripts") + 9);
                    }
                    else
                    {
                        newnode.Name = RemovePath(files[i]);
                    }
                    //Debug.WriteLine(newnode.Name);
                    newnode.Tag = new object[]
                    {
                        files[i],
                        "file.lua"
                    };
                }
            }
        }

        public void LoadFolders(string nodekey, string[] folders)
        {
            for (int i = 0; folders.Length > i; i++)
            {
                TreeNode[] neednode = treeView1.Nodes.Find(nodekey, true);
                TreeNode newnode = neednode[0].Nodes.Add(RemovePath(folders[i]));
                newnode.Name = "node" + nodenum;
                nodenum++;
                newnode.Tag = new object[]
                {
                        folders[i],
                        "folder"
                };
                string[] files = Directory.GetFiles(folders[i]);
                string[] newfolders = Directory.GetDirectories(folders[i]);
                LoadFolders(newnode.Name, newfolders);
                LoadFiles(newnode.Name, files);
            }
        }

    }
}
