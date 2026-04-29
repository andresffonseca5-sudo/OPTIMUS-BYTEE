using OPTIMUS_BYTEE.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Usuarios")]
public class Usuario
{
    [Key]
    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("nombre_completo")]
    public string NombreCompleto { get; set; } = string.Empty;

    [Column("correo")]
    public string Correo { get; set; } = string.Empty;

    [Column("contrasena_hash")]
    public string ContrasenaHash { get; set; } = string.Empty;

    [Column("activo")]
    public bool Activo { get; set; }

    [Column("id_rol")]
    public int IdRol { get; set; }

    [ForeignKey("IdRol")] // 🔥 ESTA LÍNEA ES LA SOLUCIÓN
    public Rol? Rol { get; set; }

    [Column("telefono")]
    public string Telefono { get; set; } = string.Empty;
}