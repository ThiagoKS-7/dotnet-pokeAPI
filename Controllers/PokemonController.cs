using Microsoft.AspNetCore.Mvc;
using RestSharp;
namespace AluraRestSharpAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PokemonController : ControllerBase
{
    private readonly RestClient _client;
    private Dictionary<String, Pokemon> PokeOptions = new Dictionary<String, Pokemon>();
    private static readonly string[] PokemonNames = new[]
    {
        "bulbasaur", "charmander", "squirtle"
    };

    public PokemonController()
     {
        _client = new RestClient("https://pokeapi.co/api/v2");
    }

    [HttpGet(Name = "GetPokemon")]
    public async Task<IActionResult> GetPokemonList()
    {
        try {
            // Itera na lista de nomes e chama 1 request pra cada nome
            for (int i = 0; i < PokemonNames.Length; i++) {
                var url = $"/pokemon/{PokemonNames[i].ToString()}";
                
                // Faz a request com o RestSharp
                var request = new RestRequest(url);
                var response = await _client.ExecuteGetAsync(request);
                if (!response.IsSuccessful)
                {
                     return BadRequest();
                }
                else 
                {
                    // Chama o model pokemon a partir da request e adiciona na lista
                    var pokemon = Pokemon.FromJson(response.Content != null ? response.Content : "{}");
                    if (pokemon!= null) {
                        PokeOptions.Add(PokemonNames[i].ToString(), pokemon);
                    }
                }
            }
            return Ok(PokeOptions);
        } catch (Exception e) {
            Console.WriteLine(e.Message);
            return BadRequest();
        }
                    
    }
}
