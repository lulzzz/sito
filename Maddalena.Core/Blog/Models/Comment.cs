﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Maddalena.Core.Mongo;

namespace Maddalena.Core.Blog.Models
{
    public class Comment : MongoObject
    {
        [Required]
        public string Author { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime PubDate { get; set; } = DateTime.UtcNow;

        public bool IsAdmin { get; set; }

        public string GetGravatar()
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(Email.Trim().ToLowerInvariant());
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                var sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return $"https://www.gravatar.com/avatar/{sb.ToString().ToLowerInvariant()}?s=60&d=blank";
            }
        }

        public string RenderContent()
        {
            return Content;
        }
    }
}
