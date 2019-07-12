//----------------------------------------------------------------------------------
/// <summary> 
/// 通用数据返回接口
/// </summary>
/// <author> 
/// 平扬
/// </author>
/// <history>
/// 2014.1.20 Created
/// </history>
/// <version> 
/// 1.00
/// </version>
//----------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YxLiCai.Model
{

    public class ResultApiInfo<T>
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 结果说明信息
        /// </summary>
        public string msg
        {
            get;
            set;
        }

        /// <summary>
        /// 数据
        /// </summary>
        public T data { get; set; }
    } 
    /// <summary>
    /// 操作结果类
    ///<typeparam name="T">所携带数据类型</typeparam>
    /// </summary>
    public class ResultInfo<T>
    {
        private bool result;
        private string message;
        private T data;

        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Result
        {
            get { return result; }
            set { result = value; }
        }

        /// <summary>
        /// 结果说明信息
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data
        {
            get { return data; }
            set { data = value; }
        }
    }

    /// <summary>
    /// 操作结果类
    /// </summary>
    public class ResultInfo
    {
        private bool result;
        private string message;


        public ResultInfo(bool isSuccess, string message)
        {
            this.result = isSuccess;
            this.message = message;
        }

        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Result
        {
            get { return result; }
            set { result = value; }
        }

        /// <summary>
        /// 结果说明信息
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }
}
