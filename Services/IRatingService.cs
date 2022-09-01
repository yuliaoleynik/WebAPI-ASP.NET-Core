using WebAPI_Task2.Model;

namespace WebAPI_Task2.Services
{
    public interface IRatingService
    {
        public Task<bool> AddRate(int id, Rating rating);
    }
}
