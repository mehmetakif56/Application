using DBE.ENERGY.Core.Entities;
using DBE.ENERGY.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBE.ENERGY.Infrastructure.Data
{
    public class UserContext:IUserContext
    {
        public UserEntity UserEntity { get ; set ; }
    }
}
