using GodwitWHMS.Infrastructures.Extensions;
using GodwitWHMS.Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace GodwitWHMS.Infrastructures.Menus
{
    public class MenuService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        public MenuService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public class MenuItem
        {
            public string URL { get; set; }
            public string Name { get; set; }
            public bool IsModule { get; set; }

            public MenuItem(string url, string name, bool isModule)
            {
                URL = url;
                Name = name;
                IsModule = isModule;
            }
        }

        public class TreeNode
        {
            public string id { get; set; }
            public string name { get; set; }
            public string? pid { get; set; }
            public string? navURL { get; set; }
            public bool hasChild { get; set; }
            public bool expanded { get; set; }
            public bool isSelected { get; set; }

            public TreeNode(string param_id, string param_name, string? param_pid = null, string? param_navURL = null, bool param_hasChild = false, bool param_expanded = false, bool param_selected = false)
            {
                id = param_id;
                name = param_name;
                pid = param_pid;
                navURL = param_navURL;
                hasChild = param_hasChild;
                expanded = param_expanded;
                isSelected = param_selected;
            }
        }

        public List<MenuItem> GetMenuItems()
        {
            List<MenuItem> menus = new List<MenuItem> {
                new MenuItem("#", "Dashboards", true),
                new MenuItem("/Dashboards/DefaultDashboard", "Default", false),
                new MenuItem("#", "Sales", true),
                new MenuItem("/CustomerGroups/CustomerGroupList", "Customer Group", false),
                new MenuItem("/CustomerCategories/CustomerCategoryList", "Customer Category", false),
                new MenuItem("/Customers/CustomerList", "Customer", false),
                new MenuItem("/CustomerContacts/CustomerContactList", "Customer Contact", false),
                new MenuItem("/SalesOrders/SalesOrderList", "Sales Order", false),
                new MenuItem("/SalesOrderItems/SalesOrderItemList", "Sales Report", false),
                new MenuItem("#", "Purchase", true),
                new MenuItem("/VendorGroups/VendorGroupList", "Vendor Group", false),
                new MenuItem("/VendorCategories/VendorCategoryList", "Vendor Category", false),
                new MenuItem("/Vendors/VendorList", "Vendor", false),
                new MenuItem("/VendorContacts/VendorContactList", "Vendor Contact", false),
                new MenuItem("/PurchaseOrders/PurchaseOrderList", "Purchase Order", false),
                new MenuItem("/PurchaseOrderItems/PurchaseOrderItemList", "Purchase Report", false),
                new MenuItem("#", "Inventory", true),
                new MenuItem("/UnitMeasures/UnitMeasureList", "Unit Measure", false),
                new MenuItem("/ProductGroups/ProductGroupList", "Product Group", false),
                new MenuItem("/Products/ProductList", "Product", false),
                new MenuItem("/Warehouses/WarehouseList", "Warehouse", false),
                new MenuItem("/DeliveryOrders/DeliveryOrderList", "Delivery Order", false),
                new MenuItem("/SalesReturns/SalesReturnList", "Sales Return", false),
                new MenuItem("/GoodsReceives/GoodsReceiveList", "Goods Receipt", false),
                new MenuItem("/PurchaseReturns/PurchaseReturnList", "Purchase Return", false),
                new MenuItem("/TransferOuts/TransferOutList", "Transfer Out", false),
                new MenuItem("/TransferIns/TransferInList", "Transfer In", false),
                new MenuItem("/PositiveAdjustments/PositiveAdjustmentList", "Positive Adjustment", false),
                new MenuItem("/NegativeAdjustments/NegativeAdjustmentList", "Negative Adjustment", false),
                new MenuItem("/Scrappings/ScrappingList", "Scrapping", false),
                new MenuItem("/StockCounts/StockCountList", "Stock Count", false),
                new MenuItem("/InventoryTransactions/InventoryTransactionList", "Transaction Report", false),
                new MenuItem("/InventoryStocks/InventoryStockList", "Stock Report", false),
                new MenuItem("/InventoryMovements/InventoryMovementList", "Movement Report", false),
                new MenuItem("#", "Settings", true),
                new MenuItem("/Companies/CompanyList", "Company", false),
                new MenuItem("/Taxes/TaxList", "Tax", false),
                new MenuItem("/UserProfiles/UserList", "User Profile", false),
                new MenuItem("/Users/UserList", "User List", false),
                new MenuItem("/Countries/CountryList", "Country List", false),
                new MenuItem("/Carriers/CarrierList", "Carrier List", false),
                new MenuItem("/BasePrices/BasePriceList", "Base Price List", false),
                new MenuItem("/FuelSurcharges/FuelSurchargeList", "Fuel Surcharge List", false),
                new MenuItem("/Commissions/CommissionList", "Commission List", false),
                new MenuItem("/BasePrices/TotalPriceList", "Export Sale Price List", false),
                new MenuItem("/NumberSequences/NumberSequenceList", "Sequence", false),
                new MenuItem("#", "Log", true),
                new MenuItem("/LogSessions/LogSessionList", "Session Log", false),
                new MenuItem("/LogErrors/LogErrorList", "Error Log", false),
                new MenuItem("/LogAnalytics/LogAnalyticList", "Analytic Log", false),

                
                // Add the new menu item for Generate SKU
                new MenuItem("#", "Pre Recieving", true),
                new MenuItem("/PackageSkus/PackageSkuList", "Package Skus", false)
            };

            return menus;

        }

        public List<TreeNode> GetTreeNodesComplete()
        {
            var menus = GetMenuItems();

            List<TreeNode> nodes = new List<TreeNode>();

            var index = 1;
            var pid = 0;
            foreach (var item in menus)
            {
                if (item.IsModule == true)
                {
                    nodes.Add(new TreeNode(index.ToString(), item.Name, param_hasChild: true, param_expanded: false));
                    pid = index;
                }
                else
                {
                    nodes.Add(new TreeNode(index.ToString(), item.Name, pid.ToString(), item.URL));
                }

                index++;
            }

            return nodes;
        }

        public TreeNode? GetParentNode(string folderName)
        {
            var treeNodes = GetTreeNodesComplete();
            var child = treeNodes.FirstOrDefault(x => x?.navURL?.ToPageFolderNameFromPath() == folderName);
            var parent = treeNodes.FirstOrDefault(x => x.id == child?.pid);
            return parent;
        }



        public async Task<List<TreeNode>> GetTreeNodesAsync(string currentPath, ApplicationUser applicationUser)
        {
            var roles = await _userManager.GetRolesAsync(applicationUser);
            var menus = GetMenuItems();

            List<TreeNode> nodes = new List<TreeNode>();

            var index = 1;
            var pid = 0;
            foreach (var item in menus)
            {
                if (item.IsModule == true)
                {
                    nodes.Add(new TreeNode(index.ToString(), item.Name, param_hasChild: true, param_expanded: false));
                    pid = index;
                }
                else
                {
                    if (roles.Contains(item.URL.ToPageFolderNameFromPath()))
                    {

                        if (item.URL.ToPageFolderNameFromPath() == currentPath.ToPageFolderNameFromPath())
                        {
                            nodes.Add(new TreeNode(index.ToString(), item.Name, pid.ToString(), item.URL, param_selected: true));
                            foreach (var node in nodes)
                            {
                                if (node.id == pid.ToString())
                                {
                                    node.expanded = true;
                                }
                            }
                        }
                        else
                        {
                            nodes.Add(new TreeNode(index.ToString(), item.Name, pid.ToString(), item.URL));

                        }

                    }
                }

                index++;
            }

            var nodesToRemove = nodes.Where(node => node.hasChild && !nodes.Any(x => x.pid == node.id)).ToList();

            foreach (var nodeToRemove in nodesToRemove)
            {
                nodes.Remove(nodeToRemove);
            }



            return nodes;
        }


        public async Task<string> GetJsonTreeNodeAsync(string currentPath, ApplicationUser? applicationUser)
        {
            if (applicationUser == null)
            {
                return "{}";
            }

            return JsonConvert.SerializeObject(await GetTreeNodesAsync(currentPath, applicationUser));
        }

        public List<string> GetFoldersFromMenus()
        {
            var result = new List<string>();
            var menus = GetMenuItems();
            foreach (var item in menus)
            {
                if (item.URL != "#")
                {
                    result.Add(item.URL.ToPageFolderNameFromPath());
                }
            }
            return result;
        }

        public List<string> GetAdminRoles()
        {
            var result = new List<string>();
            result = GetFoldersFromMenus();
            return result;
        }

        public List<string> GetNonAdminRoles()
        {
            var result = new List<string>();
            var adminOnly = new List<string>()
            {
                "Companies",
                "Taxes",
                "Users",
                "NumberSequences",
                "LogSessions",
                "LogErrors",
                "LogAnalytics"
            };
            result = GetFoldersFromMenus().Where(x => !adminOnly.Contains(x)).ToList();
            return result;
        }

        public List<string> GetThirdPartyRoles()
        {
            var result = new List<string>()
            {
                "UserProfiles"
            };
            return result;
        }
    }
}
