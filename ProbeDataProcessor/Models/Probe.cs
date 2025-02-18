using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProbeDataProcessor.Models
{
    [Table("probe")]
    public class Probe
    {
        [Key]
        [Column("probeid")]
        public int ProbeId { get; set; }

        [MaxLength(128)]
        [Column("probename")]
        public required string ProbeName { get; set; }

        [MaxLength(512)]
        [Column("description")]
        public string? Description { get; set; }
    }
}
