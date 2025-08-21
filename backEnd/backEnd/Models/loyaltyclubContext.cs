using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace backEnd.Models
{
    public partial class loyaltyclubContext : DbContext
    {
        public loyaltyclubContext()
        {
        }

        public loyaltyclubContext(DbContextOptions<loyaltyclubContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Articulo> Articulos { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<ClienteArticulo> ClienteArticulos { get; set; }
        public virtual DbSet<Tienda> Tiendas { get; set; }
        public virtual DbSet<TiendaArticulo> TiendaArticulos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= 		syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.

                var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                var configuration = builder.Build();
                var conexion = configuration.GetValue<string>("ConnectionStrings:DefaultDatabase");
                optionsBuilder.UseSqlServer(conexion);

            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Articulo>(entity =>
            {
                entity.HasKey(e => e.IdArticulo)
                    .HasName("PK_Membresias");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Imagen).HasMaxLength(300);

                entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.Apellidos).HasMaxLength(256);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Nombre).HasMaxLength(256);

                entity.Property(e => e.Password).HasMaxLength(256);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_AspNetUserRoles_AspNetRoles"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_AspNetUserRoles_AspNetUsers"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");

                            j.IndexerProperty<string>("UserId").HasMaxLength(128);

                            j.IndexerProperty<string>("RoleId").HasMaxLength(128);
                        });
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente);

                entity.Property(e => e.IdCliente).ValueGeneratedNever();

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Nombres)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Password).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<ClienteArticulo>(entity =>
            {
                entity.HasKey(e => new { e.IdCliente, e.IdArticulo });

                entity.ToTable("ClienteArticulo");

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.HasOne(d => d.IdArticuloNavigation)
                    .WithMany(p => p.ClienteArticulos)
                    .HasForeignKey(d => d.IdArticulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClienteArticulo_Articulos");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.ClienteArticulos)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClienteArticulo_Clientes");
            });

            modelBuilder.Entity<Tienda>(entity =>
            {
                entity.HasKey(e => e.IdTienda);

                entity.Property(e => e.IdTienda).ValueGeneratedNever();

                entity.Property(e => e.Direccion).HasMaxLength(300);

                entity.Property(e => e.Sucursal)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<TiendaArticulo>(entity =>
            {
                entity.HasKey(e => new { e.IdTienda, e.IdArticulo });

                entity.ToTable("TiendaArticulo");

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.HasOne(d => d.IdArticuloNavigation)
                    .WithMany(p => p.TiendaArticulos)
                    .HasForeignKey(d => d.IdArticulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TiendaArticulo_Articulos");

                entity.HasOne(d => d.IdTiendaNavigation)
                    .WithMany(p => p.TiendaArticulos)
                    .HasForeignKey(d => d.IdTienda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TiendaArticulo_Tiendas");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
