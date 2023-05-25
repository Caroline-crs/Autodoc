using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TestForDotNet.Class;
using static TestForDotNet.Class.Cartoon;

async Task<List<Character>> GetCharacterRickAndMorty()
{
    string url = "https://rickandmortyapi.com/api/character";
    Root root = new Root();

    try
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string body = await response.Content.ReadAsStringAsync();

                root = JsonConvert.DeserializeObject<Root>(body);

            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    return root.results;
}


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("Character"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();

List<Character> characterList = new List<Character>();

async Task GetAndStoreCharacters()
{
    List<Character> characters = await GetCharacterRickAndMorty();
    characterList.AddRange(characters);
}

app.MapGet("/Characters", async (AppDbContext dbContext) =>
{
    if (characterList.Count == 0)
        await GetAndStoreCharacters();

    return characterList;
});

app.MapGet("/Character/{id}", async (int id, AppDbContext dbContext) =>
{
    Character characterSingle = characterList.SingleOrDefault(c => c.id == id);

    if (characterSingle != null)
        return Results.Ok(characterSingle);

    else
        return Results.NotFound();
});

app.MapPut("/Character/{id}", async (int id, Character updateCharacter, AppDbContext dbContext) =>
{
    Character character = await dbContext.Characters.FindAsync(id);

    if (character != null)
    { 
        character.name = updateCharacter.name;
        character.status = updateCharacter.status;
        character.gender = updateCharacter.gender;
        character.species = updateCharacter.species;

        dbContext.Entry(character).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
    }

    else
        Results.NotFound();


    return updateCharacter;
});


app.MapDelete("/Character/{id}", async (int id, AppDbContext dbContext) =>
{
    Character character = await dbContext.Characters.FindAsync(id);

    if (character != null)
        dbContext.Characters.Remove(character);

    await dbContext.SaveChangesAsync();

    return;
});


app.MapPost("/Character", async (Character character, AppDbContext dbContext) =>
{
    dbContext.Characters.Add(character);
    await dbContext.SaveChangesAsync();

    return character;
});

app.UseSwaggerUI();
app.Run();

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options)
        : base(options) { }

    public DbSet<Character> Characters { get; set; }
    //public DbSet<Episode> Episodes { get; set; }
}