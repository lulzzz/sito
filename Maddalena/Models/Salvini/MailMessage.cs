﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Mongolino.Attributes;

namespace Maddalena.Models.Salvini
{
    public class MailMessage : Mongolino.ICollectionItem
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Subject { get; set; }

        [FullTextIndex]
        public string Body { get; set; }

        public string From { get; set; }

        public string[] To { get; set; }
        public string FilePath { get; internal set; }
    }
}
