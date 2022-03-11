using System.Collections.Concurrent;
using Application.Common;

namespace OraEmp.Application.Session;

public class SimpleMemorySessionImpl : ISessionService
{
    ConcurrentDictionary<string, object> db = new ConcurrentDictionary<string, object>();
    public ValueTask ClearAsync(CancellationToken? cancellationToken = null)
    {
        db.Clear();
        return new ValueTask(Task.CompletedTask);
    }

    public ValueTask<T> GetItemAsync<T>(string key, CancellationToken? cancellationToken = null)
    {
        var  found = db.TryGetValue(key, out var ret);
        return new ValueTask<T>((T)ret);
    }

    public ValueTask<string> GetItemAsStringAsync(string key, CancellationToken? cancellationToken = null)
    {
        var  found = db.TryGetValue(key, out var ret);
        return new ValueTask<string>((string)ret);
    }

    public string GetItemAsString(string key)
    {
        var  found = db.TryGetValue(key, out var ret);
        return (string) ret;
    }

    public ValueTask<bool> ContainKeyAsync(string key, CancellationToken? cancellationToken = null)
    {
        return new ValueTask<bool>(db.ContainsKey(key));
    }

    public ValueTask<int> LengthAsync(CancellationToken? cancellationToken = null)
    {
        return new ValueTask<int>(db.Count);
    }

    public ValueTask RemoveItemAsync(string key, CancellationToken? cancellationToken = null)
    {
        db.TryRemove(key, out var ret);
        return new ValueTask();
    }

    public ValueTask SetItemAsync<T>(string key, T data, CancellationToken? cancellationToken = null)
    {
        db.TryAdd(key, data);
        return new ValueTask();
    }

    public ValueTask SetItemAsStringAsync(string key, string data, CancellationToken? cancellationToken = null)
    {
        this.SetItemAsync(key, data);
        return new ValueTask();
    }
}