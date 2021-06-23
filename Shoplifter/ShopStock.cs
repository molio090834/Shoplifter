using System;
using StardewValley.Objects;
using System.Collections;
using System.Collections.Generic;
using StardewValley.Locations;
using StardewValley;
using StardewModdingAPI;

namespace Shoplifter
{	
	public class ShopStock
	{
		private static IMonitor monitor;
		private static IModHelper helper;

		public static ArrayList CurrentStock = new ArrayList();

		public static void gethelpers(IMonitor monitor, IModHelper helper)
		{
			ShopStock.monitor = monitor;
			ShopStock.helper = helper;
		}

		// Method from Utilities so stock can be added to a shop (why is it private?)
		private static bool addToStock(Dictionary<ISalable, int[]> stock, HashSet<int> stockIndices, StardewValley.Object objectToAdd, int[] listing)
		{
			int index = objectToAdd.ParentSheetIndex;

			if (!stockIndices.Contains(index))
			{
				stock.Add(objectToAdd, listing);
				stockIndices.Add(index);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Generates a random list of stock for the given shop
		/// </summary>
		/// <param name="maxstock">The maximum number of different stock items to generate</param>
		/// <param name="maxquantity">The maximum quantity of each stock</param>
		/// <param name="which">The shop to generate stock for</param>
		/// <returns>The generated stock list</returns>
		public static Dictionary<ISalable, int[]> generateRandomStock(int maxstock, int maxquantity, string which, CustomShopModel customshop = null)
		{
			GameLocation location = Game1.currentLocation;
			Dictionary<ISalable, int[]> stock = new Dictionary<ISalable, int[]>();
			HashSet<int> stockIndices = new HashSet<int>();
			Random random = new Random((int)Game1.uniqueIDForThisGame / 2 + (int)Game1.stats.DaysPlayed + ModEntry.PerScreenShopliftCounter.Value);
			int stocklimit = random.Next(1, maxstock + 1);
			int index;

			void GetAvailableItems(Dictionary<ISalable, int[]> availablestock)
			{
				foreach (var shopstock in availablestock)
				{
					// Stops illegal stock being added, will result in an error item
					if ((shopstock.Key as StardewValley.Object) == null || (shopstock.Key as Wallpaper) != null || (shopstock.Key as Furniture) != null || (shopstock.Key as StardewValley.Object).bigCraftable == true || (shopstock.Key as StardewValley.Object).IsRecipe == true || (shopstock.Key as Clothing) != null || (shopstock.Key as Ring) != null || (shopstock.Key as Boots) != null || (shopstock.Key as Hat) != null)
					{
						continue;
					}

					// Add object id to array
					if ((shopstock.Key as StardewValley.Object) != null && (shopstock.Key as StardewValley.Object).bigCraftable == false)
					{
						index = (shopstock.Key as StardewValley.Object).parentSheetIndex;

						CurrentStock.Add(index);
					}
				}
			}

			switch (which)
			{
				// Pierre's shop
				case "SeedShop":
					location = Game1.getLocationFromName("SeedShop");
					GetAvailableItems((location as SeedShop).shopStock());
					break;

				// Willy's shop
				case "FishShop":
					GetAvailableItems(Utility.getFishShopStock(Game1.player));
					break;

				// Robin's shop
				case "Carpenters":
					GetAvailableItems(Utility.getCarpenterStock());
					break;

				// Marnie's shop
				case "AnimalShop":
					GetAvailableItems(Utility.getAnimalShopStock());
					break;

				// Clint's shop
				case "Blacksmith":
					GetAvailableItems(Utility.getBlacksmithStock());
					break;

				// Gus' shop
				case "Saloon":
					GetAvailableItems(Utility.getSaloonStock());
					break;

				// Harvey's shop
				case "HospitalShop":
					GetAvailableItems(Utility.getHospitalStock());
					break;
				case "JojaShop":
					GetAvailableItems(Utility.getJojaStock());
					break;
				case "AdventureShop":
					GetAvailableItems(Utility.getAdventureShopStock());
					break;

				// Sandy's shop
				case "SandyShop":

					var SandyStock = helper.Reflection.GetMethod(new GameLocation(), "sandyShopStock").Invoke<Dictionary<ISalable, int[]>>();
					GetAvailableItems(SandyStock);
					break;

				case "KrobusShop":
					location = Game1.getLocationFromName("Sewer");
					GetAvailableItems((location as Sewer).getShadowShopStock());
					break;
				case "DwarfShop":
					GetAvailableItems(Utility.getDwarfShopStock());
					break;
				case "HatShop":
					GetAvailableItems(Utility.getHatStock());
					break;
				default:
					if (ModEntry.api != null)
					{
						customshop.ItemsForSale = ModEntry.api.GetItemPriceAndStock(customshop.ShopName);

					}

					if (customshop.ItemsForSale != null)
					{
						GetAvailableItems(customshop.ItemsForSale);
					}
					else
					{
						CurrentStock.Add(494);
					}

					break;
			}


			// Add generated stock to store from array
			for (int i = 0; i < stocklimit; i++)
			{
				int quantity = random.Next(1, maxquantity + 1);
				int item = random.Next(0, CurrentStock.Count);

				ShopStock.addToStock(stock, stockIndices, new StardewValley.Object((int)CurrentStock[item], quantity), new int[2]
				{
					0,
					quantity
				});
			}

			// Clear stock array
			CurrentStock.Clear();

			return stock;
		}
	}
}