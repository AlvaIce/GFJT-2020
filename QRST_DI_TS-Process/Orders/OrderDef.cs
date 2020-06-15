using System;
using System.Collections.Generic;
using QRST_DI_TS_Process.Tasks;

namespace QRST_DI_TS_Process.Orders
{
    /// <summary>
    /// 实体类orderdef 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class OrderDef
    {
        public OrderDef()
        { }
        #region Model
        private int _id;
        private string _name;
        private string _qrst_code;
        private string _tserversite;
        private string _status;
        private string _type;
        private string _owner;
        private DateTime? _submittime;
        private string _tasks;
        private string _taskparams;
        private string _phase;
        private string _ordercode;
        private string _priority;
        private string _orderparams;
        private string _description;

        public List<taskdef> TaskDefList
        {
            get { return taskdef.GetTaskDefsByStr(Tasks); }
        }
        /// <summary>
        /// auto_increment
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string QRST_CODE
        {
            set { _qrst_code = value; }
            get { return _qrst_code; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TServerSite
        {
            set { _tserversite = value; }
            get { return _tserversite; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Owner
        {
            set { _owner = value; }
            get { return _owner; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? SubmitTime
        {
            set { _submittime = value; }
            get { return _submittime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Tasks
        {
            set { _tasks = value; }
            get { return _tasks; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TaskParams
        {
            set { _taskparams = value; }
            get { return _taskparams; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Phase
        {
            set { _phase = value; }
            get { return _phase; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrderCode
        {
            set { _ordercode = value; }
            get { return _ordercode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Priority
        {
            set { _priority = value; }
            get { return _priority; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrderParams
        {
            set { _orderparams = value; }
            get { return _orderparams; }
        }
        #endregion Model

    }
}
