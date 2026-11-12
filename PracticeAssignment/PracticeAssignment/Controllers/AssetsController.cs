using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PracticeAssignment.Models;
using PracticeAssignment.Models.DTOs;
using PracticeAssignment.Repositories;

namespace PracticeAssignment.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Requires valid JWT Bearer Token for all endpoints
public class AssetsController : ControllerBase
{
    private readonly IAssetRepository _assetRepository;
    private readonly ILogger<AssetsController> _logger;

    public AssetsController(IAssetRepository assetRepository, ILogger<AssetsController> logger)
    {
        _assetRepository = assetRepository;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves a list of all inventory assets (sp_GetAllAssets).
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Asset>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var assets = await _assetRepository.GetAllAssetsAsync();
        return Ok(assets);
    }

    /// <summary>
    /// Retrieves a specific asset by its AssetId (sp_GetAssetById).
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Asset), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new { message = "Asset ID must be a positive integer." });
        }

        var asset = await _assetRepository.GetAssetByIdAsync(id);
        if (asset == null)
        {
            return NotFound(new { message = $"Asset with ID {id} was not found." });
        }

        return Ok(asset);
    }

    /// <summary>
    /// Creates a new asset record (sp_InsertAsset).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Asset), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] AssetCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var asset = new Asset
        {
            AssetName = dto.AssetName,
            Category = dto.Category,
            PurchaseDate = dto.PurchaseDate,
            Status = dto.Status,
            Price = dto.Price
        };

        var newAssetId = await _assetRepository.InsertAssetAsync(asset);
        asset.AssetId = newAssetId;
        asset.CreatedAt = DateTime.UtcNow;

        return CreatedAtAction(nameof(GetById), new { id = newAssetId }, asset);
    }

    /// <summary>
    /// Updates an existing asset record (sp_UpdateAsset).
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Asset), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] AssetUpdateDto dto)
    {
        if (id <= 0)
        {
            return BadRequest(new { message = "Asset ID must be a positive integer." });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Verify if asset exists
        var existingAsset = await _assetRepository.GetAssetByIdAsync(id);
        if (existingAsset == null)
        {
            return NotFound(new { message = $"Asset with ID {id} was not found." });
        }

        existingAsset.AssetName = dto.AssetName;
        existingAsset.Category = dto.Category;
        existingAsset.PurchaseDate = dto.PurchaseDate;
        existingAsset.Status = dto.Status;
        existingAsset.Price = dto.Price;

        var updated = await _assetRepository.UpdateAssetAsync(id, existingAsset);
        if (!updated)
        {
            return NotFound(new { message = $"Failed to update. Asset with ID {id} was not found." });
        }

        var refreshedAsset = await _assetRepository.GetAssetByIdAsync(id);
        return Ok(refreshedAsset);
    }

    /// <summary>
    /// Deletes an asset record (sp_DeleteAsset).
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new { message = "Asset ID must be a positive integer." });
        }

        var deleted = await _assetRepository.DeleteAssetAsync(id);
        if (!deleted)
        {
            return NotFound(new { message = $"Asset with ID {id} was not found." });
        }

        return Ok(new { message = $"Asset with ID {id} was successfully deleted." });
    }
}
