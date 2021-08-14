using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace ProyectoDIV1.Helpers
{
    public static class Settings
    {
        private const string _usuario = "usuario";
        private static readonly string _stringDefault = string.Empty;
        private const string _candidato = "candidato";
        private static readonly string _candidatoDefault = string.Empty;
        private const string _empresa = "empresa";
        private static readonly string _empresaDefault = string.Empty;
        private const string _token = "token";
        private static readonly string _tokenDefault = string.Empty;
        private static ISettings AppSettings => CrossSettings.Current;

        public static string Usuario
        {
            get => AppSettings.GetValueOrDefault(_usuario, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_usuario, value);
        }

        public static string Candidato
        {
            get => AppSettings.GetValueOrDefault(_candidato, _candidatoDefault);
            set => AppSettings.AddOrUpdateValue(_candidato, value);
        }

        public static string Empresa
        {
            get => AppSettings.GetValueOrDefault(_empresa, _empresaDefault);
            set => AppSettings.AddOrUpdateValue(_empresa, value);
        }

        public static string Token
        {
            get => AppSettings.GetValueOrDefault(_token, _tokenDefault);
            set => AppSettings.AddOrUpdateValue(_token, value);
        }
    }
}
