using System.Collections.Concurrent;
using Application.Common;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.DataProtection;

namespace OraEmp.Application.Session;

public class TabStorageSessionImpl : ISessionService
{
    private readonly ISessionStorageService _db;
    private IDataProtector _protector;

    public TabStorageSessionImpl(ISessionStorageService db, IDataProtectionProvider protectionProvider)
    {
        _db = db;
        _protector = protectionProvider.CreateProtector("OraEmp");
    }

    public ValueTask ClearAsync(CancellationToken? cancellationToken = null)
    {
        return _db.ClearAsync();
    }

    public ValueTask<T> GetItemAsync<T>(string key, CancellationToken? cancellationToken = null)
    {
        var result = Task.Run(async() => await _db.GetItemAsync<T>(key)).Result;
        return new ValueTask<T>(result);
    }

    public ValueTask<string> GetItemAsStringAsync(string key, CancellationToken? cancellationToken = null)
    {
        return _db.GetItemAsStringAsync(key);
    }

    public ValueTask<bool> ContainKeyAsync(string key, CancellationToken? cancellationToken = null)
    {
        return _db.ContainKeyAsync(key);
    }

    public ValueTask<int> LengthAsync(CancellationToken? cancellationToken = null)
    {
        return _db.LengthAsync();
    }

    public ValueTask RemoveItemAsync(string key, CancellationToken? cancellationToken = null)
    {
        return _db.RemoveItemAsync(key, cancellationToken);
    }

    public ValueTask SetItemAsync<T>(string key, T data, CancellationToken? cancellationToken = null)
    {
        return _db.SetItemAsync(key, data,cancellationToken);
    }

    public ValueTask SetItemAsStringAsync(string key, string data, CancellationToken? cancellationToken = null)
    {
        return _db.SetItemAsStringAsync(key, data);
    }
}