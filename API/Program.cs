using BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>{
  options.AddPolicy("CORS", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
builder.Services.Configure<HostOptions>(x =>{
  x.ServicesStartConcurrently = true;
  x.ServicesStopConcurrently = false;
});
builder.Services.AddHostedService<AvregeRatingBGService>();
builder.Services.AddHostedService<BestInGenreBGService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CORS");
app.UseAuthorization();

app.MapControllers();

app.Run();
