﻿using BookApi_MySQL.Models;
using BookApi_MySQL.Models.DTO;

namespace BookApi_MySQL.Services
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(User user);
        Task<string> GenerateRefreshToken(int userId, string token);
    }
}