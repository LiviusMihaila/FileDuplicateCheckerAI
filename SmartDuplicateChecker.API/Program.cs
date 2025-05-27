using SmartDuplicateChecker.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();                
builder.Services.AddEndpointsApiExplorer();       // for Swagger
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IVectorStore>(new JsonVectorStore("vectors.json"));
builder.Services.Configure<EmbeddingServiceOptions>(builder.Configuration.GetSection("EmbeddingService"));


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
