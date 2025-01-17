﻿using MediatR;
using NerdStore.Core.Messages;

namespace NerdStore.Core.Bus;

internal sealed class MediatorHandler : IMediatorHandler
{

    private readonly IMediator _mediator;


    public MediatorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<bool> EnviarComando<T>(T comando) where T : Command
    {
        return await _mediator.Send(comando);
    }

    public async Task PublicarEvento<T>(T evento) where T : Event
    {
        await _mediator.Publish(evento);
    }
}
