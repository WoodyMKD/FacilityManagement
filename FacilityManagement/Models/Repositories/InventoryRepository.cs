using FacilityManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Models.Repositories
{
    public class InventoryRepository: IInventoryRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public InventoryRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<InventoryObject> GetAllInventoryObjects()
        {
            return _appDbContext.InventoryObjects;
        }
        

        public IEnumerable<InventoryObject> GetInventoryObjectsByType(string inventoryObjectType)
        {
            throw new NotImplementedException();
        }

        public InventoryObject GetInventoryObjectById(int inventoryObjectId)
        {
            return _appDbContext.InventoryObjects.FirstOrDefault(p => p.Id == inventoryObjectId);
        }
    }
}
