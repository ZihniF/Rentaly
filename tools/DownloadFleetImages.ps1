param(
    [string]$OutputDirectory = "Rentaly.WebUI\wwwroot\rentaly\images\cars-models"
)

$ErrorActionPreference = "Stop"
$expectedRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot "..\Rentaly.WebUI\wwwroot\rentaly\images\cars-models"))
$resolvedOutput = [System.IO.Path]::GetFullPath((Join-Path (Get-Location) $OutputDirectory))
if ($resolvedOutput -ne $expectedRoot) {
    throw "Beklenmeyen görsel hedefi: $resolvedOutput"
}

New-Item -ItemType Directory -Path $resolvedOutput -Force | Out-Null

$vehicles = @(
    @{ Slug = "bmw-m5"; Query = "BMW M5 G90" },
    @{ Slug = "volkswagen-polo"; Query = "Volkswagen Polo VI" },
    @{ Slug = "toyota-rav4"; Query = "Toyota RAV4" },
    @{ Slug = "jeep-renegade"; Query = "Jeep Renegade" },
    @{ Slug = "mini-cooper"; Query = "MINI Cooper F56" },
    @{ Slug = "ford-raptor"; Query = "Ford F-150 Raptor" },
    @{ Slug = "hyundai-staria"; Query = "Hyundai Staria" },
    @{ Slug = "range-rover-sport"; Query = "Range Rover Sport" },
    @{ Slug = "lexus-rx"; Query = "Lexus RX" },
    @{ Slug = "renault-clio"; Query = "Renault Clio" },
    @{ Slug = "hyundai-i20"; Query = "Hyundai i20 BC3" },
    @{ Slug = "peugeot-208"; Query = "Peugeot 208" },
    @{ Slug = "opel-corsa"; Query = "Opel Corsa F" },
    @{ Slug = "skoda-fabia"; Query = "Skoda Fabia" },
    @{ Slug = "dacia-sandero"; Query = "Dacia Sandero" },
    @{ Slug = "seat-ibiza"; Query = "SEAT Ibiza KJ" },
    @{ Slug = "renault-megane-sedan"; Query = "Renault Megane Grandcoupe" },
    @{ Slug = "toyota-corolla"; Query = "Toyota Corolla Sedan E210" },
    @{ Slug = "skoda-octavia"; Query = "Skoda Octavia IV" },
    @{ Slug = "volkswagen-passat"; Query = "Volkswagen Passat B8" },
    @{ Slug = "skoda-superb"; Query = "Skoda Superb IV" },
    @{ Slug = "honda-civic"; Query = "Honda Civic" },
    @{ Slug = "fiat-egea"; Query = "Fiat Tipo Sedan" },
    @{ Slug = "ford-focus"; Query = "Ford Focus Mk4" },
    @{ Slug = "dacia-duster"; Query = "Dacia Duster" },
    @{ Slug = "nissan-qashqai"; Query = "Nissan Qashqai" },
    @{ Slug = "hyundai-tucson"; Query = "Hyundai Tucson" },
    @{ Slug = "kia-sportage"; Query = "Kia Sportage" },
    @{ Slug = "skoda-karoq"; Query = "Skoda Karoq" },
    @{ Slug = "skoda-kodiaq"; Query = "Skoda Kodiaq" },
    @{ Slug = "toyota-chr"; Query = "Toyota C-HR" },
    @{ Slug = "peugeot-3008"; Query = "Peugeot 3008" },
    @{ Slug = "renault-austral"; Query = "Renault Austral" },
    @{ Slug = "bmw-320i"; Query = "BMW 3 Series G20" },
    @{ Slug = "bmw-520i"; Query = "BMW 5 Series G60" },
    @{ Slug = "mercedes-c200"; Query = "Mercedes-Benz W206" },
    @{ Slug = "mercedes-e200"; Query = "Mercedes-Benz W214" },
    @{ Slug = "audi-a4"; Query = "Audi A4 B9" },
    @{ Slug = "audi-a6"; Query = "Audi A6 C8" },
    @{ Slug = "bmw-x1"; Query = "BMW X1 U11" },
    @{ Slug = "bmw-x3"; Query = "BMW X3 G45" },
    @{ Slug = "mercedes-glc200"; Query = "Mercedes-Benz X254" },
    @{ Slug = "audi-q3"; Query = "Audi Q3 F3" },
    @{ Slug = "audi-q5"; Query = "Audi Q5 FY" }
)

