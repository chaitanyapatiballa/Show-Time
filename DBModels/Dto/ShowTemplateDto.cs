namespace DBModels.Dto
{
    public class ShowtemplateDto
    {

        public decimal Baseprice { get; set; }
        public string Format { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public int Movieid { get; set; }
        public int Theaterid { get; set; }
    }
}