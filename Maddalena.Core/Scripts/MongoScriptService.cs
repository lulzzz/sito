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
        private readonly JavascriptRunner _js;
        private readonly IMongoCollection<Script> _scripts;

        public MongoScriptService(string connectionString, IServiceProvider services)
        {
            _scripts = MongoUtil.FromConnectionString<Script>(connectionString, "scripts");
            _js = new JavascriptRunner(services);
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
            switch (script.Language)
            {
                case ScriptLanguage.Javascript: return _js.Run(script);
                case ScriptLanguage.R: return Task.FromResult(new ScriptContext());
                    
            }
            throw new ArgumentOutOfRangeException();
        }
    }
}
