using FacilityManagement.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.API.Repositories
{
    public interface IInventoryObjectRepository
    {
        // Inventory Object
        Task<InventoryObject[]> GetAllInventoryObjectsAsync();
        Task<InventoryObject> GetInventoryObjectByIdAsync(int id,
            bool includeTypes = false, bool includeSystems = false, bool includeParts = false);
        Task<bool> UpdateInventoryObjectAsync(InventoryObject newModel);
        void DeleteInventoryObject(InventoryObject toDelete);

        // Inventory Object System
        void AddInventoryObjectSystem(InventoryObjectSystem toAddModel);
        Task<InventoryObjectSystem[]> GetInventoryObjectSystemsByTypeIdAsync(int id);
        Task<InventoryObjectSystem> GetInventoryObjectSystemsByIdAsync(int id);
        Task<bool> UpdateInventoryObjectSystemAsync(InventoryObjectSystem newModel);
        void DeleteInventoryObjectSystem(InventoryObjectSystem toDelete);

        // Inventory Object Type
        void AddInventoryObjectType(InventoryObjectType toAddModel);
        Task<InventoryObjectType[]> GetInventoryObjectTypesByInventoryObjectIdAsync(int id);
        Task<InventoryObjectType> GetInventoryObjectTypeByIdAsync(int id);
        Task<bool> UpdateInventoryObjectTypeAsync(InventoryObjectType newModel);
        void DeleteInventoryObjectType(InventoryObjectType toDelete);
        
        // Inventory Object Part
        void AddInventoryObjectPart(InventoryObjectPart toAddModel);
        Task<InventoryObjectPart> GetInventoryObjectPartByIdAsync(int id);
        Task<bool> UpdateInventoryObjectPartAsync(InventoryObjectPart newModel);
        void DeleteInventoryObjectPart(InventoryObjectPart toDelete);
    }
}
