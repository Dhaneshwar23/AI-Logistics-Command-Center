using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AILogistics.Application.DTOs.Shipments
{
    public class UpdateShipment : IValidatableObject
    {
        [Required, StringLength(300)]
        public string Origin { get; set; }
        [Required, StringLength(300)]
        public string Destination { get; set; }
        [Range(typeof(decimal), "0.01", "1000000")]
        public decimal WeightKg { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime DeliveryDate { get; set; }

        [Required]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DeliveryDate < PickupDate)
            {
                yield return new ValidationResult(
                    "DeliveryDate must be on or after PickupDate.",
                    new[] { nameof(DeliveryDate) });
            }
        }
    }
}
