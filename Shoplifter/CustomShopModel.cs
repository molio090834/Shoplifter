using System;
using System.Collections.Generic;
using StardewValley;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoplifter
{
    public class CustomShopModel
    {
        public string ShopName { get; set; }
        public int OpenTime { get; set; }
        public int CloseTime { get; set; }
        public string PrimaryShopKeeper { get; set; }
        public string[] AdditionalShopKeepers { get; set; } = null;
        public string[] CaughtDialogue { get; set; } = null;
        public Dictionary<ISalable, int[]> ItemsForSale { get; set; } = null;
        public int MaxNumberofItems { get; set; } = 3;
        public int MaxQuantityofItems { get; set; } = 3;
    }
}
