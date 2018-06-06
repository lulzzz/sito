﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Mongolino;
using Mongolino.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maddalena.Porn
{
    public class PornHubItem : ICollectionItem
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string IFrame { get; set; }

        public string Preview { get; set; }

        public string[] Rolls { get; set; }

        [AscendingIndex]
        [FullTextIndex]
        public string Title { get; set; }

        [AscendingIndex]
        public string[] Tags { get; set; }

        [AscendingIndex]
        public string[] Categories { get; set; }
    }
}