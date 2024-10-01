namespace TalentTrail.Dto
{
    public class CertificationUpdateDto
    {
        public int CertificationId { get; set; }
        public string CertificationName { get; set; }
        public string CertificatePicturePath { get; set; } 
        public DateTime DateIssued { get; set; }
    }
}
