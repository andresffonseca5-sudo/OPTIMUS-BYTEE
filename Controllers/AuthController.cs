using BC = BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Mvc;
using OPTIMUS_BYTEE.Models;
using UPTIMUS.Data;
using UPTIMUS.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace UPTIMUS.Controllers
{
    public class AuthController : Controller
    {
        private readonly UptimusDBContext _db;

        public AuthController(UptimusDBContext db) => _db = db;

        // GET: /Auth/Login
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UsuarioId") != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var usuario = _db.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefault(u => u.Correo == model.Correo && u.Activo);

            if (usuario == null || !BC.Verify(model.Contrasena, usuario.ContrasenaHash))
            {
                // Registrar intento fallido
                _db.IntentosFallidos.Add(new IntentoFallido
                {
                    Correo = model.Correo
                });
                _db.SaveChanges();

                ModelState.AddModelError("", "Correo o contraseña incorrectos");
                return View(model);
            }

            // Guardar sesión
            HttpContext.Session.SetString("UsuarioId", usuario.IdUsuario.ToString());
            HttpContext.Session.SetString("UsuarioNombre", usuario.NombreCompleto);
            HttpContext.Session.SetString("UsuarioRol", usuario.Rol.NombreRol);

            // Registrar en auditoría
            _db.LogAuditoria.Add(new LogAuditoria
            {
                IdUsuario = usuario.IdUsuario,
                Accion = "Inicio de sesión exitoso",
                Modulo = "Autenticación"
            });
            _db.SaveChanges();

            // Redirigir según el rol
            return usuario.Rol.NombreRol switch
            {
                "Administrador" => RedirectToAction("Dashboard", "Admin"),
                "Recepcionista" => RedirectToAction("Index", "Recepcion"),
                "Mecanico" => RedirectToAction("Index", "Mecanico"),
                _ => RedirectToAction("Index", "Home")
            };
        }


        

        // GET: /Auth/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Register()
        {
            return View();
        }

        // POST: /Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var existe = _db.Usuarios.Any(u => u.Correo == model.Correo);
            if (existe)
            {
                ModelState.AddModelError("", "El correo ya existe");
                return View(model);
            }

            var usuario = new Usuario
            {
                NombreCompleto = "Nuevo Usuario",
                Correo = model.Correo,
                Telefono = "000000000",
                ContrasenaHash = BC.HashPassword(model.Contrasena), // 🔥 AQUÍ
                IdRol = 1,
                Activo = true
            };

            _db.Usuarios.Add(usuario);
            _db.SaveChanges();

            return RedirectToAction("Login");
        }

    }
}