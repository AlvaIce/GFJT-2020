using System.Data;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System.Data.Odbc;

namespace QRST.WorldGlobeTool.VisualForms
{
    public partial class ShapeFileInfoDlg : Form
    {
        public ShapeFileInfoDlg(string dbfPath, bool isInZip)
        {
            InitializeComponent();
            this.Text += string.Format("\"{0}\"", dbfPath);
            if (dbfPath != "")
            {
                if (!isInZip)
                    setDbfInfo(dbfPath);
                else
                    setDbfInfoFromZip(dbfPath);
            }
        }


        private void setDbfInfo(string dbfPath)
        {
            DataTable dt = getInfoFromDBF(dbfPath);
            dataGridViewShapeDBF.DataSource = dt;
            dataGridViewShapeDBF.Tag = dbfPath;
        }


        private void setDbfInfoFromZip(string dbfPath)
        {
            try
            {
                //Navigate the Zip to find the files and update their index
                ZipFile zFile = new ZipFile(dbfPath);
                foreach (ZipEntry ze in zFile)
                {
                    if (ze.Name.ToLower().EndsWith(".dbf"))
                    {
                        //Extracts the file in temp
                        FastZip fz = new FastZip();
                        fz.ExtractZip(dbfPath, Path.GetTempPath(),
                            ICSharpCode.SharpZipLib.Zip.FastZip.Overwrite.Always, null, "", "",true);
                        setDbfInfo(Path.Combine(Path.GetTempPath(), ze.Name));
                    }
                }
            }
            catch { return; }	
        }

        /// <summary>
        /// gets the information from a dbf as a DataTable (null if there was an error)
        /// </summary>		
        private DataTable getInfoFromDBF(string dbfPath)
        {
            try
            {
                string connectionString = "Driver={Microsoft dBASE Driver (*.dbf)};DBQ=" +
                    Path.GetDirectoryName(Path.GetFullPath(dbfPath));
                OdbcConnection conn = new OdbcConnection(connectionString);
                OdbcCommand command = new OdbcCommand("SELECT * FROM "
                    + Path.GetFileNameWithoutExtension(dbfPath), conn);
                DataSet ds = new DataSet();
                OdbcDataAdapter da = new OdbcDataAdapter(command);
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch
            {
                return null;
            }
        }

    }
}
