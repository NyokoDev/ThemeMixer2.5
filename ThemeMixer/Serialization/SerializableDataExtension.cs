using ICities;
using ThemeMixer.Themes;

namespace ThemeMixer.Serialization
{
    public class SerializableDataExtension : SerializableDataExtensionBase
    {
        public override void OnSaveData()
        {
            base.OnSaveData();
            ThemeManager.Instance.OnSaveData(serializableDataManager);
        }

        public override void OnLoadData()
        {
            base.OnLoadData();
            ThemeManager.Instance.OnLoadData(serializableDataManager);
        }
    }
}
