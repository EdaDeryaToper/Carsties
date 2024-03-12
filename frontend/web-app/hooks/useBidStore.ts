import { Bid } from "@/types"
import { create } from "zustand"

type State = {
    bids: Bid[]
    open: boolean
}

type Actions = {
    setBids: (bids: Bid[]) => void
    addBid: (bid: Bid) => void
    setOpen: (value: boolean) => void
}

export const useBidStore = create<State & Actions>((set) => ({
    bids: [],
    open: true,

    setBids: (bids: Bid[]) => {
        set(() => ({
            bids
        }))
    },
    //önce bid id'si kontrol edilir var mı diye 
    //false ise ekle true ise ekleme varolan state return et
    addBid: (bid: Bid) => {
        set((state) => ({
            bids: !state.bids.find(x => x.id === bid.id) ? [bid, ...state.bids] : [...state.bids]
        }))
    },

    setOpen: (value: boolean) => {
        set(() => ({
            open: value
        }))
    }
}))