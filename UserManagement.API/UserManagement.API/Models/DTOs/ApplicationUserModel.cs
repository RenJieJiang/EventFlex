﻿using System.Text.Json.Serialization;

namespace UserManagement.API.Models.DTOs
{
    public class ApplicationUserModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        [JsonIgnore]
        public string Password { get; set; } = string.Empty;
    }
}
