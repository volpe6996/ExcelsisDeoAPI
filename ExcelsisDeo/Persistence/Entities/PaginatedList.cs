namespace ExcelsisDeo.Persistence.Entities;

public class PaginatedList<T> where T : class
{
    public int CurrentPage { get; set; }
    public decimal PageCount { get; set; }
    public List<T> ItemList { get; set; }
    
    public PaginatedList(decimal pageCount, int currentPage, List<T> itemList)
    {
        PageCount = pageCount;
        CurrentPage = currentPage;
        ItemList = itemList;
    }   
}