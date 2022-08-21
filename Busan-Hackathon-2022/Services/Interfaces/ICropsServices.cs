namespace Busan_Hackathon_2022.Services.Interfaces
{
    public interface ICropsService
    {
        Task<IEnumerable<Crops>> GetCrops(string queryString);
        Task<bool> GetCropsBoolById(string id);
        Task<Crops>GetCropsById(string id);
        Task<Crops> GetCropsByCoordinate(int x, int y);
        Task CreateCrops(Crops crops);
        Task UpdateCrops(Crops crops);
        Task DeleteCrop(string id);
    }
}
