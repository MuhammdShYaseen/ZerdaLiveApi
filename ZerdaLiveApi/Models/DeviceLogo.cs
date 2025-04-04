using System;
using System.Collections.Generic;

#nullable disable

namespace ZerdaLiveApi.Models
{
    public partial class DeviceLogo
    {
        public int DeviceLogoId { get; set; }
        public DateTime? DeviceLogoDate { get; set; }
        public int? DeviceTokenId { get; set; }

        public virtual DeviceToken DeviceToken { get; set; }
    }
}
