namespace Batch.Domain.Contracts;

/// <summary>
/// Represents a paged response shape used by list endpoints.
/// </summary>
/// <typeparam name="T">The item type.</typeparam>
public sealed class PagedResult<T>
{
    /// <summary>
    /// Gets or sets the items returned for the current page.
    /// </summary>
    public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();

    /// <summary>
    /// Gets or sets the current one-based page number.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the requested page size.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total number of items available across all pages.
    /// </summary>
    public int TotalCount { get; set; }
}
