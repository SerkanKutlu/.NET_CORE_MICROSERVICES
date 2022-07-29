using Common.Commands;
using MassTransit;
using OrderService.Application.Interfaces;

namespace OrderService.Application.Consumers;

public class DeleteOrdersConsumer:IConsumer<IDeleteCustomerRelatedOrders>
{
    private readonly IOrderRepository _orderRepository;

    public DeleteOrdersConsumer(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Consume(ConsumeContext<IDeleteCustomerRelatedOrders> context)
    {
        await Task.Run(() =>
        {
            Console.WriteLine("DeleteOrdersConsumer worked");
            var message = context.Message;
            _orderRepository.DeleteOrderOfCustomer(message.CustomerId);
        });
        
    }
}