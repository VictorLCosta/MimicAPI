namespace MimicAPI.Helpers
{
    public class Pagination
    {
        public int PageNumber { get; set; }
        public int RegisterPerPage { get; set; }
        public int TotalRegisters { get; set; }
        public int TotalPages { get; set; }
    }
}