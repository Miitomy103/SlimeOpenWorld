param(
    [Parameter(Mandatory)][string]$FilePath
)

$metaPath = $FilePath + ".meta"

if (-not (Test-Path $metaPath)) {
    Write-Host "[$( Split-Path $FilePath -Leaf )] .meta not found: $metaPath" -ForegroundColor Red
    exit 1
}

$guidLine = Get-Content $metaPath | Select-String "^guid:"
if (-not $guidLine) {
    Write-Host "[$( Split-Path $FilePath -Leaf )] GUID not found" -ForegroundColor Red
    exit 1
}

$guid = ($guidLine.Line -replace "^guid:\s*", "").Trim()
$name = Split-Path $FilePath -Leaf

Write-Host "[$name] guid: $guid" -ForegroundColor Cyan

$hits = Get-ChildItem -Path . -Recurse -Include "*.unity","*.prefab","*.asset","*.cs" |
    Select-String -Pattern $guid -List

if ($hits) {
    Write-Host "Used in:" -ForegroundColor Green
    foreach ($h in $hits) {
        $rel = $h.Path -replace [regex]::Escape((Get-Location).Path + "\"), ""
        Write-Host "  $rel (line $($h.LineNumber))"
    }
} else {
    Write-Host "  -> Not used anywhere" -ForegroundColor Yellow
}
