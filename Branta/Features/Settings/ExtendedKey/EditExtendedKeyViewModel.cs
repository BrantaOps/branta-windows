using Branta.Classes;
using Branta.Core.Data.Domain;
using Branta.Features.ClipboardGuardian;
using Branta.Features.Main;
using Branta.Features.Settings.ExtendedKey;
using Branta.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;

namespace Branta.ViewModels;

public partial class EditExtendedKeyViewModel : ObservableValidator, IValidateViewModel
{
    private readonly ExtendedKeyStore _extendedKeyStore;

    public readonly ExtendedKey ExtendedKey;

    public LanguageStore LanguageStore { get; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
    [CustomValidation(typeof(EditExtendedKeyViewModel), nameof(ValidateNameRequired))]
    [CustomValidation(typeof(EditExtendedKeyViewModel), nameof(ValidateNameUnique))]
    private string _name;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
    [CustomValidation(typeof(EditExtendedKeyViewModel), nameof(ValidateExtendedKeyRequired))]
    [CustomValidation(typeof(EditExtendedKeyViewModel), nameof(ValidateExtendedKeyUnique))]
    [CustomValidation(typeof(EditExtendedKeyViewModel), nameof(ValidateExtendedKey))]
    private string _value;

    public Action CloseAction { get; set; }

    public IEnumerable<ExtendedKey> ExtendedKeys => _extendedKeyStore.ExtendedKeys;

    [RelayCommand(CanExecute = nameof(CanSubmit))]
    public async Task Submit()
    {
        if (ExtendedKey == null)
        {
            await _extendedKeyStore.AddAsync(Name, Value);
        }
        else
        {
            await _extendedKeyStore.UpdateAsync(ExtendedKey.Id, Name, Value);
        }

        CloseAction?.Invoke();
    }

    public bool CanSubmit()
    {
        return Name != null && Value != null && !HasErrors;
    }

    public EditExtendedKeyViewModel(ExtendedKeyStore extendedKeyStore, ExtendedKey extendedKey, LanguageStore languageStore)
    {
        _extendedKeyStore = extendedKeyStore;
        ExtendedKey = extendedKey;
        LanguageStore = languageStore;

        if (ExtendedKey != null)
        {
            Name = ExtendedKey.Name;
            Value = ExtendedKey.Value;
        }
    }

    public static ValidationResult ValidateNameRequired(string value, ValidationContext context)
    {
        return context.Validate<EditExtendedKeyViewModel>(
            value,
            (_, value) => string.IsNullOrEmpty(value),
            "Validation_ExtendedKey_Name_Required");
    }

    public static ValidationResult ValidateNameUnique(string value, ValidationContext context)
    {
        return context.Validate<EditExtendedKeyViewModel>(
            value,
            (viewModel, value) => viewModel.ExtendedKeys.Any(k => k.Name == value && k.Id != viewModel.ExtendedKey?.Id),
            "Validation_ExtendedKey_Name_Unique");
    }

    public static ValidationResult ValidateExtendedKeyRequired(string value, ValidationContext context)
    {
        return context.Validate<EditExtendedKeyViewModel>(
            value,
            (_, value) => string.IsNullOrEmpty(value),
            "Validation_ExtendedKey_Value_Required");
    }

    public static ValidationResult ValidateExtendedKeyUnique(string value, ValidationContext context)
    {
        return context.Validate<EditExtendedKeyViewModel>(
            value,
            (viewModel, value) => viewModel.ExtendedKeys.Any(k => k.Value == value && k.Id != viewModel.ExtendedKey?.Id),
            "Validation_ExtendedKey_Value_Unique");
    }

    public static ValidationResult ValidateExtendedKey(string value, ValidationContext context)
    {
        return context.Validate<EditExtendedKeyViewModel>(
            value,
            (_, value) => !ClipboardGuardianCommand.CheckForXPub(value),
            "Validation_ExtendedKey_Value_Valid");
    }
}
