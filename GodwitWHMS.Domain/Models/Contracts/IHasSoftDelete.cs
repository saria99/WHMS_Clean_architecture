namespace GodwitWHMS.Domain.Models.Contracts
{
    public interface IHasSoftDelete
    {
        bool IsNotDeleted { get; set; }
    }
}
