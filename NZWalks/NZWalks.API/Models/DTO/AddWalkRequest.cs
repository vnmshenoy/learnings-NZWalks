namespace NZWalks.API.Models.DTO
{
    //Same as  Walk except the navigation properties because navigation
    // properties are not exactly for walk but for connecting
    //also we dont want id
    public class AddWalkRequest
    {
        public string Name { get; set; }
        public double Length { get; set; }
        public Guid RegionId { get; set; }
        public Guid WalkDifficultyId { get; set; }
    }
}
