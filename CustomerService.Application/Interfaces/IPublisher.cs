using CustomerService.Application.Dto;
using CustomerService.Domain.Entities;

namespace CustomerService.Application.Interfaces;

public interface IPublisher
{
    Task PublishForLog(CustomerForLogDto customer);
}