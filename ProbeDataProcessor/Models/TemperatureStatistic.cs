using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProbeDataProcessor.Models
{
    [Table("temperaturestatistic")]
    public class TemperatureStatistic
    {
        [Key]
        [Column("temperaturestatisticid")]
        public long TemperatureStatisticId { get; set; }

        [Column("probeid")]
        public int ProbeId { get; set; }

        [ForeignKey(nameof(ProbeId))]
        public Probe? Probe { get; set; }

        [Column("datacount")]
        public int DataCount { get; set; }

        [Column("mean")]
        public decimal Mean { get; set; }

        [Column("standarddeviation")]
        public decimal StandardDeviation { get; set; }

        [Column("minimum")]
        public decimal Minimum { get; set; }

        [Column("maximum")]
        public decimal Maximum { get; set; }

        [Column("measurementdate")]
        public DateTime MeasurementDate { get; set; }
    }
}
