using System;
using System.Data;
using DevExpress.XtraEditors;
using QRST_DI_DS_Metadata.MetaDataDefiner;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class mucSatalliteSensor : DevExpress.XtraEditors.XtraUserControl
    {
        public bool isAdd = false;

        private Sensors sensorModel = new Sensors();
        private Satellite satelliteMoedl = new Satellite();
        public string datatype = "satellites";
        
        private bool isFirstLoad = true;

        public mucSatalliteSensor()
        {
            InitializeComponent();
        }



        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveUpdate();
        }

        public void SaveUpdate()
        {
            if (!string.IsNullOrEmpty(textName.Text))
            {
                if (datatype.Equals("satellites"))
                {
                  
                    if (!isAdd) //更新
                    {
                        satelliteMoedl.NAME = textName.Text;
                        satelliteMoedl.DESCRIPTION = textdescription.Text;
                        Satellite.Update(satelliteMoedl, TheUniversal.EVDB.sqlUtilities);
                    }
                    else//添加
                    {
                        Satellite salmdl = new Satellite() {NAME = textName.Text,DESCRIPTION = textdescription.Text };
                        Satellite.Add(salmdl, TheUniversal.EVDB.sqlUtilities);
                    }
                    GetSatalliteDataSource("");
                }
                else
                {
                    if (!isAdd) //更新
                    {
                        sensorModel.NAME = textName.Text;
                        sensorModel.DESCRIPTION = textdescription.Text;
                        Sensors.Update(sensorModel, TheUniversal.EVDB.sqlUtilities);
                    }
                    else//添加
                    {
                        Sensors senmdl = new Sensors() { NAME = textName.Text, DESCRIPTION = textdescription.Text };
                        Sensors.Add(senmdl, TheUniversal.EVDB.sqlUtilities);
                    }
                    GetSensorsDataSource("");
                }
            }
            else
            {
                XtraMessageBox.Show("请输入名称！");
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            textName.Visible = false;
            textdescription.Visible = false;
            btnCancle.Visible = false;
            btnSave.Visible = false;

            labeldescription.Visible = true;
            labelName.Visible = true;
        }



        private void labeldescription_Click(object sender, EventArgs e)
        {
            textName.Visible = true;
            textdescription.Visible = true;
            btnCancle.Visible = true;
            btnSave.Visible = true;

            labeldescription.Visible = false;
            labelName.Visible = false;

            isAdd = false;
        }

        private void labelName_Click(object sender, EventArgs e)
        {
            textName.Visible = true;
            textdescription.Visible = true;
            btnCancle.Visible = true;
            btnSave.Visible = true;
            
            labeldescription.Visible = false;
            labelName.Visible = false;

            isAdd = false;
        }

        public void GetSatalliteDataSource(string whereCondition="")
        {
            string path = TheUniversal.EVDB.sqlUtilities.GetDbConnection();
            gridControl1.DataSource = TheUniversal.EVDB.sqlUtilities.GetDataSet("select * from satellites " + whereCondition).Tables[0];
            if (((DataTable)gridControl1.DataSource).Rows.Count>0)
            {
                gridView1.FocusedRowHandle = 0;
            }
        }

        public void GetSensorsDataSource(string whereCondition = "")
        {
            gridControl1.DataSource = TheUniversal.EVDB.sqlUtilities.GetDataSet("select * from sensors " + whereCondition).Tables[0];
            if (((DataTable)gridControl1.DataSource).Rows.Count > 0)
            {
                gridView1.FocusedRowHandle = 0;
            }
        }
        
        /// <summary>
        /// 换行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataTable dataSource = (DataTable)gridControl1.DataSource;
            if (e.FocusedRowHandle >= 0)
            {
                labeldescription.Visible =true;
                labelName.Visible = true;

                labelName.Text = dataSource.Rows[e.FocusedRowHandle]["NAME"].ToString();
                labeldescription.Text = dataSource.Rows[e.FocusedRowHandle]["DESCRIPTION"].ToString();
                textName.Text = dataSource.Rows[e.FocusedRowHandle]["NAME"].ToString();
                textdescription.Text = dataSource.Rows[e.FocusedRowHandle]["DESCRIPTION"].ToString();
                textName.Visible = false;
                textdescription.Visible = false;
                btnCancle.Visible = false;
                btnSave.Visible = false;

                if (datatype.Equals("satellites"))
                {
                    satelliteMoedl.ID = Convert.ToInt32(dataSource.Rows[e.FocusedRowHandle]["ID"]);
                    satelliteMoedl.NAME = labelName.Text;
                    satelliteMoedl.DESCRIPTION = labeldescription.Text;
                    satelliteMoedl.QRST_CODE = dataSource.Rows[e.FocusedRowHandle]["QRST_CODE"].ToString();
                }
                else
                {
                    sensorModel.ID = Convert.ToInt32(dataSource.Rows[e.FocusedRowHandle]["ID"]);
                    sensorModel.NAME = labelName.Text;
                    sensorModel.DESCRIPTION = labeldescription.Text;
                    sensorModel.QRST_CODE = dataSource.Rows[e.FocusedRowHandle]["QRST_CODE"].ToString();
                }
            }
    
        }

        private void mucSatalliteSensor_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible && isFirstLoad)
            {
                GetSatalliteDataSource("");

                labelName.Text = ((DataTable)gridControl1.DataSource).Rows[gridView1.FocusedRowHandle]["NAME"].ToString();
                labeldescription.Text = ((DataTable)gridControl1.DataSource).Rows[gridView1.FocusedRowHandle]["DESCRIPTION"].ToString();
                textName.Text = ((DataTable)gridControl1.DataSource).Rows[gridView1.FocusedRowHandle]["NAME"].ToString();
                textdescription.Text = ((DataTable)gridControl1.DataSource).Rows[gridView1.FocusedRowHandle]["DESCRIPTION"].ToString();

                isFirstLoad = false;
            }
        }


        //添加新的卫星或传感器
        void AddNewItem()
        {
            textName.Visible = true;
            textName.Text = "";
            textdescription.Visible = true;
            textdescription.Text = "";

            if (datatype.Equals("satellites"))
            {
                satelliteMoedl = new Satellite();
            }
            else
            {
                sensorModel = new Sensors();
            }
        }

        //设置类型，卫星或者传感器
        public void SetType(string equipmentType)
        {
            datatype = equipmentType;
        }

        /// <summary>
        /// 初始化添加控件
        /// </summary>
        public void  InitializeAddControl()
        {
            textName.Visible = true;
            textName.Text = "";
            textdescription.Visible = true;
            textdescription.Text = "";
            btnCancle.Visible = true;
            btnSave.Visible = true;

            labeldescription.Visible = false;
            labelName.Visible = false;
        }
    }
}
