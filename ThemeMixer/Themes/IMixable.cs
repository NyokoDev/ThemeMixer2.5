namespace ThemeMixer.Themes
{
    public interface IMixable
    {
        bool Load(string themeID);
        void SetCustomValue(object value);
    }
}
