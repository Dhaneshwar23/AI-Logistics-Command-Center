using System.ComponentModel.DataAnnotations;

namespace AILogistics.Application.Customers;

public sealed class UpdateCustomerRequest : CreateCustomerRequest
{
    [Required]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
