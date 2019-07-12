using YxLiCai.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model
{
    
    public class ResultModel
    {
        public ResultModel(bool isSuccess, string message)
            : this(isSuccess, message, null)
        {
        }

        public ResultModel(bool isSuccess, string message, object data)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
            this.Data = data;
        }

        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public object Data { get; set; }
    }

    public class ResultModel<TResult>
    {
        public ResultModel(int status, string message, TResult result)
        {
            this.Status = status;
            this.Message = message;
            this.Result = result;
        }

        public static ResultModel<TResult> Conclude(Enum status)
        {
            return Conclude(status, default(TResult));
        }

        public static ResultModel<TResult> Conclude(Enum status, TResult result)
        {
            var enumItem = EnumExtenstion.GetEnumItem(status.GetType(), status);
            return new ResultModel<TResult>(enumItem.Value, enumItem.Text, result);
        }

        public int Status { get; protected set; }
        public string Message { get; protected set; }
        public TResult Result { get; protected set; }
    }
}
