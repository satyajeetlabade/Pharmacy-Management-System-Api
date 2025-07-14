using Microsoft.EntityFrameworkCore;
using PharmacyManagementSystem.Data;
using PharmacyManagementSystem.Interfaces;
using PharmacyManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyManagementSystem.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ApplicationDbContext _context;

        public SupplierRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            return await _context.Suppliers
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _context.Suppliers
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Supplier> AddAsync(Supplier supplier)
        {
            if (supplier == null)
            {
                throw new ArgumentNullException(nameof(supplier), "Supplier cannot be null.");
            }

            try
            {
                _context.Suppliers.Add(supplier);
                await _context.SaveChangesAsync();
                return supplier;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding the supplier.", ex);
            }
        }

        public async Task UpdateAsync(Supplier supplier)
        {
            if (supplier == null)
            {
                throw new ArgumentNullException(nameof(supplier), "Supplier cannot be null.");
            }

            var existingSupplier = await _context.Suppliers.FindAsync(supplier.Id);
            if (existingSupplier == null)
            {
                throw new KeyNotFoundException($"Supplier with ID {supplier.Id} not found.");
            }

            existingSupplier.Name = supplier.Name;
            existingSupplier.ContactEmail = supplier.ContactEmail;
            existingSupplier.ContactNumber = supplier.ContactNumber;
            existingSupplier.Address = supplier.Address;

            try
            {
                await _context.SaveChangesAsync(); 
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the supplier.", ex);
            }
        }


        public async Task DeleteAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                try
                {
                    _context.Suppliers.Remove(supplier);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("An error occurred while deleting the supplier.", ex);
                }
            }
        }
    }
}
