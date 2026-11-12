using APICalidadAire.Data;
using APICalidadAire.DTOs;
using APICalidadAire.Models;
using APICalidadAire.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICalidadAire.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly TokenService _tokenService;

        public AuthController(
            ApplicationDbContext context,
            IPasswordHasher<Usuario> passwordHasher,
            TokenService tokenService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        // POST: api/auth/registrar
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar(RegistroDTO dto)
        {
            var email = dto.Email.Trim().ToLowerInvariant();

            var existe = await _context.Usuarios.AnyAsync(u => u.Email == email);

            if (existe)
                return Conflict(new { mensaje = "El correo ya está registrado." });

            var usuario = new Usuario
            {
                Nombre = dto.Nombre.Trim(),
                Email = email,
                FechaRegistro = DateTime.UtcNow
            };

            usuario.PasswordHash = _passwordHasher.HashPassword(usuario, dto.Password);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Usuario registrado correctamente.",
                usuario.Id,
                usuario.Nombre,
                usuario.Email,
                usuario.FechaRegistro
            });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var email = dto.Email.Trim().ToLowerInvariant();

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
                return Unauthorized(new { mensaje = "Credenciales incorrectas." });

            var resultado = _passwordHasher.VerifyHashedPassword(
                usuario,
                usuario.PasswordHash,
                dto.Password
            );

            if (resultado == PasswordVerificationResult.Failed)
                return Unauthorized(new { mensaje = "Credenciales incorrectas." });

            var respuesta = _tokenService.GenerarToken(usuario);

            return Ok(respuesta);
        }
    }
}
