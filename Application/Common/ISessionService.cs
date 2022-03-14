namespace Application.Common;

public interface ISessionService
{
    ValueTask ClearAsync(CancellationToken? cancellationToken = null);

    ValueTask<T> GetItemAsync<T>(string key, CancellationToken? cancellationToken = null);

    ValueTask<string> GetItemAsStringAsync(
        string key,
        CancellationToken? cancellationToken = null);


   // ValueTask<string> KeyAsync(int index, CancellationToken? cancellationToken = null);

    ValueTask<bool> ContainKeyAsync(string key, CancellationToken? cancellationToken = null);

    ValueTask<int> LengthAsync(CancellationToken? cancellationToken = null);

    ValueTask RemoveItemAsync(string key, CancellationToken? cancellationToken = null);

    ValueTask SetItemAsync<T>(string key, T data, CancellationToken? cancellationToken = null);

    ValueTask SetItemAsStringAsync(
        string key,
        string data,
        CancellationToken? cancellationToken = null);

    //event EventHandler<ChangingEventArgs> Changing;

    //event EventHandler<ChangedEventArgs> Changed;
}