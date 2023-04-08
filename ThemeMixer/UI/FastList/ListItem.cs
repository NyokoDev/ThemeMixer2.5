using ThemeMixer.Themes.Enums;

namespace ThemeMixer.UI.FastList
{
    public class ListItem
    {
        public readonly string ID;
        public readonly string DisplayName;
        public readonly string Author;
        public bool IsFavourite;
        public bool IsBlacklisted;
        public ThemeCategory Category;

        public ListItem(string id, string displayName, string author, bool isFavourite, bool isBlacklisted, ThemeCategory category)
        {
            ID = id;
            DisplayName = displayName;
            Author = author;
            IsFavourite = isFavourite;
            IsBlacklisted = isBlacklisted;
            Category = category;
        }
    }
}
