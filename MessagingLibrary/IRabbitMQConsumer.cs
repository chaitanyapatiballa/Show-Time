using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingLibrary
{
    public interface IRabbitMQConsumer
    {
        void StartConsuming(string queueName, Action<string> onMessageReceived);
    }
}
