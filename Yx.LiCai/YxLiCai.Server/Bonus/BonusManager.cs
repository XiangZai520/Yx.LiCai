using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.Model.InterestBonus;

namespace YxLiCai.Server.Bonus
{
    /// <summary>
    /// 加息券相关操作业务逻辑类
    /// </summary>
    public class BonusManager
    {
        private YxLiCai.Dao.InterestBonus.InterestBonusDAO _dao = new Dao.InterestBonus.InterestBonusDAO();
        public delegate bool AddBonusDelegate(Int64 userID);

        #region Add
        public bool AddCategory(InterestBonusCategoryModel model)
        {
            model.CreateDate = DateTime.Now;

            return _dao.Add(model);
        }
        /// <summary>
        /// 生成加息券
        /// </summary>
        /// <returns></returns>
        public static bool AddInterestBonus(Int64 userID)
        {
            return false;
        }
        #endregion

        #region Update
        public bool UpdateCategory(InterestBonusCategoryModel model)
        {
            return _dao.Update(model);
        }
        #endregion

        #region Get
        public InterestBonusCategoryModel GetCategoryByID(int id)
        {
            return _dao.GetModel(id);
        }
        public List<InterestBonusCategoryModel> GetCategoryList(string strWhere)
        {
            var ds = _dao.GetList(strWhere);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                var list = new List<InterestBonusCategoryModel>();
                foreach(var row in ds.Tables[0].Rows)
                {
                    list.Add(_dao.DataRowToModel(row as System.Data.DataRow));           
                }

                return list;
            }

            return null;
        }
        #endregion
    }
    /// <summary>
    /// 注册送加息券
    /// </summary>
    public class RegisterBonus : IBonus
    {
        /// <summary>
        /// 注册送加息券
        /// </summary>
        /// <returns></returns>
        public virtual bool GenerateInterestBonus()
        {
            return false;
        }
        /// <summary>
        /// 注册送红包
        /// </summary>
        /// <returns></returns>
        public virtual bool GenerateLuckyMoney()
        {
            return false;
        }
        /// <summary>
        /// 注册送特享金
        /// </summary>
        /// <returns></returns>
        public virtual bool GenerateSpecialPrincipal()
        {
            return false;
        }
    }
    /// <summary>
    /// 邀请好友送加息券
    /// </summary>
    public class InviteBonus: IBonus
    {
        /// <summary>
        /// 邀请送加息券
        /// </summary>
        /// <returns></returns>
        public virtual bool GenerateInterestBonus()
        {
            return false;
        }
        /// <summary>
        /// 邀请送红包
        /// </summary>
        /// <returns></returns>
        public virtual bool GenerateLuckyMoney()
        {
            return false;
        }
        /// <summary>
        /// 邀请送特享金
        /// </summary>
        /// <returns></returns>
        public virtual bool GenerateSpecialPrincipal()
        {
            return false;
        }
    }
}
