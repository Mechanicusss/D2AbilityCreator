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
        public class MyNodeData
        {
            public string name { get; set; }
            public List<MyNodeData> childs { get; set; }
            public Dictionary<string, string> data { get; set; }
            public int level { get; set; }

            public void AddStringStringData(string str1, string str2, int level)
            {
                if (level > this.level)
                {
                    childs[childs.Count - 1].AddStringStringData(str1, str2, level);
                }
                else
                {
                    data[str1] = str2;
                }
            }

            public void CreateChield(string name, int level)
            {
                if (level > this.level)
                {
                    childs[childs.Count - 1].CreateChield(name, level);
                }
                else
                {
                    MyNodeData newcield = new MyNodeData();
                    newcield.name = name;
                    newcield.level = this.level + 1;
                    newcield.childs = new List<MyNodeData>();
                    newcield.data = new Dictionary<string, string>();
                    childs.Add(newcield);
                }
            }

            public MyNodeData GetChield(int level)
            {
                if (level > this.level)
                {
                    return childs[0];
                }
                else
                {
                    return childs[0].GetChield(level);
                }
            }
        }
    }
}
