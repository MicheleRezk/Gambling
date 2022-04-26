using Gambling.Backend;
using Gambling.Backend.Common;
using Gambling.Backend.Entities;
using Gambling.Backend.Services;
using Gambling.Backend.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ServiceSettings>(builder.Configuration.GetSection("ServiceSettings"));  
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services
    .AddMongo()
    .AddMongoRepository<Player>("Players");
builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});
builder.Services.AddSingleton<IPlayerServices, PlayerServices>();
builder.Services.AddSingleton<IBetServices, BetServices>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();