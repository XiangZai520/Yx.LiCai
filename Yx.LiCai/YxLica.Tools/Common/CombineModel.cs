using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Tools.Common
{
    public static class CombineModel<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalModel"></param>
        /// <param name="updatedMode"></param>
        /// <returns></returns>
        public static T MergeModels(T originalModel, T updatedMode) 
        {
            var original_Type = originalModel.GetType();
            var updated_type = updatedMode.GetType();

            foreach (var property in updated_type.GetProperties())
            {
                var propertyName = property.Name;
                var value = property.GetValue(updatedMode, null);
                if (value != null)
                {
                    System.Reflection.PropertyInfo propertyInfo = original_Type.GetProperty(property.Name);
                    propertyInfo.SetValue(originalModel, value, null);
                }
            }

            return originalModel;
        }
    }
}
