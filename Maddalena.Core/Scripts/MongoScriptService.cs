using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Maddalena.Core.GridFs;
using Maddalena.Core.Mongo;
using Maddalena.Core.Scripts.Js;
using Maddalena.Core.Scripts.Model;
using MongoDB.Driver;

namespace Maddalena.Core.Scripts
{
    public class MongoScriptService : IScriptService
    {
        private readonly IMongoCollection<Script> _scripts;

        public MongoScriptService(string connectionString)
        {
            _scripts = MongoUtil.FromConnectionString<Script>(connectionString, "scripts");
        }

        public Task Create(Script script) => _scripts.InsertOneAsync(script);

        public async Task<Script> ById(string id)
        {
            var found = await _scripts.FindAsync(x => x.Id == id, new FindOptions<Script> {Limit = 1});
            return await found.FirstOrDefaultAsync();
        }

        public async Task<List<Script>> All() => await (await _scripts.FindAsync(x=>true)).ToListAsync();

        public async Task<List<Script>> Where(Expression<Func<Script, bool>> where) => await (await _scripts.FindAsync(where)).ToListAsync();

        public Task Update(Script script) => _scripts.ReplaceOneAsync(x => x.Id == script.Id, script);

        public Task Delete(string id) => _scripts.DeleteOneAsync(x => x.Id == id);

        public Task<ScriptContext> Run(Script script)
        {
            var context = new ScriptContext
            {
                SystemInterface = new SystemInterface(),
                Script = script
            };

            try
            {
                switch (script.Language)
                {
                    case ScriptLanguage.Javascript:
                        JavascriptRunner.Run(context);
                        break;
                    case ScriptLanguage.R:
                        break;
                }
            }
            catch (Exception e)
            {
                context.Exception = e;
            }

            return Task.FromResult(context);
        }
    }
}
