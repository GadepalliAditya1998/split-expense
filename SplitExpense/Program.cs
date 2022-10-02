using SplitExpense.Core.Services;
using SplitExpense.Core.Services.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConfigireServices();

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


void ConfigireServices()
{
    builder.Services.AddScoped<DatabaseContext>(db => new DatabaseContext(builder.Configuration.GetConnectionString("DefaultConnectionString")));
    builder.Services.AddSingleton<IConfiguration>(c => builder.Configuration);
    builder.Services.AddScoped<ExpenseService>();
    builder.Services.AddScoped<ExpenseGroupService>();
    builder.Services.AddScoped<UserInviteService>();
}