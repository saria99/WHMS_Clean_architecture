﻿using GodwitWHMS.Infrastructures.Data;
using GodwitWHMS.Infrastructures.Repositories;
using GodwitWHMS.Domain.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace GodwitWHMS.Applications.CustomerContacts
{
    public class CustomerContactService : Repository<CustomerContact>
    {
        public CustomerContactService(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IAuditColumnTransformer auditColumnTransformer) :
                base(
                    context,
                    httpContextAccessor,
                    auditColumnTransformer)
        {
        }



    }
}
