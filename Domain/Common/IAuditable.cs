namespace OraEmp.Domain.Common
{
    public interface IAuditable
    {
        string CreateUser { get; set; }
        DateTime CreateDateTime { get; set; }
        decimal? CreatePtyId { get; set; }
        string UpdateUser { get; set; }
        DateTime? UpdateDateTime { get; set; }
        decimal? UpdatePtyId { get; set; }
    }
}