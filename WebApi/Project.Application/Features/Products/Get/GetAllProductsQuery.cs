// Ubicación: WebApi/Project.Application/Features/Products/Get/GetAllProductsQuery.cs
using Common.CleanArch;
using Project.Domain.Entities;

namespace Project.Application.Features.Products.Get;

// No recibe parámetros, devuelve una lista de productos
public record GetAllProductsQuery() : IResultRequest<IEnumerable<Product>>;