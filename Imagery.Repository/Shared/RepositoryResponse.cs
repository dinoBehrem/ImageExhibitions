﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Repository.Shared
{
    public class RepositoryResponse<TEntity>
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string InnerMessage { get; set; }
        public bool IsSuccess { get; set; }
        public TEntity Content { get; set; }
    }
}
