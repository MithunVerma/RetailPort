using Catalog.Application.Commands;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class DeleteProductByIdCommondHandler : IRequestHandler<DeleteProductByIdCommond, bool>
    {
        private readonly IProductRepository _productRepository;
        public DeleteProductByIdCommondHandler(IProductRepository productRepository) 
        {
        _productRepository = productRepository;
        }
        public async Task<bool> Handle(DeleteProductByIdCommond request, CancellationToken cancellationToken)
        {
            return await _productRepository.DeleteProduct(request.Id);
        }
    }
}
