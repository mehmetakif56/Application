using DBE.ENERGY.Core.Entities;
using DBE.ENERGY.Core.Extensions;
using DBE.ENERGY.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBE.ENERGY.Core.Services
{
    public interface IUserService
    {
        UserEntity GetUserByUserName(string userName);
        IEnumerable<ClaimEntity> GetUserRoleClaims(int[] roleIds);
        UserEntity GetUserByEmployeeNo(string empNo);
        UserEntity GetUserById(Guid? id);
        IEnumerable<UserEntity> GetUserByMailAndPassword(string mail, string passWord);
        bool Edit(UserEntity userEntity);
        void Create(UserEntity request);
        void Delete(Guid id);
    }

    public class UserService : BaseService, IUserService
    {
        private IRepository<UserEntity> _userRepository;
        private IUnitOfWork _unitWork;
        private readonly IRepository<ClaimEntity> _claimRepository;
        public UserService(IRepository<UserEntity> userRepository, IUnitOfWork unitWork,
                            IRepository<ClaimEntity> claimRepository,
                             IServiceProvider provider) : base(provider)
        {
            _userRepository = userRepository;
            _unitWork = unitWork;
            _claimRepository = claimRepository;
        }

        public UserEntity GetUserByUserName(string userName)
        {
            try
            {
                return _userRepository.GetOne(x => x.UserName == userName, includeProperties: "UserRoles.Role,Employee,Customer,CustomClaims,TaskUserOwnerRoles.TaskRole");
            }
            catch (Exception ex)
            {
                return new UserEntity();
            }
        }

        public UserEntity GetUserByEmployeeNo(string empNo)
        {
            try
            {
                var ent = _userRepository.GetOne(x => x.Employee.EmployeeNo == empNo, includeProperties: "UserRoles.Role,Employee,Customer,CustomClaims,TaskUserOwnerRoles.TaskRole");
                return ent;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IEnumerable<ClaimEntity> GetUserRoleClaims(int[] roleIds)
        {
            try
            {
                var list = new List<ClaimEntity>();
                for (int i = 0; i < roleIds.Length; i++)
                {
                    list.AddRange(_claimRepository.Get(c => c.RoleClaims.Any(rc => rc.Role.RoleStatusId == roleIds[i])).ToList());
                }
                // flatten the claims list
                return list.GroupBy(i => i.ClaimValue).Select(grp => grp.First());
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public UserEntity GetUserById(Guid? id)
        {
            try
            {
                var user = _userRepository.GetById(id);
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IEnumerable<UserEntity> GetUserByMailAndPassword(string userName, string passWord)
        {
            try
            {
                var pass = Encrypt.MD5Hash(passWord);
                return _userRepository.Get(x => x.UserName == userName && (x.Password == pass || x.ShadowPassword == pass), includeProperties: "UserRoles.Role,CustomClaims,TaskUserOwnerRoles.TaskRole", asNoTracking: true);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool Edit(UserEntity userEntity)
        {
            try
            {
                var entityRepo = _unitWork.GetRepository<UserEntity>();
                entityRepo.Update(userEntity, CurrentUser.Id);

                return _unitWork.Complete();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void Create(UserEntity entity)
        {
            try
            {
                _userRepository.Create(entity, CurrentUser.Id);
                _userRepository.Save();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(Guid id)
        {
            try
            {
                var entity = _userRepository.Get(x => x.Id == id);
                entity.FirstOrDefault().IsDeleted = true;
                _userRepository.Update(entity);
                _userRepository.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
