using Maddalena.Core.Mongo;

namespace Maddalena.Core.Settings
{
    class SettingSlot : MongoObject
    {
        public string Type { get; set; }

        public string Document { get; set; }
    }
}
