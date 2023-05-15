using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddControllers();
builder.Services.AddDbContext<ToDoDBContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORS",
        policy =>
        {
            policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});
var app = builder.Build();
app.UseCors("CORS");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});
app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});




app.MapGet("/items", (ToDoDBContext context) =>
{
    return context.Items.ToListAsync();
});
app.MapPost("/items",async (Item item, ToDoDBContext context) =>
{
    context.Items.Add(item);
    await context.SaveChangesAsync();
    return Results.Created($"/items/{item.Id}", item);
});
app.MapPut("/items/{id}", async (int id, Item updateItem, ToDoDBContext context) =>
{
    var update =await context.Items.FindAsync(id);
    if (update is null)
        return Results.NotFound();
    update.Name = updateItem.Name;
    update.IsComplete = updateItem.IsComplete;
    context.Update(update);
    await context.SaveChangesAsync();
    return Results.NoContent();

});
app.MapDelete("/items/{id}", async (int id, ToDoDBContext context) =>
{
    var item = context.Items.Find(id);
    if (await context.Items.FindAsync(id) is Item todo){
         context.Items.Remove(todo);
        await context.SaveChangesAsync();
        return Results.Ok(todo);
    } 
    return Results.NotFound();
    

});
app.Run();
