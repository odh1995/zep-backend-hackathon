namespace Busan_Hackathon_2022.Models.Request
{
    public class CropsRequest
    {
        public string? Id { get; set; }
        public int UserId { get; set; }
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }

    }
}
