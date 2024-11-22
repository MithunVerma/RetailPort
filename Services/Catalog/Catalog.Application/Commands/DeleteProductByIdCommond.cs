using MediatR;

namespace Catalog.Application.Commands
{
    public class DeleteProductByIdCommond:IRequest<bool>
    {
        public string Id { get; set; }

        public DeleteProductByIdCommond(string Id) 
        {
            Id = Id;   
        }

    }
}
