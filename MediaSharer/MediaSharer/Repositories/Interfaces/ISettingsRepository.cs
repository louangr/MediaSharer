namespace MediaSharer.Repositories.Interfaces
{
    public interface ISettingsRepository
    {
        bool AutoPlay { get; set; }

        bool AutoStop { get; set; }

        bool IsProjectionWindowFullScreenEnabled { get; set; }

        bool IsProjectionWindowAlwaysOnTopWhenSharing { get; set; }
    }
}
