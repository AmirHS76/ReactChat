﻿namespace ReactChat.Core.Entities.Login
{
    public class BaseUser
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}