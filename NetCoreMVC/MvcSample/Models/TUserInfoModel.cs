using System;
using System.Runtime.Serialization;

namespace MvcSample.Models
{
    [DataContract(Name = "t_user_info")]
    public class TUserInfoModel
    {
        [DataMember(Name = "user_id")]
        public String userId { get; set; }

        [DataMember(Name = "address_id")]
        public int addressId { get; set; }

        [DataMember(Name = "birthday")]
        public String birthday { get; set; }

        [DataMember(Name = "delete_flag")]
        public bool deleteFlag { get; set; }
    }
}