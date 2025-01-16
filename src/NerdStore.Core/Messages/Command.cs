using FluentValidation.Results;
using MediatR;

namespace NerdStore.Core.Messages;

public abstract class Command : Message, IRequest<bool>
{

    protected Command()
    {
        TimeStamp = DateTime.Now;
    }

    public DateTime TimeStamp { get; private set; }

    public ValidationResult ValidationResult { get; set; }

    public abstract bool EhValido();

}
