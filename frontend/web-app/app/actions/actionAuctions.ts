'use server'
import { Auction, PagedResult } from '@/types';

//API'dan veriyi çekme json formatında return etme

export async function getData(query: string):Promise<PagedResult<Auction>>{
    //getData(pageNumber: number, pageSize:number)
    //`http://localhost:6001/searches/SearchWithHelper?PageSize=${pageSize}&pageNumber=${pageNumber}`
    const res = await fetch(`http://localhost:6001/searches/SearchWithHelper${query}`);

    if(!res.ok) throw new Error('Failed to fetch data');
    return res.json();
}