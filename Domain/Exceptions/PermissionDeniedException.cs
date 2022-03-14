namespace OraEmp.Domain.Exceptions;

public class PermissionDeniedException :Exception
{
    public PermissionDeniedException()
    {

    }

    public PermissionDeniedException(string save) : base(save)
    {
    }
}