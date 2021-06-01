/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.Linq;
using System.Threading.Tasks;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;

namespace CoreCms.Net.Services
{
    /// <summary>
    ///     用户表 接口实现
    /// </summary>
    public class SysUserServices : BaseServices<SysUser>, ISysUserServices
    {
        private readonly ISysUserRepository _dal;
        private readonly ISysRoleRepository _sysRoleRepository;
        private readonly ISysUserRoleRepository _sysUserRoleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SysUserServices(IUnitOfWork unitOfWork, ISysUserRepository dal, ISysRoleRepository sysRoleRepository,
            ISysUserRoleRepository sysUserRoleRepository)
        {
            _dal = dal;
            BaseDal = dal;
            _unitOfWork = unitOfWork;
            _sysRoleRepository = sysRoleRepository;
            _sysUserRoleRepository = sysUserRoleRepository;
        }


        /// <summary>
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPwd"></param>
        /// <returns></returns>
        public async Task<string> GetUserRoleNameStr(string loginName, string loginPwd)
        {
            var roleName = "";
            var user = await QueryByClauseAsync(a => a.userName == loginName && a.passWord == loginPwd);
            var roleList = await _sysRoleRepository.QueryListByClauseAsync(a => a.deleted == false);
            if (user != null)
            {
                var userRoles = await _sysUserRoleRepository.QueryListByClauseAsync(ur => ur.userId == user.id);
                if (userRoles.Count > 0)
                {
                    var arr = userRoles.Select(ur => ur.roleId).ToList();
                    var roles = roleList.Where(d => arr.Contains(d.id));

                    roleName = string.Join(',', roles.Select(r => r.roleCode).ToArray());
                }
            }

            return roleName;
        }
    }
}