'use client'

import React, { useEffect, useState } from 'react'
import AuctionCard from './AuctionCard';
import AppPagination from '../components/AppPagination';
import { Auction, PagedResult } from '@/types';
import { getData } from '../actions/actionAuctions';
import Filters from './Filters';
import { useParamsStore } from '@/hooks/useParamsStore';
import { shallow } from 'zustand/shallow';
import qs from 'query-string';
import EmptyFilter from '../components/EmptyFilter';


//Verinin client kısmı
export default  function Listings() {
  const [data,setData] = useState<PagedResult<Auction>>(); //zustand
  const params =useParamsStore(state=>({
    pageNumber: state.pageNumber,
    pageSize:state.pageSize,
    searchTerm:state.searchTerm,
    orderBy:state.orderBy,
    filterBy:state.filterBy
  }), shallow) //useParamsStore içerisindeki State parametreleri
  const setParams=useParamsStore(state=>state.setParams);
  const url = qs.stringifyUrl({url: '', query:params})

  function setPageNumber(pageNumber:number){
    setParams({pageNumber}) //useParamsStore setParams fonk
  }

  /* Bu kısım zustang kullanıldıktan sonra kaldırıldı 
  const [auctions, setAuctions] = useState<Auction[]>([]); //AuctionCard için
  const [pageCount,setPageCount] = useState(0);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize,setPageSize]=useState(4); //Filters component için
*/
//bu useEffects kısmı url update eder 
  useEffect(()=>{
    getData(url).then(data=>{
      setData(data);
      //setAuctions(data.results);
      //setPageCount(data.pageCount)
    })
  },[url]) //pageNumber AppPagination için, pageSize filter için
  if(!data) return <h3>Loading...</h3>
  //if(data.totalCount===0) return <EmptyFilter showReset/>

  return (
    <>
    {/*Filters pageSize={pageSize} setPageSize={setPageSize}*/}
    <Filters />
    {data.totalCount===0?(
    <EmptyFilter showReset/>):(
      <>
      <div className='grid grid-cols-4 gap-6'>
        {/* Veri varsa (data &&) sonuçlar üzerinde dönülerek AuctionCard bileşeni gelen auction verisi ile eşleşir ve dizi oluşur. */}
        {data.results.map(auction=>(
            <AuctionCard auction={auction} key={auction.id}/>
        ))}
    </div>
    <div className='flex justify-center mt-4'>
      <AppPagination pageChanged={setPageNumber} currentPage={params.pageNumber} pageCount={data.pageCount}/>
    </div>

      </>
    )}
    
    </>
    
  )
}
