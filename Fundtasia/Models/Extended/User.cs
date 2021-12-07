﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Fundtasia.Models
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        public string ConfirmPassword { get; set; }


        public User(Guid Id, string Email, string Role, string FirstName, string LastName)
        {
            this.Id = Id;
            this.Email = Email;
            this.Role = Role;
            this.FirstName = FirstName;
            this.LastName = LastName;
        }
    }

    public class UserMetadata
    {
        public System.Guid Id { get; set; }

        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name required")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name required")]
        public string LastName { get; set; }

        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required")]
        public string PasswordHash { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }

    public class CreateUserVM
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required")]
        public string PasswordHash { get; set; }

        public Guid ActivationCode { get; set; }

        public bool IsEmailVerified { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "User role is required")]
        public string Role { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "User status is required")]
        public string Status { get; set; }
    }

    public class UserEditVM
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "User role is required")]
        public string Role { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "User status is required")]
        public string Status { get; set; }
    }

    public class ClientUserEditVM
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "User status is required")]
        public string Status { get; set; }
    }
}