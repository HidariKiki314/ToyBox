using System;
using System.Runtime.Serialization;

namespace MvcSample.Models
{
    [DataContract(Name = "m_address")]
    public class MAddressModel
    {
        [DataMember(Name = "address_id")]
        public int addressId { get; set; }

        [DataMember(Name = "address_name")]
        public String addressName { get; set; }
    }
}