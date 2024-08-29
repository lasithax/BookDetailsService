var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("BookListService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7050");  // Replace with your BookListService URL
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

// Configure CORS policy to allow specific origins (such as BookDetailsService)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBookDetailsService",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowBookDetailsService");

app.UseAuthorization();

app.MapControllers();

app.Run();