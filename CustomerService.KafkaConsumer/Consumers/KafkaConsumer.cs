using Confluent.Kafka;
using CustomerService.Application.Events;
using CustomerService.Application.Interfaces;
using CustomerService.KafkaConsumer.Models;
using CustomerService.KafkaConsumer.Repository;

namespace CustomerService.KafkaConsumer.Consumers;

public class KafkaConsumer
{
    private readonly IConsumer<Null, CustomerCreated> _mainConsumer;
    private readonly IConsumer<Null, CustomerCreated> _retryConsumer;
    private readonly List<ConsumeResult<Null,CustomerCreated>> _messageList = new() ;
    private int _bulkSize;
    private const int BulkCapacity = 5;
    private readonly List<Log> _logList  = new();
    private readonly LogRepository _logRepository;
    private readonly IKafkaPublisher _kafkaPublisher;

    public KafkaConsumer(LogRepository logRepository, IKafkaSettings kafkaSettings, IKafkaPublisher kafkaPublisher)
    {
        _logRepository = logRepository;
        _kafkaPublisher = kafkaPublisher;
        var mainConsumerOptions = new Dictionary<string, string>
        {
            {"bootstrap.servers", kafkaSettings.BootstrapServer},
            {"security.protocol", kafkaSettings.SASLProtocol},
            {"sasl.mechanisms", kafkaSettings.SASLMechanism},
            {"sasl.username", kafkaSettings.SASLUsername},
            {"sasl.password", kafkaSettings.SASLPassword},
            {"group.id",kafkaSettings.MainGroupId},
            {"enable.auto.commit",kafkaSettings.EnableAutoCommitForMain}
        };
        var retryConsumerOptions = new Dictionary<string, string>
        {
            {"bootstrap.servers", kafkaSettings.BootstrapServer},
            {"security.protocol", kafkaSettings.SASLProtocol},
            {"sasl.mechanisms", kafkaSettings.SASLMechanism},
            {"sasl.username", kafkaSettings.SASLUsername},
            {"sasl.password", kafkaSettings.SASLPassword},
            {"group.id",kafkaSettings.RetryGroupId},
            {"enable.auto.commit",kafkaSettings.EnableAutoCommitForRetry}
        };
        var mainClientConfig = new ClientConfig(mainConsumerOptions);
        var mainConsumerConfig = new ConsumerConfig(mainClientConfig);
        
        var retryClientConfig = new ClientConfig(retryConsumerOptions);
        var retryConsumerConfig = new ConsumerConfig(retryClientConfig);
        
        
        _mainConsumer = new ConsumerBuilder<Null, CustomerCreated>(mainConsumerConfig).SetValueDeserializer(new CustomerCreated()).Build();
        _mainConsumer.Subscribe(kafkaSettings.CustomerCreatedTopic);
        _retryConsumer = new ConsumerBuilder<Null, CustomerCreated>(retryConsumerConfig)
            .SetValueDeserializer(new CustomerCreated()).Build();
    }


    public async Task StartMainConsume()
    {
        var consumeResult = _mainConsumer.Consume();
        _bulkSize++;
        _messageList.Add(consumeResult);
        var log = new Log();
        log.FillLogMessage(consumeResult.Message.Value);
        _logList.Add(log);
        if (_bulkSize == BulkCapacity)
        {
            try
            {
                await _logRepository.AddRangeAsync(_logList);
            }
            catch (Exception e)
            {
                foreach (var message in _messageList)
                {
                    await _kafkaPublisher.PublishToRetry(message);
                }
            }
            foreach (var message in _messageList)
            {
                _mainConsumer.Commit(message);
            }
            _logList.Clear();
            _messageList.Clear();
            _bulkSize = 0;
        }
    }
}


