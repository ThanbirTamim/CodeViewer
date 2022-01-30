using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastColoredTextBoxNS;
using WK.Libraries.BetterFolderBrowserNS;

namespace CodeEditor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public string rootDirectory { get; private set; }
        private TreeView ListDirectory(TreeView treeView, string path)
        {
            treeView.Nodes.Clear();
            var rootDirectoryInfo = new DirectoryInfo(path);
            treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo));
            treeView.Nodes[treeView.Nodes.Count - 1].ImageIndex = 0;
            return treeView;
        }

        private static TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeNode(directoryInfo.Name);
            foreach (var directory in directoryInfo.GetDirectories())
            {
                directoryNode.Nodes.Add(CreateDirectoryNode(directory));
                directoryNode.Nodes[directoryNode.Nodes.Count-1].ImageIndex = 0;
            }
            foreach (var file in directoryInfo.GetFiles())
            {
                directoryNode.Nodes.Add(new TreeNode(file.Name));
                directoryNode.Nodes[directoryNode.Nodes.Count - 1].ImageIndex = 1;
            }
            return directoryNode;
        }

        private void treeViewControl_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string path = $"{Path.GetDirectoryName(rootDirectory)}\\{e.Node.FullPath}";
            if(File.Exists(path))
                fastColoredTextBox.Text = File.ReadAllText(path, Encoding.UTF8);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            var betterFolderBrowser = new BetterFolderBrowser();

            betterFolderBrowser.Title = "Select a folder...";
            betterFolderBrowser.RootFolder = "C:\\";

            betterFolderBrowser.Multiselect = false;

            if (betterFolderBrowser.ShowDialog() == DialogResult.OK)
                rootDirectory = betterFolderBrowser.SelectedFolder;
            if (String.IsNullOrEmpty(rootDirectory))
                return;

            treeViewControl = ListDirectory(treeViewControl, rootDirectory);
        }
    }
}
