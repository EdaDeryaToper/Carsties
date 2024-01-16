using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;

namespace AuctionService.RequestHelpers
{
    public class MappingProfiles :Profile
    {
       public MappingProfiles()
       {
        // Auction sınıfını AuctionDto'ya haritala ve Item'i dahil et
         CreateMap<Auction,AuctionDto>().IncludeMembers(x=>x.Item);
         // Item sınıfını doğrudan AuctionDto'ya haritala
         CreateMap<Item,AuctionDto>();
         //CreateAuctionDto'yu Auction'a haritalar ve aynı zamanda Item öğesini CreateAuctionDto'dan (s) Auction'a (d) kopyalar. 
         CreateMap<CreateAuctionDto,Auction>().ForMember(d=>d.Item, o=>o.MapFrom(s=>s));
         CreateMap<CreateAuctionDto,Item>();
       } 
    }
}