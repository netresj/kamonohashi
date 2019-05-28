﻿using Nssol.Platypus.ApiModels.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.RoleApiModels
{
    public class CreateForTenantInputModel
    {
        /// <summary>
        /// 名前
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 表示名
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// 並び順。小さいほど前に表示される。一意性は不要。
        /// </summary>
        [Required]
        public int? SortOrder { get; set; }
    }
}