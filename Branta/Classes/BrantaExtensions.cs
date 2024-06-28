using Branta.Interfaces;
using Branta.Stores;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Branta.Classes;

public static class BrantaExtensions
{
    public static void SetSource(this System.Windows.Controls.Image image, string path)
    {
        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.UriSource = new Uri($"pack://application:,,,/{path}", UriKind.Absolute);
        bitmapImage.EndInit();

        image.Source = bitmapImage;
    }

    public static string Format(this TimeSpan timeSpan)
    {
        if (timeSpan.Hours > 0)
        {
            return $"{timeSpan.Hours}h";
        }

        if (timeSpan.Minutes > 0)
        {
            return $"{timeSpan.Minutes}m";
        }

        return $"{timeSpan.Seconds}s";
    }

    public static void SetLanguageDictionary(this FrameworkElement frameworkElement, LanguageStore languageStore)
    {
        frameworkElement.Resources.MergedDictionaries.Add(languageStore.ResourceDictionary);
    }

    public static ValidationResult Validate<T>(this ValidationContext context, string value, Func<T, string, bool> check, string stringResourceKey) where T : IValidateViewModel
    {
        var viewModel = (T)context.ObjectInstance;

        if (check.Invoke(viewModel, value.Trim()))
        {
            return new(viewModel.LanguageStore.Get(stringResourceKey));
        }

        return ValidationResult.Success;
    }
}