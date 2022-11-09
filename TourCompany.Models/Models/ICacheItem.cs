namespace TourCompany.Models.Models
{
    public interface ICacheItem<out T>
    {
        T GetKey();
    }
}
