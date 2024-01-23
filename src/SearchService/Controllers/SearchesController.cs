using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;
using ZstdSharp.Unsafe;

namespace SearchService.Controllers;
[ApiController]
[Route("api/searches")]
public class SearchesController:ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems(string searchTerm)
    {
        var query = DB.Find<Item>();
        query.Sort(x => x.Ascending(a => a.Make));
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query.Match(Search.Full, searchTerm).SortByTextScore();
        }
        var result = await query.ExecuteAsync();
       
        return result;
    }

   /* [HttpGet("{SearchPages}")]
    public async Task<ActionResult<List<Item>>> SearchPages(string searchTerm, int pageNumber = 1, int pageSize = 4)
    {
        //Find methodu tek bir sayfada arama yapar
        var query = DB.PagedSearch<Item>();
        
        query.Sort(x => x.Ascending(a => a.Make));
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query.Match(Search.Full, searchTerm).SortByTextScore();
        }

        query.PageNumber(pageNumber);
        query.PageSize(pageSize);
        var result = await query.ExecuteAsync();
       
        return Ok(new
        {
            results = result.Results,
            pageCount=result.PageCount,
            totalCount=result.TotalCount
        });
    }*/
    [HttpGet("{SearchWithHelper}")]
    public async Task<ActionResult<List<Item>>> SearchWithHelper([FromQuery]SearchParams searchParams)
    {
        //Find methodu tek bir sayfada arama yapar.OrderBy yapıldığından ve
        //make, new şeklinde iki parametre sorgulandığından dolayı <Item,Item> oldu.
        var query = DB.PagedSearch<Item,Item>();
        
       
        if (!string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
        }

        query = searchParams.OrderBy switch
        {
            "make" => query.Sort(x => x.Ascending(a => a.Make)),
            "new" => query.Sort(x => x.Descending(a => a.CreatedAt)),
            _ => query.Sort(x => x.Ascending(a => a.AuctionEnd))

        };

        query = searchParams.FilterBy switch
        {
            "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
            "endingSoon" => query.Match(x =>
                x.AuctionEnd < DateTime.UtcNow.AddHours(6) && x.AuctionEnd > DateTime.UtcNow),
            _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow)
        };

        if (!string.IsNullOrEmpty(searchParams.Seller))
        {
            query.Match(x => x.Seller == searchParams.Seller);
        }

        if (!string.IsNullOrEmpty(searchParams.Winner))
        {
            query.Match(x => x.Winner == searchParams.Winner);
        }

        query.PageNumber(searchParams.PageNumber);
        query.PageSize(searchParams.PageSize);
        var result = await query.ExecuteAsync();
       
        return Ok(new
        {
            results = result.Results,
            pageCount=result.PageCount,
            totalCount=result.TotalCount
        });
    }
}