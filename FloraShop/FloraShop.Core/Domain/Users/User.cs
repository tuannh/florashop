
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FloraShop.Core.Domain
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public bool Active { get; set; }

        public bool IsAdmin { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime? LastLogin { get; set; }

        public Guid ResetCode { get; set; }

        public DateTime? ResetExpiredCode { get; set; }

        public string Cellphone { get; set; }

        public string Telphone { get; set; }

        public string Address { get; set; }

        public int? DistrictId { get; set; }

        public int? ProvinceId { get; set; }

        public District District { get; set; }

        public Province Province { get; set; }
    }
}
