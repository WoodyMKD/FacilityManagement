using FacilityManagement.API.Data;
using FacilityManagement.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.API.Repositories
{
    public class InventoryObjectRepository: IInventoryObjectRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public InventoryObjectRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<InventoryObject[]> GetAllInventoryObjectsAsync()
        {
            IQueryable<InventoryObject> query = _appDbContext.InventoryObjects;
            return await query.ToArrayAsync();
        }

        public async Task<InventoryObject> GetInventoryObjectByIdAsync(int id,
            bool includeTypes = false, bool includeSystems = false, bool includeParts = false)
        {
            IQueryable<InventoryObject> query = _appDbContext.InventoryObjects;

            if (includeTypes)
            {
                if(includeSystems)
                {
                    if(includeParts)
                    {
                        query = query
                            .Include(c => c.Types)
                                .ThenInclude(cst => cst.Systems)
                                    .ThenInclude(cs => cs.Parts);
                    }
                    else
                    {
                        query = query
                            .Include(c => c.Types)
                                .ThenInclude(cst => cst.Systems);
                    }
                }
                else
                {
                    query = query
                        .Include(c => c.Types);
                }
            }

            query = query.Where(c => c.InventoryObjectId == id);
            
            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public async Task<InventoryObjectSystem[]> GetInventoryObjectSystemsByTypeIdAsync(int id)
        {
            IQueryable<InventoryObjectSystem> query = _appDbContext.InventoryObjectSystems
                .Include(cs => cs.Parts)
                .Where(c => c.InventoryObjectTypeId == id);

            var result = await query.ToArrayAsync();
            return result;
        }

        public async Task<InventoryObjectSystem> GetInventoryObjectSystemsByIdAsync(int id)
        {
            IQueryable<InventoryObjectSystem> query = _appDbContext.InventoryObjectSystems
                .Include(cs => cs.Parts)
                .Where(cs => cs.InventoryObjectSystemId == id);

            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public async Task<bool> UpdateInventoryObjectAsync(InventoryObject newModel)
        {
            _appDbContext.Update(newModel);
            await _appDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<InventoryObjectPart> GetInventoryObjectPartByIdAsync(int id)
        {
            IQueryable<InventoryObjectPart> query = _appDbContext.InventoryObjectParts
                .Where(p => p.InventoryObjectPartId == id);

            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public async Task<bool> UpdateInventoryObjectPartAsync(InventoryObjectPart newModel)
        {
            _appDbContext.Update(newModel);
            await _appDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateInventoryObjectSystemAsync(InventoryObjectSystem newModel)
        {
            _appDbContext.InventoryObjectSystems.Update(newModel);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public void DeleteInventoryObject(InventoryObject toDelete)
        {
            _appDbContext.Remove(toDelete);
            _appDbContext.SaveChanges();
        }

        public void DeleteInventoryObjectPart(InventoryObjectPart toDelete)
        {
            _appDbContext.InventoryObjectParts.Remove(toDelete);
            _appDbContext.SaveChanges();
        }

        public void AddInventoryObjectSystem(InventoryObjectSystem toAdd)
        {
            _appDbContext.InventoryObjectSystems.Add(toAdd);
            try { 
                _appDbContext.SaveChanges();
            } catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void AddInventoryObjectType(InventoryObjectType toAdd)
        {
            _appDbContext.InventoryObjectTypes.Add(toAdd);
            _appDbContext.SaveChanges();
        }

        public void AddInventoryObjectPart(InventoryObjectPart toAdd)
        {
            _appDbContext.InventoryObjectParts.Add(toAdd);
            _appDbContext.SaveChanges();
        }

        public void DeleteInventoryObjectSystem(InventoryObjectSystem toDelete)
        {
            _appDbContext.InventoryObjectSystems.Remove(toDelete);
            _appDbContext.SaveChanges();
        }

        public void DeleteInventoryObjectType(InventoryObjectType toDelete)
        {
            _appDbContext.InventoryObjectTypes.Remove(toDelete);
            _appDbContext.SaveChanges();
        }

        public async Task<bool> UpdateInventoryObjectTypeAsync(InventoryObjectType newModel)
        {
            _appDbContext.InventoryObjectTypes.Update(newModel);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<InventoryObjectType> GetInventoryObjectTypeByIdAsync(int id)
        {
            IQueryable<InventoryObjectType> query = _appDbContext.InventoryObjectTypes
                .Include(cst => cst.Systems)
                    .ThenInclude(cs => cs.Parts)
                .Where(cst => cst.InventoryObjectTypeId == id);

            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public async Task<InventoryObjectType[]> GetInventoryObjectTypesByInventoryObjectIdAsync(int id)
        {
            IQueryable<InventoryObjectType> query = _appDbContext.InventoryObjectTypes
                .Where(cst => cst.InventoryObjectId == id);

            var result = await query.ToArrayAsync();
            return result;
        }
    }
}
