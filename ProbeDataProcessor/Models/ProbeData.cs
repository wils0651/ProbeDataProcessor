using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProbeDataProcessor.Models
{

    [Table("probedata")]
    public class ProbeData
    {
        [Key]
        [Column("probedataid")]
        public long ProbeDataId { get; set; }

        [Column("probeid")]
        public int ProbeId { get; set; }

        [ForeignKey(nameof(ProbeId))]
        public Probe? Probe { get; set; }

        [Column("temperature")]
        public decimal Temperature { get; set; }

        [Column("createddate")]
        public DateTime CreatedDate { get; set; }
    }
}
