using System.ComponentModel.DataAnnotations;

namespace CustomerManagement.Api.Models;

/// <summary>
/// The request body for <c>POST /products</c>.
/// </summary>
public class AddProductRequest
{
    /// <summary>The product name. Required.</summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>An optional description of the product.</summary>
    public string? Description { get; set; }

    /// <summary>The product price. Required and must be greater than 0.</summary>
    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
    public decimal Price { get; set; }
}
