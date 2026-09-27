using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Usuarios.Login;

public record LoginCommand(string Email, string Senha) : IRequest<ResultViewModel<LoginResponse>>;

public record LoginResponse(string Token);
