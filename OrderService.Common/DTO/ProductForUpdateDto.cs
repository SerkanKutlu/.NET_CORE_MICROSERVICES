﻿namespace OrderService.Common.DTO;

public class ProductForUpdateDto
{
    public string Id { get; set; }
    public string ImageUrl { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
}