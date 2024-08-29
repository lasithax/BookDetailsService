using BookDetailsService.src.application.DTOs;
using Microsoft.AspNetCore.Mvc;

[Route("api/book-details")]
[ApiController]
public class BookDetailsController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public BookDetailsController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("BookListService");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookDetails(int id)
    {
        // Using the relative URL here since the BaseAddress is already set
        var response = await _httpClient.GetAsync($"/api/books-list/{id}");

        if (response.IsSuccessStatusCode)
        {
            var bookDetails = await response.Content.ReadFromJsonAsync<BookDto>();
            return Ok(bookDetails);
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return NotFound(new { Message = "Book not found" });
        }

        return StatusCode((int)response.StatusCode, new { Message = "Error fetching book details" });
    }
}