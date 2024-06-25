using Branta.Models;

namespace Branta.Stores;

public class ExtendedKeyStore
{
    private readonly List<ExtendedKey> _extendedKeys = [];

    public IEnumerable<ExtendedKey> ExtendedKeys => _extendedKeys;

    public event Action OnExtendedKeyUpdate;

    public void Add(string name, string value)
    {
        _extendedKeys.Add(new ExtendedKey()
        {
            Id  = _extendedKeys.Count == 0 ? 1 : _extendedKeys.Max(k => k.Id) + 1,
            Name = name,
            Value = value
        });

        OnExtendedKeyUpdate?.Invoke();
    }

    public void Update(int id, string name, string value)
    {
        var extendedKey = _extendedKeys.FirstOrDefault(k => k.Id == id);

        if (extendedKey == null)
        {
            return;
        }

        extendedKey.Name = name;
        extendedKey.Value = value;

        OnExtendedKeyUpdate?.Invoke();
    }

    public void Remove(int id)
    {
        _extendedKeys.RemoveAll(k => k.Id == id);

        OnExtendedKeyUpdate?.Invoke();
    }
}
