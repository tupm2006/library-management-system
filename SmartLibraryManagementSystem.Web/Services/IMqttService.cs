using System.Threading.Tasks;

namespace SmartLibraryManagementSystem.Services
{
    public interface IMqttService
    {
        Task PublishMessageAsync(string topic, string payload);
    }
}