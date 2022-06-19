using System;
using System.Runtime.Serialization;

namespace MvcSample.Models
{
    [DataContract(Name = "m_user")]
    public class MUserModel
    {
        [DataMember(Name = "id")]
        public String id { get; set; }

        [DataMember(Name = "password")]
        public String password { get; set; }
    }
}