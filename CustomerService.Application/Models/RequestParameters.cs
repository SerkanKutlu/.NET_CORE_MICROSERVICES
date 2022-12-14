
using CustomerService.Application.Exceptions;

namespace CustomerService.Application.Models;


public class RequestParameters
{
    const int MaxPageSize = 50;
    private int _pageNumber;
    private int _pageSize;
    private string _searchArea = "Name";
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = (value <= 0) ? _pageNumber : value;
    }
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ?
            throw new InvalidModelException($"Page size can not be greater than {MaxPageSize}"): value; 
    }
    public string SearchTerm { get; set; }
    public string OrderBy { get; set; }

    public string SearchArea
    {
        get => _searchArea;
        set => _searchArea = (!string.IsNullOrWhiteSpace(value)) ? value : "Name";
    }

    public RequestParameters()
    {
        _pageNumber = 1;
        _pageSize = 10;
    }
}