﻿using System;
using Microsoft.Azure.ServiceBus;

namespace HexMaster.BuildingBlocks.EventBus.EventBusServiceBus
{
    public interface IServiceBusPersisterConnection : IDisposable
    {
        ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder { get; }

        ITopicClient CreateModel();
    }
}