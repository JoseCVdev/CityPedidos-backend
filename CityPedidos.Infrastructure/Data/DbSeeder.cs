
using CityPedidos.Domain.Entities.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPedidos.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static void Seed(PedidosDbContext context)
        {
            // ===== ROLES =====
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Rol
                    {
                        vNombre = "ADMIN_ROLE",
                        vDescripcion = "Administrador",
                        bitEstado = true
                    },
                    new Rol
                    {
                        vNombre = "ORDER_ROLE",
                        vDescripcion = "Gestor",
                        bitEstado = true
                    }
                );

                context.SaveChanges();
            }

            // ===== MENUS =====
            if (!context.Menus.Any())
            {
                context.Menus.AddRange(
                    new Menu
                    {
                        vNombre = "Gestión de pedidos",
                        vIcono = "shopping-cart",
                        vUrl = "/pages/gestion-pedidos",
                        bitEstado = true
                    },
                    new Menu
                    {
                        vNombre = "Gestión de clientes",
                        vIcono = "users",
                        vUrl = "/pages/gestion-clientes",
                        bitEstado = true
                    }
                );

                context.SaveChanges();
            }

            // ===== ROL - MENU =====
            if (!context.RolMenus.Any())
            {
                var adminRole = context.Roles.First(r => r.vNombre == "ADMIN_ROLE");
                var orderRole = context.Roles.First(r => r.vNombre == "ORDER_ROLE");

                var menuPedidos = context.Menus.First(m => m.vUrl == "/pages/gestion-pedidos");
                var menuUsuarios = context.Menus.First(m => m.vUrl == "/pages/gestion-clientes");

                context.RolMenus.AddRange(
                    new RolMenu { NIdRol = adminRole.nIdRol, nIdMenu = menuPedidos.nIdMenu },
                    new RolMenu { NIdRol = adminRole.nIdRol, nIdMenu = menuUsuarios.nIdMenu },
                    new RolMenu { NIdRol = orderRole.nIdRol, nIdMenu = menuPedidos.nIdMenu }
                );

                context.SaveChanges();
            }

            // ===== USUARIO ADMIN =====
            if (!context.Usuarios.Any(u => u.vCorreo == "admin@admin.com"))
            {
                var adminRole = context.Roles.First(r => r.vNombre == "ADMIN_ROLE");

                var adminUser = new Usuario
                {
                    vCorreo = "admin@admin.com",
                    vContrasena = BCrypt.Net.BCrypt.HashPassword("123"),
                    vNombreUsuario = "Jose Manuel",
                    bitEstado = true,
                    nIdRol = adminRole.nIdRol,
                    nIdUsuarioCrea = 0 // sistema
                };

                context.Usuarios.Add(adminUser);
            }

            // ===== USUARIO ORDER =====
            if (!context.Usuarios.Any(u => u.vCorreo == "order@order.com"))
            {
                var orderRole = context.Roles.First(r => r.vNombre == "ORDER_ROLE");

                var orderUser = new Usuario
                {
                    vCorreo = "order@order.com",
                    vContrasena = BCrypt.Net.BCrypt.HashPassword("123"),
                    vNombreUsuario = "Sebastian Esteban",
                    bitEstado = true,
                    nIdRol = orderRole.nIdRol,
                    nIdUsuarioCrea = 0 // sistema
                };

                context.Usuarios.Add(orderUser);
            }

            context.SaveChanges();


        }
    }
}