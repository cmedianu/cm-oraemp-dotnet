using System.Text;
using Microsoft.Extensions.Primitives;

namespace OraEmp.Application.Common;


/// <summary>
///  Initial application state set from HTTP context in _Host.cshtml
/// </summary>
public class InitialApplicationState
{
    public InitialApplicationState(string username, string remoteIpAddress, string localPort, string remotePort, string userAgent)
    {
        Username = username;
        RemoteIpAddress = remoteIpAddress;
        LocalPort = localPort;
        RemotePort = remotePort;
        UserAgent = userAgent;
    }

    public string Username { get; init; } = "NOBODY";
    public string RemoteIpAddress { get; init; }
    public string LocalPort { get; init; }
    public string RemotePort { get; init; }
    public string UserAgent { get; init; }
    public string? SessionId { get; set; }

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