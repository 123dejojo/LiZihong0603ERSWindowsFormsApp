using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiZihong0603ERSWindowsFormsApp
{
    public partial class EmployeeRecordsForm : Form
    {
        private TreeNode tvRootNode;

        public EmployeeRecordsForm()
        {
            InitializeComponent();
            PopulateTreeView();
            InitalizeListView();
        }

        private void EmployeeRecordsForm_Load(object sender, EventArgs e)
        {

        }
        private void PopulateTreeView()
        {
            statusBarPanel1.Tag = "Refreshing Employee Code. Please wait...";
            this.Cursor = Cursors.WaitCursor;
            treeView1.Nodes.Clear();
            tvRootNode = new TreeNode("Employee Records");
            this.Cursor = Cursors.Default;
            treeView1.Nodes.Add(tvRootNode);

            TreeNodeCollection nodeCollection = tvRootNode.Nodes;
            XmlTextReader reader = new XmlTextReader("D:\\文档\\外教5\\MyRepos" +
                "\\LiZihong0603ERSWindowsFormsApp\\LiZihong0603ERSWindowsFormsApp\\EmpRec.xml");
            reader.MoveToElement();
            try
            {
                while (reader.Read())
                {
                    if (reader.HasAttributes && reader.NodeType == XmlNodeType.Element)
                    {
                        reader.MoveToElement();//<EmRecordsData>
                        reader.MoveToElement();//<Ecode>

                        reader.MoveToAttribute("Id");//Id="E001"
                        string strVal = reader.Value;//E001

                        reader.Read();
                        reader.Read();
                        if (reader.Name == "Dept")
                        {
                            reader.Read();
                        }
                        TreeNode EcodeNode = new TreeNode(strVal);
                        nodeCollection.Add(EcodeNode);
                    }
                }
                statusBarPanel1.Text = "Click on an employee code to see their record.";
            }
            catch (XmlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        protected void InitalizeListView()
        {
            listView1.Clear();
            listView1.Columns.Add("Emplyee Name", 255, HorizontalAlignment.Left);
            listView1.Columns.Add("Data of Join", 70, HorizontalAlignment.Right);
            listView1.Columns.Add("Gread", 105, HorizontalAlignment.Left);
            listView1.Columns.Add("Salary", 105, HorizontalAlignment.Left);
        }
        protected void PopulateListView(TreeNode crrNode)
        {
            InitalizeListView();
            XmlTextReader listRead = new XmlTextReader("D:\\文档\\外教5\\MyRepos" +
                "\\LiZihong0603ERSWindowsFormsApp\\LiZihong0603ERSWindowsFormsApp\\EmpRec.xml");
            listRead.MoveToElement();
            while (listRead.Read())
            {
                string strNodeName;
                string strNodePath;
                string name;
                string gread;
                string doj;
                string sal;
                string[] strItemsArr = new string[4];
                listRead.MoveToFirstAttribute();//Id="E001"
                strNodeName = listRead.Value;
                strNodePath = crrNode.FullPath.Remove(0, 17);
                if (strNodePath == strNodeName)
                {
                    ListViewItem lvi;

                    listRead.MoveToNextAttribute();
                    name=listRead.Value;//name "Michael Preey"
                    lvi = listView1.Items.Add(name);

                    listRead.Read();
                    listRead.Read();
                    
                    listRead.MoveToFirstAttribute();
                    doj = listRead.Value;//DateOfJoin="02-02-1999"
                    lvi.SubItems.Add(doj);

                    listRead.MoveToNextAttribute();
                    gread = listRead.Value;//Gread="A"
                    lvi.SubItems.Add(gread);

                    listRead.MoveToNextAttribute();
                    sal = listRead.Value;//salary="1750"
                    lvi.SubItems.Add(sal);

                    listRead.MoveToNextAttribute();
                    listRead.MoveToElement();
                    listRead.ReadString();
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode currNode = e.Node;
            if (tvRootNode == currNode)
            {
                statusBarPanel1.Text = "Double Click the Employee Record.";
                return;
            }
            else
            {
                statusBarPanel1.Text = "Click an Employee code to view Individual record";
            }
            PopulateListView(currNode);
        }
    }
}
