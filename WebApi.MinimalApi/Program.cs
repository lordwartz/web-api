using System.Reflection;
using Microsoft.AspNetCore.Mvc.Formatters;
using WebApi.MinimalApi.Domain;
using WebApi.MinimalApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5000");
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => {
        options.SuppressModelStateInvalidFilter = true;
        options.SuppressMapClientErrors = true;
    });
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddControllers(options =>
{
    options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()); 
    options.ReturnHttpNotAcceptable = true;
    options.RespectBrowserAcceptHeader = true;
});
builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<UserEntity, UserDto>()
        .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.LastName} {src.FirstName}"));
    cfg.CreateMap<CreateUserModel, UserEntity>()
        .ForMember(dest => dest.CurrentGameId, opt => opt.MapFrom(src => (Guid?)null))
        .ForMember(dest => dest.GamesPlayed, opt => opt.MapFrom(src => 0))
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Empty));
}, Array.Empty<Assembly>());

var app = builder.Build();

app.MapControllers();

app.Run();