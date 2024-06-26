using Branta.Commands;
using Branta.Models;
using Branta.Stores;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;

namespace Branta.ViewModels;

public partial class EditExtendedKeyViewModel : ObservableValidator
{
    private readonly ExtendedKeyStore _extendedKeyStore;

    public readonly ExtendedKey ExtendedKey;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
    [Required(ErrorMessage = "Name is required.")]
    [CustomValidation(typeof(EditExtendedKeyViewModel), nameof(ValidateIsNameUnique))]
    private string _name;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
    [Required(ErrorMessage = "Extended Key is required.")]
    [CustomValidation(typeof(EditExtendedKeyViewModel), nameof(ValidateIsValueUnique))]
    [CustomValidation(typeof(EditExtendedKeyViewModel), nameof(ValidateExtendedKey))]
    private string _value;

	public Action CloseAction { get; set; }

    public IEnumerable<ExtendedKey> ExtendedKeys => _extendedKeyStore.ExtendedKeys;

    [RelayCommand(CanExecute = nameof(CanSubmit))]
    public void Submit()
    {
        if (ExtendedKey == null)
        {
            _extendedKeyStore.Add(Name, Value);
        }
        else
        {
            _extendedKeyStore.Update(ExtendedKey.Id, Name, Value);
        }

        CloseAction?.Invoke();
    }

    public bool CanSubmit()
    {
        return Name != null && Value != null && !HasErrors;
    }

    public EditExtendedKeyViewModel(ExtendedKeyStore extendedKeyStore, ExtendedKey extendedKey)
    {
        _extendedKeyStore = extendedKeyStore;
        ExtendedKey = extendedKey;

        if (ExtendedKey != null)
        {
            Name = ExtendedKey.Name;
            Value = ExtendedKey.Value;
        }
    }

    public static ValidationResult ValidateIsNameUnique(string value, ValidationContext context)
    {
        var viewModel = (EditExtendedKeyViewModel)context.ObjectInstance;

        if (viewModel.ExtendedKey == null && viewModel.ExtendedKeys.Any(k => k.Name == value.Trim()))
        {
            return new("Name must be unique.");
        }
        
        return ValidationResult.Success;
    }

    public static ValidationResult ValidateIsValueUnique(string value, ValidationContext context)
    {
        var viewModel = (EditExtendedKeyViewModel)context.ObjectInstance;

        if (viewModel.ExtendedKey == null && viewModel.ExtendedKeys.Any(k => k.Value == value.Trim()))
        {
            return new("Extended Key must be unique.");
        }
        
        return ValidationResult.Success;
    }

    public static ValidationResult ValidateExtendedKey(string value, ValidationContext context)
    {
        var viewModel = (EditExtendedKeyViewModel)context.ObjectInstance;

        if (!ClipboardGuardianCommand.CheckForXPub(value.Trim()))
        {
            return new("xpub format is invalid.");
        }
        
        return ValidationResult.Success;
    }
}
