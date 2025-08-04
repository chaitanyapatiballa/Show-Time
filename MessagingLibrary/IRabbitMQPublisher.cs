using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingLibrary
{
    public interface IRabbitMQPublisher
    {
        void Publish(string queueName, string message);
    }
}
