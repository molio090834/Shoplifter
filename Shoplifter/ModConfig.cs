using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoplifter
{
    public class ModConfig
    {
        public uint MaxShopliftsPerDay { get; set; } = 7;
        public uint MaxShopliftsPerStore { get; set; } = 7;
        public uint MaxFine { get; set; } = 1000;
        public uint FriendshipPenalty { get; set; } = 0;
        public uint DaysBannedFor { get; set; } = 3;
        public uint CatchesBeforeBan { get; set; } = 3;
        public uint CaughtRadius { get; set; } = 7;
    }
}
