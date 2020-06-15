using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
 
namespace MoveFile
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.textBox1.Enabled = false;
            this.textBox2.Enabled = false;
            this.comboBox1.SelectedIndex = 0;
            setComboboxItem(0);
            this.sharPath=backSharPath();
            this.textBox2.Text = sharPath;
            
        }
        public string[] arr = {"上海市金山化工园区","南京化工园区","四川汶川地区","四川雅安地区","安徽池州","岳阳地区","宁波",
                                "广州","郴州市苏仙区","四川葫芦山地区","岳阳地区","北京","深圳","内蒙古二连浩特","宁夏银川"};
        public string sharPath;
        private void button1_Click(object sender, EventArgs e)
        {
            //OpenFileDialog open = new OpenFileDialog();
            //if (open.ShowDialog() == DialogResult.OK)
            //{
            //    textBox1.Text = open.FileName;
            //}
            string path = "";
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = false;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                path = folderBrowserDialog.SelectedPath;
            }
            this.textBox1.Text = path;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            setComboboxItem(this.comboBox1.SelectedIndex);
        }
        public void setComboboxItem(int cout)
        {
            this.comboBox2.Items.Clear();
            if (cout == 4)
            {
                this.comboBox2.Items.Add(arr[8]);
            }
            else if (cout > 4)
            {
                this.comboBox2.Items.Add(arr[cout * 2-1]);
                this.comboBox2.Items.Add(arr[cout * 2]);
            }
            else
            {
                this.comboBox2.Items.Add(arr[cout * 2]);
                this.comboBox2.Items.Add(arr[cout * 2 + 1]);
            }
            this.comboBox2.SelectedIndex = 0;
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text != "")
            {
                string fileName = this.comboBox2.SelectedItem.ToString();
                string newDir = sharPath + "\\" + fileName;
                judgeFileExit(newDir);
                copyFile(this.textBox1.Text, newDir);
                MessageBox.Show("发布成功！");
                this.textBox1.Text = "";
               
            }
            else 
            {
                MessageBox.Show("文件夹路径未选择！");
            }
        }
        public void copyFile(string rootPath,string newDir)
        {
            judgeFileExit(rootPath);
            judgeFileExit(newDir);
            string[] arrFile = Directory.GetFiles(rootPath);
            for (int i = 0; i < arrFile.Length; i++)
            {
                string strFileName = arrFile[i].Substring(arrFile[i].LastIndexOf("\\") + 1, arrFile[i].Length - arrFile[i].LastIndexOf("\\") - 1);
                File.Copy(arrFile[i], newDir + "\\" + strFileName, true);
            }
             
            DirectoryInfo dirInfo = new DirectoryInfo(rootPath);
            DirectoryInfo[] ZiPath = dirInfo.GetDirectories();
            for (int j = 0; j < ZiPath.Length; j++)
            {
                string strZiPath = rootPath + "\\" + ZiPath[j].ToString();
                copyFile(strZiPath, newDir+"\\"+ZiPath[j].ToString());
            }
        
        }
        public static void judgeFileExit(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
           
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = "";
            this.comboBox1.SelectedIndex = 0;
            setComboboxItem(0);

            this.Close();
        }
        public static string backSharPath()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("./HAlgPublishConfig.xml");
            XmlNode  root = doc.SelectSingleNode("/ROOT/path");
            string path = root.InnerText;
            return path;
        }
    
    
    }
}
