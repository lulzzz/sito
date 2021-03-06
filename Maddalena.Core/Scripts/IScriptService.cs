﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Maddalena.Core.Scripts.Model;

namespace Maddalena.Core.Scripts
{
    public interface IScriptService
    {
        Task Create(Script script);
        Task<List<Script>> All();
        Task<List<Script>> Where(Expression<Func<Script, bool>> where);
        Task Update(Script script);
        Task Delete(string id);
        Task<ScriptContext> Run(Script script);
        Task<Script> ById(string id);
    }
}