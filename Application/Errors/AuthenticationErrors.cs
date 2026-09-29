using Application.Common.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Errors
{
    public static class AuthenticationErrors
    {
        public static readonly InvalidCredentials InvalidCredentials = new("InvalidCredentials", "Invalid credentials provided.");
        public static readonly NotFoundError UserNotFound = new("User.NotFound", "User not found.");
        public static readonly UserAlreadyExists UserAlreadyExists = new("User.AlreadyExists", "User already exists.");
        //public static readonly string PasswordTooWeak = "Password is too weak.";
        //public static readonly string TokenExpired = "Token has expired.";
        public static readonly InvalidTokenError TokenInvalid = new("Token.Invalid", "Token is invalid.");
        public static readonly ForbiddenError Forbidden = new("Forbidden", "Can not access.");
        public static readonly UnauthorizedError Unauthorized = new("Unauthorized", "Have to login");


    }
}
