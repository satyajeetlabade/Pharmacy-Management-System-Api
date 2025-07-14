﻿namespace PharmacyManagementSystem.DTOS.Suppliers
{
    public class SupplierResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ContactNumber { get; set; }
        public string? ContactEmail { get; set; }
        public string? Address { get; set; }
    }
}
