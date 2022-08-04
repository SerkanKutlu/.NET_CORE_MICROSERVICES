

using OrderService.Application.DTO;

namespace OrderService.Application.Interfaces;

public interface IPublisher
{
    public void PublishForLog(OrderForLogDto order);
}