$headers = @{ "User-Agent" = "RentalyDemo/1.0 (local educational project)" }
$excludedTitleTerms = "rear|interior|dashboard|engine|logo|timeline|police|polizei|polizia|taxi|race|racing|rally|stcc|wreck|model car|drawing|sketch|cross|scout|variant|combi|estate|wagon|touring|avant|\bRS\b|\bR5\b|\bGTI\b"
$sourcesPath = Join-Path $resolvedOutput "sources.json"
$attributions = @()
if (Test-Path -LiteralPath $sourcesPath) {
    $loadedSources = Get-Content -LiteralPath $sourcesPath -Raw | ConvertFrom-Json
    foreach ($loadedSource in $loadedSources) {
        if ($loadedSource.Slug) {
            $attributions += $loadedSource
        }
        elseif ($loadedSource.value) {
            foreach ($nestedSource in $loadedSource.value) {
                if ($nestedSource.Slug) { $attributions += $nestedSource }
            }
        }
    }
}

function Find-CommonsImages([string]$query) {
    $search = [Uri]::EscapeDataString("intitle:`"$query`" filetype:bitmap")
    $apiUri = "https://commons.wikimedia.org/w/api.php?action=query&generator=search&gsrsearch=$search&gsrnamespace=6&gsrlimit=15&prop=imageinfo&iiprop=url%7Cmime%7Cextmetadata&iiurlwidth=960&format=json&formatversion=2"
    $response = $null
    for ($attempt = 1; $attempt -le 5; $attempt++) {
        try {
            $response = Invoke-RestMethod -Uri $apiUri -Headers $headers -ErrorAction Stop
            break
        }
        catch {
            if ($attempt -eq 5) { throw }
            Start-Sleep -Seconds (5 * $attempt)
        }
    }

    return @($response.query.pages |
        Where-Object {
            $_.imageinfo -and
            $_.imageinfo[0].mime -eq "image/jpeg" -and
            $_.imageinfo[0].thumburl -and
            $_.title -notmatch $excludedTitleTerms
        } |
        Sort-Object index)
}

foreach ($vehicle in $vehicles) {
    $target = Join-Path $resolvedOutput "$($vehicle.Slug).jpg"
    if ((Test-Path -LiteralPath $target) -and $attributions.Slug -contains $vehicle.Slug) {
        Write-Output "Zaten hazır: $($vehicle.Query)"
        continue
    }

    Write-Output "Görsel aranıyor: $($vehicle.Query)"
    $candidates = @(Find-CommonsImages $vehicle.Query)
    if (-not $candidates) {
        $fallbackQuery = (($vehicle.Slug -replace "-", " ") + " car front")
        Write-Output "Alternatif arama: $fallbackQuery"
        $candidates = @(Find-CommonsImages $fallbackQuery)
    }

    if (-not $candidates) {
        throw "Uygun JPEG bulunamadı: $($vehicle.Query)"
    }

    $candidate = $null
    foreach ($possibleCandidate in $candidates) {
        $possibleImageInfo = $possibleCandidate.imageinfo[0]
        $commonsFileName = $possibleCandidate.title.Substring(5)
        $redirectSource = "commons.wikimedia.org/wiki/Special:Redirect/file/$([Uri]::EscapeDataString($commonsFileName))%3Fwidth%3D960"
        $imageProxy = "https://images.weserv.nl/?url=$redirectSource&w=960&output=jpg&q=82"
        try {
            Invoke-WebRequest -Uri $imageProxy -Headers $headers -OutFile $target -ErrorAction Stop
            $candidate = $possibleCandidate
            break
        }
        catch {
            if (Test-Path -LiteralPath $target) {
                Remove-Item -LiteralPath $target -Force
            }
        }
    }

    if (-not $candidate) {
        throw "İndirilebilir JPEG bulunamadı: $($vehicle.Query)"
    }

    $imageInfo = $candidate.imageinfo[0]

    $metadata = $imageInfo.extmetadata
    $artistValue = if ($metadata.Artist.value) { $metadata.Artist.value } else { "Wikimedia Commons contributor" }
    $artist = [regex]::Replace($artistValue, "<[^>]+>", "")
    $license = if ($metadata.LicenseShortName.value) { $metadata.LicenseShortName.value } else { "See source page" }
    $licenseUrl = if ($metadata.LicenseUrl.value) { $metadata.LicenseUrl.value } else { $imageInfo.descriptionurl }
    $attributions += [pscustomobject]@{
        Slug = $vehicle.Slug
        Model = $vehicle.Query.Replace(" front car", "")
        File = $candidate.title
        Artist = [System.Net.WebUtility]::HtmlDecode($artist).Trim()
        License = $license
        LicenseUrl = $licenseUrl
        SourceUrl = $imageInfo.descriptionurl
    }
    ConvertTo-Json -InputObject @($attributions) -Depth 4 | Set-Content -Path $sourcesPath -Encoding utf8
    Start-Sleep -Seconds 2
}

ConvertTo-Json -InputObject @($attributions) -Depth 4 | Set-Content -Path $sourcesPath -Encoding utf8
Write-Output "$($vehicles.Count) araç görseli indirildi."
