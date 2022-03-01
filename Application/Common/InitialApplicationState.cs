using System.Text;
using Microsoft.Extensions.Primitives;

namespace OraEmp.Application.Common;

public class InitialApplicationState
{

    public string Username { get; set; } = "NOBODY";
    public string RemoteIpAddress { get; set; }
    public string LocalPort { get; set; }
    public string RemotePort { get; set; }
    public string Test { get; set; }
    public string UserAgent { get; set; }

    public override string ToString()
    {
        return GetType().GetProperties()
            .Select(info => (info.Name, Value: info.GetValue(this, null) ?? "(null)"))
            .Aggregate(
                new StringBuilder(),
                (sb, pair) => sb.AppendLine($"{pair.Name}: {pair.Value}"),
                sb => sb.ToString());
    }
}