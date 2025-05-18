# MyWebMovieApp – Funkcjonalności i Modyfikacje

## 🌤 Strona z pogodami

### 📋 Zmienne
W celu umożliwienia filtrowania prognoz, w komponencie zostały dodane dwie zmienne:
```csharp
private WeatherForecast[]? forecasts;
private WeatherForecast[]? originalForecasts;
```

- `forecasts` – aktualnie wyświetlane dane.
- `originalForecasts` – oryginalny zestaw danych, używany do resetowania filtrów.

### 🧮 Filtrowanie i wyszukiwanie

#### Filtrowanie dni z temperaturą powyżej 15°C
```csharp
private void WarmDaysFilter()
{
    if (originalForecasts is null) return;
    forecasts = originalForecasts.Where(f => f.TemperatureC > 15).ToArray();
}
```

#### Resetowanie do oryginalnych prognoz
```csharp
private void Restore()
{
    forecasts = originalForecasts.ToArray();
}
```

#### Filtrowanie po nazwie (summary)
```csharp
private void Input(ChangeEventArgs arg)
{
    searchTerm = arg.Value?.ToString() ?? "";
    forecasts = originalForecasts
        .Where(f => f.Summary?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true)
        .ToArray();
}
```

### 🧾 Widok HTML
```razor
<input class="form-control mb-3" placeholder="Filter by summary..." @oninput="Input" />
<p>Number of warm days: @warmDays</p>
<div style="display: flex; gap: 10px;">
    <button class="btn btn-primary" @onclick="WarmDaysFilter">Warm Days Filter ON</button>
    <button class="btn btn-secondary" @onclick="Restore">Original Forecasts</button>
</div>
```

## 🎬 Strona z filmami

### 📦 Klasa `Movie`
```csharp
public class Movie
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? ReleaseDate { get; set; }

    [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
    public float? Rate { get; set; }

    public int? NumberOfRatings { get; set; } = 0;

    public string? ImageUrl { get; set; }
}
```

- Obsługuje pełen CRUD (Create, Read, Update, Delete).
- Obsługa zdjęć przez upload z dysku – zdjęcie widoczne w szczegółach filmu.

### 📊 Sortowanie filmów

Dodano sortowanie w kolumnach: tytuł, data wydania, ocena. Każdy przycisk sortuje w obu kierunkach:

```razor
<QuickGrid Class="table" Items="context.Movie">
    <PropertyColumn Title="Title" Property="movie => movie.Title" Sortable="true" />
    <PropertyColumn Title="Release Year" Property="movie => movie.ReleaseDate.HasValue ? movie.ReleaseDate.Value.Year.ToString() : string.Empty" Sortable="true" />
    <PropertyColumn Title="Rate" Property="movie => movie.Rate" Sortable="true" />
</QuickGrid>
```

### 📥 Dodawanie filmu z obsługą uploadu zdjęcia

Fragment odpowiedzialny za upload zdjęcia do folderu `wwwroot/images`:
```csharp
private async Task HandleImageUpload(InputFileChangeEventArgs e)
{
    var file = e.File;
    var fileName = Path.GetRandomFileName() + Path.GetExtension(file.Name);

    var imageDirectory = Path.Combine(Env.WebRootPath, "images");
    if (!Directory.Exists(imageDirectory))
    {
        Directory.CreateDirectory(imageDirectory);
    }

    var filePath = Path.Combine(imageDirectory, fileName);



    await using var fileStream = File.Create(filePath);
    await file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024).CopyToAsync(fileStream);

    Movie.ImageUrl = $"images/{fileName}";
}
```
### Wdrożenie na Microsoft Azure

Podczas próby wdrożenia aplikacji na platformę Microsoft Azure napotkano problem wynikający z różnych regionów dla App Service i bazy danych SQL.

Problem:

Aplikacja została wdrożona w regionie East Europe, natomiast baza danych znajdowała się w East Europe 2.

W rezultacie połączenia były niestabilne lub całkowicie niemożliwe, ze względu na ograniczenia sieciowe między regionami.

Konfiguracja connection stringa

Do pliku Program.cs dodano:

```csharp
var connectionString = "Data Source=tcp:mywebmovieappdbserver.database.windows.net,1433;Initial Catalog=MyWebMovieApp_Db;User ID=xxadminxx;Password=Qwerty1234";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
```
