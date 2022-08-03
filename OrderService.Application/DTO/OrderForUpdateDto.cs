﻿using OrderService.Domain.ValueObjects;

namespace OrderService.Application.DTO;

public class OrderForUpdateDto
{
    public string Id { get; set; }
    public string Status { get; set; }
    public Address Address { get; set; }
    public List<string> ProductIds { get; set; }
}