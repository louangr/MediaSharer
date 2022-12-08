using System;
using System.Diagnostics;
using MediaSharer.Repositories.Interfaces;
using Windows.ApplicationModel;
using Windows.Management.Core;

namespace MediaSharer.Repositories.Implementations
{
    public class SettingsRepository : ISettingsRepository
    {
        #region Publics Properties

        public bool AutoPlay
        {
            get => TryGetLocalValue(nameof(AutoPlay), false);
            set => TrySetLocalValue(nameof(AutoPlay), value);
        }

        #endregion Publics Properties

        #region Private Methods

        private T TryGetLocalValue<T>(string key, T defaultValue)
        {
            try
            {
                object tValue;

                if (!ApplicationDataManager.CreateForPackageFamily(Package.Current.Id.FamilyName).LocalSettings.Values.TryGetValue(key, out tValue))
                {
                    return defaultValue;
                }

                return (T)tValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        private void TrySetLocalValue<T>(string key, T value)
        {
            try
            {
                var appData = ApplicationDataManager.CreateForPackageFamily(Package.Current.Id.FamilyName);

                if (appData.LocalSettings.Values.ContainsKey(key))
                {
                    appData.LocalSettings.Values[key] = value;
                }
                else
                {
                    appData.LocalSettings.Values.Add(key, value);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        #endregion Private Methods
    }
}
