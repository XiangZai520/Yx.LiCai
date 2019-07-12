using System;
using System.Collections.Generic;
using YxLiCai.Model;
using YxLiCai.Model.Project;
using YxLiCai.Tools;

namespace YxLiCai.Server.Project
{
    /// <summary>
    /// 项目业务操作类
    /// </summary>
    public class ProjectService
    {
        YxLiCai.Dao.Project.ProjectDao dao = new Dao.Project.ProjectDao();

        #region 添加操作
        /// <summary>
        /// 添加项目信息
        /// </summary>
        /// <param name="model">项目实体信息</param>
        /// <param name="CreateID">创建人ID</param>
        /// <returns></returns>
        public ResultInfo<bool> Add(ProjectModel model, int CreateID)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.Add(model, CreateID);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        #endregion

        #region 删除操作
        /// <summary>
        /// 删除项目信息（一条数据）
        /// </summary>
        /// <param name="Id">项目ID</param>
        /// <returns></returns>
        public ResultInfo<bool> Delete(int Id)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.Delete(Id);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        /// <summary>
        /// 删除数据批量删除
        /// </summary>
        /// <param name="Idlist"></param>
        /// <returns></returns>
        public ResultInfo<bool> DeleteList(string Idlist)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.DeleteList(Idlist);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        public bool DeleteProject(string list)
        {
            try
            {
                return dao.DeleteList(list);
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #endregion

        #region 修改操作
        /// <summary>
        /// 修改项目信息
        /// </summary>
        /// <param name="model">项目实体信息</param>
        /// <param name="OldModel">原来的数据信息</param>
        /// <param name="CreadtID">创建人ID</param>
        /// <returns></returns>
        public ResultInfo<bool> Update(ProjectModel model, ProjectModel OldModel, int CreadtID)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.Update(model, OldModel, CreadtID);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        /// <summary>
        /// 更新一条数据(修改项目审核状态)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="CreadtID">创建人ID</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateProjectStatus(ProjectModel model, int CreadtID)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.UpdateProjectStatus(model, CreadtID);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;

        }
        /// <summary>
        /// 更新一条数据(修改项目权值)
        /// </summary>
        /// <param name="model">项目实体信息</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateProjectWeight(int Weight, int ProjectID)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.UpdateProjectWeight(Weight, ProjectID);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        #endregion

        #region 查询操作
        /// <summary>
        /// 通过项目id  得到一条项目信息
        /// </summary>
        /// <param name="id">项目id</param>
        /// <returns></returns>
        public ResultInfo<ProjectModel> GetModel(int id)
        {
            ResultInfo<ProjectModel> result = new ResultInfo<ProjectModel>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = dao.GetModel(id);
                result.Result = true;

            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

        /// <summary>
        /// 获得项目信息列表
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        public List<ProjectModel> GetProjectList(string strWhere)
        {
            try
            {
                return dao.GetList(strWhere);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得项目列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<ProjectModel> GetProjuctList(string Panme, int Pstate, DateTime Sdate, DateTime Edate)
        {
            var ds = dao.GetListM(Panme, Pstate, Sdate, Edate);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                var list = new List<ProjectModel>();
                foreach (var row in ds.Tables[0].Rows)
                {
                    list.Add(dao.DataRowToModel(row as System.Data.DataRow));
                }
                return list;
            }
            return null;
        }
        /// <summary>
        /// 获得项目列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<ProjectModel> GetProjuctList(string ProjectName, DateTime Sdate, DateTime Edate, int Sweight, int Eweight)
        {
            var ds = dao.GetListM(ProjectName, Sdate, Edate, Sweight, Eweight);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                var list = new List<ProjectModel>();
                foreach (var row in ds.Tables[0].Rows)
                {
                    list.Add(dao.DataRowToModel(row as System.Data.DataRow));
                }
                return list;
            }
            return null;
        }
        #endregion

        #region 获取融资方信息ID、单位名称
        /// <summary>
        /// 获取融资方信息ID、单位名称 
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public ResultInfo<List<YxLiCai.Model.UserFinancingManagement.UserInfo_FinancingManagement_Model>> GetUserInfo_FinancingManagement_Model()
        {
            ResultInfo<List<YxLiCai.Model.UserFinancingManagement.UserInfo_FinancingManagement_Model>> result = new ResultInfo<List<YxLiCai.Model.UserFinancingManagement.UserInfo_FinancingManagement_Model>>();
            result.Result = false;
            result.Data = null;
            try
            {
                var list = dao.GetUserInfo_FinancingManagement_Model();
                if (list.Count > 0)
                {
                    result.Result = true;
                    result.Data = list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;

            }
            return result;
        }
        #endregion

        #region 判断项目名称是否重复
        /// <summary>
        /// 修改项目信息
        /// </summary>
        /// <param name="model">项目实体信息</param>
        /// <param name="Pid">项目ID</param>
        /// <returns></returns>
        public ResultInfo<int> ISProtExist(string ProjectName, int Pid)
        {
            ResultInfo<int> result = new ResultInfo<int>();
            result.Result = false;
            result.Data = 0;
            try
            {
                result.Data = dao.ISProtExist(ProjectName, Pid);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

        #endregion

        #region  通过项目ID得到一个简单的项目信息
        /// <summary>
        /// 通过项目ID得到一个简单的项目信息
        /// </summary>
        /// <param name="PID">项目ID</param>
        /// <returns></returns>
        public ResultInfo<ProjectModel> GetProjectModelSmple(int PID)
        {
            ResultInfo<ProjectModel> result = new ResultInfo<ProjectModel>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = dao.GetProjectModelSmple(PID);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        #endregion
        

        #region 添加项目日志记录信息
        /// <summary>
        /// 添加项目日志记录信息
        /// </summary>
        /// <param name="PjtID">项目ID</param>
        /// <param name="amount_sold_Old">已售金额(变化前)</param>
        /// <param name="amount_able_Old">可售金额(变化前)</param>
        /// <param name="amount_able_fz_Old">冻结金额(变化前)</param>
        /// <param name="amount_sold_New">已售金额(变化后)</param>
        /// <param name="amount_able_New">可售金额(变化后)</param>
        /// <param name="amount_able_fz_New">冻结金额(变化后)</param>
        /// <param name="creator_id">创建人ID</param>
        /// <param name="type">插入表示（0：项目修改，1：项目审核）</param>
        /// <returns></returns>
        public ResultInfo<bool> Add_Pjt_Log(int PjtID, decimal amount_sold_Old, decimal amount_able_Old, decimal amount_able_fz_Old, decimal amount_sold_New, decimal amount_able_New, decimal amount_able_fz_New, int creator_id, int type)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.Add_Pjt_Log(PjtID, amount_sold_Old, amount_able_Old, amount_able_fz_Old, amount_sold_New, amount_able_New, amount_able_fz_New, creator_id, type);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        #endregion

        #region 添加后台操作日志
        /// <summary>
        /// 添加后台操作日志
        /// </summary>
        /// <param name="model">操作</param>
        /// <returns></returns>
        public ResultInfo<bool> AddSysLog(SysActionLogModel model)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = Dao.MenuSet.SysDao.AddSysLog(model);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

        #endregion

        #region   根据项目ID获得项目的权值和审核状态
        /// <summary>
        ///  根据项目ID获得项目的权值和审核状态
        /// </summary>
        /// <param name="PID">项目ID</param>
        /// <returns></returns>
        public ResultInfo<ProjectModel> GetProjectModelWeightOrvaidate(int PID)
        {
            ResultInfo<ProjectModel> result = new ResultInfo<ProjectModel>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = dao.GetProjectModelWeightOrvaidate(PID);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        #endregion
        
    }
}
