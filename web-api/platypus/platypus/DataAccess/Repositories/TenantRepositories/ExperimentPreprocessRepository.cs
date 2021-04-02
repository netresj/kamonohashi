﻿using Microsoft.AspNetCore.Http;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Models.TenantModels;

namespace Nssol.Platypus.DataAccess.Repositories.TenantRepositories
{
    /// <summary>
    /// 実験前処理テーブルにアクセスするためのリポジトリクラス
    /// </summary>
    public class ExperimentPreprocessRepository : RepositoryForTenantBase<ExperimentPreprocess>, IExperimentPreprocessRepository
    {
        public ExperimentPreprocessRepository(CommonDbContext context, IHttpContextAccessor accessor) 
            : base(context, accessor) {}
    }
}
