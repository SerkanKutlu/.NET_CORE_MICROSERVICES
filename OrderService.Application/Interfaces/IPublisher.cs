

using OrderService.Application.DTO;

namespace OrderService.Application.Interfaces;

public interface IPublisher
{
    public Task PublishForLog(OrderForLogDto customer);
}