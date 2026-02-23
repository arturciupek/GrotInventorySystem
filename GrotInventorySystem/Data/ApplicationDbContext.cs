using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GrotInventorySystem.Domain.Entities;

namespace GrotInventorySystem.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Module> Modules => Set<Module>();
    public DbSet<Weapon> Weapons => Set<Weapon>();
    public DbSet<WeaponModuleAssignment> WeaponModuleAssignments => Set<WeaponModuleAssignment>();
}