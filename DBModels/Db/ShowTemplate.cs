namespace DBModels.Db
{
    public class ShowTemplate
    {
        public int ShowTemplateId { get; set; }
        public int MovieId { get; set; }
        public int TheaterId { get; set; }

        public ICollection<ShowInstance> ShowInstances { get; set; } = new List<ShowInstance>();
    }
}