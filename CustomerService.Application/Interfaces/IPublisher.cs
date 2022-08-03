using CustomerService.Application.Dto;
using CustomerService.Domain.Entities;

namespace CustomerService.Application.Interfaces;

public interface IPublisher
{
    void PublishForLog(CustomerForLogDto customer);
}