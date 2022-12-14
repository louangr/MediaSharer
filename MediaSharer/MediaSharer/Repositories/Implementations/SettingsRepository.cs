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

        public bool AutoStop
        {
            get => TryGetLocalValue(nameof(AutoStop), false);
            set => TrySetLocalValue(nameof(AutoStop), value);
        }

        public bool IsProjectionWindowFullScreenEnabled
        {
            get => TryGetLocalValue(nameof(IsProjectionWindowFullScreenEnabled), false);
            set => TrySetLocalValue(nameof(IsProjectionWindowFullScreenEnabled), value);
        }

        public bool IsProjectionWindowAlwaysOnTopWhenSharing
        {
            get => TryGetLocalValue(nameof(IsProjectionWindowAlwaysOnTopWhenSharing), true);
            set => TrySetLocalValue(nameof(IsProjectionWindowAlwaysOnTopWhenSharing), value);
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
