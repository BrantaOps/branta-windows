using Branta.Core.Data;
using Branta.Core.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace Branta.Stores;

public class ExtendedKeyStore
{
    private readonly BrantaContext _brantaContext;

    private List<ExtendedKey> _extendedKeys = [];

    public IEnumerable<ExtendedKey> ExtendedKeys => _extendedKeys;

    public event Action OnExtendedKeyUpdate;

    public bool IsLoading = true;

    public ExtendedKeyStore(BrantaContext brantaContext)
    {
        _brantaContext = brantaContext;
    }

    public async Task LoadAsync()
    {
        _extendedKeys = await _brantaContext.ExtendedKey
            .ToListAsync();

        IsLoading = false;

        OnExtendedKeyUpdate?.Invoke();
    }

    public async Task AddAsync(string name, string value)
    {
        var extendedKey = new ExtendedKey()
        {
            Name = name,
            Value = value,
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now
        };

        await _brantaContext.ExtendedKey.AddAsync(extendedKey);
        await _brantaContext.SaveChangesAsync();

        _extendedKeys.Add(extendedKey);

        OnExtendedKeyUpdate?.Invoke();
    }

    public async Task UpdateAsync(int id, string name, string value)
    {
        var extendedKey = _extendedKeys.FirstOrDefault(k => k.Id == id);

        if (extendedKey == null)
        {
            return;
        }

        extendedKey.Name = name;
        extendedKey.Value = value;
        extendedKey.DateUpdated = DateTime.Now;

        _brantaContext.ExtendedKey.Update(extendedKey);
        await _brantaContext.SaveChangesAsync();

        OnExtendedKeyUpdate?.Invoke();
    }

    public async Task RemoveAsync(int id)
    {
        var extendedKey = _extendedKeys.FirstOrDefault(k => k.Id == id);

        _brantaContext.ExtendedKey.Remove(extendedKey);
        await _brantaContext.SaveChangesAsync();

        _extendedKeys.Remove(extendedKey);

        OnExtendedKeyUpdate?.Invoke();
    }
}
