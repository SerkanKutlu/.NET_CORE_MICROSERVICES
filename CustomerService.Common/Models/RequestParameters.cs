﻿
using CustomerService.Common.Exceptions;


namespace CustomerService.Common.Models;


public class RequestParameters
{
    const int MaxPageSize = 50;
    private int _pageNumber;
    private int _pageSize;

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

    public RequestParameters()
    {
        _pageNumber = 1;
        _pageSize = 10;
    }
}