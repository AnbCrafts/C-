using System;
using System.Collections.Generic;

namespace DevHelper.Models
{
    [Serializable]
    public class ModuleItem
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ShortDesc { get; set; }
        public string IconClass { get; set; }
        public string BadgeColor { get; set; }
        public int DisplayOrder { get; set; }
        public List<TopicItem> Topics { get; set; } = new List<TopicItem>();
    }

    [Serializable]
    public class TopicItem
    {
        public int TopicId { get; set; }
        public int ModuleId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string PageUrl { get; set; }
        public string Difficulty { get; set; }
        public string Summary { get; set; }
        public string KeyTakeaways { get; set; }
        public string Status { get; set; }
        public string PracticeNotes { get; set; }
    }
}
