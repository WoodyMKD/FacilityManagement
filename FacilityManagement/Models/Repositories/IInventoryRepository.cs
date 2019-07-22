using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Models.Repositories
{
    public interface IInventoryRepository
    {
        IEnumerable<InventoryObject> GetAllInventoryObjects();
        IEnumerable<InventoryObject> GetInventoryObjectsByType(string inventoryObjectType);
        InventoryObject GetInventoryObjectById(int inventoryObjectId);
    }
}
