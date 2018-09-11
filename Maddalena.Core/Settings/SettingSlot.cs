using System;
using System.Collections.Generic;
using System.Text;
using Maddalena.Core.Mongo;
using MongoDB.Bson;

namespace Maddalena.Core.Settings
{
    class SettingSlot : MongoObject
    {
        public string Type { get; set; }

        public BsonDocument Document { get; set; }
    }
}
