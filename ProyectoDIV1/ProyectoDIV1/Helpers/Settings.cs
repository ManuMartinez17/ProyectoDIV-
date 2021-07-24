using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace ProyectoDIV1.Helpers
{
    public static class Settings
    {
        private const string _usuario = "usuario";
        private static readonly string _stringDefault = string.Empty;

        private static ISettings AppSettings => CrossSettings.Current;

        public static string Usuario
        {
            get => AppSettings.GetValueOrDefault(_usuario, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_usuario, value);
        }


    }
}
